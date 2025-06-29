using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Systems.VFXSystem;
using UnityEditor;
using UnityEngine;

namespace Game.Editor.EditorWindows
{
    public class VFXComponentCreator : OdinEditorWindow
    {
        [Title("VFX Configuration", bold: false)]
        [BoxGroup("Manage VFX Configurations", centerLabel: true)]
        [Title("Select VFX Configuration", bold: false)]
        [ValueDropdown(nameof(GetVFXConfigNames))]
        [OnValueChanged(nameof(OnVFXConfigSelected))]
        public string selectedVFXConfigName;

        private VFXConfig _selectedVFXConfig;

        private IEnumerable<string> GetVFXConfigNames()
        {
            var vfxConfigNames = new List<string>();
            var configGuids = AssetDatabase.FindAssets("t:VFXConfig", new[] { "Assets/Resources/ScriptableObjects/VFXData/VFXConfigs" });
            
            foreach (var guid in configGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var config = AssetDatabase.LoadAssetAtPath<VFXConfig>(path);
                if (config != null)
                {
                    vfxConfigNames.Add(config.name);
                }
            }

            return vfxConfigNames;
        }

        private void OnVFXConfigSelected()
        {
            if (!string.IsNullOrEmpty(selectedVFXConfigName))
            {
                string[] configGuids = AssetDatabase.FindAssets("t:VFXConfig", new[] { "Assets/Resources/ScriptableObjects/VFXData/VFXConfigs" });
                foreach (string guid in configGuids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    VFXConfig config = AssetDatabase.LoadAssetAtPath<VFXConfig>(path);
                    if (config != null && config.name == selectedVFXConfigName)
                    {
                        _selectedVFXConfig = config;
                        break;
                    }
                }
            }
        }

        [ShowIf(nameof(_selectedVFXConfig))]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        public VFXConfig SelectedVFXConfig => _selectedVFXConfig;

        [ShowIf(nameof(_selectedVFXConfig))]
        [BoxGroup("Manage VFX Configurations/Selected VFX Configuration", centerLabel: true)]
        [Button("Save Selected VFX Config", ButtonSizes.Medium), GUIColor(0.5f, 1.0f, 0.6f)]
        private void SaveSelectedVFXConfig()
        {
            if (_selectedVFXConfig == null)
            {
                Debug.LogError("No VFX Config selected.");
                return;
            }

            EditorUtility.SetDirty(_selectedVFXConfig);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"VFX Config {_selectedVFXConfig.name} saved successfully!");
        }

