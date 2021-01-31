namespace Linie.TheNextWeek
{
    using System;

    public class NoiseTexture : ITexture
    {
        private Perlin noise = new Perlin();

        private double scale;

        public NoiseTexture(double scale)
        {
            this.scale = scale;
        }

        public Color GetColor(double u, double v, in Point3 p) =>
            // new Color(1) * 0.5 * (1.0 + this.noise.Noise(this.scale * p));
            // new Color(1) * this.noise.Turbulence(scale * p);
            new Color(1) * 0.5 * (1 + Math.Sin(scale * p.Z + 10 * this.noise.Turbulence(p)));
    }
}