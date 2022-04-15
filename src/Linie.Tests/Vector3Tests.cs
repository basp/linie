namespace Linie.Tests;

using Xunit;

public class Vector3Tests
{
    [Fact]
    public void TestCtor()
    {
        var u = new Vector3(1, 2, 3);
        Assert.Equal(1, u.X);
        Assert.Equal(2, u.Y);
        Assert.Equal(3, u.Z);
    }

    [Fact]
    public void TestExplicitVector4()
    {
        var u = new Vector3(0, 0, 1);
        Vector4 v = (Vector4)u;
        Assert.True(v.IsDirection);
        Assert.Equal(0, v.X);
        Assert.Equal(0, v.Y);
        Assert.Equal(1, v.Z);
        Assert.Equal(0, v.W);
    }

    [Fact]
    public void TestTranslation()
    {
        var u = new Vector3(2, 3, 1);
        var m = Affine.Translate(-2, -3, -4);
        var v = m * u;
        Assert.Equal(2, u.X);
        Assert.Equal(3, u.Y);
        Assert.Equal(1, u.Z);
    }

    [Fact]
    public void TestMagnitudeSquared()
    {
        var u = new Vector3(2, 2, 2);
        var expected = 12;
        Assert.Equal(expected, Vector3.MagnitudeSquared(u));
    }

    [Fact]
    public void TestNormalize()
    {
        const double epsilon = 0.000001;
        var cmp = Vector3.GetEqualityComparer(epsilon);
        var u = new Vector3(2, 2, 2);
        var v = Vector3.Normalize(u);
        var mag = u.Magnitude();
        var expected = new Vector3(
            u.X / mag,
            u.Y / mag,
            u.Z / mag);

        Assert.Equal(expected, v, cmp);
    }
}
