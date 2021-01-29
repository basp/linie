namespace Linie.InOneWeekend
{
    using System;

    public class Lambertian : IMaterial
    {
        private readonly Color albedo;

        public Lambertian(Color albedo)
        {
            this.albedo = albedo;
        }

        public bool Scatter(
            in Ray3 rIn,
            in ShadeRecord rec,
            in Random rng,
            ref Color attenuation,
            out Ray3 scattered)
        {
            var direction = (Vector3)rec.Normal + rng.RandomUnitVector();
            if (direction.IsNearZero())
            {
                direction = (Vector3)rec.Normal;
            }

            scattered = new Ray3(rec.Point, direction);
            attenuation = this.albedo;
            return true;
        }
    }
}