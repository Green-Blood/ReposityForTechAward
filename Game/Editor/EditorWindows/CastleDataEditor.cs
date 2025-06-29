using System.Collections.Generic;
using Game.Core.StaticData;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Editor.EditorWindows
{
    public class CastleDataEditor : OdinEditorWindow
    {
        [Title("Castle Settings", bold: true)]
        [BoxGroup("New Castle", centerLabel: true)]
        [Tooltip("Here you can create and configure a new castle.")]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        public CastleStaticData newCastle;

        [Title("Castle Templates", bold: true)]
        [BoxGroup("Templates", centerLabel: true)]
        [Tooltip("This section contains available castle templates.")]
        [SerializeField, InlineEditor]
        private List<CastleStaticData> castleTemplates;

        [BoxGroup("Creation Actions", centerLabel: true)]
        [Title("Create a New Castle", bold: true)]
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        [Button(ButtonSizes.Large), GUIColor(0.3f, 1.0f, 0.3f)] // Green for create button
        [Tooltip("Click here to create a new castle.")]
        private void CreateNewCastle()
        {
            newCastle = ScriptableObject.CreateInstance<CastleStaticData>();
            newCastle.name = "New Castle";
            Debug.Log("New castle created.");
        }

        [BoxGroup("Save Actions", centerLabel: true)]
        [Title("Save and Load Castle", bold: true)]
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        [Button(ButtonSizes.Large), GUIColor(0.3f, 0.6f, 1.0f)] // Blue for save button
        [Tooltip("Save the current castle.")]
        private void SaveCastle()
        {
            if (newCastle == null)
            {
                Debug.LogError("Castle data is null!");
                return;
            }

            AssetDatabase.CreateAsset(newCastle,
                                      $"Assets/Resources/ScriptableObjects/CastleData/{newCastle.Name}StaticData.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Castle {newCastle.Name} saved successfully!");
        }

        [Button(ButtonSizes.Large), GUIColor(1.0f, 0.6f, 0.3f)] // Orange for load button
        [Tooltip("Load an existing castle from assets.")]
        private void LoadCastle()
        {
            string path = EditorUtility.OpenFilePanel("Select Castle Data",
                                                      "Assets/Resources/ScriptableObjects/CastleData", "asset");
            if (string.IsNullOrEmpty(path)) return;
            path = FileUtil.GetProjectRelativePath(path);
            newCastle = AssetDatabase.LoadAssetAtPath<CastleStaticData>(path);
            Debug.Log($"Castle {newCastle.Name} loaded.");
        }

        [BoxGroup("Delete Actions", centerLabel: true)]
        [Button(ButtonSizes.Large), GUIColor(1.0f, 0.3f, 0.3f)] // Red for delete button
        [Tooltip("Delete the current castle.")]
        private void DeleteCastle()
        {
            if (newCastle == null)
            {
                Debug.LogError("No castle to delete.");
                return;
            }

            string path = AssetDatabase.GetAssetPath(newCastle);
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.Refresh();
                newCastle = null;
                Debug.Log("Castle deleted successfully.");
            }
            else
            {
                Debug.LogError("Unable to delete castle.");
            }
        }

        [BoxGroup("Template Actions", centerLabel: true)]
        [Button(ButtonSizes.Medium), GUIColor(1.0f, 1.0f, 0.2f)] // Yellow for apply template button
        [Tooltip("Apply a template to the current castle.")]
        private void ApplyTemplate(CastleStaticData template)
        {
            if (template != null)
            {
                newCastle = Instantiate(template);
                Debug.Log($"Template {template.name} applied to new castle.");
            }
            else
            {
                Debug.LogError("Template is null!");
            }
        }
    }
}