using UnityEngine;
using System.Collections.Generic;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private List<AnimationTrigger> triggers = new List<AnimationTrigger>();

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("AnimationController: No Animator component found!");
            }
            else
            {
                Debug.Log("AnimationController: Animator component found automatically");
            }
        }
    }

    public void SetTriggerData(AnimationTriggerData triggerData)
    {
        if (triggerData != null)
        {
            triggers = new List<AnimationTrigger>(triggerData.Triggers);
            Debug.Log($"AnimationController: Loaded {triggers.Count} triggers from triggerData");
            
            // Проверяем, что все триггеры имеют уникальные типы
            var types = new HashSet<AnimationTriggerType>();
            foreach (var trigger in triggers)
            {
                if (!types.Add(trigger.type))
                {
                    Debug.LogWarning($"AnimationController: Duplicate trigger type found: {trigger.type}");
                }
            }
        }
        else
        {
            Debug.LogWarning("AnimationController: Attempting to set null trigger data!");
            triggers.Clear();
        }
    }

    public void SetTrigger(AnimationTriggerType type)
    {
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned!");
            return;
        }

        string triggerName = type.ToString();
        animator.SetTrigger(triggerName);
        Debug.Log($"Set trigger: {triggerName}");
    }

    public void ResetTrigger(AnimationTriggerType type)
    {
        if (animator == null)
        {
            Debug.LogError("AnimationController: Cannot reset trigger - Animator is null!");
            return;
        }

        string triggerName = type.ToString();
        animator.ResetTrigger(triggerName);
        Debug.Log($"AnimationController: Reset trigger {triggerName}");
    }

    public void ResetAllTriggers()
    {
        if (animator == null)
        {
            Debug.LogError("AnimationController: Cannot reset triggers - Animator is null!");
            return;
        }

        foreach (var trigger in triggers)
        {
            string triggerName = trigger.type.ToString();
            animator.ResetTrigger(triggerName);
        }
        Debug.Log("AnimationController: Reset all triggers");
    }

    public bool IsTriggerActive(AnimationTriggerType type)
    {
        if (animator == null) return false;
        
        string triggerName = type.ToString();
        return animator.GetBool(triggerName);
    }

    public List<AnimationTriggerType> GetAllTriggerTypes()
    {
        return triggers.ConvertAll(t => t.type);
    }

    public bool IsAnimationPlaying(AnimationTriggerType type)
    {
        if (animator == null) return false;
        
        string animationName = type.ToString();
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName);
    }
} 