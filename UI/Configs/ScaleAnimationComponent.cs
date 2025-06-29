using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.Configs
{
    [CreateAssetMenu(fileName = "ScaleAnimation", menuName = "Static Data/UI/ScaleAnimation")]
    public class ScaleAnimationComponent : UIAnimationComponent
    {
        [Title("Scale Settings")] [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)] [SerializeField]
        private Vector3 scaleStartValue = Vector3.zero;

        public Vector3 ScaleStartValue => scaleStartValue;

        [Title("Scale Settings")] [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)] [SerializeField]
        private Vector3 scaleEndValue = Vector3.zero;

        public Vector3 ScaleEndValue => scaleEndValue;
    }
}