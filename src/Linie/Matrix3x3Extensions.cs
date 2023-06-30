// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

using System.Linq;

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

    public static double Minor(this Matrix3x3 a, int row, int col) =>
        a.Submatrix(row, col).Determinant();

    public static double Cofactor(this Matrix3x3 a, int row, int col) =>
        (row + col) % 2 == 0 ? a.Minor(row, col) : -a.Minor(row, col);

    public static double Determinant(this Matrix3x3 a) =>
        (a[0, 0] * a.Cofactor(0, 0)) +
        (a[0, 1] * a.Cofactor(0, 1)) +
        (a[0, 2] * a.Cofactor(0, 2));
}
