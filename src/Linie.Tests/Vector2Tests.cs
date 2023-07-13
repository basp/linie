// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie.Tests;

using System;
using Xunit;

public class Vector2Tests
{
    [Fact]
    public void TestCtor()
    {
        var u = new Vector2(1, 2);
        Assert.Equal(1, u.X);
        Assert.Equal(2, u.Y);
    }

    [Fact]
    public void TestAdditionAndSubtraction()
    {
        var tests = new[]
        {
            new
            {
                U = new Vector2(2, 3),
                V = new Vector2(-2, -3),
                Op = new Func<Vector2, Vector2, Vector2>((x, y) => x + y),
                W = new Vector2(0, 0),
            },
            new
            {
                U = new Vector2(2, 3),
                V = new Vector2(-2, -3),
                Op = new Func<Vector2, Vector2, Vector2>((x, y) => x - y),
                W = new Vector2(4, 6),
            },
        };

        foreach (var @case in tests)
        {
            var w = @case.Op(@case.U, @case.V);
            Assert.Equal(@case.W, w);
        }
    }

    [Fact]
    public void TestMultiplicationAndDivision()
    {
        var tests = new[]
        {
            new
            {
                U = new Vector2(2, 3),
                S = 2.0,
                Op = new Func<Vector2,double,Vector2>((u, s) => u * s),
                W = new Vector2(4, 6),
            },
            new
            {
                U = new Vector2(2, 3),
                S = 0.5,
                Op = new Func<Vector2,double,Vector2>((u, s) => u * s),
                W = new Vector2(1, 1.5),
            },
            new
            {
                U = new Vector2(2, 3),
                S = 2.0,
                Op = new Func<Vector2,double,Vector2>((u, s) => u / s),
                W = new Vector2(1, 1.5),
            },
            new
            {
                U = new Vector2(2, 3),
                S = 0.5,
                Op = new Func<Vector2,double,Vector2>((u, s) => u / s),
                W = new Vector2(4, 6),
            },
        };

        foreach (var @case in tests)
        {
            var w = @case.Op(@case.U, @case.S);
            Assert.Equal(@case.W, w);
        }
    }

    [Fact]
    public void TestSwizzling()
    {
        var u = new Vector2(2, 3);
        var tests = new[]
        {
            new
            {
                Op = new Func<Vector2,Vector2>(u => u.XX()),
                W = new Vector2(2, 2),
            },
            new
            {
                Op = new Func<Vector2,Vector2>(u => u.XY()),
                W = new Vector2(2, 3),
            },
            new
            {
                Op = new Func<Vector2,Vector2>(u => u.YX()),
                W = new Vector2(3, 2),
            },
            new
            {
                Op = new Func<Vector2,Vector2>(u => u.YY()),
                W = new Vector2(3, 3),
            },
        };

        foreach (var @case in tests)
        {
            var w = @case.Op(u);
            Assert.Equal(@case.W, w);
        }
    }
}
