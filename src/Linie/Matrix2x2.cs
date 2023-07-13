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

    public static Vector2 GetRow(in Matrix2x2 self, int row) =>
        new Vector2(self[row, 0], self[row, 1]);

    public static Vector2 GetColumn(in Matrix2x2 self, int column) =>
        new Vector2(self[0, column], self[1, column]);

    public static void Multiply(in Matrix2x2 a, in Matrix2x2 b, ref Matrix2x2 c)
    {
        for (var i = 0; i < 2; i++)
        {
            for (var j = 0; j < 2; j++)
            {
                c[i, j] =
                    (a[i, 0] * b[0, j]) +
                    (a[i, 1] * b[1, j]);
            }
        }
    }

    public static double Determinant(in Matrix2x2 self) =>
        (self[0, 0] * self[1, 1]) - (self[0, 1] * self[1, 0]);

    public static Matrix2x2 Transpose(in Matrix2x2 a)
    {
        var c = new Matrix2x2();
        Matrix2x2.Transpose(a, ref c);
        return c;
    }

    public static void Transpose(in Matrix2x2 a, ref Matrix2x2 c)
    {
        c[0, 0] = a[0, 0];
        c[1, 0] = a[0, 1];
        c[0, 1] = a[1, 0];
        c[1, 1] = a[1, 1];
    }

    public static Matrix2x2 operator *(Matrix2x2 a, Matrix2x2 b)
    {
        var c = new Matrix2x2();
        Matrix2x2.Multiply(a, b, ref c);
        return c;
    }

    public static IEqualityComparer<Matrix2x2> GetComparer(double epsilon = 0.0) =>
        new Matrix2x2EqualityComparer(epsilon);

    public Vector2 GetRow(int i) => Matrix2x2.GetRow(this, i);

    public Vector2 GetColumn(int j) => Matrix2x2.GetColumn(this, j);

    public double Determinant() => Matrix2x2.Determinant(this);

    public Matrix2x2 Transpose() => Matrix2x2.Transpose(this);

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
