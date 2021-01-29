namespace Linie.InOneWeekend
{
    public class Camera
    {
        private readonly Point3 origin;

        private readonly Point3 lowerLeftCorner;

        private readonly Vector3 horizontal;

        private readonly Vector3 vertical;

        public Camera()
        {
            var aspectRatio = 16.0 / 9;
            var viewportHeight = 2.0;
            var viewportWidth = aspectRatio * viewportHeight;
            var focalLength = 1.0;

            this.origin = new Point3(0, 0, 0);
            this.horizontal = new Vector3(viewportWidth, 0, 0);
            this.vertical = new Vector3(0, viewportHeight, 0);
            this.lowerLeftCorner = 
                this.origin -
                (this.horizontal / 2) -
                (this.vertical / 2) -
                new Vector3(0, 0, focalLength);
        }

        public Ray3 GetRay(in double u, in double v)
        {
            var direction =
                this.lowerLeftCorner +
                (u * this.horizontal) +
                (v * this.vertical) -
                this.origin;

            return new Ray3(this.origin, direction);
        }
    }
}