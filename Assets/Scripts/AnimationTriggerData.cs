using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "AnimationTriggerData", menuName = "Animation/Trigger Data")]
public class AnimationTriggerData : ScriptableObject
{
    [SerializeField] private List<AnimationTrigger> triggers = new List<AnimationTrigger>();

    public List<AnimationTrigger> Triggers => triggers;

    public void AddTrigger(AnimationTriggerType type)
    {
        if (!HasTrigger(type))
        {
            var trigger = new AnimationTrigger(type);
            trigger.energyRange = new FloatRange(0f, 0f);
            trigger.spectralFluxRange = new FloatRange(0f, 0f);
            trigger.spectralCentroidRange = new FloatRange(0f, 0f);
            triggers.Add(trigger);
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

    
} 