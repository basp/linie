namespace Linie.InOneWeekend
{
    using System;

    public static class Utils
    {
        public static double Clamp(in double x, in double min, in double max)
        {
            if (x < min)
            {
                return min;
            }

            if (x > max)
            {
                return max;
            }

            return x;
        }

        public static bool IsNearZero(this Vector3 u)
        {
            const double s = 1e-8;
            return
                Math.Abs(u.X) < s &&
                Math.Abs(u.Y) < s &&
                Math.Abs(u.Z) < s;
        }

        public static Vector3 RandomVector(this Random rng) =>
            new Vector3(
                rng.RandomDouble(),
                rng.RandomDouble(),
                rng.RandomDouble());

        public static Vector3 RandomVector(this Random rng, in double min, in double max) =>
            new Vector3(
                rng.RandomDouble(min, max),
                rng.RandomDouble(min, max),
                rng.RandomDouble(min, max));

        public static Vector3 RandomUnitVector(this Random rng) =>
            Vector3.Normalize(rng.RandomInUnitSphere());

        public static Vector3 RandomInHemisphere(this Random rng, in Normal3 n)
        {
            var inUnitSphere = rng.RandomInUnitSphere();
            if (Vector3.Dot(inUnitSphere, (Vector3)n) > 0)
            {
                return inUnitSphere;
            }

            return -inUnitSphere;
        }

        public static Vector3 RandomInUnitSphere(this Random rng)
        {
            while (true)
            {
                var u = rng.RandomVector(-1, 1);
                if (Vector3.MagnitudeSquared(u) >= 1)
                {
                    continue;
                }

                return u;
            }
        }

        public static double RandomDouble(this Random rng) =>
            rng.NextDouble();

        public static double RandomDouble(
            this Random rng,
            double min,
            double max) =>
                min + (max - min) * rng.RandomDouble();

        public static double DegreesToRadians(double degrees) =>
            degrees * Math.PI / 180.0;
    }
}