namespace Linie.Generic
{
    using System;

    public class EFloatOperations : IOperations<EFloat>
    {
        public EFloat Add(EFloat a, EFloat b) => a + b;

        public int Compare(EFloat a, EFloat b) =>
            throw new NotImplementedException();

        public EFloat Multiply(EFloat a, EFloat b) => a * b;

        public EFloat Negate(EFloat a) => -a;

        public EFloat Pow(EFloat a, EFloat b) =>
            throw new NotImplementedException();

        public EFloat Sqrt(EFloat a) => EFloat.Sqrt(a);

        public EFloat Subtract(EFloat a, EFloat b) => a - b;
    }
}