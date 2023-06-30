// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

using System;
using System.Linq;

public static class Matrix4x4Extensions
{
    /// <summary>
    /// Calculates the determinant of given matrix <c>a</c> using Laplace 
    /// expansion.
    /// </summary>
    public static double Determinant(this Matrix4x4 a) =>
        (a[0, 0] * a.Cofactor(0, 0)) +
        (a[0, 1] * a.Cofactor(0, 1)) +
        (a[0, 2] * a.Cofactor(0, 2)) +
        (a[0, 3] * a.Cofactor(0, 3));

    public static bool IsInvertible(this Matrix4x4 a) =>
        a.Determinant() != 0;

    public static bool TryInvert(this Matrix4x4 a, ref Matrix4x4 m)
    {
        var d = a.Determinant();
        if (d == 0)
        {
            return false;
        }

        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                m[j, i] = a.Cofactor(i, j) / d;
            }
        }

        return true;
    }

    public static void Invert(this Matrix4x4 a, ref Matrix4x4 m)
    {
        if (!a.TryInvert(ref m))
        {
            throw new ArgumentException("Matrix is not invertible.", nameof(a));
        }
    }

    /// <summary>
    /// Experimental.
    /// </summary>
    internal static void Invert2(this Matrix4x4 a, Matrix4x4 m)
    {
        var d = a.Determinant();
        if (d == 0)
        {
            throw new ArgumentException("Matrix is not invertible.", nameof(a));
        }

        Parallel.For(0, 4, i =>
        {
            for (var j = 0; j < 4; j++)
            {
                m[j, i] = a.Cofactor(i, j) / d;
            }
        });
    }

    /// <summary>
    /// Returns the inverse of the given matrix.
    /// </summary>
    public static Matrix4x4 Invert(this Matrix4x4 a)
    {
        var m = new Matrix4x4(0);
        a.Invert(ref m);
        return m;
    }

    // This creates a new 3x3 matrix from a 4x4 matrix by dropping one
    // row and one column. The row and column to be dropped are specified
    // by the <c>dropRow</c> and <c>dropCol</c> arguments respectively.
    //
    // Ex. (dropRow = 2, dropCol = 1)
    // ---
    // A B C D  =>  A | C D  =>  A C D
    // E F G H      E | G H      E G H
    // I J K L      - + - -      M O P
    // M N O P      M | O P
    internal static Matrix3x3 Submatrix(this Matrix4x4 a, int dropRow, int dropCol)
    {
        var rows = Enumerable.Range(0, 4) // 0, 1, 2, 3
            .Where(x => x != dropRow)
            .ToArray();

        var cols = Enumerable.Range(0, 4)
            .Where(x => x != dropCol)
            .ToArray();

        var m = new Matrix3x3(0);
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                m[i, j] = a[rows[i], cols[j]];
            }
        }

        return m;
    }

    // For more information on minors, cofactors, etc. and how the
    // determinant is calculated see:
    //
    // * https://en.wikipedia.org/wiki/Minor_(linear_algebra)
    // * https://en.wikipedia.org/wiki/Laplace_expansion
    internal static double Minor(this Matrix4x4 a, int row, int col) =>
        a.Submatrix(row, col).Determinant();

    internal static double Cofactor(this Matrix4x4 a, int row, int col) =>
        (row + col) % 2 == 0 ? a.Minor(row, col) : -a.Minor(row, col);


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
