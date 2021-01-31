namespace Linie.TheNextWeek
{
    public struct Ray
    {
        public readonly Point3 Origin;

        public readonly Vector3 Direction;

        public readonly double Time;

        public Ray(Point3 origin, Vector3 direction, double time = 0)
        {
            this.Origin = origin;
            this.Direction = direction;
            this.Time = time;
        }

        public Point3 this[double t] => this.Origin + (t * this.Direction);

        public Point3 GetPosition(double t) => this[t];
    }
}