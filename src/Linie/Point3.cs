// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an x-, y-, and z-coordinate in 3D space.
    /// </summary>
    public record Point3(double X, double Y, double Z)
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point3"/> structure.
        /// </summary>
        public Point3(double a) : this(a, a, a)
        {
        }

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

        /// <summary>
        /// Creates a <see cref="String"/> representation of 
        /// this <see cref="Point3"/> structure.
        /// </summary>
        public override string ToString() => $"({this.X}, {this.Y}, {this.Z})";
    }
}