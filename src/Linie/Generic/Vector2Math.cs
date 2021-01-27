namespace Linie.Generic
{
    using System;

    public static class Vector2Math<T, U>
        where T : new()
        where U : IOperations<T>, new()
    {
        public static Vector2<T> Add(Vector2<T> u, Vector2<T> v)
        {
            throw new NotImplementedException();
        }

        public static Vector2<T> Subtract(Vector2<T> u, Vector2<T> v)
        {
            throw new NotImplementedException();
        }
    }
}