using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Boogie
{
    public static class YieldCheckerImplementation
    {
        public static List<Declaration> CreateYieldCheckerProcImpl(
            CivlTypeChecker civlTypeChecker,
            LinearTypeChecker linearTypeChecker,
            int layerNum,
            Dictionary<Absy, Absy> absyMap,
            Implementation impl,
            Dictionary<YieldCmd, List<PredicateCmd>> yields)
        {
            Dictionary<string, Variable> domainNameToHoleVar = new Dictionary<string, Variable>();
            Dictionary<Variable, Variable> localVarMap = new Dictionary<Variable, Variable>();
            Dictionary<Variable, Expr> map = new Dictionary<Variable, Expr>();
            List<Variable> locals = new List<Variable>();
            List<Variable> inputs = new List<Variable>();

            foreach (var domainName in linearTypeChecker.linearDomains.Keys)
            {
                var inParam = linearTypeChecker.LinearDomainInFormal(domainName);
                inputs.Add(inParam);
                domainNameToHoleVar[domainName] = inParam;
            }

            foreach (Variable local in impl.LocVars.Union(impl.InParams).Union(impl.OutParams))
            {
                var copy = CopyLocal(local);
                locals.Add(copy);
                localVarMap[local] = copy;
                map[local] = Expr.Ident(copy);
            }

            Dictionary<Variable, Expr> oldLocalMap = new Dictionary<Variable, Expr>();
            Dictionary<Variable, Expr> assumeMap = new Dictionary<Variable, Expr>(map);
            foreach (Variable g in civlTypeChecker.sharedVariables)
            {
                var copy = OldLocalLocal(g);
                locals.Add(copy);
                oldLocalMap[g] = Expr.Ident(copy);
                Formal f = OldGlobalFormal(g);
                inputs.Add(f);
                assumeMap[g] = Expr.Ident(f);
            }

            var linearHelper = new LinearHelper(civlTypeChecker, linearTypeChecker, layerNum, absyMap, domainNameToHoleVar, localVarMap);
            Substitution assumeSubst = Substituter.SubstitutionFromHashtable(assumeMap);
            Substitution oldSubst = Substituter.SubstitutionFromHashtable(oldLocalMap);
            Substitution subst = Substituter.SubstitutionFromHashtable(map);
            List<Block> yieldCheckerBlocks = new List<Block>();
            List<String> labels = new List<String>();
            List<Block> labelTargets = new List<Block>();
            Block yieldCheckerBlock = new Block(Token.NoToken, "exit", new List<Cmd>(), new ReturnCmd(Token.NoToken));
            labels.Add(yieldCheckerBlock.Label);
            labelTargets.Add(yieldCheckerBlock);
            yieldCheckerBlocks.Add(yieldCheckerBlock);
            int yieldCount = 0;
            foreach (var kv in yields)
            {
                var yieldCmd = kv.Key;
                var yieldPredicates = kv.Value;
                List<Cmd> newCmds = linearHelper.DisjointnessAssumeCmds(yieldCmd,false);
                foreach (var predCmd in yieldPredicates)
                {
                    var newExpr = Substituter.ApplyReplacingOldExprs(assumeSubst, oldSubst, predCmd.Expr);
                    newCmds.Add(new AssumeCmd(Token.NoToken, newExpr));
                }

                foreach (var predCmd in yieldPredicates)
                {
                    if (predCmd is AssertCmd)
                    {
                        var newExpr = Substituter.ApplyReplacingOldExprs(subst, oldSubst, predCmd.Expr);
                        AssertCmd assertCmd = new AssertCmd(predCmd.tok, newExpr, predCmd.Attributes);
                        assertCmd.ErrorData = "Non-interference check failed";
                        newCmds.Add(assertCmd);
                    }
                }

                newCmds.Add(new AssumeCmd(Token.NoToken, Expr.False));
                yieldCheckerBlock = new Block(Token.NoToken, "L" + yieldCount++, newCmds, new ReturnCmd(Token.NoToken));
                labels.Add(yieldCheckerBlock.Label);
                labelTargets.Add(yieldCheckerBlock);
                yieldCheckerBlocks.Add(yieldCheckerBlock);
            }

            yieldCheckerBlocks.Insert(0,
                new Block(Token.NoToken, "enter", new List<Cmd>(), new GotoCmd(Token.NoToken, labels, labelTargets)));

            // Create the yield checker procedure
            var yieldCheckerName = $"Impl_YieldChecker_{impl.Name}";
            var yieldCheckerProc = new Procedure(Token.NoToken, yieldCheckerName, impl.TypeParameters, inputs,
                new List<Variable>(), new List<Requires>(), new List<IdentifierExpr>(), new List<Ensures>());
            CivlUtil.AddInlineAttribute(yieldCheckerProc);

            // Create the yield checker implementation
            var yieldCheckerImpl = new Implementation(Token.NoToken, yieldCheckerName, impl.TypeParameters, inputs,
                new List<Variable>(), locals, yieldCheckerBlocks);
            yieldCheckerImpl.Proc = yieldCheckerProc;
            CivlUtil.AddInlineAttribute(yieldCheckerImpl);
            return new List<Declaration> {yieldCheckerProc, yieldCheckerImpl};
        }

        private static LocalVariable CopyLocal(Variable v)
        {
            return new LocalVariable(Token.NoToken, new TypedIdent(Token.NoToken, v.Name, v.TypedIdent.Type));
        }

        private static Formal OldGlobalFormal(Variable v)
        {
            return new Formal(Token.NoToken,
                new TypedIdent(Token.NoToken, $"og_global_old_{v.Name}", v.TypedIdent.Type), true);
        }

        private static LocalVariable OldLocalLocal(Variable v)
        {
            return new LocalVariable(Token.NoToken,
                new TypedIdent(Token.NoToken, $"og_local_old_{v.Name}", v.TypedIdent.Type));
        }
    }
}