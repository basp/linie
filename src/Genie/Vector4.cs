namespace Genie
{
    using System;

    public struct Vector4<T>
        where T : IComparable, IComparable<T>
    {
        public readonly T X, Y, Z, W;

        public Vector4(T x, T y, T z, T w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }
    }

    public static class Vector4
    {
        public static Vector4<T> Create<T>(T x, T y, T z, T w)
            where T : IComparable<T>, IComparable =>
            new Vector4<T>(x, y, z, w);
    }
}
