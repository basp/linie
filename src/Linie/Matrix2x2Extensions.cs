// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

public static class Matrix2x2Extensions
{
    public static void Multiply(this Matrix2x2 self, in Matrix2x2 b, ref Matrix2x2 a)
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
