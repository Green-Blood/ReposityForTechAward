using System.Linq;
using Game.Core.StaticData;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Editor.EditorWindows
{
    public class GameDataEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("Tools/Game Editors/Unit Static Data Editor")]
        private static void OpenWindow()
        {
            var window = GetWindow<GameDataEditorWindow>();
            window.titleContent = new GUIContent("Game Data Editor");
            window.Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(supportsMultiSelect: true)
            {
                Config =
                {
                    DrawSearchToolbar = true,
                    DefaultMenuStyle =
                    {
                        Height = 25, IconSize = 24, BorderPadding = 5
                    },
                }
            };

            // Hero Data
            tree.Add("Heroes/Create New Hero", ScriptableObject.CreateInstance<HeroDataEditor>());
            tree.AddAllAssetsAtPath("Heroes/Existing Heroes",
                                    "Assets/Resources/ScriptableObjects/HeroData",
                                    typeof(HeroStaticData),
                                    true);

            // Enemy Data
            tree.Add("Enemies/Create New Enemy", ScriptableObject.CreateInstance<EnemyDataEditor>());
            tree.AddAllAssetsAtPath("Enemies/Existing Enemies",
                                    "Assets/Resources/ScriptableObjects/EnemyData",
                                    typeof(EnemyStaticData),
                                    true);

            // Castle Data
            tree.Add("Castles/Create New Castle", ScriptableObject.CreateInstance<CastleDataEditor>());
            tree.AddAllAssetsAtPath("Castles/Existing Castles",
                                    "Assets/Resources/ScriptableObjects/CastleData",
                                    typeof(CastleStaticData),
                                    true);

            // Ability Data
            tree.Add("Abilities/Create New Ability", ScriptableObject.CreateInstance<AbilityDataEditor>());
            tree.AddAllAssetsAtPath("Abilities/Existing Abilities",
                                    "Assets/Resources/ScriptableObjects/AbilityData",
                                    typeof(AbilityStaticData),
                                    true);

            // Projectile Data (as requested)
            tree.Add("Projectiles/Create New Projectile", ScriptableObject.CreateInstance<ProjectileDataEditor>());  // Use new instance instead of GetWindow
            tree.AddAllAssetsAtPath("Projectiles/Existing Projectiles",
                                    "Assets/Resources/ScriptableObjects/ProjectileData",
                                    typeof(ProjectileStaticData),
                                    true);

            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            var selectedItem = this.MenuTree?.Selection?.FirstOrDefault();

            if(selectedItem != null)
            {
                this.MenuWidth = 300;
                GUILayout.BeginVertical();
                // Display item specific title
                DisplaySelectedItemDetails(selectedItem);
                GUILayout.EndVertical();
            }
        }

        private void DisplaySelectedItemDetails(OdinMenuItem selectedItem)
        {
            if(selectedItem.Value is HeroStaticData)
            {
                GUILayout.Label("Hero Data", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox("You are editing a Hero data asset.", MessageType.Info);
            }
            else if(selectedItem.Value is EnemyStaticData)
            {
                GUILayout.Label("Enemy Data", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox("You are editing an Enemy data asset.", MessageType.Info);
            }
            else if(selectedItem.Value is CastleStaticData)
            {
                GUILayout.Label("Castle Data", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox("You are editing a Castle data asset.", MessageType.Info);
            }
            else if(selectedItem.Value is AbilityStaticData)
            {
                GUILayout.Label("Ability Data", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox("You are editing an Ability data asset.", MessageType.Info);
            }
            else if(selectedItem.Value is ProjectileStaticData)
            {
                GUILayout.Label("Projectile Data", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox("You are editing a Projectile data asset.", MessageType.Info);
            }
        }
    }
}