namespace Linie.InOneWeekend
{
    using System;
    
    public interface IMaterial
    {
        bool Scatter(
            in Ray3 rIn, 
            in ShadeRecord rec,
            in Random rng, 
            out Color attenuation, 
            out Ray3 scattered);
    }
}