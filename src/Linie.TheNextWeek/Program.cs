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
            Color background,
            IGeometricObject world,
            int depth,
            Random rng)
        {
            if (depth <= 0)
            {
                return new Color(0);
            }

            const double tmin = 0.001;
            const double tmax = double.PositiveInfinity;

            ShadeRecord sr = null;
            if (!world.TryIntersect(ray, tmin, tmax, ref sr))
            {
                return background;
            }

            var emitted = sr.Material.Emitted(sr.U, sr.V, sr.Point);

            if (!sr.Material.Scatter(
                ray,
                sr,
                rng,
                out var attenuation,
                out var scattered))
            {
                return emitted;
            }

            return emitted + attenuation * Shade(
                scattered,
                background,
                world,
                depth - 1,
                rng);
        }

        static void Main(string[] args)
        {
            // image
            const double aspectRatio = 1.0;
            // const double aspectRatio = 16.0 / 9;
            const int imageWidth = 400;
            const int imageHeight = (int)(imageWidth / aspectRatio);
            const int samplesPerPixel = 3000;
            const int maxDepth = 64;

            // world
            var world = CreateFinalScene();
            // var background = new Color(0.7, 0.8, 1.0);
            var background = new Color(0);

            // camera
            var viewportHeight = 2.0;
            var viewportWidth = aspectRatio * viewportHeight;
            var lookFrom = new Point3(478, 278, -600);
            var lookAt = new Point3(278, 278, 0);
            // var lookFrom = new Point3(-2, 2, 1);
            // var lookAt = new Point3(0, 0, -1);
            var vup = new Vector3(0, 1, 0);
            var focusDistance = (lookFrom - lookAt).Magnitude();
            var aperture = 0.05;
            var cam = new Camera(
                lookFrom,
                lookAt,
                vup,
                vfov: 40,
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
                                color += Shade(r, background, world, maxDepth, rng);
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
                                color += Shade(r, background, world, maxDepth, rng);
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

        static Group CreateFinalScene()
        {
            var rng = new Random(6);
            var ground = new Lambertian(new Color(0.48, 0.83, 0.53));
            var boxes1 = new Group();
            const int boxesPerSide = 20;
            for (var i = 0; i < boxesPerSide; i++)
            {
                for (var j = 0; j < boxesPerSide; j++)
                {
                    var w = 100.0;
                    var x0 = -1000.0 + (i * w);
                    var z0 = -1000.0 + (j * w);
                    var y0 = 0.0;
                    var x1 = x0 + w;
                    var y1 = rng.RandomDouble(1, 101);
                    var z1 = z0 + w;
                    boxes1.Objects.Add(
                        new Box(
                            new Point3(x0, y0, z0),
                            new Point3(x1, y1, z1),
                            ground));
                }
            }

            var world = new Group();
            world.Objects.Add(new BVHNode(boxes1.Objects, 0, 1));

            var light = new DiffuseLight(new Color(7, 7, 7));
            world.Objects.Add(
                new XZRect(123, 423, 147, 412, 554, light));

            var center1 = new Point3(400, 400, 200);
            var center2 = center1 + new Vector3(30, 0, 0);
            var movingSphereMaterial = new Lambertian(new Color(0.7, 0.3, 0.1));
            world.Objects.Add(
                new MovingSphere(
                    center1,
                    center2,
                    0,
                    1,
                    50,
                    movingSphereMaterial));

            world.Objects.Add(
                new Sphere(
                    new Point3(260, 150, 45),
                    50,
                    new Dielectric(1.5)));

            world.Objects.Add(
                new Sphere(
                    new Point3(0, 150, 145),
                    50,
                    new Metal(new Color(0.8, 0.8, 0.9), 0.6)));

            var boundary = new Sphere(
                new Point3(360, 150, 145), 70, new Dielectric(1.5));
            world.Objects.Add(boundary);
            world.Objects.Add(
                new ConstantMedium(boundary, 0.2, new Color(0.2, 0.4, 0.9)));

            boundary = new Sphere(
                new Point3(0, 0, 0),
                5000,
                new Dielectric(1.5));
            world.Objects.Add(
                new ConstantMedium(boundary, 0.0001, new Color(1)));

            var emat = new Lambertian(new ImageTexture(@".\earthmap3.jpg"));
            world.Objects.Add(
                new Sphere(
                    new Point3(400, 200, 400),
                    100,
                    emat));

            var pertext = new NoiseTexture(0.1);
            world.Objects.Add(
                new Sphere(
                    new Point3(220, 280, 300),
                    80,
                    new Lambertian(pertext)));

            var boxes2 = new Group();
            var white = new Lambertian(new Color(0.73));
            var ns = 1000;
            for (var j = 0; j < ns; j++)
            {
                boxes2.Objects.Add(
                    new Sphere(
                        (Point3)rng.RandomVector(0, 165),
                        10,
                        white));
            }

            world.Objects.Add(
                new Translate(
                    new RotateY(new BVHNode(boxes2.Objects, 0.0, 1.0), 15),
                    new Vector3(-100, 270, 395)));

            return world;
        }

        static Group CreateEarth()
        {
            var text = new ImageTexture(@".\earthmap3.jpg");
            var surf = new Lambertian(text);
            var globe = new Sphere(new Point3(0, 0, 0), 2, surf);
            var world = new Group();
            world.Objects.Add(globe);
            return world;
        }

        static Group CreateCornellBox()
        {
            var world = new Group();

            var red = new Lambertian(new Color(0.65, 0.05, 0.05));
            var white = new Lambertian(new Color(0.73, 0.73, 0.73));
            var green = new Lambertian(new Color(0.12, 0.45, 0.15));
            var light = new DiffuseLight(new Color(15, 15, 15));

            world.Objects.Add(
                new YZRect(0, 555, 0, 555, 555, green));
            world.Objects.Add(
                new YZRect(0, 555, 0, 555, 0, red));
            world.Objects.Add(
                new XZRect(213, 343, 227, 332, 554, light));
            world.Objects.Add(
                new XZRect(0, 555, 0, 555, 0, white));
            world.Objects.Add(
                new XZRect(0, 555, 0, 555, 555, white));
            world.Objects.Add(
                new XYRect(0, 555, 0, 555, 555, white));

            IGeometricObject box1 = new Box(
                new Point3(0, 0, 0),
                new Point3(165, 330, 165),
                white);

            box1 = new RotateY(box1, 15);
            box1 = new Translate(box1, new Vector3(265, 0, 295));
            world.Objects.Add(box1);

            IGeometricObject box2 = new Box(
                new Point3(0, 0, 0),
                new Point3(165, 165, 165),
                white);

            box2 = new RotateY(box2, -18);
            box2 = new Translate(box2, new Vector3(130, 0, 65));
            world.Objects.Add(box2);

            return world;
        }

        static Group CreateSimpleLight()
        {
            var pertext = new NoiseTexture(4);
            var world = new Group();
            world.Objects.Add(
                new Sphere(
                    new Point3(0, -1000, 0),
                    1000,
                    new Lambertian(pertext)));

            world.Objects.Add(
                new Sphere(
                    new Point3(0, 2, 0),
                    2,
                    new Lambertian(pertext)));

            var difflight = new DiffuseLight(new Color(4, 4, 4));
            world.Objects.Add(
                new XYRect(3, 5, 1, 3, -2, difflight));

            world.Objects.Add(
                new Sphere(
                    new Point3(0, 7, 0),
                    2,
                    difflight));

            return world;
        }

        static Group CreateTwoPerlinSpheres()
        {
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
            var materialRight = new Metal(new Color(0.8, 0.6, 0.2), fuzz: 1.0);

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
