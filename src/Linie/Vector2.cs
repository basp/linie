namespace Linie;

public readonly struct Vector2<T>
    where T : INumber<T>
{
    public readonly T X, Y;

    public Vector2(T x, T y)
    {
        this.X = x;
        this.Y = y;
    }

    public T this[int index] =>
        index switch
        {
            0 => this.X,
            _ => this.Y,
        };
}

public static class Vector2
{
    public static Vector2<T> Create<T>(T x, T y)
        where T : INumber<T> =>
        new(x, y);
}
