namespace Linie.TheNextWeek
{
    using System;
    using System.Threading.Tasks;
    using Linie;
    using ShellProgressBar;

    class Program
    {
        static Color GetScaledColor(in Color c, int samplesPerPixel)
        {
            var scale = 1.0 / samplesPerPixel;

            var r = Math.Sqrt(c.R * scale);
            var g = Math.Sqrt(c.G * scale);
            var b = Math.Sqrt(c.B * scale);

            return new Color(r, g, b);
        }

        static Color Shade(
            in Ray ray,
            Group world,
            int depth,
            Random rng)
        {
            if (depth <= 0)
            {
                return new Color(0);
            }

            const double tmin = 0.001;
            const double tmax = double.PositiveInfinity;

            if (world.TryIntersect(ray, tmin, tmax, out var sr))
            {
                // check if we get a scattered ray and if we do
                // send a new ray back into the world with attenuation
                // along the scatter direction
                if (sr.Material.Scatter(
                    ray,
                    sr,
                    rng,
                    out var attenuation,
                    out var scattered))
                {
                    return attenuation * Shade(scattered, world, depth - 1, rng);
                }

                return new Color(0);
            }

            var unitDirection = Vector3.Normalize(ray.Direction);
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
            var world = CreateTwoPerlinSpheres();

            // camera
            var viewportHeight = 2.0;
            var viewportWidth = aspectRatio * viewportHeight;
            var lookFrom = new Point3(13, 2, 3);
            var lookAt = new Point3(0, 0, 0);
            var vup = new Vector3(0, 1, 0);
            var focusDistance = 10.0;
            var aperture = 0.1;
            var cam = new Camera(
                lookFrom,
                lookAt,
                vup,
                vfov: 20,
                aspectRatio,
                aperture,
                focusDistance,
                time0: 0,
                time1: 1);

            var img = new Canvas(imageWidth, imageHeight);

            void RenderParallel()
            {
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
                                var r = cam.GetRay(u, v, rng);
                                color += Shade(r, world, maxDepth, rng);
                            }

                            img[i, j] = GetScaledColor(color, samplesPerPixel);
                        }

                        progressBar.Tick();
                    });
                }
            }

            void RenderSequential()
            {
                var rng = new Random();
                using (var progressBar = new ProgressBar(imageHeight, "rendering"))
                {
                    for (var j = imageHeight - 1; j >= 0; j--)
                    {
                        for (var i = 0; i < imageWidth; i++)
                        {
                            var color = new Color(0);
                            for (var s = 0; s < samplesPerPixel; s++)
                            {
                                var u = (i + rng.RandomDouble()) / (imageWidth - 1);
                                var v = (j + rng.RandomDouble()) / (imageHeight - 1);
                                var r = cam.GetRay(u, v, rng);
                                color += Shade(r, world, maxDepth, rng);
                            }

                            img[i, j] = GetScaledColor(color, samplesPerPixel);
                        }

                        progressBar.Tick();
                    }
                }
            }

            RenderParallel();
            // RenderSequential();

            img.SavePpm(@".\out.ppm");
        }

        static Group CreateTwoPerlinSpheres()
        {
            var rng = new Random();
            var pertext = new NoiseTexture(4);
            var world = new Group();
            world.Objects.Add(
                new Sphere(
                    new Point3(0, -1000, 0), 1000, new Lambertian(pertext)));
            world.Objects.Add(
                new Sphere(
                    new Point3(0, 2, 0), 2, new Lambertian(pertext)));
            return world;
        }

        static Group CreateRandomScene()
        {
            var rng = new Random(3);

            var world = new Group();

            var checker = new CheckerTexture(
                new Color(0.2, 0.3, 0.1),
                new Color(0.9, 0.9, 0.9));
            var groundMaterial = new Lambertian(checker);
            world.Objects.Add(
                new Sphere(
                    new Point3(0, -1000, 0), 
                    1000, 
                    groundMaterial));

            for (var a = -11; a < 11; a++)
            {
                for (var b = -11; b < 11; b++)
                {
                    var chooseMat = rng.RandomDouble();
                    var center = new Point3(
                        a + 0.9 * rng.RandomDouble(),
                        0.2,
                        b + 0.9 * rng.RandomDouble());

                    var refPoint = new Point3(4, 0.2, 0);
                    if ((center - refPoint).Magnitude() > 0.9)
                    {
                        if (chooseMat < 0.8)
                        {
                            // diffuse
                            var albedo = rng.RandomColor() * rng.RandomColor();
                            var mat = new Lambertian(albedo);
                            var center2 = center + new Vector3(0, rng.RandomDouble(0, 0.5), 0);
                            world.Objects.Add(
                                new MovingSphere(center, center2, 0, 1, 0.2, mat));
                        }
                        else if (chooseMat < 0.95)
                        {
                            // metal
                            var albedo = rng.RandomColor();
                            var fuzz = rng.RandomDouble(0, 0.5);
                            var mat = new Metal(albedo, fuzz);
                            world.Objects.Add(
                                new Sphere(center, 0.2, mat));
                        }
                        else
                        {
                            // glass
                            var mat = new Dielectric(1.5);
                            world.Objects.Add(
                                new Sphere(center, 0.2, mat));
                        }
                    }
                }
            }

            var mat1 = new Dielectric(1.5);
            var mat2 = new Lambertian(new Color(0.4, 0.2, 0.1));
            var mat3 = new Metal(new Color(0.7, 0.6, 0.5));

            world.Objects.Add(
                new Sphere(new Point3(0, 1, 0), 1, mat1));
            world.Objects.Add(
                new Sphere(new Point3(-4, 1, 0), 1, mat2));
            world.Objects.Add(
                new Sphere(new Point3(4, 1, 0), 1, mat3));

            return world;
        }

        static Group CreateTestScene()
        {
            var materialGround = new Lambertian(new Color(0.8, 0.8, 0.0));
            var materialCenter = new Lambertian(new Color(0.1, 0.2, 0.5));
            var materialLeft = new Dielectric(1.5);
            var materialRight = new Metal(new Color(0.8, 0.6, 0.2));

            var world = new Group();

            world.Objects.Add(
                new Sphere(new Point3(0, -100.5, -1), 100, materialGround));
            world.Objects.Add(
                new Sphere(new Point3(0, 0, -1), 0.5, materialCenter));
            world.Objects.Add(
                new Sphere(new Point3(-1, 0, -1), 0.5, materialLeft));
            world.Objects.Add(
                new Sphere(new Point3(-1, 0, -1), -0.4, materialLeft));
            world.Objects.Add(
                new Sphere(new Point3(1, 0, -1), 0.5, materialRight));

            return world;
        }
    }
}
