namespace Linie.TheNextWeek
{
    using System;
    
    public interface IMaterial
    {
        bool Scatter(
            in Ray rIn, 
            in ShadeRecord rec,
            in Random rng, 
            out Color attenuation, 
            out Ray scattered);
    }
}