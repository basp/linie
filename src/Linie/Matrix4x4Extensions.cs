// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

using System;
using System.Linq;

public static class Matrix4x4Extensions
{
    public static void Map(
        in Matrix4x4 a,
        in Func<int, int, double, double> mapf,
        ref Matrix4x4 c)
    {
        for (var j = 0; j < 4; j++)
        {
            for (var i = 0; i < 4; i++)
            {
                c[i, j] = mapf(i, j, a[i, j]);
            }
        }
    }

    /// <summary>
    /// Applies a translation matrix to an existing matrix.
    /// </summary>
    public static Matrix4x4 Translate(
        this Matrix4x4 m,
        double x,
        double y,
        double z) => Affine.Translate(x, y, z) * m;

    /// <summary>
    /// Applies a scale matrix to an existing matrix using 
    /// constant scale factor in all dimensions.
    /// </summary>
    public static Matrix4x4 Scale(this Matrix4x4 m, double s) =>
        Scale(m, s, s, s);

    /// <summary>
    /// Applies a scale matrix to an existing matrix.
    /// </summary>
    public static Matrix4x4 Scale(
        this Matrix4x4 m,
        double x,
        double y,
        double z) => Affine.Scale(x, y, z) * m;

    /// <summary>
    /// Applies a x-axis rotation matrix to a given matrix.
    /// </summary>
    public static Matrix4x4 RotateX(this Matrix4x4 m, double r) =>
        Affine.RotateX(r) * m;

    /// <summary>
    /// Applies an y-axis rotation matrix to a given matrix.
    /// </summary>
    public static Matrix4x4 RotateY(this Matrix4x4 m, double r) =>
        Affine.RotateY(r) * m;

    /// <summary>
    /// Applies a z-axis rotation matrix to a given matrix.
    /// </summary>
    public static Matrix4x4 RotateZ(this Matrix4x4 m, double r) =>
        Affine.RotateZ(r) * m;

    /// <summary>
    /// Applies a shear matrix to a given matrix.
    /// </summary>
    public static Matrix4x4 Shear(
        this Matrix4x4 m,
        double xy,
        double xz,
        double yx,
        double yz,
        double zx,
        double zy) => Affine.Shear(xy, xz, yx, yz, zx, zy) * m;
}
