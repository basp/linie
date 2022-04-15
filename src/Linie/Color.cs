// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a RGB (red, green, blue) color.
    /// </summary>
    public record Color(double R, double G, double B)
    {
        public static Color White => new Color(1, 1, 1);

        public static Color Black => new Color(0, 0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> structure.
        /// </summary>
        public Color(double a) : this(a, a, a)
        {
        }

        public static Color operator +(Color a, Color b) =>
            new Color(
                a.R + b.R,
                a.G + b.G,
                a.B + b.B);

        public static Color operator -(Color a, Color b) =>
            new Color(
                a.R - b.R,
                a.G - b.G,
                a.B - b.B);

        public static Color operator *(Color a, double s) =>
            new Color(
                a.R * s,
                a.G * s,
                a.B * s);

        public static Color operator *(double s, Color a) => a * s;

        public static Color operator /(Color a, double s) => a * (1 / s);

        /// <summary>
        /// Hadamard (entrywise) product of two color instances.
        /// </summary>
        public static Color operator *(Color c1, Color c2) =>
            new Color(
                c1.R * c2.R,
                c1.G * c2.G,
                c1.B * c2.B);

        /// <summary>
        /// Hadamard (entrywise) product of two color instances.
        /// </summary>
        public static Color Hadamard(Color c1, Color c2) => c1 * c2;

        // var f = 1 / 255.0; new Color(r * f, g * f, b * f);
        public static Color FromByteValues(byte r, byte g, byte b) =>
            new Color(r / 255.0, g / 255.0, b / 255.0);

        public static IEqualityComparer<Color> GetEqualityComparer(double epsilon = 0.0) =>
            new ApproxColorEqualityComparer(epsilon);

        /// <summary>
        /// Creates a <see cref="String"/> representation of 
        /// this <see cref="Color"/> structure.
        /// </summary>
        public override string ToString() =>
            $"({this.R}, {this.G}, {this.B})";
    }

    internal class ApproxColorEqualityComparer : ApproxEqualityComparer<Color>
    {
        public ApproxColorEqualityComparer(double epsilon = 0.0)
            : base(epsilon)
        {
        }

        public override bool Equals(Color x, Color y) =>
            this.ApproxEqual(x.R, y.R) &&
            this.ApproxEqual(x.G, y.G) &&
            this.ApproxEqual(x.B, y.B);

        public override int GetHashCode(Color obj) =>
            HashCode.Combine(obj.R, obj.G, obj.B);
    }
}
