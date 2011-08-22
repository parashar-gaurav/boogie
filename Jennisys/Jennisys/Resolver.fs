﻿//  ####################################################################
///   Utilities for resolving the "Unresolved" constants with respect to 
///   a given context (heap/env/ctx)
///
///   author: Aleksandar Milicevic (t-alekm@microsoft.com)
//  ####################################################################

module Resolver

open Ast
open AstUtils 
open Printer
open EnvUtils
open DafnyModelUtils

type Obj = { name: string; objType: Type }

type AssignmentType = 
  | FieldAssignment of (Obj * VarDecl) * Expr // the first string is the symbolic name of an object literal
  | ArbitraryStatement of Stmt

type HeapInstance = {
   objs: Map<string, Obj>;
   modifiableObjs: Set<Obj>;
   assignments: AssignmentType list;
   concreteValues: AssignmentType list;
   methodArgs: Map<string, Const>;
   methodRetVals: Map<string, Expr>;
   globals: Map<string, Expr>;
}

let NoObj = { name = ""; objType = NamedType("", []) }

// use the orginal method, not the one with an extra precondition
let FixSolution origComp origMeth sol =
  sol |> Map.fold (fun acc (cc,mm) v -> 
                      if CheckSameMethods (cc,mm) (origComp,origMeth) then
                        acc |> Map.add (origComp,origMeth) v
                      else 
                        acc |> Map.add (cc,mm) v) Map.empty

let ConvertToStatements heapInst onModifiableObjsOnly = 
  let stmtLst1 = heapInst.assignments |> List.choose (fun asgn ->                                                        
                                                        match asgn with
                                                        | FieldAssignment((o,f),e) when (not onModifiableObjsOnly || Set.contains o heapInst.modifiableObjs) ->
                                                            let fldName = GetVarName f
                                                            if fldName = "" then
                                                              Some(ExprStmt(e))
                                                            else
                                                              Some(Assign(Dot(ObjLiteral(o.name), fldName), e))
                                                        | ArbitraryStatement(stmt) -> Some(stmt)
                                                        | _ -> None)
  let stmtLst2 = heapInst.methodRetVals |> Map.toList
                                        |> List.map (fun (retVarName, retVarVal) -> Assign(VarLiteral(retVarName), retVarVal))
  stmtLst1 @ stmtLst2
                            
// resolving values
exception ConstResolveFailed of string
 
//  ================================================================
/// Resolves a given Const (cst) with respect to a given env/ctx. 
///
/// If unable to resolve, it just delegates the task to the
/// failResolver function
//  ================================================================
let rec ResolveCont hModel failResolver cst =
  match cst with
  | Unresolved(_) as u -> 
      // see if it is in the env map first
      let envVal = Map.tryFind cst hModel.env
      match envVal with
      | Some(c) -> ResolveCont hModel failResolver c
      | None -> 
          // not found in the env map --> check the equality sets
          let eq = hModel.ctx |> Set.filter (fun eqSet -> Set.contains u eqSet)
                              |> Utils.SetToOption
          match eq with 
          | Some(eqSet) -> 
              let cOpt = eqSet |> Set.filter (function Unresolved(_) -> false | _ -> true)
                               |> Utils.SetToOption
              match cOpt with 
              | Some(c) -> c
              | _ -> failResolver cst hModel
          | None ->
              failResolver cst hModel
//              // finally, see if it's an *input* (have no way of telling input from output params here) method argument
//              let m = hModel.env |> Map.filter (fun k v -> v = u && match k with VarConst(name) -> true | _ -> false) |> Map.toList
//              match m with 
//              | (vc,_) :: [] -> vc
//              | _ -> failResolver cst hModel
  | SeqConst(cseq) -> 
      let resolvedLst = cseq |> List.rev |> List.fold (fun acc c -> ResolveCont hModel failResolver c :: acc) []
      SeqConst(resolvedLst)
  | SetConst(cset) ->
      let resolvedSet = cset |> Set.fold (fun acc c -> acc |> Set.add (ResolveCont hModel failResolver c)) Set.empty
      SetConst(resolvedSet)
  | _ -> cst

//  =====================================================================
/// Tries to resolve a given Const (cst) with respect to a given env/ctx. 
///
/// If unable to resolve, just returns the original Unresolved const.
//  =====================================================================
let TryResolve hModel cst = 
  ResolveCont hModel (fun c _ -> c) cst

