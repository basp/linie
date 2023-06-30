// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Linsi.Tests")]
namespace Linie;

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 4x4 matrix of <c>double</c> values.
/// </summary>
public class Matrix4x4 : IEquatable<Matrix4x4>, IFormattable
{
    internal readonly double[] data;

#pragma warning disable SA1117 // ParametersMustBeOnSameLineOrSeparateLines
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
#pragma warning restore SA1117 // ParametersMustBeOnSameLineOrSeparateLines

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
        Matrix4x4 a,
        Func<int, int, double, double> mapf,
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

    public static void Multiply(Matrix4x4 a, Matrix4x4 b, ref Matrix4x4 c)
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

    public static Ray4 operator *(Matrix4x4 m, Ray4 r) =>
        new Ray4(m * r.Origin, m * r.Direction);

    public static Ray3 operator *(Matrix4x4 m, Ray3 r) =>
        new Ray3(m * r.Origin, m * r.Direction);

    public static void Transpose(Matrix4x4 a, ref Matrix4x4 m)
    {
        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                m[i, j] = a[j, i];
            }
        }
    }

    public static Matrix4x4 Transpose(Matrix4x4 a)
    {
        var m = new Matrix4x4(0);
        Transpose(a, ref m);
        return m;
    }

    public static IEqualityComparer<Matrix4x4> GetComparer(double epsilon = 0.0) =>
        new Matrix4x4EqualityComparer(epsilon);

    public Matrix4x4 Transpose() => Matrix4x4.Transpose(this);

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

    public override int GetHashCode() =>
        HashCode.Combine(
            this.GetColumn(0).GetHashCode(),
            this.GetColumn(1).GetHashCode(),
            this.GetColumn(2).GetHashCode(),
            this.GetColumn(3).GetHashCode());

    public override string ToString() => this.ToString(null, null);

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

public static class Matrix4x4Extensions
{
    /// <summary>
    /// Calculates the determinant of given matrix <c>a</c> using Laplace 
    /// expansion.
    /// </summary>
    public static double Determinant(this Matrix4x4 a) =>
        (a[0, 0] * a.Cofactor(0, 0)) +
        (a[0, 1] * a.Cofactor(0, 1)) +
        (a[0, 2] * a.Cofactor(0, 2)) +
        (a[0, 3] * a.Cofactor(0, 3));

    public static bool IsInvertible(this Matrix4x4 a) =>
        a.Determinant() != 0;

    public static void Invert(this Matrix4x4 a, ref Matrix4x4 m)
    {
        var d = a.Determinant();
        if (d == 0)
        {
            throw new ArgumentException("Matrix is not invertible.", nameof(a));
        }

        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                m[j, i] = a.Cofactor(i, j) / d;
            }
        }
    }

    public static void Invert2(this Matrix4x4 a, Matrix4x4 m)
    {
        var d = a.Determinant();
        if (d == 0)
        {
            throw new ArgumentException("Matrix is not invertible.", nameof(a));
        }

        Parallel.For(0, 4, i =>
        {
            for (var j = 0; j < 4; j++)
            {
                m[j, i] = a.Cofactor(i, j) / d;
            }
        });
    }

    /// <summary>
    /// Returns the inverse of the given matrix.
    /// </summary>
    public static Matrix4x4 Invert(this Matrix4x4 a)
    {
        var m = new Matrix4x4(0);
        a.Invert(ref m);
        return m;
    }

    // This creates a new 3x3 matrix from a 4x4 matrix by dropping one
    // row and one column. The row and column to be dropped are specified
    // by the <c>dropRow</c> and <c>dropCol</c> arguments respectively.
    //
    // Ex. (dropRow = 2, dropCol = 1)
    // ---
    // A B C D  =>  A | C D  =>  A C D
    // E F G H      E | G H      E G H
    // I J K L      - + - -      M O P
    // M N O P      M | O P
    internal static Matrix3x3 Submatrix(this Matrix4x4 a, int dropRow, int dropCol)
    {
        var rows = Enumerable.Range(0, 4) // 0, 1, 2, 3
            .Where(x => x != dropRow)
            .ToArray();

        var cols = Enumerable.Range(0, 4)
            .Where(x => x != dropCol)
            .ToArray();

        var m = new Matrix3x3(0);
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                m[i, j] = a[rows[i], cols[j]];
            }
        }

        return m;
    }

    // For more information on minors, cofactors, etc. and how the
    // determinant is calculated see:
    //
    // * https://en.wikipedia.org/wiki/Minor_(linear_algebra)
    // * https://en.wikipedia.org/wiki/Laplace_expansion
    internal static double Minor(this Matrix4x4 a, int row, int col) =>
        a.Submatrix(row, col).Determinant();

    internal static double Cofactor(this Matrix4x4 a, int row, int col) =>
        (row + col) % 2 == 0 ? a.Minor(row, col) : -a.Minor(row, col);


    /// <summary>
    /// Applies a translation matrix to an existing matrix.
    /// </summary>
    public static Matrix4x4 Translate(
        this Matrix4x4 m,
        double x,
        double y,
        double z) => Affine.Translate(x, y, z) * m;

    /// <summary>
    /// Applies a scale matrix to an existing matrix using 
    /// constant scale factor in all dimensions.
    /// </summary>
    public static Matrix4x4 Scale(this Matrix4x4 m, double s) =>
        Scale(m, s, s, s);

    /// <summary>
    /// Applies a scale matrix to an existing matrix.
    /// </summary>
    public static Matrix4x4 Scale(
        this Matrix4x4 m,
        double x,
        double y,
        double z) => Affine.Scale(x, y, z) * m;

    /// <summary>
    /// Applies a x-axis rotation matrix to a given matrix.
    /// </summary>
    public static Matrix4x4 RotateX(this Matrix4x4 m, double r) =>
        Affine.RotateX(r) * m;

    /// <summary>
    /// Applies an y-axis rotation matrix to a given matrix.
    /// </summary>
    public static Matrix4x4 RotateY(this Matrix4x4 m, double r) =>
        Affine.RotateY(r) * m;

    /// <summary>
    /// Applies a z-axis rotation matrix to a given matrix.
    /// </summary>
    public static Matrix4x4 RotateZ(this Matrix4x4 m, double r) =>
        Affine.RotateZ(r) * m;

    /// <summary>
    /// Applies a shear matrix to a given matrix.
    /// </summary>
    public static Matrix4x4 Shear(
        this Matrix4x4 m,
        double xy,
        double xz,
        double yx,
        double yz,
        double zx,
        double zy) => Affine.Shear(xy, xz, yx, yz, zx, zy) * m;
}
