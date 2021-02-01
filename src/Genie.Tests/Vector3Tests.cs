namespace Genie.Tests
{
    using Xunit;

    public class Vector3Tests
    {
        [Fact]
        public void AddVectors()
        {
            var u = Vector3.Create(1, 2, 3);
            var v = Vector3.Create(-2, -4, -6);
            var w = u + v;
            Assert.Equal(-1, w.X);
            Assert.Equal(-2, w.Y);
            Assert.Equal(-3, w.Z);
        }

        [Fact]
        public void SubtractVectors()
        {
            var u = Vector3.Create(1, 2, 3);
            var v = Vector3.Create(4, 5, 6);
            var w = u - v;
            Assert.Equal(-3, w.X);
            Assert.Equal(-3, w.Y);
            Assert.Equal(-3, w.Z);
        }

        [Fact]
        public void ScaleVector()
        {
            var u = Vector3.Create(1, 2, 3);
            var v = u * 2;
            Assert.Equal(2, v.X);
            Assert.Equal(4, v.Y);
            Assert.Equal(6, v.Z);
        }
    }
}