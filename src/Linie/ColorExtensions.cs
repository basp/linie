// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

public static class ColorExtensions
{
    /// <summary>
    /// Returns the given <see cref="Color"/> as a 3-tuple of 
    /// clamped <c>int</c> values in the interval <c>[0, 255]</c>.
    /// </summary>
    public static Tuple<int, int, int> GetColorBytes(this Color self) =>
        Tuple.Create(
            Utils.Clamp((int)(self.R * 255), 0, 255),
            Utils.Clamp((int)(self.G * 255), 0, 255),
            Utils.Clamp((int)(self.B * 255), 0, 255));
}
