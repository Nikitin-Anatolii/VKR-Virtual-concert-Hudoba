using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class FrameParser
{
    public static AudioAnalysis ParseJsonToFrames(string jsonPath)
    {
        try
        {
            string jsonContent = File.ReadAllText(jsonPath);
            var audioAnalysis = JsonUtility.FromJson<AudioAnalysis>(jsonContent);
            return audioAnalysis;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error parsing JSON: {ex.Message}");
            return new AudioAnalysis { frames = new List<Frame>() };
        }
    }
} 