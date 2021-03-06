namespace Linie.InOneWeekend
{
    public class ShadeRecord
    {
        public Point3 Point { get; set; }

        public Normal3 Normal
        {
            get; 
            private set;
        }

        public double T { get; set; }

        public IMaterial Material { get; set; }

        public bool IsFrontFace { get; private set; }

        public void SetFaceNormal(in Ray3 r, in Normal3 outwardNormal)
        {
            this.IsFrontFace = 
                Vector3.Dot(r.Direction, (Vector3)outwardNormal) < 0;
            this.Normal = this.IsFrontFace ? outwardNormal : -outwardNormal;
        }
    }
}