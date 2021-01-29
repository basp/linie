namespace Linie.InOneWeekend
{
    using System;

    public class Dielectric : IMaterial
    {
        private readonly double ir;

        public Dielectric(double ir)
        {
            this.ir = ir;
        }

        public bool Scatter(
            in Ray3 rIn,
            in ShadeRecord rec,
            in Random rng,
            out Color attenuation,
            out Ray3 scattered)
        {
            attenuation = new Color(1, 1, 1);
            var refractionRatio = rec.IsFrontFace ? (1.0 / ir) : ir;
            var unitDirection = Vector3.Normalize(rIn.Direction);
            var cosTheta = Math.Min(Vector3.Dot(-unitDirection, rec.Normal), 1);
            var sinTheta = Math.Sqrt(1.0 - cosTheta * cosTheta);
            var cannotRefract = refractionRatio * sinTheta > 1;
            var reflectance = GetReflectance(cosTheta, refractionRatio);
            var mustReflect = cannotRefract || reflectance > rng.RandomDouble();
            var direction = mustReflect
                ? unitDirection.Reflect(rec.Normal)
                : unitDirection.Refract(rec.Normal, refractionRatio);
            var refracted = unitDirection.Refract(rec.Normal, refractionRatio);
            scattered = new Ray3(rec.Point, direction);
            return true;
        }

        private static double GetReflectance(double cos, double idx)
        {
            // Schlick's approximation for reflectance
            var r0 = (1 - idx) / (1 + idx);
            r0 = r0 * r0;
            return r0 + (1 - r0) * Math.Pow((1 - cos), 5);
        }
    }
}