namespace Linie.Generic
{
    public interface IOperations<T>
    {
        T Negate(T a);

        T Add(T a, T b);

        T Subtract(T a, T b);

        T Multiply(T a, T b);

        T Sqrt(T a);

        T Pow(T a, T b);

        int Compare(T a, T b);
    }
}