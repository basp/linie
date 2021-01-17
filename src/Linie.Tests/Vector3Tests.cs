namespace Linie.Tests
{
    using System;
    using Xunit;

    public class Vector3Tests
    {
        [Fact]
        public void TestCtor()
        {
            var u = new Vector3(1, 2, 3);
            Assert.Equal(1, u.X);
            Assert.Equal(2, u.Y);
            Assert.Equal(3, u.Z);
        }
    }
}
