namespace Genie.Tests
{
    using System;
    using Xunit;

    public class Vector2Tests
    {
        [Fact]
        public void TestAddVectors()
        {
            var u = Vector2.Create(1, 2);
            var v = Vector2.Create(2, 1);
            var w = u + v;
            Assert.Equal(3, w.X);
            Assert.Equal(3, w.Y);
        }

        [Fact]
        public void TestSubtractVectors()
        {
            var u = Vector2.Create(1, 2);
            var v = Vector2.Create(2, 1);
            var w = u - v;
            Assert.Equal(-1, w.X);
            Assert.Equal(1, w.Y);
        }

        [Fact]
        public void TestScaleVector()
        {
            var u = Vector2.Create(1, 2);
            var v = u * 2;
            Assert.Equal(2, v.X);
            Assert.Equal(4, v.Y);
        }

        [Fact]
        public void TestMagnitudeSquared()
        {
            var u = Vector2.Create(Math.Sqrt(2), Math.Sqrt(2));
            var s = Vector2.MagnitudeSquared(u);
            Assert.Equal(4, s, precision: 6);
        }

        [Fact]
        public void TestMagnitude()
        {
            var u = Vector2.Create(MathF.Sqrt(2), MathF.Sqrt(2));
            var s = Vector2.Magnitude(u);
            Assert.Equal(2, s, precision: 6);
        }
    }
}
