// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

using System;
using System.IO;

/// <summary>
/// Represents an rectangular drawing surface.
/// </summary>
public class Canvas
{
    private int width;
    private int height;
    private Color[] data;

    /// <summary>
    /// Constructs a new <see cref="Canvs"/> with given dimensions.
    /// </summary>
    public Canvas(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.data = new Color[width * height];
    }

    /// <summary>
    /// Returns the color of the pixel at given coordinates.
    /// </summary>
    public Color this[int x, int y]
    {
        get => this.data[y * this.width + x];
        set => this.data[y * this.width + x] = value;
    }

    /// <summary>
    /// Gets the width of the canvas.
    /// </summary>
    public int Width => this.width;

    /// <summary>
    /// Gets the height of the canvas.
    /// </summary>
    public int Height => this.height;

    public void SavePpm(string filename)
    {
        using (var s = File.OpenWrite(filename))
        using (var w = new StreamWriter(s))
        {
            w.WriteLine($"P3");
            w.WriteLine($"{this.Width} {this.Height}");
            w.WriteLine($"255");

            for (var j = this.height - 1; j >= 0; j--)
            {
                for (var i = 0; i < this.width; i++)
                {
                    var rgb = GetColorBytes(this[i, j]);
                    w.WriteLine($"{rgb.Item1} {rgb.Item2} {rgb.Item3}");
                }
            }
        }
    }

    /// <summary>
    /// Returns the given <see cref="Color"/> as a 3-tuple of 
    /// clamped <c>int</c> values in the closed interval [0,255].
    /// </summary>
    public static Tuple<int, int, int> GetColorBytes(Color c) =>
        Tuple.Create(
            Clamp((int)(c.R * 255), 0, 255),
            Clamp((int)(c.G * 255), 0, 255),
            Clamp((int)(c.B * 255), 0, 255));

    private static int Clamp(int v, int min, int max)
    {
        if (v < min) return min;
        if (v > max) return max;
        return v;
    }
}
