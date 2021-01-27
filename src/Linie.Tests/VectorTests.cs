namespace Linie.Tests
{
    using System;
    using Xunit;
    using Linie.Generic;

    public class VectorTests
    {
        [Fact]
        public void Sandbox()
        {
            var u = Vector2.Create(1.0f, 2);
            var v = Vector2.Create(1.0f, 2);
            // var w = Vector2Math<float, FloatOperations>.Add(u, v);
        }
    }
}