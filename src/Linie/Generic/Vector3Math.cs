namespace Linie.Generic
{
    public class Vector3Math<T, U>
        where T : new()
        where U : IOperations<T>, new()
    {
        private static IOperations<T> ops = new U();

        public static Vector3<T> Add(Vector3<T> u, Vector3<T> v)
        {
            var x = ops.Add(u.X, v.X);
            var y = ops.Add(u.Y, v.Y);
            var z = ops.Add(u.Z, v.Z);
            return Vector3.Create(x, y, z);
        }

        public static Vector3<T> Subtract(Vector3<T> u, Vector3<T> v)
        {
            var x = ops.Subtract(u.X, v.X);
            var y = ops.Subtract(u.Y, v.Y);
            var z = ops.Subtract(u.Z, v.Z);
            return Vector3.Create(x, y, z);
        }
    }
}