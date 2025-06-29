using Game.GamePlay.Character.Base.Movement.Interfaces;
using Game.GamePlay.Character.Other.Interfaces;
using ProjectDawn.Navigation.Hybrid;
using UnityEngine;

namespace Game.GamePlay.Character.Base.Movement
{
    public class MovableAnimation : IMovableAnimator
    {
        private readonly Animator _characterAnimator;
        private readonly Transform _characterTransform;
        private readonly AgentAuthoring _characterAgent;

        private float _currentSpeed;
        private Vector3 _lastPosition;

        public MovableAnimation(ICharacterView characterView)
        {
            _characterAnimator = characterView.Animator;
            _characterTransform = characterView.TransformView;
            _characterAgent = characterView.Agent;
        }

        public void SetMovableAnimatorSpeed()
        {
            // _characterAnimator.SetFloat(AnimatorTexts.Speed, GetCurrentSpeed());
            _characterAnimator.SetFloat(AnimatorTexts.Speed, _characterAgent.EntityBody.Speed);
        }

        private float GetCurrentSpeed()
        {
            var position = _characterTransform.position;
            _currentSpeed = Mathf.Lerp(_currentSpeed, (position - _lastPosition).magnitude / Time.deltaTime,
                0.50f);
            _lastPosition = position;

            return _currentSpeed;
        }
    }
}