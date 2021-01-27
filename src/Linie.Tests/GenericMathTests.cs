using Xunit;
using Linie.Generic;

namespace Linie.Tests
{
    using vec2 = Linie.Generic.Vector2;
    using mathf = Vector2Math<float, FloatOperations>;
    using math = Vector2Math<double, DoubleOperations>;

    public class GenericVectorTests
    {
        [Fact]
        public void AddFloatVectors()
        {
            var u = vec2.Create(1f, 2f);
            var v = vec2.Create(1f, 2f);
            var w = mathf.Add(u, v);
            Assert.Equal(2, w.X);
            Assert.Equal(4, w.Y);
        }

        [Fact]
        public void AddDoubleVectors()
        {
            var u = vec2.Create(1.0, 2.0);
            var v = vec2.Create(1.0, 2.0);
            var w = math.Add(u, v);
            Assert.Equal(2, w.X);
            Assert.Equal(4, w.Y);
        }
    }
}