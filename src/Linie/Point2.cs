namespace Linie;

public readonly struct Point2<T>
    where T : INumber<T>
{
    public readonly T X, Y;
}