// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie.Tool;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Linie;

public class MultiplicationBenchmark
{
    private Matrix4x4 c = new Matrix4x4(0);

    [Benchmark]
    public void Multiplication()
    {
        var a = new Matrix4x4(1.0);
        var b = new Matrix4x4(2.5);
        var c = a * b;
    }

    [Benchmark]
    public void InPlaceMultiplication()
    {
        var a = new Matrix4x4(1.0);
        var b = new Matrix4x4(2.5);
        Matrix4x4.Multiply(a, b, ref c);
    }
}

public class InversionBenchmark
{
    private Matrix4x4 c = new Matrix4x4(0);

    [Benchmark]
    public void Invert()
    {
        var m = Matrix4x4.Identity;
        var i = m.Invert();
    }

    [Benchmark]
    public void InvertInPlace()
    {
        var m = Matrix4x4.Identity;
        Matrix4x4.Invert(m, ref c);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // BenchmarkRunner.Run<InversionBenchmark>();
    }
}