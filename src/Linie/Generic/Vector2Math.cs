namespace Linie.Generic
{
    public static class Vector2Math<T, U>
        where T : new()
        where U : IOperations<T>, new()
    {
        private static IOperations<T> ops = new U();

        public static Vector2<T> Add(Vector2<T> u, Vector2<T> v)
        {
            var x = ops.Add(u.X, v.X);
            var y = ops.Add(u.Y, v.Y);
            return Vector2.Create(x, y);
        }

        public static Vector2<T> Subtract(Vector2<T> u, Vector2<T> v)
        {
            var x = ops.Subtract(u.X, v.X);
            var y = ops.Subtract(u.Y, v.Y);
            return Vector2.Create(x, y);
        }
    }
}