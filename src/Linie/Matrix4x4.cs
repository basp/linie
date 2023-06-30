// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Linsi.Tests")]
namespace Linie;

using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable SA1117 // ParametersMustBeOnSameLineOrSeparateLines

/// <summary>
/// 4x4 matrix of <c>double</c> values.
/// </summary>
public class Matrix4x4 : IEquatable<Matrix4x4>, IFormattable
{
    internal readonly double[] data;

    public Matrix4x4()
        : this(0)
    {
    }

    public Matrix4x4(double v)
    {
        this.data = new[]
        {
                v, v, v, v,
                v, v, v, v,
                v, v, v, v,
                v, v, v, v,
            };
    }

    public Matrix4x4(
        double m00, double m01, double m02, double m03,
        double m10, double m11, double m12, double m13,
        double m20, double m21, double m22, double m23,
        double m30, double m31, double m32, double m33)
    {
        this.data = new[]
        {
                m00, m01, m02, m03,
                m10, m11, m12, m13,
                m20, m21, m22, m23,
                m30, m31, m32, m33,
            };
    }

    public static Matrix4x4 Identity =>
        new Matrix4x4(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1);

    public double this[int row, int col]
    {
        get => this.data[(row * 4) + col];
        set => this.data[(row * 4) + col] = value;
    }

    public Vector4 GetRow(int i) =>
        new Vector4(
            this[i, 0],
            this[i, 1],
            this[i, 2],
            this[i, 3]);

    public Vector4 GetColumn(int j) =>
        new Vector4(
            this[0, j],
            this[1, j],
            this[2, j],
            this[3, j]);

    public static void Map(
        in Matrix4x4 a,
        in Func<int, int, double, double> mapf,
        ref Matrix4x4 c)
    {
        for (var j = 0; j < 4; j++)
        {
            for (var i = 0; i < 4; i++)
            {
                c[i, j] = mapf(i, j, a[i, j]);
            }
        }
    }

    public static void Multiply(in Matrix4x4 a, in Matrix4x4 b, ref Matrix4x4 c)
    {
        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                c[i, j] =
                    (a[i, 0] * b[0, j]) +
                    (a[i, 1] * b[1, j]) +
                    (a[i, 2] * b[2, j]) +
                    (a[i, 3] * b[3, j]);
            }
        }
    }

    public static Matrix4x4 operator *(Matrix4x4 a, Matrix4x4 b)
    {
        var m = new Matrix4x4(0);
        Multiply(a, b, ref m);
        return m;
    }

    public static Vector4 operator *(Matrix4x4 m, Vector4 u)
    {
        var x = (m[0, 0] * u.X) + (m[0, 1] * u.Y) + (m[0, 2] * u.Z) + (m[0, 3] * u.W);
        var y = (m[1, 0] * u.X) + (m[1, 1] * u.Y) + (m[1, 2] * u.Z) + (m[1, 3] * u.W);
        var z = (m[2, 0] * u.X) + (m[2, 1] * u.Y) + (m[2, 2] * u.Z) + (m[2, 3] * u.W);
        var w = (m[3, 0] * u.X) + (m[3, 1] * u.Y) + (m[3, 2] * u.Z) + (m[3, 3] * u.W);
        return new Vector4(x, y, z, w);
    }

    public static Vector3 operator *(Matrix4x4 m, Vector3 u)
    {
        var x = (m[0, 0] * u.X) + (m[0, 1] * u.Y) + (m[0, 2] * u.Z);
        var y = (m[1, 0] * u.X) + (m[1, 1] * u.Y) + (m[1, 2] * u.Z);
        var z = (m[2, 0] * u.X) + (m[2, 1] * u.Y) + (m[2, 2] * u.Z);
        return new Vector3(x, y, z);
    }

    public static Normal3 operator *(Matrix4x4 m, Normal3 n)
    {
        var x = (m[0, 0] * n.X) + (m[1, 0] * n.Y) + (m[2, 0] * n.Z);
        var y = (m[0, 1] * n.X) + (m[1, 1] * n.Y) + (m[2, 1] * n.Z);
        var z = (m[0, 2] * n.X) + (m[1, 2] * n.Y) + (m[2, 2] * n.Z);
        return new Normal3(x, y, z);
    }

    public static Point3 operator *(Matrix4x4 m, Point3 p)
    {
        var x = (m[0, 0] * p.X) + (m[0, 1] * p.Y) + (m[0, 2] * p.Z) + m[0, 3];
        var y = (m[1, 0] * p.X) + (m[1, 1] * p.Y) + (m[1, 2] * p.Z) + m[1, 3];
        var z = (m[2, 0] * p.X) + (m[2, 1] * p.Y) + (m[2, 2] * p.Z) + m[2, 3];
        return new Point3(x, y, z);
    }

    public static Ray operator *(Matrix4x4 m, Ray r) =>
        new Ray(m * r.Origin, m * r.Direction);

    public static void Transpose(in Matrix4x4 a, ref Matrix4x4 m)
    {
        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                m[i, j] = a[j, i];
            }
        }
    }

    public static Matrix4x4 Transpose(in Matrix4x4 a)
    {
        var m = new Matrix4x4(0);
        Transpose(a, ref m);
        return m;
    }

    public static IEqualityComparer<Matrix4x4> GetComparer(double epsilon = 0.0) =>
        new Matrix4x4EqualityComparer(epsilon);

    public Matrix4x4 Transpose() => Matrix4x4.Transpose(this);

    /// <inheritdoc />
    public bool Equals(Matrix4x4 other)
    {
        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                if (this[i, j] != other[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <inheritdoc />
    public override int GetHashCode() =>
        HashCode.Combine(
            this.GetColumn(0).GetHashCode(),
            this.GetColumn(1).GetHashCode(),
            this.GetColumn(2).GetHashCode(),
            this.GetColumn(3).GetHashCode());

    /// <inheritdoc />
    public override string ToString() => this.ToString(null, null);

    /// <inheritdoc />
    public string ToString(
        string format,
        IFormatProvider formatProvider)
    {
        var rows = Enumerable.Range(0, 4)
            .Select(row =>
            {
                var c0 = this[row, 0].ToString(format, formatProvider);
                var c1 = this[row, 1].ToString(format, formatProvider);
                var c2 = this[row, 2].ToString(format, formatProvider);
                var c3 = this[row, 3].ToString(format, formatProvider);
                return $"<{c0} {c1} {c2} {c3}>";
            })
            .ToArray();

        return $"[{string.Join(" ", rows)}]";
    }
}

#pragma warning restore SA1117 // ParametersMustBeOnSameLineOrSeparateLines
