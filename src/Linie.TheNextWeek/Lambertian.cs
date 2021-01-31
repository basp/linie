namespace Linie.TheNextWeek
{
    using System;

    public class Lambertian : IMaterial
    {
        private readonly ITexture albedo;

        public Lambertian(Color albedo)
            : this(new SolidColor(albedo))
        {
        }

        public Lambertian(ITexture albedo)
        {
            this.albedo = albedo;
        }

        public bool Scatter(
            in Ray rIn,
            in ShadeRecord rec,
            in Random rng,
            out Color attenuation,
            out Ray scattered)
        {
            var direction = (Vector3)rec.Normal + rng.RandomUnitVector();
            if (direction.IsNearZero())
            {
                direction = (Vector3)rec.Normal;
            }

            scattered = new Ray(rec.Point, direction, rIn.Time);
            attenuation = this.albedo.GetColor(rec.U, rec.V, rec.Point);
            return true;
        }
    }
}