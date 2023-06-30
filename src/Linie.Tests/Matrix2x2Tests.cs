// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie.Tests;

using Xunit;

public class Matrix2x2Tests
{
    const double epsilon = 0.000000001;

    [Fact]
    public void TestCalculateDeterminant()
    {
        var m = new Matrix2x2(1, 5, -3, 2);
        Assert.Equal(17, m.Determinant());
    }

    static readonly Matrix2x2 m =
        new Matrix2x2(
            00, 01,
            10, 11);

    [Theory]
    [InlineData(0, 00, 01)]
    [InlineData(1, 10, 11)]
    public void TestGetRow(int i, double x, double y)
    {
        var expected = new Vector2(x, y);
        var row = m.GetRow(i);
        Assert.Equal(expected, row);
    }

    [Theory]
    [InlineData(0, 00, 10)]
    [InlineData(1, 01, 11)]
    public void TestGetColumn(int j, double x, double y)
    {
        var expected = new Vector2(x, y);
        var column = m.GetColumn(j);
        Assert.Equal(expected, column);
    }
}
