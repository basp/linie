namespace Genie
{
    using System;

    public struct Normal3<T>
        where T : IComparable<T>
    {
        public readonly T X, Y, Z;

        public Normal3(T x, T y, T z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Normal3<T> operator -(Normal3<T> n) =>
            new Normal3<T>(
                Operations.Negate(n.X),
                Operations.Negate(n.Y),
                Operations.Negate(n.Z));

        public static Normal3<T> operator +(Normal3<T> n, Normal3<T> m) =>
            new Normal3<T>(
                Operations.Add(n.X, m.X),
                Operations.Add(n.Y, m.Y),
                Operations.Add(n.Z, m.Z));

        public static Normal3<T> operator -(Normal3<T> n, Normal3<T> m) =>
            new Normal3<T>(
                Operations.Subtract(n.X, m.X),
                Operations.Subtract(n.Y, m.Y),
                Operations.Subtract(n.Z, m.Z));

        public static Normal3<T> operator *(Normal3<T> n, T s) =>
            new Normal3<T>(
                Operations.Multiply(n.X, s),
                Operations.Multiply(n.Y, s),
                Operations.Multiply(n.Z, s));

        public static Normal3<T> operator /(Normal3<T> n, T s) =>
            new Normal3<T>(
                Operations.Divide(n.X, s),
                Operations.Divide(n.Y, s),
                Operations.Divide(n.Z, s));

        public static explicit operator Normal3<T>(Vector3<T> v) =>
            new Normal3<T>(v.X, v.Y, v.Z);

        public static explicit operator Vector3<T>(Normal3<T> n) =>
            new Vector3<T>(n.X, n.Y, n.Z);
    }

    public static class Normal3
    {
        public static Normal3<T> Create<T>(T x, T y, T z)
            where T : IComparable<T> =>
            new Normal3<T>(x, y, z);

        public static T Dot<T>(Normal3<T> n, Normal3<T> m)
            where T : IComparable<T> =>
            Operations.Add(
                Operations.Multiply(n.X, m.X),
                Operations.Add(
                    Operations.Multiply(n.Y, m.Y),
                    Operations.Multiply(n.Z, m.Z)));

        public static T MagnitudeSquared<T>(Normal3<T> n)
            where T : IComparable<T> =>
            Operations.Add(
                Operations.Multiply(n.X, n.X),
                Operations.Add(
                    Operations.Multiply(n.Y, n.Y),
                    Operations.Multiply(n.Z, n.Z)));

        public static T Magnitude<T>(Normal3<T> n)
            where T : IComparable<T> =>
            Operations.Sqrt(MagnitudeSquared(n));
    }
}