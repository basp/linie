namespace Linie.Generic
{
    public struct Vector3<T>
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
        public static Vector3<T> Create<T>(T x, T y, T z) => 
            new Vector3<T>(x, y, z);
    }
}