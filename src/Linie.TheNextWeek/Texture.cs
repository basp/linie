namespace Linie.TheNextWeek
{
    public interface ITexture
    {
        Color GetColor(double u, double v, in Point3 point);
    }
}