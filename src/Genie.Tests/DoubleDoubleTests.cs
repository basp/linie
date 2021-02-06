namespace Genie.Tests
{
    using System;
    using Xunit;

    public class DoubleDoubleTests
    {
        [Fact]
        public void Add()
        {
            var a = new DoubleDouble(5);
            var b = new DoubleDouble(3);
            Assert.Equal(8, a + b);
        }

        [Fact]
        public void Subtract()
        {
            var a = new DoubleDouble(5);
            var b = new DoubleDouble(3);
            Assert.Equal(2, a - b);
        }

        [Fact]
        public void Power()
        {
            var a = new DoubleDouble(1.4);
            var b = DoubleDouble.Pow(a, 3);
            Assert.Equal(Math.Pow(1.4, 3), (double)b);
        }

        [Fact]
        public void Exp()
        {
            var a = new DoubleDouble(1.5);
            var b = DoubleDouble.Exp(a);
            Assert.Equal(Math.Exp(1.5), (double)b);
        }

        [Fact]
        public void Log()
        {
            var a = new DoubleDouble(3);
            var b = DoubleDouble.Log(a);
            var c = DoubleDouble.Exp(b);
            Assert.Equal((double)a, (double)c);
        }

        [Fact]
        public void Sqrt()
        {
            var a = new DoubleDouble(3);
            var b = DoubleDouble.Sqrt(a);
            var c = b * b;
            Assert.Equal(a, c);
        }

        [Fact]
        public void DoubleDoubleVector()
        {
            var u = Vector2.Create(
                new DoubleDouble(1.5),
                new DoubleDouble(0.7));

            var v = u * 2;

            Assert.Equal(3.0, v.X);
            Assert.Equal(1.4, v.Y);
        }

        [Fact]
        public void TryParse()
        {
            var s = "3.14";
            Assert.True(DoubleDouble.TryParse(s, out var a));
            Assert.Equal(3.14, (double)a);
        }
    }
}