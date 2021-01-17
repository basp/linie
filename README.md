# Linie
Linie is a ray tracing kernel library.

## Overview
* 4D or 3D mode
* Local epsilon
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

Going from a 3D point to a 4D vector is implicit:
```
var a = new Point3(1, 2, 3);
Vector4 u = a;
Assert.True(u.IsPoint);
Assert.Equal(1, u.X);
Assert.Equal(2, u.Y);
Assert.Equal(3, u.Z);
Assert.Equal(1, u.W); // 4D points have W = 1
```

And going from a 3D vector to a 4D vector is similar:
```
var u = new Vector3(1, 2, 3);
Vector4 v = u;
Assert.True(v.IsDirection);
...
```