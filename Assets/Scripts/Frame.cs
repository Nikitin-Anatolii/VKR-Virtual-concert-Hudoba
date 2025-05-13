using System;

[Serializable]
public class Frame
{
    public float spectral_flux { get; set; }
    public float energy { get; set; }
    public float spectral_centroid { get; set; }

    public Frame(float spectral_flux, float energy, float spectral_centroid)
    {
        this.spectral_flux = spectral_flux;
        this.energy = energy;
        this.spectral_centroid = spectral_centroid;
    }
}
