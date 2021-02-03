namespace Genie
{
    using System;

    public class Ray<T, S>
        where T : IComparable<T>
    {
        public Ray(Point3<T> origin, Vector3<T> direction, S state)
        {
            this.Origin = origin;
            this.Direction = direction;
            this.State = state;
        }

        public Point3<T> this[T t] => this.Origin + (t * this.Direction);

        public Point3<T> GetPointAt(T t) => this[t];

        public Point3<T> Origin { get; private set; }

        public Vector3<T> Direction { get; private set; }

        public S State { get; set; }
    }
}