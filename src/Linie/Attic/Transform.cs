namespace Linie;

public class Transform : IEquatable<Transform>
{
    public Transform(Matrix4x4 m)
        : this(m, m.Invert())
    {
    }

    public Transform(Matrix4x4 m, Matrix4x4 minv)
    {
        this.Matrix = m;
        this.Inverted = minv;
    }

    public Matrix4x4 Matrix { get; }

    public Matrix4x4 Inverted { get; }

    public bool Equals(Transform other) =>
        this.Matrix.Equals(other.Matrix) &&
        this.Inverted.Equals(other.Inverted);
}