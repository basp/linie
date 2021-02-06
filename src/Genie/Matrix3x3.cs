namespace Genie
{
    using System;
    using System.Linq;

    public class Matrix3x3<T>
        where T : IComparable<T>
    {
        private readonly T[] data;

        public Matrix3x3(T v)
        {
            this.data = new[]
            {
                v, v, v,
                v, v, v,
                v, v, v,
            };
        }

        public Matrix3x3(
            T m00, T m01, T m02,
            T m10, T m11, T m12,
            T m20, T m21, T m22)
        {
            this.data = new[]
            {
                m00, m01, m02,
                m10, m11, m12,
                m20, m21, m22,
            };
        }

        public T this[int row, int col]
        {
            get => this.data[(row * 3) + col];
            set => this.data[(row * 3) + col] = value;
        }
    }

    public static class Matrix3x3Extensions
    {
        internal static Matrix2x2<T> Submatrix<T>(
            this Matrix3x3<T> a,
            int dropRow,
            int dropCol) where T : IComparable<T>
        {
            var rows = Enumerable.Range(0, 3)
                .Where(x => x != dropRow)
                .ToArray();

            var cols = Enumerable.Range(0, 3)
                .Where(x => x != dropCol)
                .ToArray();

            var m = new Matrix2x2<T>(default(T));
            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    m[i, j] = a[rows[i], cols[j]];
                }
            }

            return m;
        }

        internal static T Minor<T>(this Matrix3x3<T> a, int row, int col)
            where T : IComparable<T> =>
            a.Submatrix(row, col).Determinant();

        internal static T Cofactor<T>(this Matrix3x3<T> a, int row, int col)
            where T : IComparable<T> =>
            (row + col) % 2 == 0
                ? a.Minor(row, col)
                : Operations.Negate(a.Minor(row, col));

        internal static T Determinant<T>(this Matrix3x3<T> a)
            where T : IComparable<T> =>
            Operations.Add(
                Operations.Add(
                    Operations.Multiply(a[0, 0], a.Cofactor(0, 0)),
                    Operations.Multiply(a[0, 1], a.Cofactor(0, 1))),
                Operations.Multiply(a[0, 2], a.Cofactor(0, 2)));
    }
}