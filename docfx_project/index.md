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
Linie is designed to operate in 3D space using 4D vectors and matrices as is common in computer graphics. Positions and directions are both represented by a `Vector4` but only positions have their `W` component set to anything else but zero. Directions always have their `W` component set to zero. Positions have their `W` component set to one by default but any `Vector4` with a non-zero `W` component is considered a position.

The library offers a few methods that make it a little bit more explicit what we are doing:
* The `Vector4.CreatePosition` method creates `Vector4` instances with their `W` component set to one.
* The `Vector4.CreateDirection` method creates `Vector4` instances with their `W` component set to zero.
* The `IsPosition` and `IsDirection` properties can be used to query whether a `Vector4` is a position or direction vector.
* The `AsPosition` and `AsDirection` methods can be used to force any `Vector4` into a particular mode (i.e. position or direction).

> Note that Linie does not check for valid operations. If you want to add a point to a point you are free to do so. The math will work out but the results may not be what you expect.

## transformations
The heart of the library is in the transformation suite. This builds upon the vector and matrix types to provide a consistent and fast toolset.

The primary use case of Linie is to perform calculations on points and directions in 3D in order to facilitate computer graphics operations such as ray tracing or rasterizing. All the matrices and vectors with less than four dimensions are mostly there to support the 4D calculations. They are perfectly usuable but not considered as part of the main use cases for the package as a whole.

Below is a simple example where we translate and rotate a point using the Linie transformation API.
```
var p0 = Vector4.CreatePosition(0, 0, 0);
var t = Matrix.Identity
    .Translate(0, 0, 1)
    .RotateY(Math.PI / 4);
var p1 = t * p0;
```

The example above creates a *transformation matrix* `t` that first translates a point by `<0, 0, 1>` and then rotates it `Math.PI / 4` radians along the y-axis.

It is recommended to use the *fluent* API to build up transformation matrices. This will ensure the matrix multiplication order is correct and it will read well in code. If you want to build up a transformation from scratch it makes sense to start with the *identity* matrix. You can then use this as a starting point into the fluent API in order to build up the rest of the transformation before applying it to a position or direction vector like in the example above.

Matrices are stored in **column-major** order so when you want to apply a matrix to a vector you need to put the matrix first (i.e. `MU` instead of `UM`).

### performance tradeoffs
The Microsoft recommendation is to keep the size of a `struct` type below or equal to `16` bytes. All vectors can fit into this recommendation range so they are all implemented as a `struct`. 

However, the only matrix that would fit is `Matrix2x2`. In order to keep the behavior for all matrices consistent they are all implemented as reference (i.e. `class`) types.

In order to mitigate some of the allocation drawbacks of managing matrices on the heap the library offers some imperative operations that mutate an existing instance. These are currently provided for expensive `Matrix4x4` operations. Their return type will be `void` and they will have a `ref` parameter to store the results. These are generally a bit faster (and also more memory efficient) than their *functional* counterparts (which return new instances).

For example, the following demonstrates the function and imperative multiply operations on a `Matrix4x4` instance.

```
// Functional, result is a new instance that needs to be allocated.
// This will allocate and is generally a bit easier to understand.
var a = Matrix4x4.Identity;
var b = Matrix4x4.Identity;
var c = a * b; // Equivalent to Matrix4x4.Multiply(a, b);

// Imperative, result was already allocated, mutate in place.
// This will not allocate and is generally a bit faster if we can re-use 
// instances.
var a = Matrix4x4.Identity;
var b = Matrix4x4.Identity;
var c = new Matrix4x4();
Matrix4x4.Multiply(a, b, ref result);
```