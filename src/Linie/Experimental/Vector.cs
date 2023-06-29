namespace Linie.Experimental;

using System.Collections;
using System.Numerics;

public class Vector<T> : IEnumerable<T>
    where T : INumber<T>
{
    private readonly T[] storage;

    public Vector(params T[] values)
    {
        this.storage = values;
    }

    public Vector(int length)
    {
        this.storage = new T[length];
    }

    public T this[int index]
    {
        get => this.storage[index];
        set => this.storage[index] = value;
    }

    public static Vector<T> operator +(Vector<T> u, Vector<T> w)
    {
        var xs = u.Zip(w)
            .Select(p => p.First + p.Second)
            .ToArray();
 
        return new Vector<T>(xs);
    }

    public static Vector<T> operator -(Vector<T> u, Vector<T> w)
    {
        var xs = u.Zip(w)
            .Select(p => p.First - p.Second)
            .ToArray();

        return new Vector<T>(xs);
    }

    public IEnumerator<T> GetEnumerator() =>
        new VectorEnumerator(this);

    IEnumerator IEnumerable.GetEnumerator() =>
        this.GetEnumerator();

    internal class VectorEnumerator : IEnumerator<T>
    {
        private readonly Vector<T> v;

        private int index = -1;

        public VectorEnumerator(Vector<T> v)
        {
            this.v = v;
        }

        public T Current => this.v[this.index];

        object IEnumerator.Current => this.Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            this.index += 1;
            return this.index < this.v.storage.Length;
        }

        public void Reset()
        {
            this.index = 0;
        }
    }
}