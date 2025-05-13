using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(AnimationTriggerData))]
public class AnimationTriggerEditor : Editor
{
    private SerializedProperty triggersProperty;
    private AnimationTriggerType selectedType;

    private void OnEnable()
    {
        triggersProperty = serializedObject.FindProperty("triggers");
        
        // Получаем первое доступное значение из enum
        selectedType = System.Enum.GetValues(typeof(AnimationTriggerType))
            .Cast<AnimationTriggerType>()
            .FirstOrDefault();

        // Очищаем несуществующие значения
        CleanupNonExistentTriggers();
    }

    private void CleanupNonExistentTriggers()
    {
        AnimationTriggerData triggerData = (AnimationTriggerData)target;
        var validTypes = System.Enum.GetValues(typeof(AnimationTriggerType))
            .Cast<AnimationTriggerType>()
            .ToArray();

        // Создаем список триггеров для удаления
        var triggersToRemove = triggerData.Triggers
            .Where(t => !validTypes.Contains(t.type))
            .Select(t => t.type)
            .ToList();

        // Удаляем несуществующие триггеры
        foreach (var type in triggersToRemove)
        {
            triggerData.RemoveTrigger(type);
        }

        if (triggersToRemove.Count > 0)
        {
            EditorUtility.SetDirty(triggerData);
            Debug.Log($"Removed {triggersToRemove.Count} triggers with non-existent types");
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        AnimationTriggerData triggerData = (AnimationTriggerData)target;

        EditorGUILayout.Space(10);
        
        // Секция добавления нового триггера
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Add New Trigger", EditorStyles.boldLabel);
        
        // Выпадающий список с доступными типами триггеров
        var availableTypes = System.Enum.GetValues(typeof(AnimationTriggerType))
            .Cast<AnimationTriggerType>()
            .Where(t => !triggerData.HasTrigger(t))
            .ToArray();

        if (availableTypes.Length > 0)
        {
            selectedType = (AnimationTriggerType)EditorGUILayout.EnumPopup("Trigger Type", selectedType);
            
            if (GUILayout.Button("Add Trigger", GUILayout.Height(25)))
            {
                triggerData.AddTrigger(selectedType);
                EditorUtility.SetDirty(triggerData);
            }
        }
        else
        {
            EditorGUILayout.LabelField("All trigger types are already added", EditorStyles.centeredGreyMiniLabel);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Existing Triggers", EditorStyles.boldLabel);

        if (triggersProperty != null)
        {
            for (int i = 0; i < triggersProperty.arraySize; i++)
            {
                SerializedProperty triggerProperty = triggersProperty.GetArrayElementAtIndex(i);
                SerializedProperty typeProperty = triggerProperty.FindPropertyRelative("type");
                SerializedProperty energyRangeProperty = triggerProperty.FindPropertyRelative("energyRange");
                SerializedProperty fluxRangeProperty = triggerProperty.FindPropertyRelative("spectralFluxRange");
                SerializedProperty centroidRangeProperty = triggerProperty.FindPropertyRelative("spectralCentroidRange");

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(typeProperty.enumDisplayNames[typeProperty.enumValueIndex], EditorStyles.boldLabel);
                
                // Кнопка удаления триггера
                if (GUILayout.Button("Remove", GUILayout.Width(80)))
                {
                    triggerData.RemoveTrigger((AnimationTriggerType)typeProperty.enumValueIndex);
                    EditorUtility.SetDirty(triggerData);
                    break;
                }
                EditorGUILayout.EndHorizontal();
                
                // Energy Range
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Energy Range", GUILayout.Width(100));
                SerializedProperty energyMin = energyRangeProperty.FindPropertyRelative("min");
                SerializedProperty energyMax = energyRangeProperty.FindPropertyRelative("max");
                float energyMinValue = energyMin.floatValue;
                float energyMaxValue = energyMax.floatValue;
                EditorGUILayout.MinMaxSlider(ref energyMinValue, ref energyMaxValue, 0f, 1f);
                energyMin.floatValue = energyMinValue;
                energyMax.floatValue = energyMaxValue;
                EditorGUILayout.LabelField($"{energyMinValue:F2} - {energyMaxValue:F2}", GUILayout.Width(100));
                EditorGUILayout.EndHorizontal();
                
                // Flux Range
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Flux Range", GUILayout.Width(100));
                SerializedProperty fluxMin = fluxRangeProperty.FindPropertyRelative("min");
                SerializedProperty fluxMax = fluxRangeProperty.FindPropertyRelative("max");
                float fluxMinValue = fluxMin.floatValue;
                float fluxMaxValue = fluxMax.floatValue;
                EditorGUILayout.MinMaxSlider(ref fluxMinValue, ref fluxMaxValue, 0f, 1f);
                fluxMin.floatValue = fluxMinValue;
                fluxMax.floatValue = fluxMaxValue;
                EditorGUILayout.LabelField($"{fluxMinValue:F2} - {fluxMaxValue:F2}", GUILayout.Width(100));
                EditorGUILayout.EndHorizontal();
                
                // Centroid Range
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Centroid Range", GUILayout.Width(100));
                SerializedProperty centroidMin = centroidRangeProperty.FindPropertyRelative("min");
                SerializedProperty centroidMax = centroidRangeProperty.FindPropertyRelative("max");
                float centroidMinValue = centroidMin.floatValue;
                float centroidMaxValue = centroidMax.floatValue;
                EditorGUILayout.MinMaxSlider(ref centroidMinValue, ref centroidMaxValue, 0f, 1f);
                centroidMin.floatValue = centroidMinValue;
                centroidMax.floatValue = centroidMaxValue;
                EditorGUILayout.LabelField($"{centroidMinValue:F2} - {centroidMaxValue:F2}", GUILayout.Width(100));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(5);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
} 