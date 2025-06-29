using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.Configs
{
    [CreateAssetMenu(fileName = "UI Animation Data", menuName = "Static Data/UI/AnimationData")]
    public class UIAnimationData : SerializedScriptableObject
    {
        [BoxGroup("Settings")]
        [GUIColor(0.9f, 0.95f, 1.0f)]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        [HideIf(nameof(ShouldHideFadeInEffect))]
        [field: SerializeField]
        [CanBeNull]
        private FadeAnimationComponent fadeInInAnimation;

        public FadeAnimationComponent FadeInAnimation
        {
            get => fadeInInAnimation;
            set => fadeInInAnimation = value;
        }
        [BoxGroup("Settings")]
        [GUIColor(0.9f, 0.95f, 1.0f)]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        [HideIf(nameof(ShouldHideFadeOutEffect))]
        [field: SerializeField]
        [CanBeNull]
        private FadeAnimationComponent fadeOutAnimation;

        public FadeAnimationComponent FadeOutAnimation
        {
            get => fadeOutAnimation;
            set => fadeOutAnimation = value;
        }
        
        [BoxGroup("Settings")]
        [GUIColor(0.9f, 0.95f, 1.0f)]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        [HideIf(nameof(ShouldHideScaleInEffect))]
        [field: SerializeField]
        [CanBeNull]
        private ScaleAnimationComponent scaleInAnimation;

        public ScaleAnimationComponent ScaleInAnimation
        {
            get => scaleInAnimation;
            set => scaleInAnimation = value;
        }
        [BoxGroup("Settings")]
        [GUIColor(0.9f, 0.95f, 1.0f)]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        [HideIf(nameof(ShouldHideScaleOutEffect))]
        [field: SerializeField]
        [CanBeNull]
        private ScaleAnimationComponent scaleOutAnimation;

        public ScaleAnimationComponent ScaleOutAnimation
        {
            get => scaleOutAnimation;
            set => scaleOutAnimation = value;
        }


        [FoldoutGroup("General Animation Controls")]
        [LabelText("Show All Animation Components")]
        [GUIColor(0.5f, 0.9f, 1.0f)]
        [Tooltip("Toggle this to show or hide Animation components that are not assigned.")]
        [SerializeField]
        private bool showAllVFX = true;

        #region Helper Methods for ShowIf Conditions

        private bool ShouldHideFadeInEffect() => !showAllVFX && FadeInAnimation == null;
        private bool ShouldHideFadeOutEffect() => !showAllVFX && FadeOutAnimation == null;
        private bool ShouldHideScaleInEffect() => !showAllVFX && ScaleInAnimation == null;
        private bool ShouldHideScaleOutEffect() => !showAllVFX && ScaleOutAnimation == null;
        
        

        #endregion
    }
}