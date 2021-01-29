namespace Linie.InOneWeekend
{
    using System.Collections.Generic;

    public class Group : IGeometricObject
    {
        public readonly IList<IGeometricObject> Objects =
            new List<IGeometricObject>();

        public bool TryIntersect(
            Ray3 ray, 
            double tmin, 
            double tmax, 
            out ShadeRecord sr)
        {
            sr = null;

            var hitAnything = false;
            var closestSoFar = tmax;
            
            foreach(var obj in this.Objects)
            {
                if(obj.TryIntersect(ray, tmin, closestSoFar, out var tmp))
                {
                    hitAnything = true;
                    closestSoFar = tmp.T;
                    sr = tmp;
                }
            }

            return hitAnything;
        }
    }
}