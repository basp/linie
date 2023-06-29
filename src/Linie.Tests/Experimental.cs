namespace Linie.Tests;

using Linie.Experimental;
using Xunit;

public class Vectors
{
    [Fact]
    public void Sandbox()
    {
        var v = new Vector<float>(new float[] { 1, 2, 3 });
        var u = new Vector<float>(new float[] { 1, 2, 3 });
        var w = u + v;
        Assert.Equal(new Vector<float>(new float[] { 2, 4, 6 }), w);
    }

    [Fact]
    public void Storage()
    {
        var storage = new [] { 1, 2, 3};
        var u = new Vector<int>(storage);
        var v = new Vector<int>(storage);
        const int @new = 999;
        storage[0] = @new;
        Assert.Equal(@new, u[0]);
        Assert.Equal(@new, v[0]);
    }
}