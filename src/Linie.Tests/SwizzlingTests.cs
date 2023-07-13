// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie.Tests;

using System;
using System.Collections.Generic;
using Xunit;

public class SwizzlingTests
{
    [Fact]
    public void TestVector2Swizzle()
    {
        var u = new Vector2(1, 2);
        var tests = new Dictionary<(int, int), Func<Vector2>>
        {
            [(1, 1)] = () => u.XX(),
            [(1, 2)] = () => u.XY(),

            [(2, 1)] = () => u.YX(),
            [(2, 2)] = () => u.YY(),
        };

        foreach (var @case in tests)
        {
            var (x, y) = @case.Key;
            var expected = new Vector2(x, y);
            var actual = @case.Value();
            Assert.Equal(expected, actual);
        }
    }

    [Fact]
    public void TestVector3Swizzle()
    {
        var u = new Vector3(1, 2, 3);

        var cases = new Dictionary<(int, int, int), Func<Vector3>>
        {
            [(1, 1, 1)] = () => u.XXX(),
            [(1, 1, 2)] = () => u.XXY(),
            [(1, 1, 3)] = () => u.XXZ(),

            [(1, 2, 1)] = () => u.XYX(),
            [(1, 2, 2)] = () => u.XYY(),
            [(1, 2, 3)] = () => u.XYZ(),

            [(1, 3, 1)] = () => u.XZX(),
            [(1, 3, 2)] = () => u.XZY(),
            [(1, 3, 3)] = () => u.XZZ(),

            [(2, 1, 1)] = () => u.YXX(),
            [(2, 1, 2)] = () => u.YXY(),
            [(2, 1, 3)] = () => u.YXZ(),

            [(2, 2, 1)] = () => u.YYX(),
            [(2, 2, 2)] = () => u.YYY(),
            [(2, 2, 3)] = () => u.YYZ(),

            [(2, 3, 1)] = () => u.YZX(),
            [(2, 3, 2)] = () => u.YZY(),
            [(2, 3, 3)] = () => u.YZZ(),

            [(3, 1, 1)] = () => u.ZXX(),
            [(3, 1, 2)] = () => u.ZXY(),
            [(3, 1, 3)] = () => u.ZXZ(),

            [(3, 2, 1)] = () => u.ZYX(),
            [(3, 2, 2)] = () => u.ZYY(),
            [(3, 2, 3)] = () => u.ZYZ(),

            [(3, 3, 1)] = () => u.ZZX(),
            [(3, 3, 2)] = () => u.ZZY(),
            [(3, 3, 3)] = () => u.ZZZ(),
        };

        foreach (var @case in cases)
        {
            var (x, y, z) = @case.Key;
            var expected = new Vector3(x, y, z);
            var actual = @case.Value();
            Assert.Equal(expected, actual);
        }
    }

    [Fact]
    public void TestVector4Swizzle()
    {
        var u = new Vector4(1, 2, 3, 1);

        var cases = new Dictionary<(int, int, int, int), Func<Vector4>>
        {
            [(1, 1, 1, 1)] = () => u.XXX(),
            [(1, 1, 2, 1)] = () => u.XXY(),
            [(1, 1, 3, 1)] = () => u.XXZ(),

            [(1, 2, 1, 1)] = () => u.XYX(),
            [(1, 2, 2, 1)] = () => u.XYY(),
            [(1, 2, 3, 1)] = () => u.XYZ(),

            [(1, 3, 1, 1)] = () => u.XZX(),
            [(1, 3, 2, 1)] = () => u.XZY(),
            [(1, 3, 3, 1)] = () => u.XZZ(),

            [(2, 1, 1, 1)] = () => u.YXX(),
            [(2, 1, 2, 1)] = () => u.YXY(),
            [(2, 1, 3, 1)] = () => u.YXZ(),

            [(2, 2, 1, 1)] = () => u.YYX(),
            [(2, 2, 2, 1)] = () => u.YYY(),
            [(2, 2, 3, 1)] = () => u.YYZ(),

            [(2, 3, 1, 1)] = () => u.YZX(),
            [(2, 3, 2, 1)] = () => u.YZY(),
            [(2, 3, 3, 1)] = () => u.YZZ(),

            [(3, 1, 1, 1)] = () => u.ZXX(),
            [(3, 1, 2, 1)] = () => u.ZXY(),
            [(3, 1, 3, 1)] = () => u.ZXZ(),

            [(3, 2, 1, 1)] = () => u.ZYX(),
            [(3, 2, 2, 1)] = () => u.ZYY(),
            [(3, 2, 3, 1)] = () => u.ZYZ(),

            [(3, 3, 1, 1)] = () => u.ZZX(),
            [(3, 3, 2, 1)] = () => u.ZZY(),
            [(3, 3, 3, 1)] = () => u.ZZZ(),
        };

        foreach (var @case in cases)
        {
            var (x, y, z, w) = @case.Key;
            var expected = new Vector4(x, y, z, w);
            var actual = @case.Value();
            Assert.Equal(expected, actual);
        }
    }
}