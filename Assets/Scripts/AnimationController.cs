using UnityEngine;
using System.Collections.Generic;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationTriggerData triggerData;
    private List<AnimationTrigger> triggers = new List<AnimationTrigger>();

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (triggerData != null)
        {
            triggers = new List<AnimationTrigger>(triggerData.Triggers);
        }
    }

    public void SetTriggerData(AnimationTriggerData newTriggerData)
    {
        triggerData = newTriggerData;
        triggers = new List<AnimationTrigger>(triggerData.Triggers);
    }

    public AnimationTrigger GetTrigger(AnimationTriggerType type)
    {
        return triggers.Find(t => t.type == type);
    }

    public void SetTrigger(AnimationTriggerType type)
    {
        AnimationTrigger trigger = GetTrigger(type);
        if (trigger != null)
        {
            trigger.isActive = true;
            animator.SetTrigger(type.ToString());
        }
        else
        {
            Debug.LogWarning($"Trigger {type} not found!");
        }
    }

    public void ResetTrigger(AnimationTriggerType type)
    {
        AnimationTrigger trigger = GetTrigger(type);
        if (trigger != null)
        {
            trigger.isActive = false;
            animator.ResetTrigger(type.ToString());
        }
        else
        {
            Debug.LogWarning($"Trigger {type} not found!");
        }
    }

    public void ResetAllTriggers()
    {
        foreach (var trigger in triggers)
        {
            trigger.isActive = false;
            animator.ResetTrigger(trigger.type.ToString());
        }
    }

    public bool IsTriggerActive(AnimationTriggerType type)
    {
        AnimationTrigger trigger = GetTrigger(type);
        return trigger != null && trigger.isActive;
    }

    public List<AnimationTriggerType> GetAllTriggerTypes()
    {
        return triggers.ConvertAll(t => t.type);
    }
} 