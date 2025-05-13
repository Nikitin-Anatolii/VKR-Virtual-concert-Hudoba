using UnityEngine;
using System.Collections.Generic;

public class FrameAnimationController : MonoBehaviour
{
    [SerializeField] private AnimationController animationController;
    [SerializeField] private float thresholdEnergy = 0.5f;
    [SerializeField] private float thresholdSpectralFlux = 0.3f;
    [SerializeField] private float thresholdSpectralCentroid = 0.5f;

    private List<Frame> frames;
    private int currentFrameIndex = 0;

    private void Awake()
    {
        if (animationController == null)
        {
            animationController = GetComponent<AnimationController>();
        }
    }

    public void SetFrames(List<Frame> newFrames)
    {
        frames = newFrames;
        currentFrameIndex = 0;
    }

    public void ProcessNextFrame()
    {
        if (frames == null || frames.Count == 0 || currentFrameIndex >= frames.Count)
            return;

        Frame currentFrame = frames[currentFrameIndex];
        ProcessFrame(currentFrame);
        currentFrameIndex++;
    }

    private void ProcessFrame(Frame frame)
    {
        foreach (AnimationTriggerType triggerType in System.Enum.GetValues(typeof(AnimationTriggerType)))
        {
            AnimationTrigger trigger = animationController.GetTrigger(triggerType);
            if (trigger != null && trigger.IsInRange(frame))
            {
                animationController.SetTrigger(triggerType);
            }
            else
            {
                animationController.ResetTrigger(triggerType);
            }
        }
    }

    public void ResetToStart()
    {
        currentFrameIndex = 0;
        animationController.ResetAllTriggers();
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

    // Метод для настройки пороговых значений
    public void SetThresholds(float energy, float flux, float centroid)
    {
        thresholdEnergy = energy;
        thresholdSpectralFlux = flux;
        thresholdSpectralCentroid = centroid;
    }
} 