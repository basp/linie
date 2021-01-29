namespace Linie.InOneWeekend
{
    using System;
    using System.Threading.Tasks;
    using Linie;
    using ShellProgressBar;

    class Program
    {
        static double HitSphere(in Point3 center, in double radius, in Ray3 r)
        {
            var oc = r.Origin - center;
            var a = Vector3.Dot(r.Direction, r.Direction);
            var b = 2 * Vector3.Dot(oc, r.Direction);
            var c = Vector3.Dot(oc, oc) - radius * radius;
            var discriminant = (b * b) - (4 * a * c);
            if (discriminant < 0)
            {
                return -1;
            }

            return (-b - Math.Sqrt(discriminant)) / (2 * a);
        }

        static Color ScaleColor(in Color c, int samplesPerPixel = 1)
        {
            var scale = 1.0 / samplesPerPixel;

            var r = Math.Sqrt(c.R * scale);
            var g = Math.Sqrt(c.G * scale);
            var b = Math.Sqrt(c.B * scale);

            return new Color(r, g, b);
        }

        static Color RayColor(in Ray3 r, Group world, int depth, Random rng)
        {
            if (depth <= 0)
            {
                return new Color(0);
            }

            if (world.TryIntersect(r, 0.001, double.PositiveInfinity, out var sr))
            {
                Color attenuation = new Color(0);
                if (sr.Material.Scatter(r, sr, rng, ref attenuation, out var scattered))
                {
                    return attenuation * RayColor(scattered, world, depth - 1, rng);
                }

                return new Color(0);
            }

            var unitDirection = Vector3.Normalize(r.Direction);
            var t = 0.5 * (unitDirection.Y + 1);
            var white = new Color(1, 1, 1);
            var blueish = new Color(0.5, 0.7, 1.0);
            return (1 - t) * white + t * blueish;
        }

        static void Main(string[] args)
        {
            // image
            const double aspectRatio = 16.0 / 9;
            const int imageWidth = 400;
            const int imageHeight = (int)(imageWidth / aspectRatio);
            const int samplesPerPixel = 100;
            const int maxDepth = 50;

            // world
            var materialGround= new Lambertian(new Color(0.8, 0.8, 0.0));
            var materialCenter = new Lambertian(new Color(0.7, 0.3, 0.3));
            var materialLeft = new Metal(new Color(0.8, 0.8, 0.8), 0.3);
            var materialRight = new Metal(new Color(0.8, 0.6, 0.2), 1.0);

            var world = new Group();
            world.Objects.Add(
                new Sphere(new Point3(0, -100.5, -1), 100, materialGround));
            world.Objects.Add(
                new Sphere(new Point3(0, 0, -1), 0.5, materialCenter));
            world.Objects.Add(
                new Sphere(new Point3(-1, 0, -1), 0.5, materialLeft));
            world.Objects.Add(
                new Sphere(new Point3(1, 0, -1), 0.5, materialRight));

            // camera
            var viewportHeight = 2.0;
            var viewportWidth = aspectRatio * viewportHeight;
            var focalLength = 1.0;

            var origin = new Point3(0, 0, 0);
            var horizontal = new Vector3(viewportWidth, 0, 0);
            var vertical = new Vector3(0, viewportHeight, 0);
            var lowerLeftCorner = origin -
                (horizontal / 2) -
                (vertical / 2) -
                new Vector3(0, 0, focalLength);

            var img = new Canvas(imageWidth, imageHeight);
            var cam = new Camera();

            using (var progressBar = new ProgressBar(imageHeight, "rendering"))
            {
                Parallel.For(0, imageHeight, j =>
                {
                    // make sure to give each parallel body its 
                    // own uniquely seeded RNG
                    var rng = new Random();

                    // we need `j` to be descending (imageWidth..0]
                    j = imageHeight - 1 - j;

                    for (var i = 0; i < imageWidth; i++)
                    {
                        var color = new Color(0);
                        for (var s = 0; s < samplesPerPixel; s++)
                        {
                            var u = (i + rng.RandomDouble()) / (imageWidth - 1);
                            var v = (j + rng.RandomDouble()) / (imageHeight - 1);
                            var r = cam.GetRay(u, v);
                            color += RayColor(r, world, maxDepth, rng);
                        }

                        img[i, j] = ScaleColor(color, samplesPerPixel);
                    }

                    progressBar.Tick();
                });
            }

            img.SavePpm(@".\out.ppm");
        }
    }
}
