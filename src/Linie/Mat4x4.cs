namespace Linie;

public class Mat4x4 : IEquatable<Mat4x4>
{
    public readonly float M00, M01, M02, M03;

    public readonly float M10, M11, M12, M13;

    public readonly float M20, M21, M22, M23;

    public readonly float M30, M31, M32, M33;

    public Mat4x4(
        float m00, float m01, float m02, float m03,
        float m10, float m11, float m12, float m13,
        float m20, float m21, float m22, float m23,
        float m30, float m31, float m32, float m33)
    {
        this.M00 = m00; this.M01 = m01; this.M02 = m02; this.M03 = m03;
        this.M10 = m10; this.M11 = m11; this.M12 = m12; this.M13 = m13;
        this.M20 = m20; this.M21 = m21; this.M22 = m22; this.M23 = m23;
        this.M30 = m30; this.M31 = m31; this.M32 = m32; this.M33 = m33;
    }

    public bool Equals(Mat4x4 other) =>
        this.M00 == other.M00 && 
        this.M01 == other.M01 &&
        this.M02 == other.M02 &&
        this.M03 == other.M03 &&
        this.M10 == other.M10 &&
        this.M11 == other.M11 &&
        this.M12 == other.M12 &&
        this.M13 == other.M13 &&
        this.M20 == other.M20 &&
        this.M21 == other.M21 &&
        this.M22 == other.M22 &&
        this.M23 == other.M23 &&
        this.M30 == other.M30 &&
        this.M31 == other.M31 &&
        this.M32 == other.M32 &&
        this.M33 == other.M33;
}
