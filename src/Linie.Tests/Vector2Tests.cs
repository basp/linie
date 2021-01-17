namespace Linie.Tests
{
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
    }
}
