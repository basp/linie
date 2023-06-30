// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

public class Transform : IEquatable<Transform>
{
    public Transform(Matrix4x4 m)
        : this(m, m.Invert())
    {
    }

    /// <summary>
    /// Initializes a new <see cref="Transform"/> instance using a precomputed 
    /// inversion matrix.
    /// </summary>
    /// <remarks>
    /// When using this constructor, the client code is responsible to proivde
    /// a sensible inversion matrix.
    /// </remarks>
    /// <param name="m">The transformation matrix.</param>
    /// <param name="minv">
    /// The precoumputed inverse of the transformation matrix.
    /// </param>
    public Transform(Matrix4x4 m, Matrix4x4 minv)
    {
        this.Matrix = m;
        this.Inverted = minv;
        this.InvertedTransposed = minv.Transpose();
    }

    public Matrix4x4 Matrix { get; }

    public Matrix4x4 Inverted { get; }

    public Matrix4x4 InvertedTransposed { get; }

    public bool Equals(Transform other) =>
        this.Matrix.Equals(other.Matrix) &&
        this.Inverted.Equals(other.Inverted);

    /// <summary>
    /// Returns a hash code based on the <c>Matrix</c> and <c>Inverted</c>
    /// properties of this instance.
    /// </summary>
    /// <remarks>
    /// The <c>InvertedTransposed</c> property is not included in the hash code
    /// generator since it is implied by the value of <c>Inverted</c> (which can
    /// be set by the user explicitly).
    /// </remarks>
    public override int GetHashCode() =>
        HashCode.Combine(
            this.Matrix.GetHashCode(),
            this.Inverted.GetHashCode());
}