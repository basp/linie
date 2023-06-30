// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

/// <summary>
/// Represents a ray in 3D space.
/// </summary>
public record Ray(Vector4 Origin, Vector4 Direction)
{
    /// <summary>
    /// Return a position along this ray at distance t.
    /// </summary>
    public Vector4 this[double t]
    {
        get => this.At(t);
    }

    /// <summary>
    /// Return a position along this ray at distance t.
    /// </summary>
    public Vector4 At(in double t) =>
        this.Origin + (t * this.Direction);
}
