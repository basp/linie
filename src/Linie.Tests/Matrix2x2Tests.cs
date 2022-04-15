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
}
