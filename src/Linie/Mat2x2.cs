namespace Linie;

public class Mat2x2 : IEquatable<Mat2x2>
{
    public readonly float M00, M01;

    public readonly float M10, M11;

    public Mat2x2(
        float m00, float m01,
        float m10, float m11)
    {
        this.M00 = m00; this.M01 = m01;
        this.M10 = m10; this.M11 = m11;
    }

    public bool Equals(Mat2x2 other) =>
        this.M00 == other.M00 &&
        this.M01 == other.M01 &&
        this.M10 == other.M10 &&
        this.M11 == other.M11;
}
