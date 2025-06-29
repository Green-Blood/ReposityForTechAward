using System.Collections.Generic;
using Game.Core.StaticData;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Editor.EditorWindows
{
    public class EnemyDataEditor : OdinEditorWindow
    {
        [Title("Enemy Settings", bold: true)]
        [BoxGroup("New Enemy", centerLabel: true)]
        [Tooltip("Here you can create and configure a new enemy.")]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        public EnemyStaticData newEnemy;

        [Title("Enemy Templates", bold: true)]
        [BoxGroup("Templates", centerLabel: true)]
        [Tooltip("This section contains available enemy templates.")]
        [SerializeField, InlineEditor]
        private List<EnemyStaticData> enemyTemplates;

        [BoxGroup("Creation Actions", centerLabel: true)]
        [Title("Create a New Enemy", bold: true)]
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        [Button(ButtonSizes.Large), GUIColor(0.3f, 1.0f, 0.3f)]
        [Tooltip("Click here to create a new enemy.")]
        private void CreateNewEnemy()
        {
            newEnemy = CreateInstance<EnemyStaticData>();
            newEnemy.name = "New Enemy";
            Debug.Log("New enemy created.");
        }

        [BoxGroup("Save Actions", centerLabel: true)]
        [Title("Save and Load Enemy", bold: true)]
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        [Button(ButtonSizes.Large), GUIColor(0.2f, 0.7f, 1.0f)] // Blue color for save button
        [Tooltip("Save the current enemy.")]
        private void SaveEnemy()
        {
            if (newEnemy == null)
            {
                Debug.LogError("Enemy data is null!");
                return;
            }

            if (string.IsNullOrEmpty(newEnemy.Name))
            {
                Debug.LogError("Enemy name is empty!");
                return;
            }

            AssetDatabase.CreateAsset(newEnemy,
                                      $"Assets/Resources/ScriptableObjects/EnemyData/{newEnemy.Name}StaticData.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Enemy {newEnemy.Name} saved successfully!");
        }

        [BoxGroup("Save Actions", centerLabel: true)]
        [Button(ButtonSizes.Large), GUIColor(1.0f, 0.6f, 0.2f)] // Orange color for load button
        [Tooltip("Load an existing enemy from assets.")]
        private void LoadEnemy()
        {
            string path = EditorUtility.OpenFilePanel("Select Enemy Data",
                                                      "Assets/Resources/ScriptableObjects/EnemyData", "asset");
            if (!string.IsNullOrEmpty(path))
            {
                path = FileUtil.GetProjectRelativePath(path);
                newEnemy = AssetDatabase.LoadAssetAtPath<EnemyStaticData>(path);
                Debug.Log($"Enemy {newEnemy.Name} loaded.");
            }
        }

        [BoxGroup("Templates", centerLabel: true)]
        [Button(ButtonSizes.Medium), GUIColor(0.8f, 0.9f, 0.2f)] // Yellow color for apply template button
        [Tooltip("Apply a template to the current enemy.")]
        private void ApplyTemplate(EnemyStaticData template)
        {
            if (template != null)
            {
                newEnemy = Instantiate(template);
                Debug.Log($"Template {template.name} applied to new enemy.");
            }
            else
            {
                Debug.LogError("Template is null!");
            }
        }

        [BoxGroup("Delete Actions", centerLabel: true)]
        [Button(ButtonSizes.Large), GUIColor(1.0f, 0.3f, 0.3f)] // Red for delete button
        [Tooltip("Delete the current enemy.")]
        private void DeleteEnemy()
        {
            if (newEnemy == null)
            {
                Debug.LogError("No enemy to delete.");
                return;
            }

            string path = AssetDatabase.GetAssetPath(newEnemy);
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.Refresh();
                newEnemy = null;
                Debug.Log("Enemy deleted successfully.");
            }
            else
            {
                Debug.LogError("Unable to delete enemy.");
            }
        }
    }
}