namespace Linie.TheNextWeek
{
    using System;

    public class BvhNode : IGeometricObject
    {
        public BvhNode(
            Group group,
            int start,
            int end,
            double time0,
            double time1)
        {
        }

        public IGeometricObject Left { get; private set; }

        public IGeometricObject Right { get; private set; }

        public bool TryGetBoundingBox(
            double time0,
            double time1,
            out BoundingBox aabb)
        {
            throw new NotImplementedException();
        }

        public bool TryIntersect(
            Ray ray,
            double tmin,
            double tmax,
            out ShadeRecord sr)
        {
            throw new NotImplementedException();
        }
    }
}