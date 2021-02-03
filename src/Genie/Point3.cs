namespace Genie
{
    using System;

    public struct Point3<T>
        where T : IComparable<T>
    {
        public readonly T X, Y, Z;

        public Point3(T x, T y, T z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Vector3<T> operator -(Point3<T> a, Point3<T> b) =>
            new Vector3<T>(
                Operations.Subtract(a.X, b.X),
                Operations.Subtract(a.Y, b.Y),
                Operations.Subtract(a.Z, b.Z));

        public static Point3<T> operator +(Point3<T> a, Vector3<T> u) =>
            new Point3<T>(
                Operations.Add(a.X, u.X),
                Operations.Add(a.Y, u.Y),
                Operations.Add(a.Z, u.Z));
    }

    public static class Point3
    {
        public static Point3<T> Create<T>(T x, T y, T z)
            where T : IComparable<T> => new Point3<T>(x, y, z);
    }
}