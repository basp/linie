namespace Genie
{
    using System;

    
    /// <summary>
    /// Represents an x- and y-coordinate in 2D space.
    /// </summary>
    public struct Point2<T>
        where T : IComparable<T>
    {
        public readonly T X, Y;

        /// <summary>
        /// Initializes a new instance of the <see cref="Point2"/> structure.
        /// </summary>
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