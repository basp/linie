namespace Linie;

public readonly struct Vector3<T> :
    IEquatable<Vector3<T>>
    where T : INumber<T>
{
    public readonly T X, Y, Z;

    public Vector3(T x, T y, T z)
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
    
    public bool Equals(Vector3<T> other) =>
        this.X == other.X &&
        this.Y == other.Y &&
        this.Z == other.Z;
}

public static class Vector3
{
    public static Vector3<T> Create<T>(T x, T y, T z)
        where T : INumber<T> =>
        new(x, y, z);
}