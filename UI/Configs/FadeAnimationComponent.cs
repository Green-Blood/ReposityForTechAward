using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.Configs
{
    [CreateAssetMenu(fileName = "FadeAnimation", menuName = "Static Data/UI/FadeAnimation")]
    public class FadeAnimationComponent : UIAnimationComponent
    {
        [Title("Fade Settings")] [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)] [SerializeField]
        private float fadeStartValue;
        
        public float FadeStartValue => fadeStartValue;
        [Title("Fade Settings")] [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)] [SerializeField]
        private float fadeEndValue;
        
        public float FadeEndValue => fadeEndValue;
    }
}