# linie
Linie is a ray tracing kernel library.

## features
* 4D or 3D mode
* Local epsilon
* Rays
* Transforms

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

Approximate equality is supported for the following types:

* `Point2`
* `Point3`
* `Vector2`
* `Vector3`
* `Normal3`
* `Vector4`
* `Matrix4x4`

### transforms
TODO