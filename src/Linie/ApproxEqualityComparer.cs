// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

using System;
using System.Collections.Generic;

/// <summary>
/// Base class for approximate equality comparers.
/// </summary>
internal abstract class ApproxEqualityComparer<T> : IEqualityComparer<T>
{
    protected readonly double epsilon;

    protected ApproxEqualityComparer()
        : this(Config.DefaultEpsilon)
    {
    }

    protected ApproxEqualityComparer(double epsilon)
    {
        this.epsilon = epsilon;
    }

    /// <inheritdoc />
    public abstract bool Equals(T x, T y);

    /// <inheritdoc />
    public abstract int GetHashCode(T obj);

    protected bool ApproxEqual(double v1, double v2) =>
        Math.Abs(v1 - v2) < this.epsilon;
}
