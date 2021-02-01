namespace Genie
{
    using System;

    public struct Vector3<T>
        where T : IComparable, IComparable<T>
    {
        public readonly T X, Y, Z;

        public Vector3(T x, T y, T z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }

    public static class Vector3
    {
        public static Vector3<T> Create<T>(T x, T y, T z)
            where T : IComparable<T>, IComparable =>
            new Vector3<T>(x, y, z);
    }
}
