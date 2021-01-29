# Ray Tracing in One Weekend
This is a pretty straightforward port of the ray tracer described in 
[Ray Tracing in One Weekend](https://raytracing.github.io/books/RayTracingInOneWeekend.html) 
by Peter Shirley.

The main goal of this project was to battle test `Linie` and see if it could 
help. It did. You can now ray trace in a day (or a few hours even).

The code contains the example scene from refraction and the final scene of the 
book.

## Notes
* The `hittable` abstract was renamed to `IGeometricObject` 
* The `hit_record` type was renamed to `ShadeRecord`
* Some methods and variables might have slight renames as well. However these
should be self-explanatory in the context where they are used.
* Most changes are inspired by 
[Ray Tracing from the Ground Up](http://www.raytracegroundup.com/).