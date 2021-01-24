# linie
Linie is a ray tracing kernel library.

## features
* 4D or 3D mode
* local epsilon
* rays
* transforms
* extras

### 4D or 3D mode
Linie is designed for two basic modes of operandi:

* Use all 4D vectors where a position has `W = 1` and a direction has `W = 0`
* Use explicit point, vector and normal types

When using 4D vectors it is recommended to use the `Vector4.CreatePosition` and
`Vector4.CreateDirection` methods to create points and vectors respectively. This
will make sure that the `W` component is set to the right value.

You can also be explicit and use any of the point, vector and normal classes when you don't want to mix your 3D tuples the wrong way or just to be more formal in
your interface.

> It's perfectly fine to mix and match or use only parts of Linie for your
> renderer. For example, some people like to use the `Vector3` type for
> positions, directions, colors and normals while others may prefer a more formal
> distinction.

Going from a 3D to a 4D position is implicit:
```
var a = new Point3(1, 2, 3);
Vector4 u = a;
Assert.True(u.IsPoint);
Assert.Equal(1, u.X);
Assert.Equal(2, u.Y);
Assert.Equal(3, u.Z);
Assert.Equal(1, u.W); // 4D points have W = 1
```

And going from a 3D to a 4D direction is similar:
```
var u = new Vector3(1, 2, 3);
Vector4 v = u;
Assert.True(v.IsDirection);
...
Assert.Equal(0, u.W); // 4D vectors have W = 0
```

Going from 4D to 3D takes some care but explicit cast is supported:
```
var u = Vector.CreateDirection(1, 0, 0);
var a = (Point3)u;
var v = (Vector3)u;
var n = (Normal3)u;
```

Normals will implicitly cast to vectors:
```
var n = new Normal3(0, 1, 0);
Vector4 u = n;
Assert.True(u.IsDirection);
Assert.Equal(0, u.W);
```

Note that `Normal` values do not have to be normalized. They are never
normalized automatically nor are there any checks.

### local epsilon
Since ray tracing is heavy on floating point math, all the geometric types in
Linie have a dynamic and local epsilon (wiggle room) for equality operations
using the `IEquatable<T>` interface.

All of the types also overload `Equals` but this has no approximation. In order
to compare two values with an approximation *epsilon* the static
`GetEqualityComparer(double)` method can be used.
```
var u = new Vector3(1, 0, 1);
var v = new Vector3(1.5, 0, 1.5);
var cmp = Vector3.GetEqualityComparer(epsilon: 1);
Assert.True(cmp.Equals(u, v));
```

Since every calculation can work in its own epsilon domain it is easy to
incorporate this into your rendering architecture. Whether you're using a
global epsilon or something more granular like an epsilon per object. Equality
comparers can also be cached since you will only need one of them for each type
and they are thread safe because they have no mutable state.

Approximate equality using local epsilon is supported for the following types:
* `Point2`
* `Point3`
* `Vector2`
* `Vector3`
* `Normal3`
* `Vector4`
* `Matrix4x4`

### transforms
Builtin transformations provide `Matrix4x4` affine transformations.

* `Translate`
* `Scale`
* `RotateX`
* `RotateY`
* `RotateZ`
* `Shear`

The matrix multiplication operation is supplied for all 3D and 4D types. For 
example, to translate a `Point3` you would use something like the following:
```
var a = new Point3(0);
var b = Transform.Translate(3, 2, 1) * a;
Assert.Equal(new Point3(3, 2, 1), b);
```

A similar operation is available for `Vector3`, `Normal3` and `Vector4`. Note
that translations have no effect on vectors though:
```
var u = new Vector3(0);
var v = Transform.Translate(3, 2, 1) * u;
Assert.Equal(new Vector3(0, 0, 0), v);
```

When working in 4D mode the `W` component of `Vector4` will determine whether
it will behave as a point or vector. Unless absolutely necessary it is strongly
recommended to always use the `Vector4.CreateDirection` and `Vector4.CreatePosition`
factory methods to ensure the `W` component is set correctly.

### extras
There is an `EFloat` class that could potentially be used to increase the
epsilon locality even further. The idea is that a particular calculation would
use this class instead of a more global epsilon in order to maxizime accuracy.

However, this class is highly experimental and has not been tested at all so
only use it if you are feeling adventurous.