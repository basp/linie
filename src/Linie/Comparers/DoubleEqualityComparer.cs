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