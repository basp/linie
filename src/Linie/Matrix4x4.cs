// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable SA1117 // ParametersMustBeOnSameLineOrSeparateLines

/// <summary>
/// 4x4 matrix of <c>double</c> values.
/// </summary>
public class Matrix4x4 :
    IEquatable<Matrix4x4>,
    IFormattable
{
    internal readonly double[] data;

    /// <summary>
    /// Initializes a new <see cref="Matrix4x4"/> instance with all
    /// elements set to zero.
    /// </summary>
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

    public static Vector4 GetRow(in Matrix4x4 self, int i) =>
         new Vector4(
             self[i, 0],
             self[i, 1],
             self[i, 2],
             self[i, 3]);

    public static Vector4 GetColumn(in Matrix4x4 self, int j) =>
        new Vector4(
            self[0, j],
            self[1, j],
            self[2, j],
            self[3, j]);

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
        Matrix4x4.Multiply(a, b, ref m);
        return m;
    }

    public static Vector4 operator *(Matrix4x4 m, Vector4 u)
    {
        var x =
            (m[0, 0] * u.X) +
            (m[0, 1] * u.Y) +
            (m[0, 2] * u.Z) +
            (m[0, 3] * u.W);
        var y =
            (m[1, 0] * u.X) +
            (m[1, 1] * u.Y) +
            (m[1, 2] * u.Z) +
            (m[1, 3] * u.W);
        var z =
            (m[2, 0] * u.X) +
            (m[2, 1] * u.Y) +
            (m[2, 2] * u.Z) +
            (m[2, 3] * u.W);
        var w =
            (m[3, 0] * u.X) +
            (m[3, 1] * u.Y) +
            (m[3, 2] * u.Z) +
            (m[3, 3] * u.W);
        return new Vector4(x, y, z, w);
    }

    public static Vector3 operator *(Matrix4x4 m, Vector3 u)
    {
        var x =
            (m[0, 0] * u.X) +
            (m[0, 1] * u.Y) +
            (m[0, 2] * u.Z);
        var y =
            (m[1, 0] * u.X) +
            (m[1, 1] * u.Y) +
            (m[1, 2] * u.Z);
        var z =
            (m[2, 0] * u.X) +
            (m[2, 1] * u.Y) +
            (m[2, 2] * u.Z);
        return new Vector3(x, y, z);
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

    /// <summary>
    /// Calculates the determinant of given matrix <c>a</c> using
    /// cofactor expansion.
    /// </summary>
    public static double Determinant(in Matrix4x4 a) =>
        (a[0, 0] * a.Cofactor(0, 0)) +
        (a[0, 1] * a.Cofactor(0, 1)) +
        (a[0, 2] * a.Cofactor(0, 2)) +
        (a[0, 3] * a.Cofactor(0, 3));

    public static bool IsInvertible(in Matrix4x4 a) =>
        a.Determinant() != 0;

    /// <summary>
    /// This does not perform the check on the determinant so
    /// it will blow up if that turns out to be zero.
    /// </summary>
    /// <remarks>
    /// This should only be used if you are dealing with a cached
    /// matrix that is known to be invertible (i.e. by inspecting the
    /// determinant or using the <c>IsInvertible</c> method.)
    /// </remarks>
    public static void UnsafeInvert(in Matrix4x4 a, ref Matrix4x4 m)
    {
        var d = a.Determinant();
        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                m[j, i] = a.Cofactor(i, j) / d;
            }
        }
    }

    public static bool TryInvert(in Matrix4x4 a, ref Matrix4x4 m)
    {
        var d = a.Determinant();
        if (d == 0)
        {
            return false;
        }

        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                m[j, i] = a.Cofactor(i, j) / d;
            }
        }

        return true;
    }

    public static void Invert(in Matrix4x4 a, ref Matrix4x4 m)
    {
        if (!a.TryInvert(ref m))
        {
            throw new ArgumentException("Matrix is not invertible.", nameof(a));
        }
    }

    public bool TryInvert(ref Matrix4x4 m) =>
        Matrix4x4.TryInvert(this, ref m);

    /// <summary>
    /// Returns the inverse of the given matrix.
    /// </summary>
    public static Matrix4x4 Invert(in Matrix4x4 a)
    {
        var m = new Matrix4x4(0);
        Matrix4x4.Invert(a, ref m);
        return m;
    }

    public double Determinant() => Matrix4x4.Determinant(this);

    // This creates a new 3x3 matrix from a 4x4 matrix by dropping one
    // row and one column. The row and column to be dropped are specified
    // by the <c>row</c> and <c>col</c> arguments respectively.
    //
    // Ex. (row = 2, col = 1)
    // ---
    // A B C D  =>  A | C D  =>  A C D
    // E F G H      E | G H      E G H
    // I J K L      - + - -      M O P
    // M N O P      M | O P
    public static Matrix3x3 Submatrix(in Matrix4x4 a, int row, int col)
    {
        var rows = Enumerable.Range(0, 4) // 0, 1, 2, 3
            .Where(x => x != row)
            .ToArray();

        var cols = Enumerable.Range(0, 4)
            .Where(x => x != col)
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
    public static double Minor(in Matrix4x4 a, int row, int col) =>
        a.Submatrix(row, col).Determinant();

    public static double Cofactor(in Matrix4x4 a, int row, int col) =>
        (row + col) % 2 == 0 ? a.Minor(row, col) : -a.Minor(row, col);

    public static IEqualityComparer<Matrix4x4> GetComparer(double epsilon = 0.0) =>
        new Matrix4x4EqualityComparer(epsilon);

    public Vector4 GetRow(int i) => Matrix4x4.GetRow(this, i);

    public Vector4 GetColumn(int j) => Matrix4x4.GetColumn(this, j);

    public Matrix3x3 Submatrix(int row, int column) =>
        Matrix4x4.Submatrix(this, row, column);

    public double Minor(int row, int column) =>
        Matrix4x4.Minor(this, row, column);

    public double Cofactor(int row, int column) =>
        Matrix4x4.Cofactor(this, row, column);

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
