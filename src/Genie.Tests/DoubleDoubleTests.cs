namespace Genie.Tests
{
    using Xunit;

    public class DoubleDoubleTests
    {
        [Fact]
        public void AddDoubleDoubles()
        {
            var a = new DoubleDouble(5);
            var b = new DoubleDouble(3);
            Assert.Equal(8, a + b);
        }

        [Fact]
        public void SubtractDoubleDoubles()
        {
            var a = new DoubleDouble(5);
            var b = new DoubleDouble(3);
            Assert.Equal(2, a - b);
        }
    }
}