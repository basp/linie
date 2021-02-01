namespace Linie.TheNextWeek
{
    using System;

    public class XZRect : IGeometricObject
    {
        private readonly Material material;

        private readonly double x0, x1, z0, z1, k;

        public XZRect(double x0, double x1, double z0, double z1, double k, Material material)
        {
            this.x0 = x0;
            this.x1 = x1;
            this.z0 = z0;
            this.z1 = z1;
            this.k = k;
            this.material = material;
        }

        public bool TryGetBoundingBox(double time0, double time1, out BoundingBox aabb)
        {
            aabb = new BoundingBox(
                new Point3(this.x0, this.k - 0.0001, this.z0),
                new Point3(this.x1, this.k + 0.0001, this.z1));

            return true;
        }

        public bool TryIntersect(Ray ray, double tmin, double tmax, ref ShadeRecord sr)
        {
            var t = (this.k - ray.Origin.Y) / ray.Direction.Y;
            if (t < tmin || t > tmax)
            {
                return false;
            }

            var x = ray.Origin.X + t * ray.Direction.X;
            var z = ray.Origin.Z + t * ray.Direction.Z;
            if (x < x0 || x > x1 || z < z0 || z > z1)
            {
                return false;
            }

            var u = (x - x0) / (x1 - x0);
            var v = (z - z0) / (z1 - z0);
            var n = new Normal3(0, 1, 0);
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