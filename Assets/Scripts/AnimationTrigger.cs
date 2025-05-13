using UnityEngine;

[System.Serializable]
public class AnimationTrigger
{
    public AnimationTriggerType type;
    public bool isActive;

    [Header("Parameter Ranges")]
    public FloatRange energyRange;
    public FloatRange spectralFluxRange;
    public FloatRange spectralCentroidRange;

    public AnimationTrigger(AnimationTriggerType type)
    {
        this.type = type;
        this.isActive = false;
        this.energyRange = new FloatRange(0f, 1f);
        this.spectralFluxRange = new FloatRange(0f, 1f);
        this.spectralCentroidRange = new FloatRange(0f, 1f);
    }

    public bool IsInRange(Frame frame)
    {
        return energyRange.IsInRange(frame.energy) &&
               spectralFluxRange.IsInRange(frame.spectral_flux) &&
               spectralCentroidRange.IsInRange(frame.spectral_centroid);
    }

    public void SetRanges(FloatRange energy, FloatRange flux, FloatRange centroid)
    {
        energyRange = energy;
        spectralFluxRange = flux;
        spectralCentroidRange = centroid;
    }
}

[System.Serializable]
public class FloatRange
{
    [Range(0f, 1f)]
    public float min;
    [Range(0f, 1f)]
    public float max;

    public FloatRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public bool IsInRange(float value)
    {
        return value >= min && value <= max;
    }

    // Метод для проверки и исправления значений
    public void Validate()
    {
        if (min > max)
        {
            float temp = min;
            min = max;
            max = temp;
        }
    }
} 