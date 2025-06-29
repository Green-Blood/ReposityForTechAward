using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Systems.VFXSystem
{
    [CreateAssetMenu(fileName = "Missile", menuName = "Static Data/VFX/MissileVFX")]
    public sealed class MissileVFXComponent : VFXComponent
    {
        [ShowIf(nameof(MissileEffectIsNotNull))]
        [field: SerializeField]
        public Ease MissileEase { get; set; } = Ease.OutQuad;

        [ShowIf(nameof(MissileEffectIsNotNull))]
        [field: SerializeField]
        public float MissileDuration { get; set; } = 0.75f;

        private bool MissileEffectIsNotNull()
        {
            return PrefabReference != null;
        }
    }
}