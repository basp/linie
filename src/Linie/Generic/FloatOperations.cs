namespace Linie.Generic
{
    using System;

    public class FloatOperations : IOperations<float>
    {
        public float Add(float a, float b) => a + b;

        public int Compare(float a, float b) => a.CompareTo(b);

        public float Multiply(float a, float b) => a * b;

        public float Negate(float a) => -a;

        public float Pow(float a, float b) => MathF.Pow(a, b);

        public float Sqrt(float a) => MathF.Sqrt(a);

        public float Subtract(float a, float b) => a - b;
    }
}