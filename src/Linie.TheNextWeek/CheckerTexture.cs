namespace Linie.TheNextWeek
{
    using System;

    public class CheckerTexture : ITexture
    {
        public CheckerTexture(Color c1, Color c2)
            : this(new SolidColor(c1), new SolidColor(c2))
        {
        }

        public CheckerTexture(ITexture odd, ITexture even)
        {
            this.Odd = odd;
            this.Even = even;
        }

        public ITexture Odd { get; private set; }

        public ITexture Even { get; private set; }

        public Color GetColor(double u, double v, in Point3 p)
        {
            var sines =
                Math.Sin(10 * p.X) *
                Math.Sin(10 * p.Y) *
                Math.Sin(10 * p.Z);

            if (sines < 0)
            {
                return this.Odd.GetColor(u, v, p);
            }
            else
            {
                return this.Even.GetColor(u, v, p);
            }
        }
    }
}