using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FrameAnimationController : MonoBehaviour
{
    [SerializeField] private AnimationController animationController;
    [SerializeField] private AnimationTriggerData triggerData;
    [SerializeField] private AudioController audioController;

    private int currentSong = 0;
    private List<Frame> frames;
    private int currentFrameIndex = 0;
    private bool isPlaying = false;
    private Coroutine frameProcessingCoroutine;
    private string jsonPath;



    public void Load(int number)
    {
        currentSong = number;
        
        jsonPath = "Assets/Materials/Songs/audio_analysis" + currentSong + ".json";
        
        LoadFrames();
        animationController.SetTriggerData(triggerData);
        audioController.PlayTrack(currentSong);
        StartFrameProcessing();
    }

    private void LoadFrames()
    {
        Debug.Log($"Attempting to load frames from: {jsonPath}");
        
        if (string.IsNullOrEmpty(jsonPath))
        {
            Debug.LogError("JSON path not specified!");
            return;
        }

        AudioAnalysis audioAnalysis = FrameParser.ParseJsonToFrames(jsonPath);
        if (audioAnalysis != null && audioAnalysis.frames != null)
        {
            frames = audioAnalysis.frames;
            Debug.Log($"Successfully loaded {frames.Count} frames");
        }
        else
        {
            Debug.LogError($"Failed to load frames from {jsonPath}");
        }
    }

    public void StartFrameProcessing()
    {
        Debug.Log("StartFrameProcessing called");
        
        if (isPlaying)
        {
            Debug.Log("Already playing, ignoring StartFrameProcessing call");
            return;
        }
        
        if (frames == null || frames.Count == 0)
        {
            Debug.LogError("No frames loaded, cannot start processing");
            return;
        }
        
        isPlaying = true;
        if (frameProcessingCoroutine != null)
        {
            Debug.Log("Stopping existing coroutine");
            StopCoroutine(frameProcessingCoroutine);
        }
        
        Debug.Log("Starting frame processing...");
        frameProcessingCoroutine = StartCoroutine(ProcessFramesCoroutine());
        
        if (frameProcessingCoroutine == null)
        {
            Debug.LogError("Failed to start frame processing coroutine!");
        }
        else
        {
            Debug.Log("Frame processing coroutine started successfully");
        }
    }

    public void StopFrameProcessing()
    {
        Debug.Log("StopFrameProcessing called");
        
        isPlaying = false;
        if (frameProcessingCoroutine != null)
        {
            Debug.Log("Stopping frame processing coroutine");
            StopCoroutine(frameProcessingCoroutine);
            frameProcessingCoroutine = null;
        }
        else
        {
            Debug.Log("No coroutine to stop");
        }
    }

    private IEnumerator ProcessFramesCoroutine()
    {
        Debug.Log("ProcessFramesCoroutine started");
        Debug.Log($"Starting to process {frames.Count} frames");
        
        while (isPlaying && frames != null && currentFrameIndex < frames.Count)
        {
            ProcessCurrentFrame();
            currentFrameIndex++;
            Debug.Log($"Processed frame {currentFrameIndex}/{frames.Count}");
            yield return new WaitForSeconds(1f); // Ожидание 1 секунды между фреймами
        }

        if (currentFrameIndex >= frames.Count)
        {
            Debug.Log("Reached end of frames");
            StopFrameProcessing();
        }
        
        Debug.Log("ProcessFramesCoroutine finished");
    }

    private void ProcessCurrentFrame()
    {
        if (frames == null || currentFrameIndex >= frames.Count)
        {
            Debug.LogError($"Invalid frame state: frames={frames != null}, index={currentFrameIndex}");
            return;
        }

        Frame currentFrame = frames[currentFrameIndex];
        Debug.Log($"\n=== Frame {currentFrameIndex + 1} ===");
        Debug.Log($"Current Values:");
        Debug.Log($"- Energy: {currentFrame.energy:F3}");
        Debug.Log($"- Spectral Flux: {currentFrame.spectral_flux:F3}");
        Debug.Log($"- Spectral Centroid: {currentFrame.spectral_centroid:F3}");
        
        bool anyTriggerActivated = false;
        
        // Проверяем каждый триггер из AnimationTriggerData
        foreach (var trigger in triggerData.Triggers)
        {
            if (trigger.IsInRange(currentFrame))
            {
                // Проверяем, не играет ли уже эта анимация
                if (!animationController.IsAnimationPlaying(trigger.type))
                {
                    anyTriggerActivated = true;
                    Debug.Log($"\n✓ TRIGGER ACTIVATED: {trigger.type}");
                    Debug.Log($"Values in range:");
                    Debug.Log($"- Energy: {currentFrame.energy:F3} in range [{trigger.energyRange.min:F3}, {trigger.energyRange.max:F3}]");
                    Debug.Log($"- Flux: {currentFrame.spectral_flux:F3} in range [{trigger.spectralFluxRange.min:F3}, {trigger.spectralFluxRange.max:F3}]");
                    Debug.Log($"- Centroid: {currentFrame.spectral_centroid:F3} in range [{trigger.spectralCentroidRange.min:F3}, {trigger.spectralCentroidRange.max:F3}]");
                    animationController.SetTrigger(trigger.type);
                }
                else
                {
                    Debug.Log($"\nAnimation {trigger.type} is already playing, skipping trigger");
                }
            }
        }

        if (!anyTriggerActivated)
        {
            Debug.Log("\nNo new triggers activated for this frame");
        }
    }

    public void ResetToStart()
    {
        currentFrameIndex = 0;
        Debug.Log("Reset to start");
    }

    public bool HasMoreFrames()
    {
        return frames != null && currentFrameIndex < frames.Count;
    }

    public float GetProgress()
    {
        if (frames == null || frames.Count == 0)
            return 0f;
        
        return (float)currentFrameIndex / frames.Count;
    }
} 