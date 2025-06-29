using System.Collections.Generic;
using Game.Core.StaticData;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Editor.EditorWindows
{
    public class ProjectileDataEditor : OdinEditorWindow
    {
        [Title("Projectile Settings", bold: true)]
        [BoxGroup("New Projectile", centerLabel: true)]
        [Tooltip("Here you can create and configure a new projectile.")]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        public ProjectileStaticData newProjectile;

        [Title("Projectile Templates", bold: true)]
        [BoxGroup("Templates", centerLabel: true)]
        [Tooltip("This section contains available projectile templates.")]
        [SerializeField, InlineEditor]
        private List<ProjectileStaticData> projectileTemplates;

        [BoxGroup("Creation Actions", centerLabel: true)]
        [Title("Create a New Projectile", bold: true)]
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        [Button(ButtonSizes.Large), GUIColor(0.3f, 1.0f, 0.3f)] // Green for create button
        [Tooltip("Click here to create a new projectile.")]
        private void CreateNewProjectile()
        {
            newProjectile = ScriptableObject.CreateInstance<ProjectileStaticData>();
            newProjectile.name = "New Projectile";
            Debug.Log("New projectile created.");
        }

        [BoxGroup("Save Actions", centerLabel: true)]
        [Title("Save and Load Projectile", bold: true)]
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        [Button(ButtonSizes.Large), GUIColor(0.3f, 0.6f, 1.0f)] // Blue for save button
        [Tooltip("Save the current projectile.")]
        private void SaveProjectile()
        {
            if (newProjectile == null)
            {
                Debug.LogError("Projectile data is null!");
                return;
            }

            AssetDatabase.CreateAsset(newProjectile,
                                      $"Assets/Resources/ScriptableObjects/ProjectileData/{newProjectile.Name}StaticData.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Projectile {newProjectile.Name} saved successfully!");
        }

        [Button(ButtonSizes.Large), GUIColor(1.0f, 0.6f, 0.3f)] // Orange for load button
        [Tooltip("Load an existing projectile from assets.")]
        private void LoadProjectile()
        {
            string path = EditorUtility.OpenFilePanel("Select Projectile Data",
                                                      "Assets/Resources/ScriptableObjects/ProjectileData", "asset");
            if (string.IsNullOrEmpty(path)) return;
            path = FileUtil.GetProjectRelativePath(path);
            newProjectile = AssetDatabase.LoadAssetAtPath<ProjectileStaticData>(path);
            Debug.Log($"Projectile {newProjectile.Name} loaded.");
        }

        [BoxGroup("Delete Actions", centerLabel: true)]
        [Button(ButtonSizes.Large), GUIColor(1.0f, 0.3f, 0.3f)] // Red for delete button
        [Tooltip("Delete the current projectile.")]
        private void DeleteProjectile()
        {
            if (newProjectile == null)
            {
                Debug.LogError("No projectile to delete.");
                return;
            }

            string path = AssetDatabase.GetAssetPath(newProjectile);
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.Refresh();
                newProjectile = null;
                Debug.Log("Projectile deleted successfully.");
            }
            else
            {
                Debug.LogError("Unable to delete projectile.");
            }
        }

        [BoxGroup("Template Actions", centerLabel: true)]
        [Button(ButtonSizes.Medium), GUIColor(1.0f, 1.0f, 0.2f)] // Yellow for apply template button
        [Tooltip("Apply a template to the current projectile.")]
        private void ApplyTemplate(ProjectileStaticData template)
        {
            if (template != null)
            {
                newProjectile = Instantiate(template);
                Debug.Log($"Template {template.name} applied to new projectile.");
            }
            else
            {
                Debug.LogError("Template is null!");
            }
        }
    }
}