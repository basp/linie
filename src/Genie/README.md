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

Note that in this case we are using `Genie` with the included compound `Vector2<T>` structure. If you are just doing straight up math it is probably better to not use `Genie` unless you really need the `EFloat`.

All `Math` operations that make sense should be `Vector3<T>`, `Vector4<T>`, etc. The library will delegate either to `Math` or `MathF` in the case of double or float respectively. It also has builtin support for the `EFloat` type and it is pretty easy to extend the *math providers* in case of exotic requirements.

> Note that this is still a work in progress so no all `Math` methods and operators are implemented yet.

## overview
The goal of Genie is to support vector arithmetic over general floating point types in a reasonably speedy fashion. In order to accomplish this, a lot of interfaces are implicit and not directly specified by the code. The reason for this is that `Genie` depends on **statically** compiling the arithmetic your code will use.

> This means that eventually all *primitive* types that can be used **will** have to implement implicit `Math` interface for a large part.

This mainly works by depending on the .NET runtime to compile our arithmetic delegates in `static` constructors. 

> The compilation of those delegates is wrapped up in various forms of `Lazy<T>` and internally exposed as static fields. All of this is contained by the `internal Operations<T>` class.

From a client perspective you will be dealing with the `Operations` class instead for the most part. By virtue of method type inference we can make the API a lot smoother.
```
var a = Operations.Add(3.2f, 3.5f);
var b = Operations.Add(3, 2);
var c = Operations.Add(3.0, 2.5);
```

This way we don't have to explicitly specify our `T` (and `U`) parameters since they can be inferred.

> In essence, `Operations<T>` is a JIT compiler layer for `Operations`. Client code calling `Operations` will force `Operations<T>` delegates to be compiled and used in process. Not that even though `Operations<T>` calculations are `Lazy<T>` this all happens statically. Any lazy values are resolved at the same time. See the statics in `Operations` and `Operations<T>` in order to see how these two layers interact in detail.

## notes
* `EFloat` uses `float` and `double` for *value* (`v`) and *very precise value* (`vp`) repsectively in contrast to PBRT where a quad is used for `vp`. 

> There is some work on a software based `DoubleDouble` to support `double` as the `v` value for `EFloat` values. Additionally, the plan is to include a more general `EFloat<T, U>` type.

## credits
* The following books *The Ray Tracer Challenge*, *Ray Tracing From the Ground Up*, *Ray Tracing in a Weekend*, *Ray Tracing the Next Week*, *PBRT book* and *Finite Precision Number Systems and Arithmetic*.
* Greatly inspired by [HelloKitty/Generic.Math](https://github.com/HelloKitty/Generic.Math) which in turn builds on John Skeet's `MiscUtil` library for the expression compilation helpers.
* `EFloat` and lots of `Utils` implementation is mostly taken straight from [PBRT](https://github.com/mmp/pbrt-v4) and translated to .NET C# code.
* `DoubleDouble` is due to [Library for Double-Double and Quad-Double Arithmetic](https://web.mit.edu/tabbott/Public/quaddouble-debian/qd-2.3.4-old/docs/qd.pdf) and the [sukop/doubledouble](https://github.com/sukop/doubledouble) Python implementation.
* All experts who worked on this matter and made it so accessible so that even I could implement and (somewhat) understand it in the end.
