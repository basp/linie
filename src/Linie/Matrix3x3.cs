// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

using System.Collections.Generic;
using System.Linq;

#pragma warning disable SA1117 // ParametersMustBeOnSameLineOrSeparateLines

public class Matrix3x3 :
    IEquatable<Matrix3x3>,
    IFormattable
{
    internal readonly double[] data;

    public static Matrix3x3 Identity =>
        new Matrix3x3(
            1, 0, 0,
            0, 1, 0,
            0, 0, 1);

    public Matrix3x3()
        : this(0)
    {
    }

    public Matrix3x3(double v)
    {
        this.data = new[]
        {
            v, v, v,
            v, v, v,
            v, v, v,
        };
    }

    public Matrix3x3(
        double m00, double m01, double m02,
        double m10, double m11, double m12,
        double m20, double m21, double m22)
    {
        this.data = new[]
        {
            m00, m01, m02,
            m10, m11, m12,
            m20, m21, m22,
        };
    }

    public double this[int row, int col]
    {
        get => this.data[(row * 3) + col];
        set => this.data[(row * 3) + col] = value;
    }

    public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
    {
        var c = new Matrix3x3();
        Matrix3x3.Multiply(a, b, ref c);
        return c;
    }

    public static void Multiply(in Matrix3x3 a, in Matrix3x3 b, ref Matrix3x3 c)
    {
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                c[i, j] =
                    (a[i, 0] * b[0, j]) +
                    (a[i, 1] * b[1, j]) +
                    (a[i, 2] * b[2, j]);
            }
        }
    }

    public static Matrix3x3 Transpose(in Matrix3x3 a)
    {
        var c = new Matrix3x3();
        Matrix3x3.Transpose(a, ref c);
        return c;
    }

    public static void Transpose(in Matrix3x3 a, ref Matrix3x3 c)
    {
        c[0, 0] = a[0, 0];
        c[0, 1] = a[1, 0];
        c[0, 2] = a[2, 0];
        c[1, 0] = a[0, 1];
        c[1, 1] = a[1, 1];
        c[1, 2] = a[2, 1];
        c[2, 0] = a[0, 2];
        c[2, 1] = a[1, 2];
        c[2, 2] = a[2, 2];
    }

    public static Vector3 GetRow(in Matrix3x3 self, int i) =>
        new Vector3(
            self[i, 0],
            self[i, 1],
            self[i, 2]);

    public static Vector3 GetColumn(in Matrix3x3 self, int j) =>
        new Vector3(
            self[0, j],
            self[1, j],
            self[2, j]);

    public static Matrix2x2 Submatrix(in Matrix3x3 a, int row, int col)
    {
        var rows = Enumerable.Range(0, 3)
            .Where(x => x != row)
            .ToArray();

        var cols = Enumerable.Range(0, 3)
            .Where(x => x != col)
            .ToArray();

        var m = new Matrix2x2(0);
        for (var i = 0; i < 2; i++)
        {
            for (var j = 0; j < 2; j++)
            {
                m[i, j] = a[rows[i], cols[j]];
            }
        }

        return m;
    }

    public static double Minor(in Matrix3x3 a, int row, int col) =>
        a.Submatrix(row, col).Determinant();

    public static double Cofactor(in Matrix3x3 a, int row, int col) =>
        (row + col) % 2 == 0 ? a.Minor(row, col) : -a.Minor(row, col);

    public static double Determinant(in Matrix3x3 a) =>
        (a[0, 0] * a.Cofactor(0, 0)) +
        (a[0, 1] * a.Cofactor(0, 1)) +
        (a[0, 2] * a.Cofactor(0, 2));

    public static IEqualityComparer<Matrix3x3> GetComparer(double epsilon = 0.0) =>
        new Matrix3x3EqualityComparer(epsilon);

    public Vector3 GetRow(int i) => Matrix3x3.GetRow(this, i);

    public Vector3 GetColumn(int j) => Matrix3x3.GetColumn(this, j);

    public Matrix2x2 Submatrix(int row, int column) =>
        Matrix3x3.Submatrix(this, row, column);

    public double Minor(int row, int column) =>
        Matrix3x3.Minor(this, row, column);

    public double Cofactor(int row, int column) =>
        Matrix3x3.Cofactor(this, row, column);

    public double Determinant() => Matrix3x3.Determinant(this);

    public Matrix3x3 Transpose() => Matrix3x3.Transpose(this);

    /// <inheritdoc />
    public override int GetHashCode() =>
        HashCode.Combine(
            this.GetColumn(0).GetHashCode(),
            this.GetColumn(1).GetHashCode(),
            this.GetColumn(2).GetHashCode());

    /// <inheritdoc />
    public bool Equals(Matrix3x3 other) =>
        this.data.SequenceEqual(other.data);

    /// <inheritdoc />
    public override string ToString() => this.ToString(null, null);

    /// <inheritdoc />
    public string ToString(string format, IFormatProvider formatProvider)
    {
        var rows = Enumerable.Range(0, 3)
           .Select(row =>
           {
               var c0 = this[row, 0].ToString(format, formatProvider);
               var c1 = this[row, 1].ToString(format, formatProvider);
               var c2 = this[row, 2].ToString(format, formatProvider);
               return $"<{c0} {c1} {c2}>";
           })
           .ToArray();

        return $"[{string.Join(" ", rows)}]";
    }
}

#pragma warning restore SA1117 // ParametersMustBeOnSameLineOrSeparateLines
