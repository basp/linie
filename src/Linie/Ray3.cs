namespace Linie
{
    public struct Ray3
    {
        public readonly Point3 Origin;

        public readonly Vector3 Direction;

        public Ray3(Point3 origin, Vector3 direction)
        {
            this.Origin = origin;
            this.Direction = direction;
        }

        /// <summary>
        /// Return a position along this ray at distance t.
        /// </summary>
        /// <remarks>
        /// Equivalent to calling <c>GetPosition(double)</c>.
        /// </remarks>
        public Point3 this[double t]
        {
            get => this.Origin + (t * this.Direction);
        }

        /// <summary>
        /// Return a position along this ray at distance t.
        /// </summary>
        /// <remarks>
        /// Alternative for indexer <c>this[double]</c>.
        /// </remarks>
        public Point3 GetPosition(in double t) => this[t];
    }
}