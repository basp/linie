namespace Linie.TheNextWeek
{
    using System.Collections.Generic;

    public class Group : IGeometricObject
    {
        public readonly IList<IGeometricObject> Objects =
            new List<IGeometricObject>();

        public bool TryGetBoundingBox(double time0, double time1, out BoundingBox aabb)
        {
            aabb = null;

            if (this.Objects.Count == 0)
            {
                return false;
            }

            var firstBox = true;

            foreach (var obj in this.Objects)
            {
                if (!obj.TryGetBoundingBox(time0, time1, out var tmp))
                {
                    return false;
                }

                aabb = firstBox ? tmp : BoundingBox.SurroundingBox(aabb, tmp);
                firstBox = false;
            }

            return true;
        }

        public bool TryIntersect(
            Ray ray,
            double tmin,
            double tmax,
            ref ShadeRecord sr)
        {
            var hitAnything = false;
            var closestSoFar = tmax;

            ShadeRecord tmp = null;
            foreach (var obj in this.Objects)
            {
                if (obj.TryIntersect(ray, tmin, closestSoFar, ref tmp))
                {
                    hitAnything = true;
                    closestSoFar = tmp.T;
                    sr = tmp;
                }
            }

            return hitAnything;
        }
    }
}