namespace Genie
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// An experimental struct that can be used to keep track
    /// of local calculation errors. Can be used as an alternative
    /// to a global epsilon.
    /// </summary>
    public struct EFloat : IComparable<EFloat>
    {
        private float v;

        private float low;

        private float high;

        private double vp;

        public EFloat(float v, float err = 0.0f)
        {
            if (err == 0)
            {
                this.low = v;
                this.high = v;
            }
            else
            {
                this.low = Utils.NextFloatDown(v);
                this.high = Utils.NextFloatUp(v);
            }

            this.v = v;
#if DEBUG
            this.vp = v;
#endif
        }

        public EFloat(float v, double vp, float err = 0.0f)
            : this(v, err)
        {
#if DEBUG
            this.vp = vp;
#else
            throw new NotImplementedException();
#endif
        }

        private EFloat(float v, float low, float high)
        {
            this.v = v;
            this.low = low;
            this.high = high;
            this.vp = 0;
        }

#if DEBUG
        private EFloat(float v, double vp, float low, float high)
            : this(v, low, high)
        {
            this.vp = vp;
        }
#endif
        public float LowerBound => this.low;

        public float UpperBound => this.high;

#if DEBUG
        public double PreciseValue => this.vp;
#else
        public double PreciseValue =>
            throw new NotImplementedException();
#endif

        public static EFloat operator +(EFloat a, EFloat b)
        {
            var v = a.v + b.v;
            var low = Utils.NextFloatDown(a.LowerBound + b.LowerBound);
            var high = Utils.NextFloatUp(a.UpperBound + b.UpperBound);
#if DEBUG
            var vp = a.vp + b.vp;
            EFloat.Validate(v, vp, low, high);
            return new EFloat(v, vp, low, high);
#else
            return new EFloat(v, low, high);
#endif
        }

        public static EFloat operator -(EFloat a, EFloat b)
        {
            var v = a.v - b.v;
            var low = Utils.NextFloatDown(a.LowerBound - b.UpperBound);
            var high = Utils.NextFloatUp(a.UpperBound - b.LowerBound);

#if DEBUG
            var vp = a.vp - b.vp;
            EFloat.Validate(v, vp, low, high);
            return new EFloat(v, vp, low, high);
#else
            return new EFloat(v, low, high);
#endif
        }

        public static EFloat operator *(EFloat a, EFloat b)
        {
            var v = a.v * b.v;
            var prod = new[]
            {
                a.LowerBound * b.LowerBound,
                a.UpperBound * b.LowerBound,
                a.LowerBound * b.UpperBound,
                a.UpperBound * b.UpperBound,
            };

            var low = Utils.NextFloatDown(
                MathF.Min(
                    MathF.Min(prod[0], prod[1]),
                    MathF.Min(prod[2], prod[3])));

            var high = Utils.NextFloatUp(
                MathF.Max(
                    MathF.Max(prod[0], prod[1]),
                    MathF.Max(prod[2], prod[3])));

#if DEBUG
            var vp = a.vp * b.vp;
            EFloat.Validate(v, vp, low, high);
            return new EFloat(v, vp, low, high);
#else
            return new EFloat(v, low, high);
#endif
        }

        public static EFloat operator /(EFloat a, EFloat b)
        {
            var v = a.v / b.v;

            float low, high;
            if (b.low < 0 && b.high > 0)
            {
                // Guard against divide by zero;
                // return interval of everything
                low = float.NegativeInfinity;
                high = float.PositiveInfinity;
            }
            else
            {
                var div = new[]
                {
                    a.LowerBound / b.LowerBound,
                    a.UpperBound / b.LowerBound,
                    a.LowerBound / b.UpperBound,
                    a.UpperBound / b.UpperBound,
                };

                low = Utils.NextFloatDown(
                    MathF.Min(
                        MathF.Min(div[0], div[1]),
                        MathF.Min(div[2], div[3])));

                high = Utils.NextFloatUp(
                    MathF.Max(
                        MathF.Max(div[0], div[1]),
                        MathF.Max(div[2], div[3])));
            }

#if DEBUG
            var vp = a.vp / b.vp;
            EFloat.Validate(v, vp, low, high);
            return new EFloat(v, vp, low, high);
#else
            return new EFloat(v, low, high);
#endif
        }

        public static EFloat operator -(EFloat a)
        {
            var v = -a.v;
            var low = -a.high;
            var high = -a.low;
#if DEBUG
            var vp = -a.vp;
            EFloat.Validate(v, vp, low, high);
            return new EFloat(v, vp, low, high);
#else
            return new EFloat(v, low, high);
#endif
        }

        public static EFloat operator +(EFloat a, int b) => a + new EFloat(b);

        public static EFloat operator -(EFloat a, int b) => a - new EFloat(b);

        public static EFloat operator *(EFloat a, int b) => a * new EFloat(b);

        public static EFloat operator /(EFloat a, int b) => a / new EFloat(b);

        public static EFloat operator +(int a, EFloat b) => new EFloat(a) + b;

        public static EFloat operator -(int a, EFloat b) => new EFloat(a) - b;

        public static EFloat operator *(int a, EFloat b) => new EFloat(a) * b;

        public static EFloat operator /(int a, EFloat b) => new EFloat(a) / b;

        public static EFloat operator +(EFloat a, float b) => a + new EFloat(b);

        public static EFloat operator -(EFloat a, float b) => a - new EFloat(b);

        public static EFloat operator *(EFloat a, float b) => a * new EFloat(b);

        public static EFloat operator /(EFloat a, float b) => a / new EFloat(b);

        public static EFloat operator +(float a, EFloat b) => new EFloat(a) + b;

        public static EFloat operator -(float a, EFloat b) => new EFloat(a) - b;

        public static EFloat operator *(float a, EFloat b) => new EFloat(a) * b;

        public static EFloat operator /(float a, EFloat b) => new EFloat(a) / b;

        public static EFloat Abs(EFloat f)
        {
            float v, low, high;
            double vp;

            // the entire interval is greater than zero
            if (f.low >= 0)
            {
                return f;
            }

            // the entire interval is less than zero
            if (f.high <= 0)
            {
                v = -f.v;
                low = -f.high;
                high = -f.low;
#if DEBUG
                vp = -f.vp;
                return new EFloat(v, vp, low, high);
#else
                return new EFloat(v, low, high);
#endif
            }

            // the interval straddles zero
            v = MathF.Abs(f.v);
            low = 0;
            high = MathF.Max(-f.low, f.high);
#if DEBUG
            vp = Math.Abs(f.vp);
            EFloat.Validate(v, vp, low, high);
            return new EFloat(v, vp, low, high);
#else
            return new EFloat(v, low, high);
#endif

        }

        // Negative values of `f` are not supported (yet)
        public static EFloat Sqrt(EFloat f)
        {
            var v = MathF.Sqrt(f.v);
            var low = Utils.NextFloatDown(MathF.Sqrt(f.low));
            var high = Utils.NextFloatUp(MathF.Sqrt(f.high));
#if DEBUG
            var vp = Math.Sqrt(f.vp);
            EFloat.Validate(v, vp, low, high);
            return new EFloat(v, vp, low, high);
#else
            return new EFloat(v, low, high);
#endif
        }

        public float GetAbsoluteError() =>
            Utils.NextFloatUp(
                MathF.Max(
                    MathF.Abs(this.high - this.v),
                    MathF.Abs(this.v - this.low)));

#if DEBUG
        public float GetRelativeError() =>
            (float)Math.Abs((this.vp - this.v) / this.vp);
#else
        public float GetRelativeError() =>
            throw new NotImplementedException();            
#endif

#if DEBUG
        public override string ToString() =>
            $"EFloat({this.v}, vp: {this.vp}, eta: {this.GetRelativeError()})";
#else
        public override string ToString() =>
            $"EFloat({this.v})";
#endif

        private static void Validate(float v, double vp, float low, float high)
        {
#if DEBUG
            Debug.Assert(
                low <= high,
                "lower bound must be less than or equal to upper bound");

            Debug.Assert(
                low <= v,
                "lower bound must be less than or equal to value");

            Debug.Assert(
                high >= v,
                "upper bound must be less than or equal to value");

            Debug.Assert(
                low <= vp,
                "lower bound must be less than or equal to precision value");

            Debug.Assert(
                high >= vp,
                "upper bound must be less than or equal to precision value");
#else
            throw new NotImplementedException();
#endif
        }

        public int CompareTo(EFloat other) =>
            // TODO: probably should involve lower and upper bound here
            this.v.CompareTo(other.v);
    }
}
