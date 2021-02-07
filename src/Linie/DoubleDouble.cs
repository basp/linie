namespace Linie
{
    using System;
    using System.Diagnostics;

    public struct DoubleDouble : IComparable<DoubleDouble>
    {
        public static readonly DoubleDouble E =
            new DoubleDouble(2.718281828459045, 1.4456468917292502e-16);

        public static readonly DoubleDouble PI =
            new DoubleDouble(3.141592653589793, 1.2246467991473532e-16);

        public static readonly DoubleDouble Zero = new DoubleDouble(0);

        public static readonly DoubleDouble One = new DoubleDouble(1);

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
            var (r, e) = TwoSum(a.x, b.x);
            e += a.y + b.y;
            (r, e) = QuickTwoSum(r, e);
            return new DoubleDouble(r, e);
        }

        public static DoubleDouble operator -(DoubleDouble a) =>
            new DoubleDouble(-a.x, -a.y);

        public static DoubleDouble operator -(DoubleDouble a, DoubleDouble b) =>
            a + (-b);

        public static DoubleDouble operator *(DoubleDouble a, DoubleDouble b)
        {
            var (r, e) = TwoProd(a.x, b.x);
            e += a.x * b.y + a.y * b.x;
            (r, e) = QuickTwoSum(r, e);
            return new DoubleDouble(r, e);
        }

        public static DoubleDouble operator /(DoubleDouble a, DoubleDouble b)
        {
            var r = a.x / b.x;
            var (s, f) = TwoProd(r, b.x);
            var e = (a.x - s - f + a.y - r * b.y) / b.x;
            (r, e) = QuickTwoSum(r, e);
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
                return Zero;
            }

            var r = Math.Sqrt(a.x);
            var (s, f) = TwoProd(r, r);
            var e = (a.x - s - f + a.y) * 0.5 / r;
            (r, e) = QuickTwoSum(r, e);
            return new DoubleDouble(r, e);
        }

        public static DoubleDouble Pow(DoubleDouble a, int n)
        {
            var (b, i) = (a, (int)Math.Abs(n));
            var r = One;
            while (true)
            {
                if ((i & 1) == 1)
                {
                    r *= b;
                }

                if (i <= 1)
                {
                    break;
                }

                i >>= 1;
                b *= b;
            }

            if (n < 0)
            {
                return One / r;
            }

            return r;
        }

        public static DoubleDouble Exp(DoubleDouble a)
        {
            var n = (int)(Math.Round(a.x));
            var x = a - n;
            var u = (((((((((((x +
                156) * x + 12012) * x +
                600600) * x + 21621600) * x +
                588107520) * x + 12350257920) * x +
                201132771840) * x + 2514159648000) * x +
                23465490048000) * x + 154872234316800) * x +
                647647525324800) * x + 1295295050649600;
            var v = (((((((((((x -
                156) * x + 12012) * x -
                600600) * x + 21621600) * x -
                588107520) * x + 12350257920) * x -
                201132771840) * x + 2514159648000) * x -
                23465490048000) * x + 154872234316800) * x -
                647647525324800) * x + 1295295050649600;
            return Pow(E, n) * (u / v);
        }

        public static DoubleDouble Log(DoubleDouble a)
        {
            var r = new DoubleDouble(Math.Log(a.x));
            var u = Exp(r);
            r -= (u - a) / (u + a);
            return r;
        }

        public static bool TryParse(string s, out DoubleDouble x)
        {
            x = Zero;
            s = s.Trim();
            var point = -1;
            var nd = 0;
            var e = 0;
            foreach (var ch in s)
            {
                if (ch >= '0' && ch <= '9')
                {
                    var d = ch - '0';
                    x *= 10;
                    x += d;
                    nd++;
                    continue;
                }

                switch (ch)
                {
                    case '.':
                        point = nd;
                        break;
                    default:
                        return false;
                }
            }

            if (point >= 0)
            {
                e -= (nd - point);
            }

            if (e != 0)
            {
                x *= DoubleDouble.Pow(new DoubleDouble(10), e);
            }

            return true;
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

        static (double, double) QuickTwoSum(double x, double y)
        {
            Debug.Assert(Math.Abs(x) >= Math.Abs(y));
            var r = x + y;
            var e = y - (r - x);
            return (r, e);
        }

        static (double, double) TwoSum(double x, double y)
        {
            var r = x + y;
            var t = r - x;
            var e = (x - (r - t)) + (y - t);
            return (r, e);
        }

        static (double, double) Split(double a)
        {
            var t = 134217729 * a; // 134217729 = 2^27 + 1
            var ahi = t - (t - a);
            var alo = a - ahi;
            return (ahi, alo);
        }

        static (double, double) TwoProd(double a, double b)
        {
            var r = a * b;
            var (ahi, alo) = Split(a);
            var (bhi, blo) = Split(b);
            var e = ((ahi * bhi - r) + (ahi * blo) + (alo * bhi)) + (alo * blo);
            return (r, e);
        }
    }
}
