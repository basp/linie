namespace Linie.TheNextWeek
{
    public class Box : IGeometricObject
    {
        private readonly Point3 boxMin;

        private readonly Point3 boxMax;

        private readonly Group sides;

        public Box(Point3 p0, Point3 p1, Material material)
        {
            this.boxMin = p0;
            this.boxMax = p1;
            this.sides = new Group();
            this.sides.Objects.Add(
                new XYRect(p0.X, p1.X, p0.Y, p1.Y, p1.Z, material));
            this.sides.Objects.Add(
                new XYRect(p0.X, p1.X, p0.Y, p1.Y, p0.Z, material));
            this.sides.Objects.Add(
                new XZRect(p0.X, p1.X, p0.Z, p1.Z, p1.Y, material));
            this.sides.Objects.Add(
                new XZRect(p0.X, p1.X, p0.Z, p1.Z, p0.Y, material));
            this.sides.Objects.Add(
                new YZRect(p0.Y, p1.Y, p0.Z, p1.Z, p1.X, material));
            this.sides.Objects.Add(
                new YZRect(p0.Y, p1.Y, p0.Z, p1.Z, p0.X, material));
        }

        public bool TryGetBoundingBox(
            double time0,
            double time1,
            out BoundingBox aabb)
        {
            aabb = new BoundingBox(this.boxMin, this.boxMax);
            return true;
        }

        public bool TryIntersect(
            Ray ray,
            double tmin,
            double tmax,
            ref ShadeRecord sr) => 
                this.sides.TryIntersect(ray, tmin, tmax, ref sr);
    }
}