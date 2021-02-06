namespace Genie
{
    using System;
    using System.Linq;

    public class Matrix4x4<T>
        where T : IComparable<T>
    {
        private readonly T[] data;

        public Matrix4x4(T v)
        {
            this.data = new[]
            {
                v, v, v, v,
                v, v, v, v,
                v, v, v, v,
                v, v, v, v,
            };
        }

        public Matrix4x4(
            T m00, T m01, T m02, T m03,
            T m10, T m11, T m12, T m13,
            T m20, T m21, T m22, T m23,
            T m30, T m31, T m32, T m33)
        {
            this.data = new[]
            {
                m00, m01, m02, m03,
                m10, m11, m12, m13,
                m20, m21, m22, m23,
                m30, m31, m32, m33,
            };
        }

        public T this[int row, int col]
        {
            get => this.data[(row * 4) + col];
            set => this.data[(row * 4) + col] = value;
        }
    }

    public static class Matrix4x4Extensions
    {
        public static Matrix4x4<T> Inverse<T>(this Matrix4x4<T> a)
            where T : IComparable<T>
        {
            var m = new Matrix4x4<T>(default(T));
            var d = a.Determinant();
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    m[j, i] = Operations.Divide(a.Cofactor(i, j), d);
                }
            }

            return m;
        }

        public static T Determinant<T>(this Matrix4x4<T> a)
            where T : IComparable<T> =>
            Operations.Add(
                Operations.Add(
                    Operations.Multiply(a[0, 0], a.Cofactor(0, 0)),
                    Operations.Multiply(a[0, 1], a.Cofactor(0, 1))),
                Operations.Add(
                    Operations.Multiply(a[0, 2], a.Cofactor(0, 2)),
                    Operations.Multiply(a[0, 3], a.Cofactor(0, 3))));

        internal static T Cofactor<T>(this Matrix4x4<T> a, int row, int col)
            where T : IComparable<T> =>
            (row + col) % 2 == 0
                ? a.Minor(row, col)
                : Operations.Negate(a.Minor(row, col));

        internal static T Minor<T>(this Matrix4x4<T> a, int row, int col)
            where T : IComparable<T> =>
            a.Submatrix(row, col).Determinant();

        internal static Matrix3x3<T> Submatrix<T>(
            this Matrix4x4<T> a,
            int dropRow,
            int dropCol)
            where T : IComparable<T>
        {
            var rows = Enumerable.Range(0, 4)
                .Where(x => x != dropRow)
                .ToArray();

            var cols = Enumerable.Range(0, 4)
                .Where(x => x != dropCol)
                .ToArray();

            var m = new Matrix3x3<T>(default(T));
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    m[i, j] = a[rows[i], cols[j]];
                }
            }

            return m;
        }
    }
}