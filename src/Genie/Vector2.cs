namespace Genie
{
    using System;

    public struct Vector2<T>
        where T : IComparable, IComparable<T>
    {
        public readonly T X, Y;

        public Vector2(T x, T y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    public static class Vector2
    {
        public static Vector2<T> Create<T>(T x, T y)
            where T : IComparable, IComparable<T> =>
            new Vector2<T>(x, y);
    }
}
