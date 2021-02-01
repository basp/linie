namespace Linie.TheNextWeek
{
    using System;

    public abstract class Material
    {
        public virtual Color Emitted(double u, double v, in Point3 p) =>
            new Color(0);

        public abstract bool Scatter(
            in Ray rIn,
            in ShadeRecord rec,
            in Random rng,
            out Color attenuation,
            out Ray scattered);
    }
}