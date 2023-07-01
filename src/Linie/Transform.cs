// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

public class Transform : 
    IEquatable<Transform>,
    IFormattable
{
    /// <summary>
    /// Initializes a new <see cref="Transform"/> instance.
    /// </summary>
    /// <param name="m">The transformation matrix.</param>
    public Transform(Matrix4x4 m)
        : this(m, m.Invert())
    {
    }

    /// <summary>
    /// Initializes a new <see cref="Transform"/> instance using a precomputed 
    /// inversion matrix.
    /// </summary>
    /// <param name="m">The transformation matrix.</param>
    /// <param name="minv">
    /// The precoumputed inverse of the transformation matrix.
    /// </param>
    /// <remarks>
    /// When using this constructor, the client code is responsible to proivde
    /// a sensible inversion matrix.
    /// </remarks>
    public Transform(Matrix4x4 m, Matrix4x4 minv)
    {
        this.Matrix = m;
        this.Inverted = minv;
        this.InvertedTransposed = minv.Transpose();
    }

    /// <summary>
    /// Gets the transformation matrix.
    /// </summary>
    public Matrix4x4 Matrix { get; }

    /// <summary>
    /// Gets the inverted transformation matrix.
    /// </summary>
    public Matrix4x4 Inverted { get; }

    /// <summary>
    /// Gets the inverted transposed transformation matrix.
    /// </summary>
    public Matrix4x4 InvertedTransposed { get; }

    /// <inheritdoc />
    public bool Equals(Transform other) =>
        this.Matrix.Equals(other.Matrix) &&
        this.Inverted.Equals(other.Inverted);

    /// <inheritdoc />
    public override int GetHashCode() =>
        HashCode.Combine(
            this.Matrix.GetHashCode(),
            this.Inverted.GetHashCode());

    /// <inheritdoc />
    public override string ToString() => this.ToString(null, null);

    /// <inheritdoc />
    public string ToString(string format, IFormatProvider formatProvider) =>
        this.Matrix.ToString(format, formatProvider);
}