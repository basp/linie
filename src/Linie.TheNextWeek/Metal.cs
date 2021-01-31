using System;

namespace Linie.TheNextWeek
{
    public class Metal : IMaterial
    {
        private readonly Color albedo;

        private readonly double fuzz;

        public Metal(Color albedo, double fuzz = 0)
        {
            this.albedo = albedo;
            this.fuzz = fuzz;
        }

        public bool Scatter(
            in Ray rIn,
            in ShadeRecord rec,
            in Random rng,
            out Color attenuation,
            out Ray scattered)
        {
            var reflected = Vector3.Reflect(rIn.Direction, rec.Normal);
            var direction = reflected + this.fuzz * rng.RandomInUnitSphere();
            scattered = new Ray(rec.Point, direction, rIn.Time);
            attenuation = this.albedo;
            return Vector3.Dot(scattered.Direction, rec.Normal) > 0;
        }
    }
}