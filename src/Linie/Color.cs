// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

using System;
using System.Collections.Generic;

/// <summary>
/// Represents a RGB (red, green, blue) color where the R, G and B
/// elements are in the range of <c>[0..1]</c>.
/// </summary>
public struct Color
{
    public static Color White => new Color(1, 1, 1);

    public static Color Black => new Color(0, 0, 0);

    public readonly double R, G, B;

    /// <summary>
    /// Initializes a new instance of the <see cref="Color"/> structure.
    /// </summary>
    public Color(double r, double g, double b)
    {
        this.R = r;
        this.G = g;
        this.B = b;
    }

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
    /// Hadamard (entrywise) product of two <see cref="Color"/> values.
    /// </summary>
    public static Color Hadamard(Color c1, Color c2) => c1 * c2;

    /// <summary>
    /// 
    /// </summary>
    public static Color FromByteValues(byte r, byte g, byte b) =>
        // var f = 1 / 255.0; new Color(r * f, g * f, b * f);
        new Color(r / 255.0, g / 255.0, b / 255.0);

    public static IEqualityComparer<Color> GetComparer(double epsilon = 0.0) =>
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
