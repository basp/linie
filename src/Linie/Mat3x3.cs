namespace Linie;

public class Mat3x3 : IEquatable<Mat3x3>
{
    public readonly float M00, M01, M02;

    public readonly float M10, M11, M12;

    public readonly float M20, M21, M22;

    public Mat3x3(
        float m00, float m01, float m02,
        float m10, float m11, float m12,
        float m20, float m21, float m22)
    {
        this.M00 = m00; this.M01 = m01; this.M02 = m02; 
        this.M10 = m10; this.M11 = m11; this.M12 = m12;
        this.M20 = m20; this.M21 = m21; this.M22 = m22;
    }

    public bool Equals(Mat3x3 other) =>
        this.M00 == other.M00 && 
        this.M01 == other.M01 &&
        this.M02 == other.M02 &&
        this.M10 == other.M10 &&
        this.M11 == other.M11 &&
        this.M12 == other.M12 &&
        this.M20 == other.M20 &&
        this.M21 == other.M21 &&
        this.M22 == other.M22;
}
