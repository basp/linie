namespace Genie
{
    using System;
    using System.Collections.Generic;

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

    public class Ray<T> : Ray<T, IDictionary<string, object>>
        where T : IComparable<T>
    {
        public Ray(
            Point3<T> origin,
            Vector3<T> direction,
            IDictionary<string, object> state)
            : base(origin, direction, state)
        {
        }
    }

    public static class Ray
    {
        public static Ray<T, S> Create<T, S>(
            Point3<T> origin,
            Vector3<T> direction,
            S state)
            where T : IComparable<T> =>
            new Ray<T, S>(origin, direction, state);

        public static Ray<T> Create<T>(
            Point3<T> origin,
            Vector3<T> direction)
            where T : IComparable<T> =>
            new Ray<T>(origin, direction, new Dictionary<string, object>());
    }
}