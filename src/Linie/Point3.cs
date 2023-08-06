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

    public T this[int index] =>
        index switch
        {
            0 => this.X,
            1 => this.Y,
            _ => this.Z,
        };

    public bool Equals(Point3<T> other) =>
        this.X == other.X &&
        this.Y == other.Y &&
        this.Z == other.Z;
}

public static class Point3
{
    public static Point3<T> Create<T>(T x, T y, T z)
        where T : INumber<T> =>
        new(x, y, z);
}