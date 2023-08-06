namespace Linie;

public struct Point3<T> :
    IEquatable<Point3<T>>
    where T : INumber<T>
{
    public readonly T X, Y, Z;

    public Point3()
        : this(T.Zero)
    {
    }

    public Point3(T v)
        : this(v, v, v)
    {
    }

    public Point3(T x, T y, T z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

    public bool Equals(Point3<T> other) =>
        this.X == other.X &&
        this.Y == other.Y &&
        this.Z == other.Z;
}