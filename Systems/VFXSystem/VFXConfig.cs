using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Extensions.ExtensionMethods;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Systems.VFXSystem
{
    [CreateAssetMenu(fileName = "VFXConfig", menuName = "Static Data/VFX/VFXConfig")]
    public class VFXConfig : SerializedScriptableObject
    {
        [BoxGroup("Settings")]
        [GUIColor(0.9f, 0.95f, 1.0f)]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        [HideLabel]
        [HideIf(nameof(ShouldHideTrailVFX))]
        [field: SerializeField]
        [CanBeNull]
        private TrailVFXComponent trailVFX;

        public TrailVFXComponent TrailVFX
        {
            get => trailVFX;
            set => trailVFX = value;
        }

        [BoxGroup("Settings")]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        [GUIColor(0.9f, 0.95f, 1.0f)]
        [HideLabel]
        [HideIf(nameof(ShouldHideImpactVFX))]
        [field: SerializeField]
        [CanBeNull]
        private ImpactVFXComponent impactVFX;

        public ImpactVFXComponent ImpactVFX
        {
            get => impactVFX;
            set => impactVFX = value;
        }

        [BoxGroup("Settings")]
        [GUIColor(0.9f, 0.95f, 1.0f)]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        [HideLabel]
        [HideIf(nameof(ShouldHideExplosionVFX))]
        [field: SerializeField]
        [CanBeNull]
        private ExplosionVFXComponent explosionVFX;

        public ExplosionVFXComponent ExplosionVFX
        {
            get => explosionVFX;
            set => explosionVFX = value;
        }

        [BoxGroup("Settings")]
        [GUIColor(0.9f, 0.95f, 1.0f)]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        [HideLabel]
        [CanBeNull]
        [HideIf(nameof(ShouldHideMissileVFX))]
        [field: SerializeField]
        private MissileVFXComponent missileVFX;

        public MissileVFXComponent MissileVFX
        {
            get => missileVFX;
            set => missileVFX = value;
        }


        [BoxGroup("Settings")]
        [GUIColor(0.9f, 0.95f, 1.0f)]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        [HideLabel]
        [HideIf(nameof(ShouldHideCastVFX))]
        [field: SerializeField]
        [CanBeNull]
        private CastVFXComponent castVFX;

        public CastVFXComponent CastVFX
        {
            get => castVFX;
            set => castVFX = value;
        }

        [FoldoutGroup("General VFX Controls")]
        [LabelText("Show All VFX Components")]
        [GUIColor(0.5f, 0.9f, 1.0f)]
        [Tooltip("Toggle this to show or hide VFX components that are not assigned.")]
        [SerializeField]
        private bool showAllVFX = true;

        #region Preview

        private Queue<GameObject> _instantiatedObjects = new();
#if UNITY_EDITOR
        [FoldoutGroup("General VFX Controls")]
        [Button("Preview All VFX", ButtonSizes.Large)]
        [GUIColor(0.6f, 1.0f, 0.6f)]
        public void PreviewAllVFX()
        {
            // Preview TrailVFX if it's assigned
            TrailVFX?.PrefabReference.Do(PreviewVFX);

            // Preview ImpactVFX if it's assigned
            ImpactVFX?.PrefabReference.Do(PreviewVFX);

            // Preview ExplosionVFX if it's assigned
            ExplosionVFX?.PrefabReference.Do(PreviewVFX);

            // Preview MissileVFX if it's assigned
            MissileVFX?.PrefabReference.Do(PreviewVFX);

            // Preview CastVFX if it's assigned
            CastVFX?.PrefabReference.Do(PreviewVFX);
        }

        [FoldoutGroup("General VFX Controls")]
        [Button("Delete Last Preview", ButtonSizes.Large)]
        [GUIColor(1.0f, 0.6f, 0.6f)]
        public void DeleteLastPreview()
        {
            DeletePreviewVFX();
        }

        [FoldoutGroup("General VFX Controls")]
        [Button("Clear All Previews", ButtonSizes.Large)]
        [GUIColor(1.0f, 0.8f, 0.5f)]
        public void ClearAllPreviews()
        {
            while (_instantiatedObjects.Count > 0)
            {
                DeletePreviewVFX();
            }
        }

        private void DeletePreviewVFX()
        {
            if (_instantiatedObjects.Count > 0)
            {
                var instantiatedObject = _instantiatedObjects.Dequeue();
                DestroyImmediate(instantiatedObject); // Safe destruction of scene instance only
            }
            else
            {
                Debug.LogWarning("No preview object to delete.");
            }
        }

        private async void PreviewVFX(AssetReference effect)
        {

            if (!EditorApplication.isPlaying)
            {
                Debug.Log($"Previewing effect: {effect}");

                // Check if the AssetReference is valid before proceeding
                if (!effect.RuntimeKeyIsValid())
                {
                    Debug.LogError($"Invalid AssetReference for effect: {effect}");
                    return;
                }

                // Get the existing handle if it's already loaded
                var handle = effect.OperationHandle.IsValid()
                    ? effect.OperationHandle
                    : effect.LoadAssetAsync<GameObject>();

                // Wait for the asset to load (or get the already loaded asset)
                var loadedVFX = await handle.Task.AsUniTask();

                // Check if the asset was successfully loaded
                if (loadedVFX == null)
                {
                    Debug.LogError("Failed to load the asset.");
                    return;
                }

                // Instantiate the prefab in the scene
                var instance = loadedVFX as GameObject;
                if (instance == null)
                {
                    Debug.LogError("Failed to instantiate the VFX prefab.");
                    return;
                }

                // Instantiate a scene instance of the prefab using Object.Instantiate
                var instantiatedObject = PrefabUtility.InstantiatePrefab(instance) as GameObject;
                if (instantiatedObject == null)
                {
                    Debug.LogError("Failed to instantiate the effect prefab.");
                    return;
                }

                // Set the position for the instantiated object (customize as needed)
                instantiatedObject.transform.position = Vector3.up * 2;

                // Enqueue the instantiated object for future cleanup
                _instantiatedObjects.Enqueue(instantiatedObject);
            }

        }
#endif
        #endregion

        #region Helper Methods for ShowIf Conditions

        private bool ShouldHideTrailVFX() => !showAllVFX && TrailVFX == null;
        private bool ShouldHideImpactVFX() => !showAllVFX && ImpactVFX == null;
        private bool ShouldHideExplosionVFX() => !showAllVFX && ExplosionVFX == null;
        private bool ShouldHideMissileVFX() => !showAllVFX && MissileVFX == null;
        private bool ShouldHideCastVFX() => !showAllVFX && CastVFX == null;

        #endregion
    }

}