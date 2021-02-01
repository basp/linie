using System;

namespace Linie.TheNextWeek
{
    public class Isotropic : Material
    {
        private readonly ITexture albedo;

        public Isotropic(Color c)
            : this(new SolidColor(c))
        {
        }

        public Isotropic(ITexture albedo)
        {
            this.albedo = albedo;
        }

        public override bool Scatter(
            in Ray rIn,
            in ShadeRecord rec,
            in Random rng,
            out Color attenuation,
            out Ray scattered)
        {
            scattered = new Ray(rec.Point, rng.RandomInUnitSphere(), rIn.Time);
            attenuation = this.albedo.GetColor(rec.U, rec.V, rec.Point);
            return true;
        }
    }
}