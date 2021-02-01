namespace Linie.TheNextWeek
{
    using System;

    public class MovingSphere : IGeometricObject
    {
        private readonly Point3 center0;

        private readonly Point3 center1;

        private double time0;

        private double time1;

        private readonly double radius;

        private readonly Material material;

        public MovingSphere(
            Point3 center0,
            Point3 center1,
            double time0,
            double time1,
            double radius,
            Material material)
        {
            this.center0 = center0;
            this.center1 = center1;
            this.time0 = time0;
            this.time1 = time1;
            this.radius = radius;
            this.material = material;
        }

        public Point3 GetCenter(double time) =>
            this.center0
            + ((time - time0) / (time1 - time0))
            * (this.center1 - this.center0);

        public bool TryIntersect(
            Ray ray,
            double tmin,
            double tmax,
            ref ShadeRecord sr)
        {
            var oc = ray.Origin - this.GetCenter(ray.Time);
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
                var n = (p - this.GetCenter(ray.Time)) / this.radius;
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

        public bool TryGetBoundingBox(
            double time0,
            double time1,
            out BoundingBox aabb)
        {
            var box0 = new BoundingBox(
                this.GetCenter(time0) - new Vector3(this.radius),
                this.GetCenter(time0) + new Vector3(this.radius));

            var box1 = new BoundingBox(
                this.GetCenter(time1) - new Vector3(this.radius),
                this.GetCenter(time1) + new Vector3(this.radius));

            aabb = BoundingBox.SurroundingBox(box0, box1);
            return true;
        }
    }
}