//  ==============================================================
/// Resolves a given Const (cst) with respect to a given env/ctx. 
///
/// If unable to resolve, raises a ConstResolveFailed exception
//  ==============================================================
let Resolve hModel cst =
  ResolveCont hModel (fun c _ -> 
                        match c with
                        | Unresolved(id) -> BoxConst(id)
                        | _ -> failwithf "internal error: expected Unresolved but got %O" c
                     ) cst //fun c _ -> raise (ConstResolveFailed("failed to resolve " + c.ToString()))
 
//  ==================================================================
/// Evaluates a given expression with respect to a given heap instance       
//  ==================================================================
let Eval heapInst resolveExprFunc expr = 
  let rec __EvalResolver useConcrete resolveExprFunc expr fldNameOpt = 
    let rec __FurtherResolve expr = 
      match expr with
      | SetExpr(elist)      -> SetExpr(elist |> List.map __FurtherResolve)
      | SequenceExpr(elist) -> SequenceExpr(elist |> List.map __FurtherResolve)
      | VarLiteral(_) ->
          try 
            __EvalResolver useConcrete resolveExprFunc expr None
          with 
          | _ -> expr
      | IdLiteral(id) when not (id = "this" || id = "null") ->
          try 
            __EvalResolver useConcrete resolveExprFunc expr None
          with 
          | _ -> expr
      | _ -> expr

    (* --- function body starts here --- *)
    let ex = match fldNameOpt with
             | None -> expr
             | Some(n) -> Dot(expr, n)
    if not (resolveExprFunc ex) then
      ex
    else
      match fldNameOpt with
      | None -> 
          match expr with
          | ObjLiteral("this") | ObjLiteral("null") -> expr
          | IdLiteral("this")  | IdLiteral("null") -> failwith "should never happen anymore" //TODO
          | VarLiteral(id) -> 
              match heapInst.methodArgs |> Map.tryFind id with
              | Some(argValue) -> argValue |> Const2Expr
              | None -> 
                  match heapInst.methodRetVals |> Map.tryFind id with
                  | Some(e) -> e |> __FurtherResolve
                  | None -> raise (EvalFailed("cannot find value for method parameter " + id))              
          | IdLiteral(id) ->
              let globalVal = heapInst.globals |> Map.tryFind id
              match globalVal with
              | Some(e) -> e
              | None -> __EvalResolver useConcrete resolveExprFunc ThisLiteral (Some(id))      
          | _ -> raise (EvalFailed(sprintf "I'm not supposed to resolve %O" expr))
      | Some(fldName) -> 
          match expr with
          | ObjLiteral(objName) -> 
              let asgs = if useConcrete then heapInst.concreteValues else heapInst.assignments
              let h2 = asgs |> List.filter (function FieldAssignment((o, Var(varName,_)), v) -> o.name = objName && varName = fldName | _ -> false)
              match h2 with
              | FieldAssignment((_,_),x) :: [] -> __FurtherResolve x
              | _ :: _ -> raise (EvalFailed(sprintf "can't evaluate expression deterministically: %s.%s resolves to multiple locations" objName fldName))
              | [] -> raise (EvalFailed(sprintf "can't find value for %s.%s" objName fldName))  // TODO: what if that value doesn't matter for the solution, and that's why it's not present in the model???
          | _ -> Dot(expr, fldName)

  (* --- function body starts here --- *)
  //EvalSym  (__EvalResolver resolveExprFunc) expr
  EvalSymRet  (__EvalResolver false resolveExprFunc)
              (fun expr -> 
                 // TODO: infer type of expr and then re-execute only if its type is Bool
                 let e1 = EvalSym (__EvalResolver true (fun _ -> true)) expr
                 match e1 with
                 | BoolLiteral(b) -> 
                     if b then
                       expr
                     else 
                       FalseLiteral
                 | _ -> expr
              ) expr

