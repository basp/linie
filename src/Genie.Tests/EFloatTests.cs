namespace Genie.Tests
{
    using Xunit;

    public class EFloatTests
    {
        [Fact]
        public void Sandbox()
        {
            var a = new EFloat(1.0f);
            var b = new EFloat(2.0f);
            var c = a + b;
            Assert.True(c.LowerBound <= 3);
            Assert.True(c.UpperBound >= 3);
        }

        [Fact]
        public void VectorOfEFloats()
        {
            var u = Vector2.Create(
                new EFloat(2),
                new EFloat(3));

            var v = Vector2.Create(
                new EFloat(3),
                new EFloat(2));

            var w = u - v;
            Assert.True(w.X.LowerBound <= -1);
            Assert.True(w.X.UpperBound >= -1);
            Assert.True(w.Y.LowerBound <= 1);
            Assert.True(w.Y.UpperBound >= 1);
        }
    }
}