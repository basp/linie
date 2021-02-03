namespace Genie
{
    using System;
    using System.Diagnostics;

    public struct DoubleDouble : IComparable<DoubleDouble>
    {
        private readonly double x, y;

        public DoubleDouble(double x)
            : this(x, 0)
        {
        }

        public DoubleDouble(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static DoubleDouble operator +(DoubleDouble a, DoubleDouble b)
        {
            var r = TwoSum(a.x, b.x, out var e);
            e += a.y + b.y;
            r = QuickTwoSum(r, e, out e);
            return new DoubleDouble(r, e);
        }

        public static DoubleDouble operator -(DoubleDouble a) =>
            new DoubleDouble(-a.x, -a.y);

        public static DoubleDouble operator -(DoubleDouble a, DoubleDouble b) =>
            a + (-b);

        public static DoubleDouble operator *(DoubleDouble a, DoubleDouble b)
        {
            var r = TwoProd(a.x, b.x, out var e);
            e += a.x * b.y + a.y * b.x;
            r = QuickTwoSum(r, e, out e);
            return new DoubleDouble(r, e);
        }

        public static DoubleDouble operator /(DoubleDouble a, DoubleDouble b)
        {
            var r = a.x / b.x;
            var s = TwoProd(r, b.x, out var f);
            var e = (a.x - s - f + a.y - r * b.y) / b.x;
            r = QuickTwoSum(r, e, out e);
            return new DoubleDouble(r, e);
        }

        public static bool operator <(DoubleDouble a, DoubleDouble b) =>
            a.x < b.x || (a.x == b.x && a.y < b.y);

        public static bool operator >(DoubleDouble a, DoubleDouble b) =>
            a.x > b.x || (a.x == b.x && a.y > b.y);

        public static bool operator <=(DoubleDouble a, DoubleDouble b) =>
            a.x < b.x || (a.x == b.x && a.y <= b.y);

        public static bool operator >=(DoubleDouble a, DoubleDouble b) =>
            a.x > b.x || (a.x == b.x && a.y >= b.y);

        public static bool operator ==(DoubleDouble a, DoubleDouble b) =>
            a.x == b.x && a.y == b.y;

        public static bool operator !=(DoubleDouble a, DoubleDouble b) =>
            a.x != b.x || a.y != b.y;

        public static implicit operator DoubleDouble(double a) =>
            new DoubleDouble(a, 0);

        public static explicit operator double(DoubleDouble a) => a.x;

        public static DoubleDouble Sqrt(DoubleDouble a)
        {
            if (a.x == 0)
            {
                return new DoubleDouble(0);
            }

            var r = Math.Sqrt(a.x);
            var s = TwoProd(r, r, out var f);
            var e = (a.x - s - f + a.y) * 0.5 / r;
            r = QuickTwoSum(r, e, out e);
            return r;
        }

        public static DoubleDouble Pow(DoubleDouble a, double n)
        {
            throw new NotImplementedException();
        }

        public override string ToString() => ((float)this).ToString();

        public override bool Equals(object obj)
        {
            if (!(obj is DoubleDouble))
            {
                return false;
            }

            var other = (DoubleDouble)obj;
            return this == other;
        }

        public override int GetHashCode() =>
            HashCode.Combine(this.x, this.y);

        public int CompareTo(DoubleDouble other)
        {
            if (this < other)
            {
                return -1;
            }

            if (this > other)
            {
                return 1;
            }

            return 0;
        }
        static double QuickTwoSum(double a, double b, out double e)
        {
            Debug.Assert(a >= b);
            var s = a + b;
            e = b - (s - a);
            return s;
        }

        static double TwoSum(double a, double b, out double e)
        {
            var s = a + b;
            var v = s - a;
            e = (a - (s - v)) + (b - v);
            return s;
        }

        static (double, double) Split(double a)
        {
            var t = 134217729 * a; // 134217729 = 2^27 + 1
            var ahi = t - (t - a);
            var alo = a - ahi;
            return (ahi, alo);
        }

        static double TwoProd(double a, double b, out double e)
        {
            var p = a * b;
            var (ahi, alo) = Split(a);
            var (bhi, blo) = Split(b);
            e = ((ahi * bhi - p) + (ahi * blo) + (alo * bhi)) + (alo * blo);
            return p;
        }
    }
}
