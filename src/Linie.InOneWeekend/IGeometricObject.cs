namespace Linie.InOneWeekend
{

    public interface IGeometricObject
    {
        bool TryIntersect(
            Ray3 ray, 
            double tmin, 
            double tmax, 
            out ShadeRecord sr);
    }
}