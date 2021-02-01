namespace Genie
{
    using System;

    public struct Vector2<T>
        where T : IComparable, IComparable<T>
    {
        public readonly T X, Y;

        public Vector2(T x, T y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Vector2<T> operator +(Vector2<T> u, Vector2<T> v) =>
            new Vector2<T>(
                Operations.Add(u.X, v.X),
                Operations.Add(u.Y, v.Y));

        public static Vector2<T> operator -(Vector2<T> u, Vector2<T> v) =>
            new Vector2<T>(
                Operations.Subtract(u.X, v.X),
                Operations.Subtract(u.Y, v.Y));

        public static Vector2<T> operator *(Vector2<T> u, T s) =>
            new Vector2<T>(
                Operations.Multiply(u.X, s),
                Operations.Multiply(u.Y, s));

        public static Vector2<T> operator /(Vector2<T> u, T s) =>
            new Vector2<T>(
                Operations.Divide(u.X, s),
                Operations.Divide(u.Y, s));
    }

    public static class Vector2
    {
        public static Vector2<T> Create<T>(T x, T y)
            where T : IComparable, IComparable<T> =>
            new Vector2<T>(x, y);
    }
}
