using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Systems.VFXSystem;
using UnityEditor;
using UnityEngine;

namespace Game.Editor.EditorWindows
{
    public class VFXConfigCreator : OdinEditorWindow
    {
        #region VFXConfigs

        [Title("VFX Creation", bold: false)]
        [BoxGroup("New VFX Configuration", centerLabel: true)]
        [GUIColor(0.9f, 0.85f, 1.0f)]
        public string newVFXConfigName = "New VFX Config";

        [BoxGroup("New VFX Configuration", centerLabel: true)]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        [GUIColor(0.9f, 0.85f, 1.0f)]
        public VFXConfig newVFXConfig;

        [BoxGroup("New VFX Configuration", centerLabel: true)]
        [Button("Create New VFX Config", ButtonSizes.Medium), GUIColor(0.6f, 0.7f, 1.0f)]
        private void CreateNewVFXConfig()
        {
            // Validate the name input before creating
            if (string.IsNullOrEmpty(newVFXConfigName))
            {
                Debug.LogError("Please provide a valid name for the new VFX Config.");
                return;
            }

            newVFXConfig = CreateInstance<VFXConfig>();
            newVFXConfig.name = newVFXConfigName;
            Debug.Log($"Created new VFX Config: {newVFXConfigName}");
        }


        [BoxGroup("New VFX Configuration", centerLabel: true)]
        [Button("Save VFX Config", ButtonSizes.Medium), GUIColor(0.5f, 1.0f, 0.6f)]
        private void SaveVFXConfig()
        {
            if (newVFXConfig == null)
            {
                Debug.LogError("VFX config is null!");
                return;
            }

            if (string.IsNullOrEmpty(newVFXConfig.name))
            {
                Debug.LogError("VFX config name is empty!");
                return;
            }

            string vfxPath =
                $"Assets/Resources/ScriptableObjects/VFXData/VFXConfigs/{newVFXConfig.name}VFXConfig.asset";
            AssetDatabase.CreateAsset(newVFXConfig, vfxPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"VFX Config {newVFXConfig.name} saved successfully at {vfxPath}!");
        }


        [BoxGroup("New VFX Configuration", centerLabel: true)]
        [Button("Delete VFX Config", ButtonSizes.Medium)]
        [GUIColor(1.0f, 0.5f, 0.5f)] // Red for delete button
        private void DeleteVFXConfig()
        {
            if (newVFXConfig == null)
            {
                Debug.LogError("No VFX Config selected.");
                return;
            }

            string path = AssetDatabase.GetAssetPath(newVFXConfig);
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.Refresh();
                newVFXConfig = null;
                Debug.Log("VFX Config deleted successfully.");
            }
        }

        #endregion

        #region VFXComponents
        [BoxGroup("New VFX Component", centerLabel: true)]
        [Title("New VFX Component", bold: false)]
        [GUIColor(0.8f, 0.95f, 1.0f)]
        public string newVFXComponentName = "New Component";

        [BoxGroup("New VFX Component", centerLabel: true)] [ValueDropdown(nameof(GetAvailableVFXComponentTypes))]
        public Type SelectedVFXComponentType;

        private IEnumerable<ValueDropdownItem<Type>> GetAvailableVFXComponentTypes()
        {
            // Dynamically get all types derived from VFXComponent
            var componentTypes = typeof(VFXComponent).Assembly
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(VFXComponent)) && !t.IsAbstract);

            // Return as a dropdown list
            return componentTypes.Select(type => new ValueDropdownItem<Type>(type.Name, type));
        }


        [BoxGroup("New VFX Component", centerLabel: true)]
        [Button("Create VFX Component", ButtonSizes.Medium), GUIColor(0.5f, 0.8f, 1.0f)]
        private void CreateVFXComponent()
        {
            if (SelectedVFXComponentType == null)
            {
                Debug.LogError("No VFX Component type selected.");
                return;
            }

            // Create an instance of the selected component type
            newVFXComponent = (VFXComponent)CreateInstance(SelectedVFXComponentType);
            if (newVFXComponent == null)
            {
                Debug.LogError("Failed to create VFX Component.");
                return;
            }

            newVFXComponent.name = newVFXComponentName;
            Debug.Log($"{SelectedVFXComponentType.Name} created successfully.");
        }


        [BoxGroup("New VFX Component", centerLabel: true)]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        [ShowIf("@newVFXComponent != null")]
        public VFXComponent newVFXComponent;


        [BoxGroup("New VFX Component", centerLabel: true)]
        [Button("Save VFX Component", ButtonSizes.Medium), GUIColor(0.5f, 1.0f, 0.6f)]
        private void SaveVFXComponent()
        {
            if (newVFXComponent == null)
            {
                Debug.LogError("No VFX Component created to save.");
                return;
            }

            string vfxComponentPath =
                $"Assets/Resources/ScriptableObjects/VFXData/VFXEffects/{newVFXComponent.name}.asset";
            AssetDatabase.CreateAsset(newVFXComponent, vfxComponentPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"VFX Component {newVFXComponent.name} saved at {vfxComponentPath}.");
        }

        [BoxGroup("Delete VFX Component", centerLabel: true), GUIColor(1.0f, 0.5f, 0.5f)] 
        [ValueDropdown(nameof(GetExistingVFXComponents))]
        public VFXComponent selectedVFXComponentToDelete;

        private IEnumerable<ValueDropdownItem<VFXComponent>> GetExistingVFXComponents()
        {
            var componentGuids = AssetDatabase.FindAssets("t:VFXComponent",
                new[] { "Assets/Resources/ScriptableObjects/VFXData/VFXEffects" });

            // Convert GUIDs to VFXComponents
            foreach (var guid in componentGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                VFXComponent component = AssetDatabase.LoadAssetAtPath<VFXComponent>(path);
                if (component != null)
                {
                    yield return new ValueDropdownItem<VFXComponent>(component.name, component);
                }
            }
        }

        [BoxGroup("Delete VFX Component", centerLabel: true)]
        [Button("Delete VFX Component", ButtonSizes.Medium)]
        [GUIColor(1.0f, 0.5f, 0.5f)] // Red for delete button
        private void DeleteVFXComponent()
        {
            if (selectedVFXComponentToDelete == null)
            {
                Debug.LogError("No VFX Component selected to delete.");
                return;
            }

            string path = AssetDatabase.GetAssetPath(selectedVFXComponentToDelete);
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.Refresh();
                selectedVFXComponentToDelete = null;
                Debug.Log("VFX Component deleted successfully.");
            }
        }
         #endregion
    }
}