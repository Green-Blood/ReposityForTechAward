using Sirenix.OdinInspector;
using Systems.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Systems.VFXSystem
{
    public abstract class VFXComponent: SerializedScriptableObject, IPrefabData
    {
        [FoldoutGroup("General Settings")]
        [Title("VFX Name", bold: false)]
        [Tooltip("The name of this VFX component.")]
        [SerializeField]
        private string vfxName = "BaseVFX";

        [FoldoutGroup("VFX Settings")]
        [Title("VFX Asset Reference", bold: false)]
        [Tooltip("The asset reference for the VFX.")]
        [SerializeField]
        [AssetsOnly]
        [Required]
        private AssetReference vfxAsset;

        [FoldoutGroup("VFX Settings")]
        [SuffixLabel("seconds", true)]
        [Tooltip("Duration before the VFX is despawned or completed.")]
        [SerializeField]
        [MinValue(0.1f)]
        private float vfxDuration = 1.0f;

        // Public accessor for the VFX name
        public string Name => vfxName;

        // Public accessor for the VFX asset reference
        public AssetReference PrefabReference => vfxAsset;

        // Public accessor for the VFX duration
        public float Duration => vfxDuration;
    }
}