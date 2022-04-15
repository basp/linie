// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an x- and y-coordinate in 2D space.
    /// </summary>
    public record Point2(double X, double Y)
    {
        /// <summary>
        /// Gets the element at specified index.
        /// </summary>
        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return this.X;
                    case 1: return this.Y;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public static Point2 operator +(in Point2 a, in Vector2 u) =>
            new Point2(a.X + u.X, a.Y + u.Y);

        public static Point2 operator -(in Point2 a, in Vector2 u) =>
            new Point2(a.X - u.X, a.Y - u.Y);

        public static Vector2 operator -(in Point2 a, in Point2 b) =>
            new Vector2(a.X - b.X, a.Y - b.Y);

        public static Point2 operator *(in double c, in Point2 a) =>
            new Point2(c * a.X, c * a.Y);

        public static Point2 operator *(in Point2 a, in double c) => c * a;

        public static explicit operator Vector2(in Point2 a) =>
            new Vector2(a.X, a.Y);

        public static IEqualityComparer<Point2> GetEqualityComparer(
            in double epsilon = 0) =>
                new Point2EqualityComparer(epsilon);

        /// <summary>
        /// Creates a <see cref="String"/> representation of 
        /// this <see cref="Point2"/> structure.
        /// </summary>
        public override string ToString() => $"({this.X}, {this.Y})";
    }
}