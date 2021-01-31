namespace Linie.TheNextWeek
{

    public interface IGeometricObject
    {
        bool TryIntersect(
            Ray ray, 
            double tmin, 
            double tmax, 
            out ShadeRecord sr);

        bool TryGetBoundingBox(
            double time0,
            double time1,
            out BoundingBox aabb); 
    }
}