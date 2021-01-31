namespace Linie.TheNextWeek
{
    public class SolidColor : ITexture
    {
        private readonly Color color;

        public SolidColor(Color color)
        {
            this.color = color;
        }

        public SolidColor(double r, double g, double b)
            : this(new Color(r, g, b))
        {
        }

        public Color GetColor(double u, double v, in Point3 point) =>
            this.color;
    }
}