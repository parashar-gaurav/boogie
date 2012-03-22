class {:autocontracts} RingBuffer<T>
{
  // public view of the class:
  ghost var Contents: seq<T>;  // the contents of the ring buffer
  ghost var N: nat;  // the capacity of the ring buffer

  // private implementation:
  var data: array<T>;
  var first: nat;
  var len: nat;

  // Valid encodes the consistency of RingBuffer objects (think, invariant)
  predicate Valid
  {
    data != null &&
    data.Length == N &&
    (N == 0 ==> len == first == 0 && Contents == []) &&
    (N != 0 ==> len <= N && first < N) &&
    Contents == if first + len <= N then data[first..first+len] 
                                    else data[first..] + data[..first+len-N]
  }

  constructor Create(n: nat)
    ensures Contents == [] && N == n;
  {
    data := new T[n];
    first, len := 0, 0;
    Contents, N := [], n;
  }

  method Clear()
    ensures Contents == [] && N == old(N);
  {
    len := 0;
    Contents := [];
  }

  method Head() returns (x: T)
    requires Contents != [];
    ensures x == Contents[0];
  {
    x := data[first];
  }

  method Enqueue(x: T)
    requires |Contents| != N;
    ensures Contents == old(Contents) + [x] && N == old(N);
  {
    var nextEmpty := if first + len < data.Length 
                     then first + len else first + len - data.Length;
    data[nextEmpty] := x;
    len := len + 1;
    Contents := Contents + [x];
  }

  method Dequeue() returns (x: T)
    requires Contents != [];
    ensures x == old(Contents)[0] && Contents == old(Contents)[1..] && N == old(N);
  {
    x := data[first];
    first, len := if first + 1 == data.Length then 0 else first + 1, len - 1;
    Contents := Contents[1..];
  }
}

method TestHarness(x: int, y: int, z: int)
{
  var b := new RingBuffer.Create(2);
  b.Enqueue(x);
  b.Enqueue(y);
  var h := b.Dequeue();  assert h == x;
  b.Enqueue(z);
  h := b.Dequeue();  assert h == y;
  h := b.Dequeue();  assert h == z;
}