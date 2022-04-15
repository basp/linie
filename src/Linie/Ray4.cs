// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

/// <summary>
/// Represents a ray in 4D space.
/// </summary>
public record Ray4(Vector4 Origin, Vector4 Direction)
{
    /// <summary>
    /// Return a position along this ray at distance t.
    /// </summary>
    /// <remarks>
    /// Equivalent to invoking the <c>GetPosition(double)</c> method.
    /// </remarks>
    public Vector4 this[double t]
    {
        get => this.Origin + (t * this.Direction);
    }

    /// <summary>
    /// Return a position along this ray at distance t.
    /// </summary>
    /// <remarks>
    /// Alternative for indexer <c>this[double]</c>.
    /// </remarks>
    public Vector4 GetPosition(in double t) => this[t];
}
