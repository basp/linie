# genie
**Genie** is a generic `Linie`. It supports math over generic floating point types in .NET. At the same time it tries to be practical for a basic ray tracing kernel and number library. The main goal is education with reasonable performance. We try to strike a balance between understandable and still somewhat usable in a real scenario (by default). That being said, any optimization tricks are very much welcome, theres likely a way to incorporate them in manner that will not interfere with the primary goals.

## context
Using C# it is not possible to straight port template types as you might see them in C++ code. This is good since it avoids a lot of problems but also bad in that porting is not straightforward. 

The main area were this problem manifests is when you want to have generic math operators and operations. In C++ you can use templates for this since the macro system is not checked. In C# we don't have an equivalent and when you try to overload operators for a generic type `T` the compiler will (usually) complain.

For example, this `+` operator definition will not compile, no matter what kind of `where` constraints you put on it.
```
static operator T +<T>(T u, T v) => u + v;
```

A possible solution is to define all the math operations and use dynamic dispatch to call them but the overhead is so prohibitive that this cannot really be used outside sandbox scenarios. 

The problem is then to create fast math operations without the incurred overhead of any dynamic calls. 

* There is a way to fiddle with generics in order to get good performance see [Arithmetic in generic code](http://core.loyc.net/math/maths) but that has the huge drawback of introducing an extra `M` type argument that client code has to deal with. This basically means that the client is responsible for explicitly specifying another class that deals with the actual implementation. This might not sound like a huge burden but in practice it often is. The whole point of using a generic math library is that you do not have to worry about this.

* An alternative (and still performant) way is to compile all the necessary math **statically** when the application starts. This way, the IL is exposed before we actually hit runtime and the .NET VM will hopefully do a good job of optimizing it. Next our application will run with all the delegates it needs already compiled and available but we do incur the cost of calling through those delegates for all the math operations we need. 

> For Genie we haven taken the second approach for now because it is not clear how much of an actual cost we will incur during real usage scenarios. It's not unlikely we will support the first approach (using a redirection type) at some point.

This cost is significant since our target use case will be calling these methods millions of times. However, some preliminary benchmarks are promising and have shown that expected performance is in the same order of magnitude. 

When we take `Linie` performance as a reference, the `Genie` performance on matrix inversion (a demanding operation) is about 0.09 times slower when benchmarked in a tight loop of a million iterations:
```
Linie.Matrix4x4(double) : 30.27s
Genie.Matrix4x4<double> : 32.92s
```

As measured with [LINQPad](https://www.linqpad.net/).

As a basic rule we can say that `Genie` is about 10% slower than using the `Linie` equivalent. 

> This compared `Linie.Matrix4x4` (which uses `double`) to the `Genie.Matrix4x4<double>` generic implementation. Another benchmark ran using a `Genie.Matrix4x4<float>` which is a little bit slower (33s) but still well within the same order of magnitude.

Note that `float` and `double` yield mostly the same results compared to `Linie` so for those use cases it is pretty safe to use either one of them. However, when you use a custom type for `T` in `Genie` then you need to be careful about the performance of this `T` implementation. For example, when we run the same benchmark with `DoubleDouble` we get different results.

## example
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

## overview
The goal of Genie is to support vector arithmetic over general numeric point types in a reasonably speedy fashion. In order to accomplish this, a lot of interfaces are implicit and not directly specified by the code. The reason for this is that `Genie` depends on **statically** compiling the arithmetic your code will use.

> This means that eventually all *primitive* types that can be used **will** have to implement an implicit `Math` interface. One of the near future goals is to explicitly document what exactly it means to be a `Genie` compatible type and explicitly define the operations it needs to support.

This mainly works by depending on the .NET runtime to compile our arithmetic delegates in `static` constructors. 

> The compilation of those delegates is wrapped up in various forms of `Lazy<T>` and internally exposed as static fields. All of this is contained by the `internal Operations<T>` class.

From a client perspective you will be dealing with the `Operations` class instead for the most part. By virtue of method type inference we can make the API a lot smoother.
```
var a = Operations.Add(3.2f, 3.5f);
var b = Operations.Add(3, 2);
var c = Operations.Add(3.0, 2.5);
```

This way we don't have to explicitly specify our `T` (and `U`) parameters since they can be inferred.

> In essence, `Operations<T>` is a JIT compiler layer for `Operations`. Client code calling `Operations` will force `Operations<T>` delegates to be compiled and used in process. Even though `Operations<T>` calculations are `Lazy<T>` this all happens statically. Any lazy values are resolved at the same time. See the statics in `Operations` and `Operations<T>` in order to see how these two layers interact in detail.

## how it works
Genie will delegate either to `Math` or `MathF` in the case of `double` or `float` respectively by default. It can also find the correct *provider* for `EFloat`. 

The way it works is that during the static constructor of `Operations<T>` it uses a type mapping from `T` to `U` to find the correct math provider `U` for type `T`. 

So for example, when you use a `Vector3<double>` then (statically) under the hood an `Operations<double>` class will be compiled as well. The static `.ctor` of this `Operations<T>` class will then look for a type provider of type `U` for the type given for `T`. In this case `T == double`. Since `double` is pretty much the default in .NET it can simply return `System.Math` as the provider so that `U == System.Math`.

Now that we have a *math provider* that (hopefully) has all the methods we need we can statically compile all the related math delegates. Instead of hard wiring `System.Math` for compliation we can just use our custom type instead. This is how operations like `Sin`, `Pow` and `Sqrt` are redirected when they are required in the implementation of a custom type.

> When implementing custom numeric types such as `EFloat` it is custom to have `T == U`. This means that the custom numeric type `T` provides its own `System.Math` equivalents. If you look at the `EFloat` class for example you'll notice it has a bunch of `static` methods that closely resemble the `Math` API. These are there so it can be used as both `T` and `U` - both a value and a provider of math operations.

To make things more concrete, at the top of the `Operations<T>` class you'll find the following code:
```
private static IDictionary<Type, Type> providers = new Dictionary<Type, Type>
{
    [typeof(double)] = typeof(Math),
    [typeof(float)] = typeof(MathF),
    [typeof(int)] = typeof(Math),
    [typeof(EFloat)] = typeof(EFloat),
};
```

This is the math provider mapping. You can see that for `double` and `int` it will map to `Math` and for `float` it will map to the new `MathF`. There's a custom `EFloat` provider here (outside of the .NET framework) but included with Genie.

At the start of the `static` constructor there's a single line of code that looks up the appropriate math provider:
```
var math = providers[typeof(T)];
```
> Note that `math` is a `Type` here and we are still running in static constructor.

Now when we need compile our math delegates, instead of using `System.Math` we will inject our custom type. Below we are telling the `CreateStaticCall<T, T>` method to use our `math` type for the `Sqrt` operation instead of using the default `System.Math` type.
```
sqrt = new Lazy<Func<T, T>>(() =>
    ExpressionUtil.CreateStaticCall<T, T>(math, "Sqrt"));
```

This will use the `ExpressionUtil` (based on `MiscUtil`) to statically compile a delegate and cache it in the `sqrt` field. It will do the same for all other operations that would otherwise be handled by either an `operator` or `Math` call (or at least the subset of operations that `Genie` currently supports).

> The choice to have this be `Lazy<Func<T, T>>` is debatable since it seemed to have originated from a framework version change that involved inmplicit behavior. See the [HelloKitty/Generic.Math readme](https://github.com/HelloKitty/Generic.Math) for some additional info.

In the end, after the static constructor has been run all the required math delegates have been compiled and cached into their respective fields. The rest of the code can now use the `Operations` class and do general arithmetic where needed. 

> The `Operations` class has overloads to support method type inference and tries to unify all the various `T` types so that any client code can be more generic.

Types can easily be build upon this so that end usage does not have to deal with the underlying mechanics. In other words, it is not hard to implement a completely new and foreign arithmetic if a client would want to do so. The only problem is that the current API is underdocumented.

## notes
* `EFloat` uses `float` and `double` for *value* (`v`) and *very precise value* (`vp`) repsectively in contrast to PBRT where a quad is used for `vp`. 

> There is some work on a software based `DoubleDouble` to support `double` as the `v` value for `EFloat` values. Additionally, the plan is to include a more general `EFloat<T, U>` type.

## credits
* The following books *The Ray Tracer Challenge*, *Ray Tracing From the Ground Up*, *Ray Tracing in a Weekend*, *Ray Tracing the Next Week*, *PBRT* and *Finite Precision Number Systems and Arithmetic*.
* [HelloKitty/Generic.Math](https://github.com/HelloKitty/Generic.Math) which in turn builds on John Skeet's `MiscUtil` library for the expression compilation helpers showed how to do generic `T` math operators in C# with reasonable performance.
* `EFloat` and related `Utils` implementation are mostly taken straight from [PBRT](https://github.com/mmp/pbrt-v4).
* `DoubleDouble` is due to [Library for Double-Double and Quad-Double Arithmetic](https://web.mit.edu/tabbott/Public/quaddouble-debian/qd-2.3.4-old/docs/qd.pdf), the [sukop/doubledouble](https://github.com/sukop/doubledouble) Python implementation and [A floating-point technique for extending the available precision](https://link.springer.com/article/10.1007/BF01397083) by Dekker.
