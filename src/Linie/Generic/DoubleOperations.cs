namespace Linie.Generic
{
    using System;

    public class DoubleOperations : IOperations<double>
    {
        public double Add(double a, double b) => a + b;

        public int Compare(double a, double b) => a.CompareTo(b);

        public double Multiply(double a, double b) => a * b;

        public double Negate(double a) => -a;

        public double Pow(double a, double b) => Math.Pow(a, b);

        public double Sqrt(double a) => Math.Sqrt(a);

        public double Subtract(double a, double b) => a - b;
    }
}