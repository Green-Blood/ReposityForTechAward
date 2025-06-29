using System.Linq;
using Sirenix.OdinInspector.Editor;
using UI.Configs;
using UnityEditor;
using UnityEngine;

namespace Game.Editor.EditorWindows
{
    public class UIAnimationEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("Tools/Game Editors/UI Animation Editor")]
        private static void OpenWindow()
        {
            var window = GetWindow<UIAnimationEditorWindow>();
            window.titleContent = new GUIContent("UI Animation Editor");
            window.Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(supportsMultiSelect: true)
            {
                Config =
                {
                    DrawSearchToolbar = true, 
                    DefaultMenuStyle = { Height = 25, IconSize = 20 },
                }
            };
            
            AddCreationMenus(tree);
            AddExistingAssets(tree);
            
            return tree;
        }

        private void AddCreationMenus(OdinMenuTree tree)
        {
            tree.Add("Create New/UI Animation Data", CreateInstance<UIAnimationData>());
            tree.Add("Create New/Fade Animation", CreateInstance<FadeAnimationComponent>());
            tree.Add("Create New/Scale Animation", CreateInstance<ScaleAnimationComponent>());
        }

        private void AddExistingAssets(OdinMenuTree tree)
        {
            tree.AddAllAssetsAtPath("UI Animations",
                                    "Assets/Addressables/UI/UIData/UIAnimationConfigs", typeof(UIAnimationData), true);
            tree.AddAllAssetsAtPath("UI Animation Components",
                                    "Assets/Addressables/UI/UIData/UIAnimationComponents", typeof(UIAnimationComponent), true);
        }

        protected override void OnBeginDrawEditors()
        {
            var originalColor = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0.15f, 0.25f, 0.35f);
            base.OnBeginDrawEditors();
            GUI.backgroundColor = originalColor;

            var selected = this.MenuTree.Selection.FirstOrDefault();
            if (selected != null)
            {
                MenuWidth = 300;
                DisplaySelectedItemLabel(selected);
            }
        }

        private void DisplaySelectedItemLabel(OdinMenuItem selected)
        {
            if (selected.Value is UIAnimationData)
            {
                GUILayout.Label("🎭 UI Animation Data Selected", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox("Editing UI Animation Data.", MessageType.Info);
            }
            else if (selected.Value is UIAnimationComponent)
            {
                GUILayout.Label("🎬 UI Animation Component Selected", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox("Editing UI Animation Component.", MessageType.Info);
            }
            else
            {
                GUILayout.Label("Unknown Item Selected", EditorStyles.boldLabel);
            }
        }
    }
}