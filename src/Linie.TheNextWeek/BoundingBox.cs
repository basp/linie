using System;
using System.Threading;

namespace Linie.TheNextWeek
{
    public class BoundingBox
    {
        public readonly Point3 Minimum;

        public readonly Point3 Maximum;

        public BoundingBox(Point3 min, Point3 max)
        {
            this.Minimum = min;
            this.Maximum = max;
        }

        public static BoundingBox SurroundingBox(
            BoundingBox box0,
            BoundingBox box1)
        {
            var small = new Point3(
                Math.Min(box0.Minimum.X, box1.Minimum.X),
                Math.Min(box0.Minimum.Y, box1.Minimum.Y),
                Math.Min(box0.Minimum.Z, box1.Minimum.Z));

            var big = new Point3(
                Math.Max(box0.Maximum.X, box1.Maximum.X),
                Math.Max(box0.Maximum.Y, box1.Maximum.Y),
                Math.Max(box0.Maximum.Z, box1.Maximum.Z));

            return new BoundingBox(small, big);
        }

        public bool TryIntersect(in Ray ray, double tmin, double tmax)
        {
            for (var a = 0; a < 3; a++)
            {
                var invD = 1.0 / ray.Direction[a];
                var t0 = (this.Minimum[a] - ray.Origin[a]) * invD;
                var t1 = (this.Maximum[a] - ray.Origin[a]) * invD;
                if (invD < 0)
                {
                    (t0, t1) = (t1, t0);
                }

                tmin = t0 > tmin ? t0 : tmin;
                tmax = t1 < tmax ? t1 : tmax;

                if (tmax <= tmin)
                {
                    return false;
                }
            }

            return true;
        }
    }
}