namespace Linie;

public readonly struct Vec2 : IEquatable<Vec2>
{
    public readonly float X, Y;

    public Vec2(float x, float y)
    {
        this.X = x;
        this.Y = y;
    }

    public bool Equals(Vec2 other) =>
        this.X == other.X &&
        this.Y == other.Y;

    public static Vec2 operator +(Vec2 u, Vec2 v) =>
        new Vec2(u.X + v.X, u.Y + v.Y);
}
