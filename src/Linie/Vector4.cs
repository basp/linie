// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

using System;
using System.Collections.Generic;

/// <summary>
/// Represents a displacement or point in 4D space.
/// </summary>
/// <remarks>
/// Even though this is technically a 4D structure, 4D vectors and matrices are
/// commonly used to represent 3D space in computer graphics. Therefor it is
/// best to think of this type as a fancy 3D vector or point instead of a true
/// four dimensional object.
/// </remarks>
public readonly struct Vector4 :
    IEquatable<Vector4>,
    IFormattable
{
    public readonly double X, Y, Z, W;

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector4"/> structure.
    /// </summary>
    public Vector4(double x, double y, double z, double w)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.W = w;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector4"/> structure.
    /// </summary>
    public Vector4(double a) : this(a, a, a, a)
    {
    }

    public static Vector4 Zero => new Vector4(0, 0, 0, 0);

    public bool IsPosition => this.W == 1.0;

    public bool IsDirection => this.W == 0.0;

    public static Vector4 operator +(in Vector4 a, in Vector4 b) =>
        new Vector4(
            a.X + b.X,
            a.Y + b.Y,
            a.Z + b.Z,
            a.W + b.W);

    public static Vector4 operator -(in Vector4 a, in Vector4 b) =>
        new Vector4(
            a.X - b.X,
            a.Y - b.Y,
            a.Z - b.Z,
            a.W - b.W);

    public static Vector4 operator -(in Vector4 a) =>
        new Vector4(-a.X, -a.Y, -a.Z, -a.W);

    public static Vector4 operator *(in Vector4 a, in double s) =>
        new Vector4(
            a.X * s,
            a.Y * s,
            a.Z * s,
            a.W * s);

    public static Vector4 operator *(in double s, in Vector4 a) => a * s;

    public static Vector4 operator /(in Vector4 a, in double s) =>
        new Vector4(
            a.X / s,
            a.Y / s,
            a.Z / s,
            a.W / s);

    public static explicit operator Vector3(in Vector4 u) =>
        new Vector3(u.X, u.Y, u.Z);

    public static double MagnitudeSquared(in Vector4 a) =>
        (a.X * a.X) +
        (a.Y * a.Y) +
        (a.Z * a.Z) +
        (a.W * a.W);

    public static double Magnitude(in Vector4 a) =>
        Math.Sqrt(Vector4.MagnitudeSquared(a));

    public static Vector4 Normalize(in Vector4 a) => a / a.Magnitude();

    public static double Dot(in Vector4 a, in Vector4 b) =>
        (a.X * b.X) +
        (a.Y * b.Y) +
        (a.Z * b.Z) +
        (a.W * b.W);

    /// <summary>
    /// Performs a cross product on two `Vector4` instances
    /// disregarding the `W` component. Essentially treating
    /// `a` and `b` as 3d vectors. The resulting 3d vector
    /// is wrapped as a 4d direction vector (`w` = 1).
    /// </summary>
    public static Vector4 Cross3(in Vector4 a, in Vector4 b) =>
        Vector4.CreateDirection(
            (a.Y * b.Z) - (a.Z * b.Y),
            (a.Z * b.X) - (a.X * b.Z),
            (a.X * b.Y) - (a.Y * b.X));

    public static Vector4 Reflect(in Vector4 a, in Vector4 n) =>
        a - (n * 2 * Dot(a, n));

    /// <summary>
    /// Constructs a point vector.
    /// </summary>
    /// <remarks>
    /// Note that point "vectors have their `w` component set to
    /// zero and are thus unaffected by translations.
    /// </remarks>
    /// <param name="x">Value for the <c>X</c> component.</param>
    /// <param name="y">Value for the <c>Y</c> component.</param>
    /// <param name="z">Value for the <c>Z</c> component.</param>
    public static Vector4 CreatePosition(in double x, in double y, in double z) =>
        new Vector4(x, y, z, 1);

    /// <summary>
    /// Constructs a direction vector.
    /// </summary>
    /// <remarks>
    /// Note that direction vectors have their `w` component set to
    /// zero and are thus unaffected by translations.
    /// </remarks>
    /// <param name="x">Value for the <c>X</c> component.</param>
    /// <param name="y">Value for the <c>Y</c> component.</param>
    /// <param name="z">Value for the <c>Z</c> component.</param>
    public static Vector4 CreateDirection(in double x, in double y, in double z) =>
        new Vector4(x, y, z, 0);

    public static IEqualityComparer<Vector4> GetComparer(in double epsilon = 0.0) =>
        new Vector4EqualityComparer(epsilon);

    public Vector4 Cross3(Vector4 b) => Cross3(this, b);

    public double Magnitude() => Vector4.Magnitude(this);

    public Vector4 Normalize() => Vector4.Normalize(this);

    public double Dot(in Vector4 v) => Vector4.Dot(this, v);

    public Vector4 Reflect(in Vector4 n) => Vector4.Reflect(this, n);

    /// <inheritdoc />
    public override int GetHashCode() =>
        HashCode.Combine(this.X, this.Y, this.Z, this.W);

    /// <inheritdoc />
    public override string ToString() => this.ToString(null, null);

    /// <inheritdoc/>
    public bool Equals(Vector4 other) =>
        this.X == other.X &&
        this.Y == other.Y &&
        this.Z == other.Z &&
        this.W == other.W;

    /// <inheritdoc />
    public string ToString(string format, IFormatProvider formatProvider)
    {
        var x = this.X.ToString(format, formatProvider);
        var y = this.Y.ToString(format, formatProvider);
        var z = this.Z.ToString(format, formatProvider);
        var w = this.W.ToString(format, formatProvider);
        return $"<{x} {y} {z} {w}>";
    }
}

public static class Vector4Extensions
{
    public static Vector4 AsPosition(this Vector4 self) =>
        Vector4.CreatePosition(self.X, self.Y, self.Z);

    public static Vector4 AsDirection(this Vector4 self) =>
        Vector4.CreateDirection(self.X, self.Y, self.Z);
}
