using Game.GamePlay.Character.Base.Movement.Interfaces;
using Game.GamePlay.Character.Other.Interfaces;
using UnityEngine;

namespace Game.GamePlay.Character.Base.Movement
{
    public class GroundRotation : IRotate
    {
        private readonly Transform _characterTransform;
        private readonly Quaternion _initialRotation;

        public GroundRotation(ICharacterView characterView)
        {
            _characterTransform = characterView.TransformView;
            _initialRotation = _characterTransform.rotation;
        }

        public void RotateTowards(Vector3 targetPosition, float rotationSpeed)
        {
            var direction = (targetPosition - _characterTransform.position).normalized;
            var lookRotation = Quaternion.LookRotation(direction);
            _characterTransform.rotation = Quaternion.Slerp(_characterTransform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        public void ResetRotation() => _characterTransform.rotation = _initialRotation;
    }
}