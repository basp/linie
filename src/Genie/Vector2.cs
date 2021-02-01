namespace Genie
{
    using System;

    public struct Vector2<T>
        where T : IComparable<T>
    {
        public readonly T X, Y;

        public Vector2(T x, T y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Vector2<T> operator -(in Vector2<T> u) =>
            new Vector2<T>(
                Operations.Negate(u.X),
                Operations.Negate(u.Y));

        public static Vector2<T> operator +(in Vector2<T> u, in Vector2<T> v) =>
            new Vector2<T>(
                Operations.Add(u.X, v.X),
                Operations.Add(u.Y, v.Y));

        public static Vector2<T> operator -(in Vector2<T> u, in Vector2<T> v) =>
            new Vector2<T>(
                Operations.Subtract(u.X, v.X),
                Operations.Subtract(u.Y, v.Y));

        public static Vector2<T> operator *(in Vector2<T> u, T s) =>
            new Vector2<T>(
                Operations.Multiply(u.X, s),
                Operations.Multiply(u.Y, s));

        public static Vector2<T> operator /(in Vector2<T> u, T s) =>
            new Vector2<T>(
                Operations.Divide(u.X, s),
                Operations.Divide(u.Y, s));

        public static T Dot(in Vector2<T> u, in Vector2<T> v) =>
            Operations.Add(
                Operations.Multiply(u.X, v.X),
                Operations.Multiply(u.Y, v.Y));

        public static T MagnitudeSquared(in Vector2<T> u) =>
            Operations.Add(
                Operations.Multiply(u.X, u.X),
                Operations.Multiply(u.Y, u.Y));

        public static T Magnitude(in Vector2<T> u) =>
            Operations.Sqrt(MagnitudeSquared(u));
    }

    public static class Vector2
    {
        public static Vector2<T> Create<T>(T x, T y)
            where T : IComparable<T> =>
            new Vector2<T>(x, y);

        public static T Dot<T>(Vector2<T> u, Vector2<T> v)
            where T : IComparable<T> =>
            Vector2<T>.Dot(u, v);

        public static T MagnitudeSquared<T>(Vector2<T> u)
            where T : IComparable<T> =>
            Vector2<T>.MagnitudeSquared(u);

        public static T Magnitude<T>(Vector2<T> u)
            where T : IComparable<T> =>
            Vector2<T>.Magnitude(u);
    }
}
