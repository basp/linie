// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

using System.Collections.Generic;
using System.Linq;

internal class Matrix3x3
    : IEquatable<Matrix3x3>, IFormattable
{
    internal readonly double[] data;

    public Matrix3x3(double v)
    {
        this.data = new[]
        {
                v, v, v,
                v, v, v,
                v, v, v,
            };
    }

#pragma warning disable SA1117 // ParametersMustBeOnSameLineOrSeparateLines
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
#pragma warning restore SA1117 // ParametersMustBeOnSameLineOrSeparateLines

    public double this[int row, int col]
    {
        get => this.data[(row * 3) + col];
        set => this.data[(row * 3) + col] = value;
    }

    public Vector3 GetRow(int i) =>
        new Vector3(
            this[i, 0],
            this[i, 1],
            this[i, 2]);

    public Vector3 GetColumn(int j) =>
        new Vector3(
            this[0, j],
            this[1, j],
            this[2, j]);

    public static IEqualityComparer<Matrix3x3> GetComparer(double epsilon = 0.0) =>
        new Matrix3x3EqualityComparer(epsilon);

    public override int GetHashCode() =>
        HashCode.Combine(
            this.GetColumn(0).GetHashCode(),
            this.GetColumn(1).GetHashCode(),
            this.GetColumn(2).GetHashCode());

    public bool Equals(Matrix3x3 other) =>
        this.data.SequenceEqual(other.data);

    public override string ToString() => this.ToString(null, null);

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

public static class Matrix3x3Extensions
{
    internal static Matrix2x2 Submatrix(
        this Matrix3x3 a,
        int dropRow,
        int dropCol)
    {
        var rows = Enumerable.Range(0, 3)
            .Where(x => x != dropRow)
            .ToArray();

        var cols = Enumerable.Range(0, 3)
            .Where(x => x != dropCol)
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

    internal static double Minor(this Matrix3x3 a, int row, int col) =>
        a.Submatrix(row, col).Determinant();

    internal static double Cofactor(this Matrix3x3 a, int row, int col) =>
        (row + col) % 2 == 0 ? a.Minor(row, col) : -a.Minor(row, col);

    internal static double Determinant(this Matrix3x3 a) =>
        (a[0, 0] * a.Cofactor(0, 0)) +
        (a[0, 1] * a.Cofactor(0, 1)) +
        (a[0, 2] * a.Cofactor(0, 2));
}
