namespace Genie
{
    using System;

    public struct Vector3<T>
        where T : IComparable<T>
    {
        public readonly T X, Y, Z;

        public Vector3(T x, T y, T z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Vector3<T> operator -(Vector3<T> u) =>
            new Vector3<T>(
                Operations.Negate(u.X),
                Operations.Negate(u.Y),
                Operations.Negate(u.Z));

        public static Vector3<T> operator +(Vector3<T> u, Vector3<T> v) =>
            new Vector3<T>(
                Operations.Add(u.X, v.X),
                Operations.Add(u.Y, v.Y),
                Operations.Add(u.Z, v.Z));

        public static Vector3<T> operator -(Vector3<T> u, Vector3<T> v) =>
            new Vector3<T>(
                Operations.Subtract(u.X, v.X),
                Operations.Subtract(u.Y, v.Y),
                Operations.Subtract(u.Z, v.Z));

        public static Vector3<T> operator *(Vector3<T> u, T s) =>
            new Vector3<T>(
                Operations.Multiply(u.X, s),
                Operations.Multiply(u.Y, s),
                Operations.Multiply(u.Z, s));

        public static Vector3<T> operator *(T s, Vector3<T> u) => u * s;

        public static Vector3<T> operator /(Vector3<T> u, T s) =>
            new Vector3<T>(
                Operations.Divide(u.X, s),
                Operations.Divide(u.Y, s),
                Operations.Divide(u.Z, s));

        public static T Dot(Vector3<T> u, Vector3<T> v) =>
            Operations.Add(
                Operations.Multiply(u.X, v.X),
                Operations.Add(
                    Operations.Multiply(u.Y, v.Y),
                    Operations.Multiply(u.Z, v.Z)));

        public static T MagnitudeSquared(Vector3<T> u) =>
            Operations.Add(
                Operations.Multiply(u.X, u.X),
                Operations.Add(
                    Operations.Multiply(u.Y, u.Y),
                    Operations.Multiply(u.Z, u.Z)));

        public static T Magnitude(Vector3<T> u) =>
            Operations.Sqrt(MagnitudeSquared(u));
    }

    public static class Vector3
    {
        public static Vector3<T> Create<T>(T x, T y, T z)
            where T : IComparable<T> =>
            new Vector3<T>(x, y, z);

        public static T Dot<T>(Vector3<T> u, Vector3<T> v)
            where T : IComparable<T> =>
            Operations.Add(
                Operations.Multiply(u.X, v.X),
                Operations.Add(
                    Operations.Multiply(u.Y, v.Y),
                    Operations.Multiply(u.Z, v.Z)));

        public static Vector3<T> Cross<T>(Vector3<T> u, Vector3<T> v)
            where T : IComparable<T> =>
            new Vector3<T>(
                Operations.Subtract(
                    Operations.Multiply(u.Y, v.Z),
                    Operations.Multiply(u.Z, v.Y)),
                Operations.Subtract(
                    Operations.Multiply(u.Z, v.X),
                    Operations.Multiply(u.X, v.Z)),
                Operations.Subtract(
                    Operations.Multiply(u.X, v.Y),
                    Operations.Multiply(u.Y, v.X)));

        public static T MagnitudeSquared<T>(Vector3<T> u)
            where T : IComparable<T> =>
            Operations.Add(
                Operations.Multiply(u.X, u.X),
                Operations.Add(
                    Operations.Multiply(u.Y, u.Y),
                    Operations.Multiply(u.Z, u.Z)));

        public static T Magnitude<T>(Vector3<T> u)
            where T : IComparable<T> =>
            Operations.Sqrt(MagnitudeSquared(u));
    }
}
