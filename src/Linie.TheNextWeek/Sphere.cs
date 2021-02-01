namespace Linie.TheNextWeek
{
    using System;

    public class Sphere : IGeometricObject
    {
        private readonly Point3 center;

        private readonly double radius;

        private readonly Material material;

        public Sphere(Point3 center, double radius, Material material)
        {
            this.center = center;
            this.radius = radius;
            this.material = material;
        }

        public bool TryGetBoundingBox(
            double time0,
            double time1,
            out BoundingBox aabb)
        {
            aabb = new BoundingBox(
                this.center - new Vector3(this.radius),
                this.center + new Vector3(this.radius));

            return true;
        }

        public bool TryIntersect(
            Ray ray,
            double tmin,
            double tmax,
            ref ShadeRecord sr)
        {
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
                (sr.U, sr.V) = GetSphereUV((Point3)n);
                return sr;
            }

            sr = CreateShadeRecord(root);
            return true;
        }

        private static (double, double) GetSphereUV(in Point3 p)
        {
            var theta = Math.Acos(-p.Y);
            var phi = Math.Atan2(-p.Z, p.X) + Math.PI;
            var u = phi / (2 * Math.PI);
            var v = theta / Math.PI;
            return (u, v);
        }
    }
}