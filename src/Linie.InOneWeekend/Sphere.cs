namespace Linie.InOneWeekend
{
    using System;

    public class Sphere : IGeometricObject
    {
        private readonly Point3 center;

        private readonly double radius;

        private readonly IMaterial material;

        public Sphere(Point3 center, double radius, IMaterial material)
        {
            this.center = center;
            this.radius = radius;
            this.material = material;
        }

        public bool TryIntersect(
            Ray3 ray,
            double tmin,
            double tmax,
            out ShadeRecord sr)
        {
            sr = null;

            var oc = ray.Origin - this.center;
            var a = Vector3.MagnitudeSquared(ray.Direction);
            var halfB = Vector3.Dot(oc, ray.Direction);
            var c = Vector3.MagnitudeSquared(oc) - this.radius * this.radius;

            var discriminant = halfB * halfB - a * c;
            if (discriminant < 0)
            {
                return false;
            }

            var sqrtd = Math.Sqrt(discriminant);
            var root = (-halfB - sqrtd) / a;
            if (root < tmin || tmax < root)
            {
                root = (-halfB + sqrtd) / a;
                if (root < tmin || tmax < root)
                {
                    return false;
                }
            }

            ShadeRecord CreateShadeRecord(double t)
            {
                var p = ray.GetPosition(t);
                var n = (p - this.center) / this.radius;
                var sr = new ShadeRecord
                {
                    Point = p,
                    T = t,
                    Material = this.material,
                };

                sr.SetFaceNormal(ray, (Normal3)n);
                return sr;
            }

            sr = CreateShadeRecord(root);
            return true;
        }
    }
}