namespace Linie.TheNextWeek
{
    using System;

    public class ConstantMedium : IGeometricObject
    {
        private readonly IGeometricObject boundary;

        private readonly Material phaseFunction;

        private readonly double negInvDensity;

        public ConstantMedium(IGeometricObject b, double d, Material m)
        {
            this.boundary = b;
            this.negInvDensity = -1.0 / d;
            this.phaseFunction = m;
        }

        public ConstantMedium(IGeometricObject b, double d, ITexture a)
            : this(b, d, new Isotropic(a))
        {
        }

        public ConstantMedium(IGeometricObject b, double d, Color c)
            : this(b, d, new Isotropic(c))
        {
        }

        public bool TryGetBoundingBox(
            double time0,
            double time1,
            out BoundingBox aabb)
        {
            return this.boundary.TryGetBoundingBox(time0, time1, out aabb);
        }

        public bool TryIntersect(
            Ray ray,
            double tmin,
            double tmax,
            ref ShadeRecord sr)
        {
            var rng = new Random();

            ShadeRecord rec1 = null, rec2 = null;
            if (!this.boundary.TryIntersect(
                ray,
                double.NegativeInfinity,
                double.PositiveInfinity,
                ref rec1))
            {
                return false;
            }

            if (!this.boundary.TryIntersect(
                ray,
                rec1.T + 0.0001, 
                double.PositiveInfinity, 
                ref rec2))
            {
                return false;
            }

            if(rec1.T < tmin)
            {
                rec1.T = tmin;
            }

            if(rec2.T > tmax)
            {
                rec2.T = tmax;
            }

            if(rec1.T >= rec2.T)
            {
                return false;
            }

            if(rec1.T < 0)
            {
                rec1.T = 0;
            }

            var rayLength = ray.Direction.Magnitude();
            var distanceInsideBoundary = (rec2.T - rec1.T) * rayLength;
            var hitDistance = this.negInvDensity * Math.Log(rng.RandomDouble());

            if(hitDistance > distanceInsideBoundary)
            {
                return false;
            }

            sr = new ShadeRecord();
            sr.T = rec1.T + hitDistance / rayLength;
            sr.Point = ray.GetPosition(sr.T);
            var n = new Normal3(1, 0, 0);
            sr.SetFaceNormal(ray, n);
            sr.Material = this.phaseFunction;
            return true;
        }
    }
}