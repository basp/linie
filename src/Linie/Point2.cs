// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie
{
    using System;
    using System.Collections.Generic;

    public struct Point2 : IEquatable<Point2>
    {
        public readonly double X, Y;

        public Point2(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point2(double a) : this(a, a)
        {
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

        public static IEqualityComparer<Point2> GetEqualityComparer(in double epsilon = 0) =>
            new Point2EqualityComparer(epsilon);

        public override string ToString() => $"({this.X}, {this.Y})";

        public bool Equals(Point2 other) =>
            this.X == other.X &&
            this.Y == other.Y;
    }
}