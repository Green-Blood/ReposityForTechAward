using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.Configs
{
    public abstract class UIAnimationComponent : SerializedScriptableObject
    {
        [FoldoutGroup("Animation Settings")] [SuffixLabel("seconds", true)] [Tooltip("Duration of animation effect.")] [SerializeField] [MinValue(0.1f)]
        private float duration = 0.75f;

        [FoldoutGroup("Animation Settings")] [SerializeField] [Tooltip("Easing function for the fade.")]
        public Ease ease = Ease.Linear;

        [FoldoutGroup("Animation Settings")] [SerializeField] [Tooltip("Delay before the fade starts.")]
        public float delay;

        public float Duration => duration;
        public float Delay => delay;
        public Ease Ease => ease;
    }
}