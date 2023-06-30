// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

public static class CanvasExtensions
{
    /// <summary>
    /// Writes the <see cref="Canvas"/> instance as
    /// a Portable Pixmap Format (PPM) file.
    /// </summary>
    public static void WritePortablePixmap(this Canvas self, string filename)
    {
        using (var s = File.OpenWrite(filename))
        using (var w = new StreamWriter(s))
        {
            w.WriteLine($"P3");
            w.WriteLine($"{self.Width} {self.Height}");
            w.WriteLine($"255");

            for (var j = self.Height - 1; j >= 0; j--)
            {
                for (var i = 0; i < self.Width; i++)
                {
                    var rgb = self[i, j].GetColorBytes();
                    w.WriteLine($"{rgb.Item1} {rgb.Item2} {rgb.Item3}");
                }
            }
        }
    }
}
