namespace Linie.TheNextWeek
{
    using System;

    public class XYRect : IGeometricObject
    {
        private readonly Material material;

        private readonly double x0, x1, y0, y1, k;

        public XYRect(double x0, double x1, double y0, double y1, double k, Material material)
        {
            this.x0 = x0;
            this.x1 = x1;
            this.y0 = y0;
            this.y1 = y1;
            this.k = k;
            this.material = material;
        }

        public bool TryGetBoundingBox(double time0, double time1, out BoundingBox aabb)
        {
            aabb =new BoundingBox(
                new Point3(this.x0, this.y0, this.k - 0.0001),
                new Point3(this.x1, this.y1, this.k + 0.0001));

            return true;            
        }

        public bool TryIntersect(Ray ray, double tmin, double tmax, ref ShadeRecord sr)
        {
            var t = (this.k - ray.Origin.Z) / ray.Direction.Z;
            if (t < tmin || t > tmax)
            {
                return false;
            }

            var x = ray.Origin.X + t * ray.Direction.X;
            var y = ray.Origin.Y + t * ray.Direction.Y;
            if(x < x0 || x > x1 || y < y0 || y > y1)
            {
                return false;
            }

            var u = (x - x0) / (x1 - x0);
            var v = (y - y0) / (y1 - y0);
            var n = new Normal3(0, 0, 1);
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