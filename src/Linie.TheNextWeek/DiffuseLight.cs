using System;

namespace Linie.TheNextWeek
{
    public class DiffuseLight : Material
    {
        private readonly ITexture emit;

        public DiffuseLight(Color c)
            : this(new SolidColor(c))
        {
        }

        public DiffuseLight(ITexture emit)
        {
            this.emit = emit;
        }

        public override bool Scatter(
            in Ray rIn,
            in ShadeRecord rec,
            in Random rng,
            out Color attenuation,
            out Ray scattered)
        {
            attenuation = new Color();
            scattered = new Ray();
            return false;
        }

        public override Color Emitted(double u, double v, in Point3 p) =>
            this.emit.GetColor(u, v, p);
    }
}