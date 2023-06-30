// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

using System.Collections.Generic;

public class Matrix2x2
    : IEquatable<Matrix2x2>, IFormattable
{
    internal readonly double[] data;

    public Matrix2x2(double v)
    {
        this.data = new[]
        {
            v, v,
            v, v,
        };
    }

#pragma warning disable SA1117 // ParametersMustBeOnSameLineOrSeparateLines
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
#pragma warning restore SA1117 // ParametersMustBeOnSameLineOrSeparateLines

    public double this[int row, int col]
    {
        get => this.data[(row * 2) + col];
        set => this.data[(row * 2) + col] = value;
    }

    public static IEqualityComparer<Matrix2x2> GetComparer(double epsilon = 0.0) =>
        new Matrix2x2EqualityComparer(epsilon);

    public override int GetHashCode() =>
        HashCode.Combine(
            this.GetColumn(0).GetHashCode(),
            this.GetColumn(1).GetHashCode());

    public bool Equals(Matrix2x2 other) =>
        this.data.SequenceEqual(other.data);

    public override string ToString() => this.ToString(null, null);

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

public static class Matrix2x2Extensions
{
    public static void Multiply(this Matrix2x2 self, Matrix2x2 b, ref Matrix2x2 a)
    {
        throw new NotImplementedException();
    }

    public static Vector2 GetRow(this Matrix2x2 self, int row) =>
        new Vector2(self[row, 0], self[row, 1]);

    public static Vector2 GetColumn(this Matrix2x2 self, int column) =>
        new Vector2(self[0, column], self[1, column]);

    public static double Determinant(this Matrix2x2 self) =>
        (self[0, 0] * self[1, 1]) - (self[0, 1] * self[1, 0]);
}
