namespace Genie
{
    using System;

    public class Matrix2x2<T>
        where T : IComparable<T>
    {
        private readonly T[] data;

        public Matrix2x2(T v)
        {
            this.data = new[]
            {
                v, v,
                v, v,
            };
        }

        public Matrix2x2(T m00, T m01, T m10, T m11)
        {
            this.data = new[]
            {
                m00, m01,
                m10, m11,
            };
        }

        public T this[int row, int col]
        {
            get => this.data[(row * 2) + col];
            set => this.data[(row * 2) + col] = value;
        }
    }

    public static class Matrix2x2Extensions
    {
        internal static T Determinant<T>(this Matrix2x2<T> m)
            where T : IComparable<T> =>
            Operations.Subtract(
                Operations.Multiply(m[0, 0], m[1, 1]),
                Operations.Multiply(m[0, 1], m[1, 0]));
    }
}