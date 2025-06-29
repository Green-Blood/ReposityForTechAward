using System.Collections.Generic;
using Game.Core.StaticData;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Editor.EditorWindows
{
    public class HeroDataEditor : OdinEditorWindow
    {
        [Title("Hero Settings", bold: true)]
        [BoxGroup("Hero Data", centerLabel: true)]
        [Tooltip("Here you can create a new hero.")]
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        public HeroStaticData newHero;

        [Title("Hero Templates", bold: true)]
        [BoxGroup("Hero Data/Templates", centerLabel: true)]
        [Tooltip("This section contains available hero templates.")]
        [SerializeField, InlineEditor]
        private List<HeroStaticData> heroTemplates;

        [BoxGroup("Hero Data/Creation Actions", centerLabel: true)]
        [Title("Create a New Hero", bold: true)]
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        [Button(ButtonSizes.Large), GUIColor(0.3f, 0.8f, 0.4f)] // Green color for the create button
        [Tooltip("Click here to create a new hero.")]
        private void CreateNewHero()
        {
            newHero = CreateInstance<HeroStaticData>();
            newHero.name = "New Hero";
        }


        [BoxGroup("Hero Data/Save Actions", centerLabel: true)]
        [Title("Save and Load Hero", bold: true)]
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        [Button(ButtonSizes.Large), GUIColor(0.2f, 0.7f, 1.0f)] // Blue color for save button
        [Tooltip("Save the current hero.")]
        private void SaveHero()
        {
            if (newHero == null)
            {
                Debug.LogError("Hero data is null!");
                return;
            }

            if (string.IsNullOrEmpty(newHero.Name))
            {
                Debug.LogError("Hero name is empty!");
                return;
            }

            // Logic to save the hero data
            AssetDatabase.CreateAsset(newHero,
                                      $"Assets/Resources/ScriptableObjects/HeroData/{newHero.Name}StaticData.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Hero {newHero.Name} saved successfully!");
        }

        [BoxGroup("Hero Data/Save Actions", centerLabel: true)]
        [Button(ButtonSizes.Large), GUIColor(1.0f, 0.6f, 0.2f)] // Orange color for load button
        [Tooltip("Load an existing hero from assets.")]
        private void LoadHero()
        {
            string path = EditorUtility.OpenFilePanel("Select Hero Data",
                                                      "Assets/Resources/ScriptableObjects/HeroData",
                                                      "asset");
            if (string.IsNullOrEmpty(path)) return;
            path = FileUtil.GetProjectRelativePath(path);
            newHero = AssetDatabase.LoadAssetAtPath<HeroStaticData>(path);
        }

        [BoxGroup("Hero Data/Templates", centerLabel: true)]
        [Button(ButtonSizes.Medium), GUIColor(0.8f, 0.9f, 0.2f)] // Yellow color for apply template button
        [Tooltip("Apply a template to the current hero.")]
        private void ApplyTemplate(HeroStaticData template)
        {
            if (template != null)
            {
                newHero = Instantiate(template);
            }
        }
    }
}