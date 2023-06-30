// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

/// <summary>
/// Represents an rectangular drawing surface.
/// </summary>
/// <remarks>
/// The <see cref="Canvas"/> class is included as a lightweight drawing
/// canvas that can be used in lieux of any alternatives. It is basically
/// a tiny wrapper around an array of <see cref="Color"/> values that can
/// be saved as a Portable Pixmap (PPM) file.
/// </remarks>
public class Canvas
{
    private int width;

    private int height;

    private Color[] data;

    /// <summary>
    /// Initializes a new <see cref="Canvas"/> instance of given size.
    /// </summary>
    public Canvas(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.data = new Color[width * height];
    }

    /// <summary>
    /// Gets the color of the pixel at given coordinates.
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
}
