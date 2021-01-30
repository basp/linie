# Ray Tracing in One Weekend
This is a pretty straightforward port of the ray tracer described in 
[Ray Tracing in One Weekend](https://raytracing.github.io/books/RayTracingInOneWeekend.html) 
by Peter Shirley.

The main goal of this project was to battle test `Linie` and see if it could 
help to implement the ray tracer more quicly. You can now ray trace in a day (or a few hours even).

The code contains the example scene from refraction and the final scene of the 
book. It was was built up using the reference images from the book to verify the 
results. Each and every step of the book can now be reproduced using `Linie` as
a base to start from.

## Overview
The code tries to be faithful to the book. However, some changes have been made to make it more compatible with .NET. Most of these changes should be obvious. Only the final version of the code suggested by the book is preserved but this does show that `Linie` can indeed be used as a ray tracing kernel. Some bugs in the 3D calculations where fixed as part of this process. Notably `Vector3` divide and magnitude operations did not work right but they do now.

The ray tracer as proposed in the book writes directly to `stdout` and is presumed to be used in a piped fashion. The version presented here can do the same thing but also features an optimized parallel rendering that uses the `Linie.Canvas` type to store its results before writing them out to disk. 

This intermediate storage was required since we cannot write out the results directly to `stdout` if we are rendering lines in parallel. In the end it turns out that using this memory storage is a huge win even when running sequentially. Because we are not writing to a stream for every pixel the whole process naturally is a lot faster. 

And now that we have intermediate storage we can also render out lines in [embarrasingly parralel](https://en.wikipedia.org/wiki/Embarrassingly_parallel) fashion. Technically we could easily do this on a pixel basis but it turns out the switching overhead is prohibitive on a general CPU (with the current sampling complexity). 

Rendering a line seems about the right granularity to parallelize givent he current .NET scheduler implementation. This turns out to be a huge win where an image can be rendered in (on average) 30% percent of the time versus the sequential algorithm (observed on 6 cores). 

Both the `RenderSequental` and `RenderParallel` implementations are included so that one can easily switch and observe the difference.

## Running
This is targeting .NET 5 so you will need that runtime to run it. Probably.

Next, you want to inspect the following constants in `Program.cs` and tweak them:
* `imageWidth` is pretty self-explanatory. Set this to your desired width, height will be calculated using the `aspectRatio`.
* `aspectRatio` is the ratio between width and height of your image. `16/9` or `3/2` are common values.
* `samplesPerPixel` is the amount of rays we shoot into the scene for each pixel. Setting this to `1` will give a really grainy image that still has a certain quality. Setting it to `100` or above will generally produce a good quality image. Higher means better quality in this case. It is not uncommon to shoot 1000+ rays for every pixel in production renders.
* `maxDepth` is the amount of times that a ray can bounce within the environment. The `RayColor` method is recursive in that it will shoot of more rays in all kinds of directions and call itself with those new rays in order to get a color value. This value determines how many times it can call itself in order to get a result.

After you're happy with all those things you just run:
```
cd .\Linie.InOneWeekend
dotnet run -c Release
```

I have added the `-c Release` flag because if you run in `Debug` mode (as is default) the code will be at least 2x slower. However, when you're developing your ray tracer you typically don't run with 1024 super sampling and 256 ray recursion at a resolution width of 1280. It's more common to render test images at 400 width with low values of 32 for super sampling and ray recursion. In those cases you can easily run in `Debug` and your images still render in less then a few seconds.

## Notes
* The `hittable` abstract was renamed to `IGeometricObject`.
* The `hit_record` type was renamed to `ShadeRecord`.
* Purely abstract classes are defined as interfaces.
* Some methods and variables might have slight renames as well. However these should be self-explanatory in the context where they are used.
* Most naming changes are inspired by 
[Ray Tracing from the Ground Up](http://www.raytracegroundup.com/) by Kevin Suffern.