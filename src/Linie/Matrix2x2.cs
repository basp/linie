// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

using System.Collections.Generic;

#pragma warning disable SA1117 // ParametersMustBeOnSameLineOrSeparateLines

public class Matrix2x2
    : IEquatable<Matrix2x2>, IFormattable
{
    internal readonly double[] data;

    public static Matrix2x2 Identity =>
        new Matrix2x2(
            1, 0,
            0, 1);

    public Matrix2x2()
        : this(0)
    {
    }

    public Matrix2x2(double v)
    {
        this.data = new[]
        {
            v, v,
            v, v,
        };
    }

    public Matrix2x2(
        double m00, double m01,
        double m10, double m11)
    {
        this.data = new[]
        {
            m00, m01,
            m10, m11,
        };
    }

    public double this[int row, int col]
    {
        get => this.data[(row * 2) + col];
        set => this.data[(row * 2) + col] = value;
    }

    public static IEqualityComparer<Matrix2x2> GetComparer(double epsilon = 0.0) =>
        new Matrix2x2EqualityComparer(epsilon);

    /// <inheritdoc />
    public override int GetHashCode() =>
        HashCode.Combine(
            this.GetColumn(0).GetHashCode(),
            this.GetColumn(1).GetHashCode());

    /// <inheritdoc />
    public bool Equals(Matrix2x2 other) =>
        this.data.SequenceEqual(other.data);

    /// <inheritdoc />
    public override string ToString() => this.ToString(null, null);

    /// <inheritdoc />
    public string ToString(string format, IFormatProvider formatProvider)
    {
        var rows = Enumerable.Range(0, 2)
           .Select(row =>
           {
               var c0 = this[row, 0].ToString(format, formatProvider);
               var c1 = this[row, 1].ToString(format, formatProvider);
               return $"<{c0} {c1}>";
           })
           .ToArray();

        return $"[{string.Join(" ", rows)}]";
    }
}

#pragma warning restore SA1117 // ParametersMustBeOnSameLineOrSeparateLines
