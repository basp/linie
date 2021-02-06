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

        public static Matrix4x4<T> operator *(Matrix4x4<T> a, Matrix4x4<T> b)
        {
            var m = new Matrix4x4<T>(default(T));
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    m[i, j] =
                        Operations.Add(
                            Operations.Add(
                                Operations.Multiply(a[i, 0], b[0, j]),
                                Operations.Multiply(a[i, 1], b[1, j])),
                            Operations.Add(
                                Operations.Multiply(a[i, 2], b[2, j]),
                                Operations.Multiply(a[i, 3], b[3, j])));
                }
            }

            return m;
        }

        public static Vector4<T> operator *(Matrix4x4<T> m, Vector4<T> u)
        {
            var x =
                Operations.Add(
                    Operations.Add(
                        Operations.Multiply(m[0, 0], u.X),
                        Operations.Multiply(m[0, 1], u.Y)),
                    Operations.Add(
                        Operations.Multiply(m[0, 2], u.Z),
                        Operations.Multiply(m[0, 3], u.W)));

            var y =
                Operations.Add(
                    Operations.Add(
                        Operations.Multiply(m[1, 0], u.X),
                        Operations.Multiply(m[1, 1], u.Y)),
                    Operations.Add(
                        Operations.Multiply(m[1, 2], u.Z),
                        Operations.Multiply(m[1, 3], u.W)));

            var z =
                Operations.Add(
                    Operations.Add(
                        Operations.Multiply(m[2, 0], u.X),
                        Operations.Multiply(m[2, 1], u.Y)),
                    Operations.Add(
                        Operations.Multiply(m[2, 2], u.Z),
                        Operations.Multiply(m[2, 3], u.W)));
            var w =
                Operations.Add(
                    Operations.Add(
                        Operations.Multiply(m[3, 0], u.X),
                        Operations.Multiply(m[3, 1], u.Y)),
                    Operations.Add(
                        Operations.Multiply(m[3, 2], u.Z),
                        Operations.Multiply(m[3, 3], u.W)));

            return Vector4.Create(x, y, z, w);
        }

        public static Vector3<T> operator *(Matrix4x4<T> m, Vector3<T> u)
        {
            var x =
                Operations.Add(
                    Operations.Add(
                        Operations.Multiply(m[0, 0], u.X),
                        Operations.Multiply(m[0, 1], u.Y)),
                    Operations.Multiply(m[0, 2], u.Z));

            var y =
                Operations.Add(
                    Operations.Add(
                        Operations.Multiply(m[1, 0], u.X),
                        Operations.Multiply(m[1, 1], u.Y)),
                    Operations.Multiply(m[1, 2], u.Z));

            var z =
                Operations.Add(
                    Operations.Add(
                        Operations.Multiply(m[2, 0], u.X),
                        Operations.Multiply(m[2, 1], u.Y)),
                    Operations.Multiply(m[2, 2], u.Z));

            return Vector3.Create(x, y, z);
        }

        public static Normal3<T> operator *(Matrix4x4<T> m, Normal3<T> u)
        {
            var x =
                Operations.Add(
                    Operations.Add(
                        Operations.Multiply(m[0, 0], u.X),
                        Operations.Multiply(m[1, 0], u.Y)),
                    Operations.Multiply(m[2, 0], u.Z));

            var y =
                Operations.Add(
                    Operations.Add(
                        Operations.Multiply(m[0, 1], u.X),
                        Operations.Multiply(m[1, 1], u.Y)),
                    Operations.Multiply(m[2, 1], u.Z));

            var z =
                Operations.Add(
                    Operations.Add(
                        Operations.Multiply(m[0, 2], u.X),
                        Operations.Multiply(m[1, 2], u.Y)),
                    Operations.Multiply(m[2, 2], u.Z));

            return Normal3.Create(x, y, z);
        }

        public static Point3<T> operator *(Matrix4x4<T> m, Point3<T> a)
        {
            var x =
                Operations.Add(
                    Operations.Add(
                        Operations.Multiply(m[0, 0], a.X),
                        Operations.Multiply(m[0, 1], a.Y)),
                    Operations.Add(
                        Operations.Multiply(m[0, 2], a.Z),
                        m[0, 3]));

            var y =
                Operations.Add(
                    Operations.Add(
                        Operations.Multiply(m[1, 0], a.X),
                        Operations.Multiply(m[1, 1], a.Y)),
                    Operations.Add(
                        Operations.Multiply(m[1, 2], a.Z),
                        m[1, 3]));

            var z =
                Operations.Add(
                    Operations.Add(
                        Operations.Multiply(m[2, 0], a.X),
                        Operations.Multiply(m[2, 1], a.Y)),
                    Operations.Add(
                        Operations.Multiply(m[2, 2], a.Z),
                        m[2, 3]));

            return Point3.Create(x, y, z);
        }
    }

    public static class Matrix4x4Extensions
    {
        public static Matrix4x4<T> Transpose<T>(this Matrix4x4<T> a)
            where T : IComparable<T>
        {
            var m = new Matrix4x4<T>(default(T));
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    m[i, j] = a[j, i];
                }
            }

            return m;
        }

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