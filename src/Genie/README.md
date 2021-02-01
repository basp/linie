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

But also with other types. All builtin numeric types should (mostly) work (this includes `int`, `double`, `long`) but you can also use custom types if you support a few math operations. Below is an example using the included `EFloat` type:
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

> Note that this is still a work in progress so no all `Math` methods and operators are implemented yet.

## overview
The goal of Genie is to support vector arithmetic over general floating point types in a reasonably speedy fashion. In order to accomplish this, a lot of interfaces are implicit and not directly specified by the code.

Genie works by depending on the .NET runtime to compile our arithmetic delegates in `static` constructors. 

> The compilation of those delegates is wrapped up in various forms of `Lazy<T>` and internally exposed as static fields. All of this is contained by the `internal Operations<T>` class.

From a client perspective you will be dealing with the `Operations` class instead for the most part. By virtue of method type inference we can make the API a lot smoother.
```
var a = Operations.Add(3.2f, 3.5f);
var b = Operations.Add(3, 2);
var c = Operations.Add(3.0, 2.5);
```

This way we don't have to explicitly specify our `T` (and `U`) parameters sincce they can be inferred.

> In essence, `Operations<T>` is a JIT compiler layer for `Operations`. Client code calling `Operations` will force `Operations<T>` delegates to be compiled and used in process. Not that even though `Operations<T>` calculations are `Lazy<T>` this all happens statically. Any lazy values are resolved at the same time. See the statics in `Operations` and `Operations<T>` in order to see how these two layers interact in detail.

## credits
* Greatly inspired by [HelloKitty/Generic.Math](https://github.com/HelloKitty/Generic.Math) which in turn builds on John Skeet's `MiscUtil` library for the expression compilation helpers.
* `EFloat` implementation is mostly taken straight from PBRT and translated to .NET C# code. This uses `float` and `double` for *very precise* value since we don't have builtin support for quad precision in .NET and I did not want to depend on something exotic.