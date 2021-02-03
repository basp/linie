namespace Genie
{
    using System;

    public static class Utils
    {
        public const int BinaryBaseNumber = 2;

        public const int SinglePrecision = 24;

        public static float MachineEpsilon =>
            (float)Math.Pow(BinaryBaseNumber, -SinglePrecision);

        public static float Gamma(int n) =>
            (n * MachineEpsilon) / (1 - (n * MachineEpsilon));

        public static uint SingleToUInt32Bits(float v) =>
            BitConverter.ToUInt32(BitConverter.GetBytes(v), 0);

        public static float UInt32BitsToSingle(uint v) =>
            BitConverter.ToSingle(BitConverter.GetBytes(v), 0);

        public static ulong DoubleToUInt64Bits(double v) =>
            BitConverter.ToUInt64(BitConverter.GetBytes(v), 0);

        public static double UInt64BitsToDouble(ulong v) =>
            BitConverter.ToDouble(BitConverter.GetBytes(v), 0);

        public static float NextFloatUp(float v)
        {
            // guard against +inf
            if (float.IsInfinity(v) && v > 0)
            {
                return v;
            }

            // skip zero when `v` is minus zero
            v = v == -0 ? 0 : v;
            var ui = SingleToUInt32Bits(v);


            // because of unsigned the "up" direction
            // maybe be switched when we convert the float
            // so take care of the up direction
            ui = v > 0 ? ui + 1 : ui - 1;
            return UInt32BitsToSingle(ui);
        }

        public static float NextFloatDown(float v)
        {
            // guard against -inf
            if (float.IsInfinity(v) && v < 0)
            {
                return v;
            }

            // skip minus zero when `v` is zero
            v = v == 0 ? -0 : v;
            var ui = SingleToUInt32Bits(v);

            // take care of the down direction
            ui = v > 0 ? ui - 1 : ui + 1;
            return UInt32BitsToSingle(ui);
        }

        public static double NextDoubleUp(double v)
        {
            if (double.IsInfinity(v) && v > 0)
            {
                return v;
            }

            v = v == -0 ? 0 : v;
            var ui = DoubleToUInt64Bits(v);

            ui = v > 0 ? ui + 1 : ui - 1;
            return UInt64BitsToDouble(ui);
        }

        public static double NextDoubleDown(double v)
        {
            if (double.IsNegativeInfinity(v) && v < 0)
            {
                return v;
            }

            v = v == 0 ? -0 : v;
            var ui = DoubleToUInt64Bits(v);

            ui = v > 0 ? ui - 1 : ui + 1;
            return UInt64BitsToDouble(ui);
        }
    }
}
