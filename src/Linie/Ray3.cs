// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

public record Ray3(Point3 Origin, Vector3 Direction)
{
    /// <summary>
    /// Return a position along this ray at distance t.
    /// </summary>
    /// <remarks>
    /// Equivalent to calling <c>GetPosition(double)</c>.
    /// </remarks>
    public Point3 this[double t]
    {
        get => this.Origin + (t * this.Direction);
    }

    /// <summary>
    /// Return a position along this ray at distance t.
    /// </summary>
    /// <remarks>
    /// Alternative for indexer <c>this[double]</c>.
    /// </remarks>
    public Point3 GetPosition(in double t) => this[t];
}
