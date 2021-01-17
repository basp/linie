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

Going from 4D mode to 3D mode and back:
```
var u4 = Vector4.CreateDirection(0, 1, 0);
var a4 = Vector4.CreatePosition(1, 0, 0);
var u3 = 

```