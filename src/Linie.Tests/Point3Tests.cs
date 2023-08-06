namespace Linie.Tests;

public class Point3Tests
{
    [Fact]
    public void TestCreation()
    {
        var p = Point3.Create(1.0, 2, 3);

        Assert.Equal(1, p.X);
        Assert.Equal(2, p.Y);
        Assert.Equal(3, p.Z);
    }

    [Fact]
    public void TestIndexing()
    {
        var p = Point3.Create(1, 2, 3);
        
        Assert.Equal(1, p[0]);
        Assert.Equal(2, p[1]);
        Assert.Equal(3, p[2]);
    }
}