        [ShowIf(nameof(_selectedVFXConfig))]
        [BoxGroup("Manage VFX Configurations/Selected VFX Configuration", centerLabel: true)]
        [Button("Delete Selected VFX Config", ButtonSizes.Medium), GUIColor(1.0f, 0.5f, 0.5f)] // Red for delete
        private void DeleteSelectedVFXConfig()
        {
            if (_selectedVFXConfig == null)
            {
                Debug.LogError("No VFX Config selected.");
                return;
            }

            string path = AssetDatabase.GetAssetPath(_selectedVFXConfig);
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.Refresh();
                _selectedVFXConfig = null;
                Debug.Log("VFX Config deleted successfully.");
            }
        }

        [Title("VFX Configuration", bold: false)]
        [BoxGroup("Manage VFX Configurations", centerLabel: true)]
        [Title("Existing VFX Configurations", bold: false)]
        [ListDrawerSettings(ShowFoldout = true)]
        public List<VFXConfig> allVFXConfigs;

        [BoxGroup("Manage VFX Configurations", centerLabel: true)]
        [Button("Refresh VFX Configs", ButtonSizes.Medium), GUIColor(0.6f, 1.0f, 0.6f)] // Green for refresh button
        private void RefreshVFXConfigs()
        {
            string[] configGuids = AssetDatabase.FindAssets("t:VFXConfig",
                new[] { "Assets/Resources/ScriptableObjects/VFXData/VFXConfigs" });
            allVFXConfigs = new List<VFXConfig>();
            foreach (string guid in configGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                VFXConfig config = AssetDatabase.LoadAssetAtPath<VFXConfig>(path);
                if (config != null)
                {
                    allVFXConfigs.Add(config);
                }
            }

            Debug.Log("VFX Configurations refreshed.");
        }

        [BoxGroup("Manage VFX Configurations", centerLabel: true)]
        [PropertySpace(10)]
        [Button("DeleteFirstConfigInList", ButtonSizes.Medium), GUIColor(1.0f, 0.5f, 0.5f)] // Red for delete
        private void DeleteFirstConfigInList()
        {
            if (allVFXConfigs.Count == 0)
            {
                Debug.LogError("No VFX Configs available to delete.");
                return;
            }

            // Select a config to delete
            VFXConfig selectedConfig = allVFXConfigs[0]; // Replace with actual selection logic if needed

            string path = AssetDatabase.GetAssetPath(selectedConfig);
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.Refresh();
                allVFXConfigs.Remove(selectedConfig);
                Debug.Log("VFX Config deleted successfully.");
            }
        }

        [Title("VFX Components", bold: false)]
        [BoxGroup("Manage VFX Components", centerLabel: true)]
        [ValueDropdown(nameof(GetVFXComponentNames))]
        [OnValueChanged(nameof(OnVFXComponentSelected))]
        public string selectedVFXComponentName;
        private VFXComponent selectedVFXComponent;
        
        private IEnumerable<string> GetVFXComponentNames()
        {
            List<string> vfxComponentNames = new List<string>();
            string[] componentGuids = AssetDatabase.FindAssets("t:VFXComponent", new[] { "Assets/Resources/ScriptableObjects/VFXData/VFXEffects" });

            foreach (string guid in componentGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                VFXComponent component = AssetDatabase.LoadAssetAtPath<VFXComponent>(path);
                if (component != null)
                {
                    vfxComponentNames.Add(component.name);
                }
            }

            return vfxComponentNames;
        }

        private void OnVFXComponentSelected()
        {
            if (!string.IsNullOrEmpty(selectedVFXComponentName))
            {
                string[] componentGuids = AssetDatabase.FindAssets("t:VFXComponent", new[] { "Assets/Resources/ScriptableObjects/VFXData/VFXEffects" });
                foreach (string guid in componentGuids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    VFXComponent component = AssetDatabase.LoadAssetAtPath<VFXComponent>(path);
                    if (component != null && component.name == selectedVFXComponentName)
                    {
                        selectedVFXComponent = component;
                        break;
                    }
                }
            }
        }
        [ShowIf(nameof(selectedVFXComponent))]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        public VFXComponent SelectedVFXComponent => selectedVFXComponent;
        
        [ShowIf(nameof(selectedVFXComponent))]
        [BoxGroup("Manage VFX Components/Selected VFX Component", centerLabel: true)]
        [Button("Save Selected VFX Component", ButtonSizes.Medium), GUIColor(0.5f, 1.0f, 0.6f)]
        private void SaveSelectedVFXComponent()
        {
            if (selectedVFXComponent == null)
            {
                Debug.LogError("No VFX Component selected.");
                return;
            }

            EditorUtility.SetDirty(selectedVFXComponent);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"VFX Component {selectedVFXComponent.name} saved successfully!");
        }

        [ShowIf(nameof(selectedVFXComponent))]
        [BoxGroup("Manage VFX Components/Selected VFX Component", centerLabel: true)]
        [Button("Delete Selected VFX Component", ButtonSizes.Medium), GUIColor(1.0f, 0.5f, 0.5f)] // Red for delete
        private void DeleteSelectedVFXComponent()
        {
            if (selectedVFXComponent == null)
            {
                Debug.LogError("No VFX Component selected.");
                return;
            }

            string path = AssetDatabase.GetAssetPath(selectedVFXComponent);
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.Refresh();
                selectedVFXComponent = null;
                Debug.Log("VFX Component deleted successfully.");
            }
        }
        
        
        [BoxGroup("Manage VFX Components", centerLabel: true)]
        [Title("Existing VFX Components", bold: false)]
        [ListDrawerSettings(ShowFoldout = true)]
        public List<VFXComponent> allVFXComponents;

        [BoxGroup("Manage VFX Components", centerLabel: true)]
        [Button("Refresh VFX Components", ButtonSizes.Medium), GUIColor(0.6f, 1.0f, 0.6f)] // Green for refresh button
        private void RefreshVFXComponents()
        {
            string[] componentGuids = AssetDatabase.FindAssets("t:VFXComponent",
                new[] { "Assets/Resources/ScriptableObjects/VFXData/VFXEffects" });
            allVFXComponents = new List<VFXComponent>();
            foreach (string guid in componentGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                VFXComponent component = AssetDatabase.LoadAssetAtPath<VFXComponent>(path);
                if (component != null)
                {
                    allVFXComponents.Add(component);
                }
            }

            Debug.Log("VFX Components refreshed.");
        }

         
    }
}