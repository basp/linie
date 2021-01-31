// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie
{
    using System;
    using System.Collections.Generic;

    public struct Point3 : IEquatable<Point3>
    {
        public readonly double X, Y, Z;

        public Point3(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Point3(double a) : this(a, a, a)
        {
        }

        public double this[int i]
        {
            get
            {
                switch(i)
                {
                    case 0: return this.X;
                    case 1: return this.Y;
                    case 2: return this.Z;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public static Point3 operator +(in Point3 a, in Vector3 u) =>
            new Point3(a.X + u.X, a.Y + u.Y, a.Z + u.Z);

        public static Point3 operator -(in Point3 a, in Vector3 u) =>
            new Point3(a.X - u.X, a.Y - u.Y, a.Z - u.Z);

        public static Vector3 operator -(in Point3 a, in Point3 b) =>
            new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Point3 operator *(in double c, in Point3 a) =>
            new Point3(c * a.X, c * a.Y, c * a.Z);

        public static Point3 operator *(in Point3 a, in double c) => c * a;

        public static explicit operator Vector4(in Point3 a) =>
            Vector4.CreatePosition(a.X, a.Y, a.Z);

        public static IEqualityComparer<Point3> GetEqualityComparer(in double epsilon = 0) =>
            new Point3EqualityComparer(epsilon);

        public override string ToString() => $"({this.X}, {this.Y}, {this.Z})";

        public bool Equals(Point3 other) =>
            this.X == other.X &&
            this.Y == other.Y &&
            this.Z == other.Z;
    }
}