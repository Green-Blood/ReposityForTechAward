using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Systems.VFXSystem;
using UnityEditor;
using UnityEngine;

namespace Game.Editor.EditorWindows
{
    public class VFXEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("Tools/Game Editors/VFX Config Editor")]
        private static void OpenWindow()
        {
            var window = GetWindow<VFXEditorWindow>();
            window.titleContent = new GUIContent("VFX Config Editor");
            window.Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(supportsMultiSelect: true)
            {
                Config =
                {
                    DrawSearchToolbar = true, // Enable search bar for the tree
                    DefaultMenuStyle = { Height = 25, IconSize = 20 },
                }
            };
            AddCreationMenus(tree);
            AddExistingAssets(tree);

            return tree;
        }

        private void AddCreationMenus(OdinMenuTree tree)
        {
            tree.Add("Create New/VFX Config", CreateInstance<VFXConfigCreator>());
            tree.Add("Create New/VFX Component", CreateInstance<VFXComponentCreator>());
        }

        private void AddExistingAssets(OdinMenuTree tree)
        {
            tree.AddAllAssetsAtPath("VFX Configurations",
                "Assets/Resources/ScriptableObjects/VFXData/VFXConfigs", typeof(VFXConfig), true);
            tree.AddAllAssetsAtPath("VFX Components",
                "Assets/Resources/ScriptableObjects/VFXData/VFXEffects", typeof(VFXComponent), true);
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
                DisplayMultiSelectFeedback();
                if (selected.Value is VFXConfig)
                {
                    GUILayout.Label("🎨 VFX Config Selected",
                        new GUIStyle
                            { fontSize = 14, fontStyle = FontStyle.Bold, normal = { textColor = Color.white } });
                }
                else if (selected.Value is VFXComponent)
                {
                    GUILayout.Label("🔥 VFX Component Selected",
                        new GUIStyle
                            { fontSize = 14, fontStyle = FontStyle.Bold, normal = { textColor = Color.white } });
                }
            }
        }

        private void DisplaySelectedItemLabel(OdinMenuItem selected)
        {
            if (selected.Value is VFXConfig)
            {
                GUILayout.Label("VFX Config Selected", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox("You are editing a VFX configuration.", MessageType.Info);
            }
            else if (selected.Value is VFXComponent)
            {
                GUILayout.Label("VFX Component Selected", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox("You are editing a VFX component.", MessageType.Info);
            }
            else
            {
                GUILayout.Label("Unknown Item Selected", EditorStyles.boldLabel);
            }
        }

        private void DisplayMultiSelectFeedback()
        {
            if (MenuTree.Selection.Count > 1)
            {
                GUILayout.Space(10);
                GUILayout.Label($"{MenuTree.Selection.Count} items selected", EditorStyles.boldLabel);
            }
        }
    }
}