// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

internal class DoubleEqualityComparer : ApproxEqualityComparer<double>
{
    public DoubleEqualityComparer(double epsilon)
        : base(epsilon)
    {
    }

    public override bool Equals(double x, double y) =>
        this.ApproxEqual(x, y);

    public override int GetHashCode(double obj) =>
        obj.GetHashCode();
}