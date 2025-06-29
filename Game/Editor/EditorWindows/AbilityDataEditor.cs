using System.Collections.Generic;
using Game.Core.StaticData;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Editor.EditorWindows
{
    public class AbilityDataEditor : OdinEditorWindow
    {
        [Title("Ability Settings", bold: true)]
        [BoxGroup("New Ability", centerLabel: true)]
        [Tooltip("Here you can create and configure a new ability.")]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        public AbilityStaticData newAbility;

        [Title("Ability Templates", bold: true)]
        [BoxGroup("Templates", centerLabel: true)]
        [Tooltip("This section contains available ability templates.")]
        [SerializeField, InlineEditor]
        private List<AbilityStaticData> abilityTemplates;

        [BoxGroup("Creation Actions", centerLabel: true)]
        [Title("Create a New Ability", bold: true)]
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        [Button(ButtonSizes.Large), GUIColor(0.3f, 1.0f, 0.3f)] // Green for create button
        [Tooltip("Click here to create a new ability.")]
        private void CreateNewAbility()
        {
            newAbility = ScriptableObject.CreateInstance<AbilityStaticData>();
            newAbility.name = "New Ability";
            Debug.Log("New ability created.");
        }

        [BoxGroup("Save Actions", centerLabel: true)]
        [Title("Save and Load Ability", bold: true)]
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        [Button(ButtonSizes.Large), GUIColor(0.3f, 0.6f, 1.0f)] // Blue for save button
        [Tooltip("Save the current ability.")]
        private void SaveAbility()
        {
            if (newAbility == null)
            {
                Debug.LogError("Ability data is null!");
                return;
            }

            AssetDatabase.CreateAsset(newAbility,
                                      $"Assets/Resources/ScriptableObjects/AbilityData/{newAbility.Name}StaticData.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Ability {newAbility.Name} saved successfully!");
        }

        [Button(ButtonSizes.Large), GUIColor(1.0f, 0.6f, 0.3f)] // Orange for load button
        [Tooltip("Load an existing ability from assets.")]
        private void LoadAbility()
        {
            string path = EditorUtility.OpenFilePanel("Select Ability Data",
                                                      "Assets/Resources/ScriptableObjects/AbilityData", "asset");
            if (string.IsNullOrEmpty(path)) return;
            path = FileUtil.GetProjectRelativePath(path);
            newAbility = AssetDatabase.LoadAssetAtPath<AbilityStaticData>(path);
            Debug.Log($"Ability {newAbility.Name} loaded.");
        }

        [BoxGroup("Delete Actions", centerLabel: true)]
        [Button(ButtonSizes.Large), GUIColor(1.0f, 0.3f, 0.3f)] // Red for delete button
        [Tooltip("Delete the current ability.")]
        private void DeleteAbility()
        {
            if (newAbility == null)
            {
                Debug.LogError("No ability to delete.");
                return;
            }

            string path = AssetDatabase.GetAssetPath(newAbility);
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.Refresh();
                newAbility = null;
                Debug.Log("Ability deleted successfully.");
            }
            else
            {
                Debug.LogError("Unable to delete ability.");
            }
        }

        [BoxGroup("Template Actions", centerLabel: true)]
        [Button(ButtonSizes.Medium), GUIColor(1.0f, 1.0f, 0.2f)] // Yellow for apply template button
        [Tooltip("Apply a template to the current ability.")]
        private void ApplyTemplate(AbilityStaticData template)
        {
            if (template != null)
            {
                newAbility = Instantiate(template);
                Debug.Log($"Template {template.name} applied to new ability.");
            }
            else
            {
                Debug.LogError("Template is null!");
            }
        }
    }
}