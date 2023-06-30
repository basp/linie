# Linie
Linie is a mathematics library that is very loosely based on [glm](https://github.com/) with an ideomatic .NET API. The main goal is to provide a foundation that can be easily integrated to your graphics related software. Linie is **not** a general purpose linear algebra library.

## features
* Linie is 100% .NET, written in C# and has no dependencies.
* It is tiny and can easily be included as source.
* Has an ideomatic API that confirms to .NET conventions.
* Supports most of the common linear algebra operations.
* Runs completly on the CPU by design.
* Geared towards beginners and educational purposes.
* Tries to be fast but prefers ease of use over performance.
* All vectors are implemented as *value* types (i.e. `struct`).
* All matrices are implemented as *reference* types (i.e. `class`).

## overview
The core of Linie consists of the vector and matrix types. Since Linie is geared towards computer graphics we support fixed size vectors and matrices of the following types:

* `Vector2`
* `Vector3`
* `Vector4`
* `Matrix2x2`
* `Matrix3x3`
* `Matrix4x4`

Most of the focus is on the `Vector4` and `Matrix4x4` types since it is expected these will be used most often for the cases that Linie is designed for.

## positions and directions
Linie is designed to operate in 3D space using 4D vectors and matrices as is common in computer graphics. Positions and directions are both represented by a `Vector4` but only points have their `W` component set to anything else but zero. Directions always have their `W` component set to zero. Points have their `W` component set to one by default.

> There have been periods in time where Linie would explicitly differentiate between a direction and a point and would not allow operations between them that would not make sense. Although this does provide some safety it was decided to drop this and leave it up to the client of the library instead.

The library does offer a few methods that make it a little bit more explicit what we are doing.

* The `Vector4.CreatePosition` method creates `Vector4` instances with their `W` component set to one.
* The `Vector4.CreateDirection` method creates `Vector4` instances with their `W` component set to zero.
* The `IsPosition` and `IsDirection` properties can be used to query whether a `Vector4` is a position or direction vector.

> Note that Linie does not check for valid operations. If you want to add a point to a point you are free to do so. The math will work out but the results may not be what you expect.

### performance tradeoffs
The Microsoft recommendation is to keep the size of a `struct` type below or equal to `16` bytes. All vectors can fit into this recommendation range so they are all implemented as a `struct`. 

However, the only matrix that would fit is `Matrix2x2`. In order to keep the behavior for all matrices consistent they are all implemented as reference (i.e. `class`) types.

In order to mitigate some of the allocation drawbacks of managing matrices on the heap the library offers some imperative operations that mutate an existing instance. These are currently provided for expensive `Matrix4x4` operations. Their return type will be `void` and they will have a `ref` parameter to store the results. These are generally a bit faster (and also more memory efficient) than their *functional* counterparts (which return new instances).

