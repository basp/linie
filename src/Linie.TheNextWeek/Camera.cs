namespace Linie.TheNextWeek
{
    using System;

    public class Camera
    {
        private readonly Point3 origin;

        private readonly Point3 lowerLeftCorner;

        private readonly Vector3 horizontal;

        private readonly Vector3 vertical;

        private readonly Vector3 u, v, w;

        private readonly double lensRadius;

        private readonly double time0;

        private readonly double time1;

        public Camera(
            Point3 lookFrom,
            Point3 lookAt,
            Vector3 vup,
            double vfov,
            double aspectRatio,
            double aperture,
            double focusDistance,
            double time0 = 0,
            double time1 = 0)
        {
            var theta = Utils.DegreesToRadians(vfov);
            var h = Math.Tan(theta / 2);
            var viewportHeight = 2.0 * h;
            var viewportWidth = aspectRatio * viewportHeight;

            this.w = Vector3.Normalize(lookFrom - lookAt);
            this.u = Vector3.Normalize(vup.Cross(w));
            this.v = w.Cross(u);

            this.origin = lookFrom;
            this.horizontal = focusDistance * viewportWidth * u;
            this.vertical = focusDistance * viewportHeight * v;
            this.lowerLeftCorner = this.origin 
                - (this.horizontal / 2) 
                - (this.vertical / 2) 
                - focusDistance * w;

            this.lensRadius = aperture / 2;
            this.time0 = time0;
            this.time1 = time1;
        }

        public Ray GetRay(in double s, in double t, Random rng)
        {
            var rd = this.lensRadius * rng.RandomInUnitDisk();
            var offset = this.u * rd.X + this.v * rd.Y;
            var origin = this.origin + offset;

            var direction =
                this.lowerLeftCorner 
                + (s * this.horizontal) 
                + (t * this.vertical) 
                - this.origin 
                - offset;

            var time = rng.RandomDouble(this.time0, this.time1);
            return new Ray(origin, direction, time);
        }
    }
}