namespace Linie.Tests
{
    using Xunit;

    public class Normal3Tests
    {
        [Fact]
        public void TestCtor()
        {
            var n = new Normal3(1, 2, 3);
            Assert.Equal(1, n.X);
            Assert.Equal(2, n.Y);
            Assert.Equal(3, n.Z);
        }

        [Fact]
        public void TestImplicitVector4()
        {
            var n = new Normal3(1, 2, 3);
            Vector4 u = n;
            Assert.True(u.IsDirection);
            Assert.Equal(1, u.X);
            Assert.Equal(2, u.Y);
            Assert.Equal(3, u.Z);
        }
    }
}