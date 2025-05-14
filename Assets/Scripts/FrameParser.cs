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
            Debug.Log($"FrameParser: Attempting to read file from path: {jsonPath}");
            
            // Проверяем существование файла
            if (!File.Exists(jsonPath))
            {
                Debug.LogError($"FrameParser: File does not exist at path: {jsonPath}");
                return null;
            }

            string jsonContent = File.ReadAllText(jsonPath);
            Debug.Log($"FrameParser: Successfully read file, content length: {jsonContent.Length} characters");
            
            var audioAnalysis = JsonUtility.FromJson<AudioAnalysis>(jsonContent);
            if (audioAnalysis == null)
            {
                Debug.LogError("FrameParser: Failed to parse JSON content");
                return null;
            }

            Debug.Log($"FrameParser: Successfully parsed JSON, found {audioAnalysis.frames?.Count ?? 0} frames");
            return audioAnalysis;
        }
        catch (Exception ex)
        {
            Debug.LogError($"FrameParser: Error parsing JSON: {ex.Message}\nStack trace: {ex.StackTrace}");
            return null;
        }
    }
} 