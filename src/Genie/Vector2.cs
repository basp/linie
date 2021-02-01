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
        public Vector2<T> Create<T>(T x, T y) => new Vector2<T>(x, y);
    }
}
