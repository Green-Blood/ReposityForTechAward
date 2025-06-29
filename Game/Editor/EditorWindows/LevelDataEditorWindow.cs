using System.Collections.Generic;
using Game.Core.StaticData;
using Game.GamePlay.WaveSystem;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Editor.EditorWindows
{
    public class LevelDataEditorWindow : OdinEditorWindow
    {
        [MenuItem("Tools/Game Editors/Level Data Editor")]
        private static void OpenWindow()
        {
            var window = GetWindow<LevelDataEditorWindow>();
            window.titleContent = new GUIContent("Level Data Editor");
            window.Show();
        }

        [Title("Level Settings", bold: true)]
        [BoxGroup("New Level", centerLabel: true)]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        public LevelStaticData newLevel;

        [Title("Level Templates", bold: true)]
        [BoxGroup("Templates", centerLabel: true)]
        [Tooltip("Available level templates")]
        [SerializeField, InlineEditor]
        private List<LevelStaticData> levelTemplates;

        [BoxGroup("Creation Actions", centerLabel: true)]
        [Title("Create New Level", bold: true)]
        [ValueDropdown(nameof(levelTemplates))]
        public LevelStaticData selectedTemplate;

        [Button(ButtonSizes.Large), GUIColor(0.3f, 1.0f, 0.3f)] // Green button
        private void CreateNewLevel()
        {
            if (selectedTemplate != null)
            {
                newLevel = Instantiate(selectedTemplate);
                newLevel.name = $"New Level {selectedTemplate.LevelNumber}";
            }
            else
            {
                newLevel = ScriptableObject.CreateInstance<LevelStaticData>();
                newLevel.LevelNumber = GetNextLevelNumber();
                newLevel.WaveData = new List<WaveData>();
            }
            Debug.Log("New Level created.");
        }

        [BoxGroup("Save Actions", centerLabel: true)]
        [Button(ButtonSizes.Large), GUIColor(0.3f, 0.6f, 1.0f)] // Blue button for saving
        private void SaveLevel()
        {
            if (newLevel == null)
            {
                Debug.LogError("Level data is null!");
                return;
            }

            Undo.RecordObject(newLevel, "Saved Level Data");
            string path = GetUniqueLevelPath();
            AssetDatabase.CreateAsset(newLevel, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Level {newLevel.LevelNumber} saved successfully!");
        }

        [BoxGroup("Load Actions", centerLabel: true)]
        [Button(ButtonSizes.Large), GUIColor(1.0f, 0.6f, 0.3f)] // Orange for load button
        private void LoadLevel()
        {
            string[] guids = AssetDatabase.FindAssets("t:LevelStaticData", new[] { "Assets/Resources/ScriptableObjects/Levels" });
            if (guids.Length == 0)
            {
                Debug.LogError("No levels found!");
                return;
            }

            string selectedGuid = guids[0]; // Modify this to show a selection window if needed.
            string path = AssetDatabase.GUIDToAssetPath(selectedGuid);
            newLevel = AssetDatabase.LoadAssetAtPath<LevelStaticData>(path);
            Debug.Log($"Loaded Level: {newLevel.LevelNumber}");
        }

        [BoxGroup("Delete Actions", centerLabel: true)]
        [Button(ButtonSizes.Large), GUIColor(1.0f, 0.3f, 0.3f)] // Red for delete button
        private void DeleteLevel()
        {
            if (newLevel == null)
            {
                Debug.LogError("No level to delete.");
                return;
            }

            string path = AssetDatabase.GetAssetPath(newLevel);
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.Refresh();
                newLevel = null;
                Debug.Log("Level deleted successfully.");
            }
            else
            {
                Debug.LogError("Unable to delete level.");
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            LoadLevelTemplates();
        }

        private void LoadLevelTemplates()
        {
            levelTemplates = new List<LevelStaticData>();
            string[] guids = AssetDatabase.FindAssets("t:LevelStaticData", new[] { "Assets/Resources/ScriptableObjects/Levels" });
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                LevelStaticData levelData = AssetDatabase.LoadAssetAtPath<LevelStaticData>(path);
                if (levelData != null)
                {
                    levelTemplates.Add(levelData);
                }
            }
        }

        private int GetNextLevelNumber()
        {
            int maxLevel = 0;
            foreach (var level in levelTemplates)
            {
                if (level.LevelNumber > maxLevel)
                    maxLevel = level.LevelNumber;
            }
            return maxLevel + 1;
        }

        private string GetUniqueLevelPath()
        {
            string basePath = "Assets/Resources/ScriptableObjects/Levels";
            string path;
            int counter = 1;
            do
            {
                path = $"{basePath}/Level_{newLevel.LevelNumber}_{counter}Data.asset";
                counter++;
            } while (AssetDatabase.LoadAssetAtPath<LevelStaticData>(path) != null);
            return path;
        }
    }
}
