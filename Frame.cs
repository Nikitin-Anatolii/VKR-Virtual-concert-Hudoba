using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

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

public class AudioAnalysis
{
    public int bpm { get; set; }
    public List<Frame> frames { get; set; }
}

public class FrameParser
{
    public static AudioAnalysis ParseJsonToFrames(string jsonPath)
    {
        try
        {
            string jsonContent = File.ReadAllText(jsonPath);
            var audioAnalysis = JsonSerializer.Deserialize<AudioAnalysis>(jsonContent);
            return audioAnalysis;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing JSON: {ex.Message}");
            return new AudioAnalysis { frames = new List<Frame>() };
        }
    }
} 