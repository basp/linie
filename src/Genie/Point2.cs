namespace Genie
{
    using System;

    public struct Point2<T>
        where T : IComparable<T>
    {
        public readonly T X, Y;

        public Point2(T x, T y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Vector2<T> operator -(Point2<T> a, Point2<T> b) =>
            new Vector2<T>(
                Operations.Subtract(a.X, b.X),
                Operations.Subtract(a.Y, b.Y));

        public static Point2<T> operator +(Point2<T> a, Vector2<T> u) =>
            new Point2<T>(
                Operations.Add(a.X, u.X),
                Operations.Add(a.Y, u.Y));
    }

    public static class Point2
    {
        public static Point2<T> Create<T>(T x, T y)
            where T : IComparable<T> => new Point2<T>(x, y);
    }
}