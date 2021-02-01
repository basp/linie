namespace Linie.TheNextWeek
{
    using System;
    using System.Collections.Generic;

    public class BVHNode : IGeometricObject
    {
        private readonly IGeometricObject left;

        private readonly IGeometricObject right;

        private readonly BoundingBox box;

        public BVHNode(
            IList<IGeometricObject> objects,
            double time0,
            double time1)
            : this(objects, 0, objects.Count, time0, time1)
        {
        }

        public BVHNode(
            IList<IGeometricObject> sourceObjects,
            int start,
            int end,
            double time0,
            double time1)
        {
            var rng = new Random();
            var objects = new List<IGeometricObject>(sourceObjects);
            var axis = rng.RandomInt(0, 2);

            var comparer =
                (axis == 0)
                ? BoxXComparer()
                : (axis == 1)
                    ? BoxYComparer()
                    : BoxZComparer();

            var span = end - start;

            if (span == 1)
            {
                this.left = this.right = objects[start];
            }
            else if (span == 2)
            {
                this.left = objects[start];
                this.right = objects[start + 1];
            }
            else
            {
                objects.Sort(start, span, comparer);
                var mid = start + (span / 2);
                this.left = new BVHNode(objects, start, mid, time0, time1);
                this.right = new BVHNode(objects, mid, end, time0, time1);
            }

            if (!this.left.TryGetBoundingBox(time0, time1, out var boxLeft) ||
               !this.right.TryGetBoundingBox(time0, time1, out var boxRight))
            {
                throw new Exception("No bounding box in BVH node ctor");
            }

            this.box = BoundingBox.SurroundingBox(boxLeft, boxRight);
        }

        public bool TryGetBoundingBox(
            double time0,
            double time1,
            out BoundingBox aabb)
        {
            aabb = this.box;
            return true;
        }

        public bool TryIntersect(
            Ray ray,
            double tmin,
            double tmax,
            ref ShadeRecord sr)
        {
            if (!this.box.TryIntersect(ray, tmin, tmax))
            {
                // sr = null;
                return false;
            }

            var hitLeft = this.left.TryIntersect(ray, tmin, tmax, ref sr);
            var hitRight = this.right.TryIntersect(ray, tmin, hitLeft ? sr.T : tmax, ref sr);
            return hitLeft || hitRight;
        }

        static BoxComparer BoxXComparer() =>
            new BoxComparer(0);

        static BoxComparer BoxYComparer() =>
            new BoxComparer(1);

        static BoxComparer BoxZComparer() =>
            new BoxComparer(2);
    }

    internal class BoxComparer : IComparer<IGeometricObject>
    {
        private readonly int axis;

        public BoxComparer(int axis)
        {
            this.axis = axis;
        }

        public int Compare(IGeometricObject x, IGeometricObject y)
        {
            if (!x.TryGetBoundingBox(0, 0, out var boxA) || !y.TryGetBoundingBox(0, 0, out var boxB))
            {
                throw new Exception("No bounding box in BVH node ctor");
            }

            return boxA.Minimum[axis].CompareTo(boxB.Minimum[axis]);
        }
    }
}