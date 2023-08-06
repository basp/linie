﻿namespace Linie.Tests;

public class Vector2Tests
{
    [Fact]
    public void TestCreation()
    {
        var ud = Vector2.Create(1.0, 2);
        var uf = Vector2.Create(1.0f, 2);
        var ui = Vector2.Create(1, 2);

        Assert.IsType<Vector2<double>>(ud);
        Assert.Equal(1.0, ud.X);
        Assert.Equal(2.0, ud.Y);
        
        Assert.IsType<Vector2<float>>(uf);
        Assert.Equal(1.0f, uf.X);
        Assert.Equal(2.0f, uf.Y);
        
        Assert.IsType<Vector2<int>>(ui);
        Assert.Equal(1, ui.X);
        Assert.Equal(2, ui.Y);
    }

    [Fact]
    public void TestIndexing()
    {
        var u = Vector2.Create(1, 2);
        
        Assert.Equal(1, u[0]);
        Assert.Equal(2, u[1]);
        
        Assert.Equal(u[0], u.X);
        Assert.Equal(u[1], u.Y);
    }

    [Fact]
    public void TestEquality()
    {
        var u = Vector2.Create(1.0, 2);
        var v = Vector2.Create(1.0, 2);
        var w = Vector2.Create(2.0, 2);
        
        Assert.Equal(u, v);
        Assert.Equal(v, u);
        
        Assert.NotEqual(u, w);
        Assert.NotEqual(v, w);
    }

    [Fact]
    public void TestAddition()
    {
        var u = Vector2.Create(1, 2);
        var v = Vector2.Create(2, 3);
        var w = Vector2.Add(u, v);
        Assert.Equal(Vector2.Create(3, 5), w);
    }

    [Fact]
    public void TestSubtraction()
    {
        var u = Vector2.Create(1, 2);
        var v = Vector2.Create(2, 3);
        var w = Vector2.Subtract(u, v);
        Assert.Equal(Vector2.Create(-1, -1), w);
    }

    [Fact]
    public void TestAbs()
    {
        var u = Vector2.Create(-1, 2);
        var v = Vector2.Abs(u);
        Assert.Equal(1, v.X);
        Assert.Equal(2, v.Y);
    }
}