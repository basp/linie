# getting started
We will create a new dotnet solution called `clock`.
```
> mkdir clock
> cd clock
> dotnet new sln
```

This should create a `clock.sln` file in the current directory.
```
> ls

    Directory: D:\basp\clock

Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
-a---          30/06/2023    21:00            441 clock.sln
```

Next we will create a `src` dircectory to contain the actual projects and navigate into that directory
```
> mkdir src
> cd .\src
```

We just need a .NET project to exercise the Linie package. We'll use a console application for this.
```
> dotnet new console -o .\Example
```

Now that we have the project we should add it to the solution:
```
> cd ..
> dotnet sln add .\src\Example\Example.csproj
```

Make sure it builds:
```
> dotnet build
```

Navigate back into the `Example` project directory:
```
> cd .\src\Example
```

Add the Linie package:
```
> dotnet add package Linie
```

Utilize the package (in `Program.cs`):
```
static void Main(string[] args)
{
    var t = Matrix4x4.Identity
        .Translate(2, 0, 1)
        .RotateY(Math.PI / 3)
        .Scale(2);
    var p0 = Vector4.CreatePosition(0, 0, 0);
    var p1 = t * p0;
    
    // ...<snip>
}
```
