// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

using System;

#pragma warning disable SA1117 // ParametersMustBeOnSameLineOrSeparateLines

/// <summary>
/// Built-in affine transformations.
/// </summary>
public static class Affine
{
    /// <summary>
    /// Creates a translation matrix.
    /// </summary>
    /// <param name="dx">The number of units to translate along the x-axis.</param>
    /// <param name="dy">The number of units to translate along the y-axis.</param>
    /// <param name="dz">The number of units to translate along the z-axis.</param>
    /// <returns>A new <see cref="Matrix4x4"/> translation matrix.</returns>
    public static Matrix4x4 Translate(double dx, double dy, double dz) =>
        new Matrix4x4(
            1, 0, 0, dx,
            0, 1, 0, dy,
            0, 0, 1, dz,
            0, 0, 0, 1);

    /// <summary>
    /// Creates a scale matrix.
    /// </summary>
    public static Matrix4x4 Scale(double sx, double sy, double sz) =>
        new Matrix4x4(
            sx, 0, 0, 0,
            0, sy, 0, 0,
            0, 0, sz, 0,
            0, 0, 0, 1);

    /// <summary>
    /// Creates uniform scale matrix.
    /// </summary>
    public static Matrix4x4 Scale(double s) => Scale(s, s, s);

    /// <summary>
    /// Creates a rotation matrix along the x-axis.
    /// </summary>
    public static Matrix4x4 RotateX(double r) =>
        new Matrix4x4(
            1, 0, 0, 0,
            0, (double)Math.Cos(r), -(double)Math.Sin(r), 0,
            0, (double)Math.Sin(r), (double)Math.Cos(r), 0,
            0, 0, 0, 1);

    /// <summary>
    /// Creates a rotation matrix along the y-axis.
    /// </summary>
    public static Matrix4x4 RotateY(double r) =>
        new Matrix4x4(
            (double)Math.Cos(r), 0, (double)Math.Sin(r), 0,
            0, 1, 0, 0,
            -(double)Math.Sin(r), 0, (double)Math.Cos(r), 0,
            0, 0, 0, 1);

    /// <summary>
    /// Creates a rotation matrix along the z-axis.
    /// </summary>
    public static Matrix4x4 RotateZ(double r) =>
        new Matrix4x4(
            (double)Math.Cos(r), -(double)Math.Sin(r), 0, 0,
            (double)Math.Sin(r), (double)Math.Cos(r), 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1);

    /// <summary>
    /// Creates a shearing matrix.
    /// </summary>
    public static Matrix4x4 Shear(double xy, double xz, double yx, double yz, double zx, double zy) =>
        new Matrix4x4(
            1, xy, xz, 0,
            yx, 1, yz, 0,
            zx, zy, 1, 0,
            0, 0, 0, 1);

    /// <summary>
    /// Constructs a perspective view (camera) matrix.
    /// </summary>
    /// <param name="from">The position of the camera.</param>
    /// <param name="to">Where the camera is pointed at.</param>
    /// <param name="up">How the camera is oriented.</param>
    /// <returns>A new <see cref="Matrix4x4"/> view transformation.</returns>
    public static Matrix4x4 View(Vector4 from, Vector4 to, Vector4 up)
    {
        var fwd = (to - from).Normalize();
        var left = Vector4.Cross3(fwd, up.Normalize());
        var trueUp = Vector4.Cross3(left, fwd);
        var orientation =
            new Matrix4x4(
                left.X, left.Y, left.Z, 0,
                trueUp.X, trueUp.Y, trueUp.Z, 0,
                -fwd.X, -fwd.Y, -fwd.Z, 0,
                0, 0, 0, 1);

        return orientation * Translate(-from.X, -from.Y, -from.Z);
    }
}

#pragma warning restore SA1117 // ParametersMustBeOnSameLineOrSeparateLines
