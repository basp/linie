namespace Linie;

public class Mat2x2 : IEquatable<Mat2x2>
{
    private float m00, m01;

    private float m10, m11;

    public Mat2x2(
        float m00, float m01,
        float m10, float m11)
    {
        this.m00 = m00; this.m01 = m01;
        this.m10 = m10; this.m11 = m11;
    }

    public float M00 => this.m00;

    public float M01 => this.m01;

    public float M10 => this.m10;

    public float M11 => this.m11;

    public bool Equals(Mat2x2 other) =>
        this.m00 == other.m00 &&
        this.m01 == other.m01 &&
        this.m10 == other.m10 &&
        this.m11 == other.m11;

    public static void Multiply(Mat2x2 m, Mat2x2 n, out Mat2x2 o)
    {
    }
}
