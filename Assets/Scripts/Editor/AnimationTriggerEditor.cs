using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AnimationTriggerData))]
public class AnimationTriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AnimationTriggerData triggerData = (AnimationTriggerData)target;

        EditorGUILayout.Space();
        if (GUILayout.Button("Create Default Triggers"))
        {
            triggerData.CreateDefaultTriggers();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Triggers", EditorStyles.boldLabel);

        for (int i = 0; i < triggerData.Triggers.Count; i++)
        {
            AnimationTrigger trigger = triggerData.Triggers[i];
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            EditorGUILayout.LabelField(trigger.type.ToString(), EditorStyles.boldLabel);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Energy Range", GUILayout.Width(100));
            EditorGUILayout.MinMaxSlider(ref trigger.energyRange.min, ref trigger.energyRange.max, 0f, 1f);
            EditorGUILayout.LabelField($"{trigger.energyRange.min:F2} - {trigger.energyRange.max:F2}", GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Flux Range", GUILayout.Width(100));
            EditorGUILayout.MinMaxSlider(ref trigger.spectralFluxRange.min, ref trigger.spectralFluxRange.max, 0f, 1f);
            EditorGUILayout.LabelField($"{trigger.spectralFluxRange.min:F2} - {trigger.spectralFluxRange.max:F2}", GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Centroid Range", GUILayout.Width(100));
            EditorGUILayout.MinMaxSlider(ref trigger.spectralCentroidRange.min, ref trigger.spectralCentroidRange.max, 0f, 1f);
            EditorGUILayout.LabelField($"{trigger.spectralCentroidRange.min:F2} - {trigger.spectralCentroidRange.max:F2}", GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(triggerData);
        }
    }
} 