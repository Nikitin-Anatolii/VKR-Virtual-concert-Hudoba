using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AnimationTriggerData", menuName = "Animation/Trigger Data")]
public class AnimationTriggerData : ScriptableObject
{
    [SerializeField] private List<AnimationTrigger> triggers = new List<AnimationTrigger>();

    public List<AnimationTrigger> Triggers => triggers;

    public void AddTrigger(AnimationTriggerType type)
    {
        if (!triggers.Exists(t => t.type == type))
        {
            triggers.Add(new AnimationTrigger(type));
        }
    }

    public void RemoveTrigger(AnimationTriggerType type)
    {
        triggers.RemoveAll(t => t.type == type);
    }

    public bool HasTrigger(AnimationTriggerType type)
    {
        return triggers.Exists(t => t.type == type);
    }

    public AnimationTrigger GetTrigger(AnimationTriggerType type)
    {
        return triggers.Find(t => t.type == type);
    }

    public void SetTriggerRanges(AnimationTriggerType type, FloatRange energy, FloatRange flux, FloatRange centroid)
    {
        var trigger = GetTrigger(type);
        if (trigger != null)
        {
            trigger.SetRanges(energy, flux, centroid);
        }
    }

    public List<AnimationTriggerType> GetTriggerTypes()
    {
        return triggers.ConvertAll(t => t.type);
    }

    // Метод для создания предустановленных триггеров
    public void CreateDefaultTriggers()
    {
        // Очищаем существующие триггеры
        triggers.Clear();

        // Создаем триггеры с предустановленными диапазонами
        // High Energy
        var highEnergy = new AnimationTrigger(AnimationTriggerType.HighEnergy);
        highEnergy.SetRanges(
            new FloatRange(0.7f, 1.0f),  // Energy
            new FloatRange(0.0f, 1.0f),  // Flux
            new FloatRange(0.0f, 1.0f)   // Centroid
        );
        triggers.Add(highEnergy);

        // Low Energy
        var lowEnergy = new AnimationTrigger(AnimationTriggerType.LowEnergy);
        lowEnergy.SetRanges(
            new FloatRange(0.0f, 0.3f),  // Energy
            new FloatRange(0.0f, 1.0f),  // Flux
            new FloatRange(0.0f, 1.0f)   // Centroid
        );
        triggers.Add(lowEnergy);

        // High Flux
        var highFlux = new AnimationTrigger(AnimationTriggerType.HighFlux);
        highFlux.SetRanges(
            new FloatRange(0.0f, 1.0f),  // Energy
            new FloatRange(0.7f, 1.0f),  // Flux
            new FloatRange(0.0f, 1.0f)   // Centroid
        );
        triggers.Add(highFlux);

        // Low Flux
        var lowFlux = new AnimationTrigger(AnimationTriggerType.LowFlux);
        lowFlux.SetRanges(
            new FloatRange(0.0f, 1.0f),  // Energy
            new FloatRange(0.0f, 0.3f),  // Flux
            new FloatRange(0.0f, 1.0f)   // Centroid
        );
        triggers.Add(lowFlux);

        // High Centroid
        var highCentroid = new AnimationTrigger(AnimationTriggerType.HighCentroid);
        highCentroid.SetRanges(
            new FloatRange(0.0f, 1.0f),  // Energy
            new FloatRange(0.0f, 1.0f),  // Flux
            new FloatRange(0.7f, 1.0f)   // Centroid
        );
        triggers.Add(highCentroid);

        // Low Centroid
        var lowCentroid = new AnimationTrigger(AnimationTriggerType.LowCentroid);
        lowCentroid.SetRanges(
            new FloatRange(0.0f, 1.0f),  // Energy
            new FloatRange(0.0f, 1.0f),  // Flux
            new FloatRange(0.0f, 0.3f)   // Centroid
        );
        triggers.Add(lowCentroid);

        // Beat (комбинация высоких значений)
        var beat = new AnimationTrigger(AnimationTriggerType.Beat);
        beat.SetRanges(
            new FloatRange(0.6f, 1.0f),  // Energy
            new FloatRange(0.6f, 1.0f),  // Flux
            new FloatRange(0.5f, 1.0f)   // Centroid
        );
        triggers.Add(beat);

        // Drop (очень высокие значения)
        var drop = new AnimationTrigger(AnimationTriggerType.Drop);
        drop.SetRanges(
            new FloatRange(0.8f, 1.0f),  // Energy
            new FloatRange(0.8f, 1.0f),  // Flux
            new FloatRange(0.7f, 1.0f)   // Centroid
        );
        triggers.Add(drop);
    }
} 