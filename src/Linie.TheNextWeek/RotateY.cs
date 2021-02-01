using System;

namespace Linie.TheNextWeek
{
    public class RotateY : IGeometricObject
    {
        private readonly IGeometricObject obj;

        private readonly double sinTheta;

        private readonly double cosTheta;

        private bool hasBox;

        private BoundingBox bbox;

        public RotateY(IGeometricObject obj, double angle)
        {
            var radians = Utils.DegreesToRadians(angle);
            this.obj = obj;
            this.sinTheta = Math.Sin(radians);
            this.cosTheta = Math.Cos(radians);
            this.hasBox = this.obj.TryGetBoundingBox(0, 1, out this.bbox);

            var min = new[]
            {
                double.PositiveInfinity,
                double.PositiveInfinity,
                double.PositiveInfinity,
            };

            var max = new[]
            {
                double.NegativeInfinity,
                double.NegativeInfinity,
                double.NegativeInfinity,
            };

            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    for (var k = 0; k < 2; k++)
                    {
                        var x = i * this.bbox.Maximum.X + (1 - i) * this.bbox.Minimum.X;
                        var y = j * this.bbox.Maximum.Y + (1 - j) * this.bbox.Minimum.Y;
                        var z = k * this.bbox.Maximum.Z + (1 - k) * this.bbox.Minimum.Z;

                        var newX = this.cosTheta * x + this.sinTheta * z;
                        var newZ = -this.sinTheta * x + this.cosTheta * z;

                        var tester = new Vector3(newX, y, newZ);

                        for (var c = 0; c < 3; c++)
                        {
                            min[c] = Math.Min(min[c], tester[c]);
                            max[c] = Math.Max(max[c], tester[c]);
                        }
                    }
                }
            }

            this.bbox = new BoundingBox(
                new Point3(min[0], min[1], min[2]),
                new Point3(max[0], max[1], max[2]));
        }

        public bool TryGetBoundingBox(
            double time0,
            double time1,
            out BoundingBox aabb)
        {
            aabb = this.bbox;
            return this.hasBox;
        }

        public bool TryIntersect(
            Ray ray,
            double tmin,
            double tmax,
            ref ShadeRecord sr)
        {
            var origin = new Point3(
                this.cosTheta * ray.Origin.X - this.sinTheta * ray.Origin.Z,
                ray.Origin.Y,
                this.sinTheta * ray.Origin.X + this.cosTheta * ray.Origin.Z);

            var direction = new Vector3(
                this.cosTheta * ray.Direction.X - this.sinTheta * ray.Direction.Z,
                ray.Direction.Y,
                this.sinTheta * ray.Direction.X + this.cosTheta * ray.Direction.Z);

            var rotatedRay = new Ray(origin, direction, ray.Time);

            if(!this.obj.TryIntersect(rotatedRay, tmin, tmax, ref sr))
            {
                return false;
            }

            var p = new Point3(
                this.cosTheta * sr.Point.X + this.sinTheta * sr.Point.Z,
                sr.Point.Y,
                -this.sinTheta * sr.Point.X + this.cosTheta * sr.Point.Z);

            var n = new Normal3(
                this.cosTheta * sr.Normal.X + this.sinTheta * sr.Normal.Z,
                sr.Normal.Y,
                -this.sinTheta * sr.Normal.X + this.cosTheta * sr.Normal.Z);

            sr.Point = p;
            sr.SetFaceNormal(rotatedRay, n);

            return true;
        }
    }
}