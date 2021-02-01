namespace Linie.TheNextWeek
{
    public class Translate : IGeometricObject
    {
        private readonly IGeometricObject obj;

        private readonly Vector3 offset;

        public Translate(IGeometricObject obj, in Vector3 offset)
        {
            this.obj = obj;
            this.offset = offset;
        }

        public bool TryGetBoundingBox(
            double time0,
            double time1,
            out BoundingBox aabb)
        {
            aabb = null;

            if (!this.obj.TryGetBoundingBox(time0, time1, out var box))
            {
                return false;
            }

            aabb = new BoundingBox(
                box.Minimum + offset,
                box.Maximum + offset);

            return true;
        }

        public bool TryIntersect(
            Ray ray,
            double tmin,
            double tmax,
            ref ShadeRecord sr)
        {
            var movedRay = new Ray(
                ray.Origin - offset,
                ray.Direction,
                ray.Time);

            if (!this.obj.TryIntersect(movedRay, tmin, tmax, ref sr))
            {
                return false;
            }

            sr.Point += offset;
            sr.SetFaceNormal(movedRay, sr.Normal);
            return true;
        }
    }
}