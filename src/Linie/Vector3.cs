// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

using System;
using System.Collections.Generic;

/// <summary>
/// Represents a displacement in 3D space.
/// </summary>
public struct Vector3 : IEquatable<Vector3>
{
    public readonly double X, Y, Z;

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3"/> structure.
    /// </summary>
    public Vector3(double x, double y, double z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3"/> structure.
    /// </summary>
    public Vector3(double a) : this(a, a, a)
    {
    }

    public double this[int i]
    {
        get
        {
            switch (i)
            {
                case 0: return this.X;
                case 1: return this.Y;
                case 2: return this.Z;
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    public static Vector3 Zero => new Vector3(0, 0, 0);

    public static Vector3 operator +(in Vector3 a, in Vector3 b) =>
        new Vector3(
            a.X + b.X,
            a.Y + b.Y,
            a.Z + b.Z);

    public static Vector3 operator -(in Vector3 a, in Vector3 b) =>
        new Vector3(
            a.X - b.X,
            a.Y - b.Y,
            a.Z - b.Z);

    public static Vector3 operator *(in Vector3 a, in double s) =>
        new Vector3(
            a.X * s,
            a.Y * s,
            a.Z * s);

    public static Vector3 operator *(in double s, in Vector3 a) => a * s;

    public static Vector3 operator /(in Vector3 a, in double s) =>
        new Vector3(
            a.X / s,
            a.Y / s,
            a.Z / s);

    public static Vector3 operator -(in Vector3 a) =>
        new Vector3(-a.X, -a.Y, -a.Z);

    public static explicit operator Vector4(in Vector3 u) =>
        Vector4.CreateDirection(u.X, u.Y, u.Z);

    public static explicit operator Normal3(in Vector3 u) =>
        new Normal3(u.X, u.Y, u.Z);

    public static explicit operator Point3(in Vector3 u) =>
        new Point3(u.X, u.Y, u.Z);

    public static double MagnitudeSquared(in Vector3 a) =>
        (a.X * a.X) +
        (a.Y * a.Y) +
        (a.Z * a.Z);

    public static double Magnitude(in Vector3 a) =>
        Math.Sqrt(Vector3.MagnitudeSquared(a));

    public static Vector3 Normalize(in Vector3 a) => a / a.Magnitude();

    public static double Dot(in Vector3 a, in Vector3 b) =>
        (a.X * b.X) +
        (a.Y * b.Y) +
        (a.Z * b.Z);

    public static double Dot(in Vector3 a, in Normal3 n) =>
        Dot(a, (Vector3)n);

    public static Vector3 Cross(in Vector3 a, in Vector3 b) =>
        new Vector3(
            (a.Y * b.Z) - (a.Z * b.Y),
            (a.Z * b.X) - (a.X * b.Z),
            (a.X * b.Y) - (a.Y * b.X));

    public static Vector3 Reflect(in Vector3 a, in Vector3 n) =>
        a - (n * 2 * Dot(a, n));

    public static Vector3 Reflect(in Vector3 a, in Normal3 n) =>
        Reflect(a, (Vector3)n);

    public static IEqualityComparer<Vector3> GetEqualityComparer(in double epsilon = 0) =>
        new Vector3EqualityComparer(epsilon);

    public double Magnitude() => Vector3.Magnitude(this);

    public Vector3 Normalize() => Vector3.Normalize(this);

    public double Dot(in Vector3 v) => Vector3.Dot(this, v);

    public Vector3 Cross(in Vector3 v) => Vector3.Cross(this, v);

    public Vector3 Reflect(in Vector3 n) => Vector3.Reflect(this, n);

    public Vector3 Reflect(in Normal3 n) => Vector3.Reflect(this, n);

    /// <summary>
    /// Creates a <see cref="String"/> representation of 
    /// this <see cref="Vector3"/> structure.
    /// </summary>
    public override string ToString() =>
        $"({this.X}, {this.Y}, {this.Z})";

    /// <inheritdoc/>
    public bool Equals(Vector3 other) =>
        this.X == other.X &&
        this.Y == other.Y &&
        this.Z == other.Z;
}
