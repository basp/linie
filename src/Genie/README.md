# Genie
Genie is a generic Linie. It supports math over generic floating point types.

## Example
We can do math with `float`:
```
var u = Vector2.Create(2f, 3f);
var v = Vector2.Create(-3f, -5f);
var w = u + v;
Assert.Equal(-1, w.X);
Assert.Equal(-2, w.Y);
```

But also with other types. All builtin types should work out of the box but you can also use custom types if you support a few math operations. Below is an example using the included `EFloat` type:
```
var u = Vector2.Create(
    new EFloat(2),
    new EFloat(3));

var v = Vector2.Create(
    new EFloat(-3),
    new EFloat(-5));

var w = u + v;
Assert.True(w.X.LowerBound <= -1);
Assert.True(w.X.HigherBound >= -1);
Assert.True(w.Y.LowerBound <= -2);
Assert.True(w.Y.HigherBound >= -2);
```

This is supported by `Vector3<T>`, `Vector4<T>`, etc.

Implementation was greatly inspired by [HelloKitty/Generic.Math](https://github.com/HelloKitty/Generic.Math).