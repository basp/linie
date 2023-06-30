// Licensed under the MIT license. See LICENSE file in the samples root for full license information.

namespace Linie;

public static class Utils
{
    public static double Clamp(double v, double min, double max)
    {
        if (v < min)
        {
            return min;
        }

        if (v > max)
        {
            return max;
        }

        return v;
    }

    public static int Clamp(int v, int min, int max)
    {
        if (v < min)
        {
            return min;
        }

        if (v > max)
        {
            return max;
        }
        
        return v;
    }
}