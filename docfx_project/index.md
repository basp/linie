# Linie
Linie is a mathematics library that is very loosely based on [glm](https://github.com/) with an ideomatic .NET API. The main goal is to provide a foundation that can be easily integrated to your graphics related software. Linie is **not** a general purpose linear algebra library.

## Features
* Linie is 100% .NET, written in C# and has no dependencies.
* It is tiny and can easily be included as source.
* Has an ideomatic API that confirms to .NET conventions.
* Supports most of the common linear algebra operations.
* Runs completly on the CPU by design.
* Geared towards beginners and educational purposes.
* Tries to be fast but prefers ease of use over performance.
* All vectors are implemented as *value* types (i.e. `struct`).
* All matrices are implemented as *reference* types (i.e. `class`).

## Overview
The core of Linie consists of the vector and matrix types. Since Linie is geared towards computer graphics we support fixed size vectors and matrices of the following types:

* `Vector2`
* `Vector3`
* `Vector4`
* `Matrix2x2`
* `Matrix3x3`
* `Matrix4x4`

### Tradeoffs
The Microsoft recommendation is to keep the size of a `struct` type below or equal to `16` bytes. All vectors can fit into this recommendation range so they are all implemented as a `struct`. However, the only matrix that would fit is `Matrix2x2`. In order to keep the behavior for all matrices consistent they are all implemented as classes. This turns out to be useful since for optimization purposes we might want to mutate a particular matrix in place (in order to save on allocations).

> Funny enough, Microsoft breaks this rule in `System.Numerics` where even the `Matrix4x4` class is defined as a struct.