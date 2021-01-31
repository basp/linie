namespace Linie.TheNextWeek
{
    using System;
    using System.Linq;

    public class Perlin
    {
        static readonly Random rng = new Random(1);

        const int pointCount = 256;

        private Vector3[] ranvec;

        private int[] permX;

        private int[] permY;

        private int[] permZ;

        public Perlin()
        {
            this.ranvec = Enumerable
                .Range(0, pointCount)
                .Select(_ => rng.RandomVector(-1, 1))
                .ToArray();

            this.permX = GeneratePerm();
            this.permY = GeneratePerm();
            this.permZ = GeneratePerm();
        }

        public double Turbulence(in Point3 p, int depth = 7)
        {
            var acc = 0.0;
            var tmp = p;
            var weight = 1.0;

            for (var i = 0; i < depth; i++)
            {
                acc += weight * this.Noise(tmp);
                weight *= 0.5;
                tmp *= 2;
            }

            return Math.Abs(acc);
        }

        public double Noise(in Point3 p)
        {
            var u = p.X - Math.Floor(p.X);
            var v = p.Y - Math.Floor(p.Y);
            var w = p.Z - Math.Floor(p.Z);

            var i = (int)(Math.Floor(p.X));
            var j = (int)(Math.Floor(p.Y));
            var k = (int)(Math.Floor(p.Z));

            var c = new Vector3[2, 2, 2];

            for (var di = 0; di < 2; di++)
            {
                for (var dj = 0; dj < 2; dj++)
                {
                    for (var dk = 0; dk < 2; dk++)
                    {
                        c[di, dj, dk] = this.ranvec[
                            this.permX[(i + di) & 255] ^
                            this.permY[(j + dj) & 255] ^
                            this.permZ[(k + dk) & 255]];
                    }
                }
            }

            return TrilinearInterp(c, u, v, w);
        }

        static int[] GeneratePerm()
        {
            var p = Enumerable.Range(0, pointCount).ToArray();
            Permute(p, pointCount);
            return p;
        }

        static void Permute(int[] p, int n)
        {
            for (var i = n - 1; i > 0; i--)
            {
                var target = rng.RandomInt(0, i);
                var tmp = p[i];
                p[i] = p[target];
                p[target] = tmp;
            }
        }

        static double TrilinearInterp(Vector3[,,] c, double u, double v, double w)
        {
            var uu = u * u * (3 - 2 * u);
            var vv = v * v * (3 - 2 * v);
            var ww = w * w * (3 - 2 * w);
            var acc = 0.0;
            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    for (var k = 0; k < 2; k++)
                    {
                        var wv = new Vector3(u - i, v - j, w - k);
                        acc +=
                            (i * uu + (1 - i) * (1 - uu)) *
                            (j * vv + (1 - j) * (1 - vv)) *
                            (k * ww + (1 - k) * (1 - ww)) *
                            Vector3.Dot(c[i, j, k], wv);
                    }
                }
            }

            return acc;
        }
    }
}