//  =====================================================================
/// Takes an unresolved model of the heap (HeapModel), resolves all 
/// references in the model and returns an instance of the heap 
/// (HeapInstance), where all fields for all objects have explicit 
/// assignments.
//  =====================================================================
let ResolveModel hModel meth = 
  let outArgs = GetMethodOutArgs meth
  let hmap = hModel.heap |> Map.fold (fun acc (o,f) l ->
                                        let objName, objTypeOpt = match Resolve hModel o with
                                                                  | ThisConst(_,t) -> "this", t;
                                                                  | NewObj(name, t) -> PrintGenSym name, t
                                                                  | _ -> failwith ("unresolved object ref: " + o.ToString())
                                        let objType = objTypeOpt |> Utils.ExtractOptionMsg "unknown object type"
                                        let obj = {name = objName; objType = objType}
                                        let value = TryResolve hModel l |> Const2Expr
                                        Utils.ListMapAdd (obj, f) value acc 
                                     ) []
                         |> List.map (fun el -> FieldAssignment(el))
  let objs, modObjs = hmap |> List.fold (fun (acc1,acc2) asgn -> 
                                           match asgn with
                                           | FieldAssignment((obj,_),_) -> 
                                               let acc1' = acc1 |> Map.add obj.name obj
                                               let acc2' = 
                                                 if IsModifiableObj obj meth then
                                                   acc2 |> Set.add obj
                                                 else
                                                   acc2
                                               acc1',acc2'
                                           | _ -> acc1,acc2
                                        ) (Map.empty, Set.empty)
  let argmap, retvals = hModel.env |> Map.fold (fun (acc1,acc2) k v -> 
                                                  match k with
                                                  | VarConst(name) -> 
                                                      let resolvedValExpr = Resolve hModel v
                                                      if outArgs |> List.exists (function Var(varName, _) -> varName = name) then
                                                        acc1, acc2 |> Map.add name (resolvedValExpr |> Const2Expr)
                                                      else
                                                        acc1 |> Map.add name resolvedValExpr, acc2
                                                  | _ -> acc1, acc2
                                               ) (Map.empty, Map.empty)
  { objs           = objs;
    modifiableObjs = modObjs;
    assignments    = hmap; 
    concreteValues = hmap;
    methodArgs     = argmap; 
    methodRetVals  = retvals;
    globals        = Map.empty }

let rec GetCallGraph solutions graph = 
  let rec __SearchExprsForMethodCalls elist acc = 
    match elist with
    | e :: rest ->
        match e with 
        // no need to descend for, just check if the top-level one is MEthodCall
        | MethodCall(_,cname,mname,_) -> __SearchExprsForMethodCalls rest (acc |> Set.add (cname,mname))
        | _ -> __SearchExprsForMethodCalls rest acc 
    | [] -> acc
  match solutions with
  | ((comp,m), sol) :: rest -> 
        let callees = sol |> List.fold (fun acc (cond, hInst) ->
                                          hInst.assignments |> List.fold (fun acc asgn -> 
                                                                            match asgn with
                                                                            | FieldAssignment(_,e) ->
                                                                                __SearchExprsForMethodCalls [e] acc
                                                                            | ArbitraryStatement(stmt) -> 
                                                                                let exprs = ExtractTopLevelExpressions stmt
                                                                                __SearchExprsForMethodCalls exprs acc
                                                                         ) acc
                                       ) Set.empty
        let graph' = graph |> Map.add (comp,m) callees
        GetCallGraph rest graph'
  | [] -> graph

//////////////////////////////

let Is1stLevelExpr heapInst expr = 
  DescendExpr2 (fun expr acc ->
                  if not acc then
                    false
                  else
                    match expr with
                    | Dot(discr, fldName) -> 
                        let obj = Eval heapInst (fun _ -> true) discr
                        match obj with 
                        | ObjLiteral(id) -> id = "this"
                        | _ -> failwithf "Didn't expect the discriminator of a Dot to not be ObjLiteral"
                    | _ -> true                          
               ) expr true

let IsSolution1stLevelOnly heapInst = 
  let rec __IsSol1stLevel stmts = 
    match stmts with
    | stmt :: rest -> 
        match stmt with
        | Assign(_, e)
        | ExprStmt(e) -> 
            let ok = Is1stLevelExpr heapInst e
            ok && __IsSol1stLevel rest
        | Block(stmts) -> __IsSol1stLevel (stmts @ rest)
    | [] -> true
  (* --- function body starts here --- *)
  __IsSol1stLevel (ConvertToStatements heapInst true)