namespace Linie.TheNextWeek
{
    using System;

    public class YZRect : IGeometricObject
    {
        private readonly Material material;

        private readonly double y0, y1, z0, z1, k;

        public YZRect(double x0, double x1, double z0, double z1, double k, Material material)
        {
            this.y0 = x0;
            this.y1 = x1;
            this.z0 = z0;
            this.z1 = z1;
            this.k = k;
            this.material = material;
        }

        public bool TryGetBoundingBox(double time0, double time1, out BoundingBox aabb)
        {
            aabb = new BoundingBox(
                new Point3(this.k - 0.0001, this.y0, this.z0),
                new Point3(this.k + 0.0001, this.y1, this.z1));

            return true;
        }

        public bool TryIntersect(Ray ray, double tmin, double tmax, ref ShadeRecord sr)
        {
            var t = (this.k - ray.Origin.X) / ray.Direction.X;
            if (t < tmin || t > tmax)
            {
                return false;
            }

            var y = ray.Origin.Y + t * ray.Direction.Y;
            var z = ray.Origin.Z + t * ray.Direction.Z;
            if (y < y0 || y > y1 || z < z0 || z > z1)
            {
                return false;
            }

            var u = (y - y0) / (y1 - y0);
            var v = (z - z0) / (z1 - z0);
            var n = new Normal3(1, 0, 0);
            sr = new ShadeRecord
            {
                U = u,
                V = v,
                T = t,
                Material = this.material,
                Point = ray.GetPosition(t),
            };

            sr.SetFaceNormal(ray, n);
            return true;
        }
    }
}