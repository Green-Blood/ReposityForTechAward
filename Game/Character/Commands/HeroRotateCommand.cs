using Cysharp.Threading.Tasks;
using Game.Core.Commands.Interfaces;
using Game.GamePlay.Character.Base.Movement.Interfaces;
using UnityEngine;

namespace Game.GamePlay.Character.Commands
{
    public class HeroRotateCommand : ICommand
    {
        private readonly Vector3 _targetPosition;
        private readonly float _rotationSpeed;
        private readonly bool _isAnybodyAround;
        private readonly IRotate _characterRotation;
        public byte Priority => 2;

        public HeroRotateCommand(IRotate characterRotation, Vector3 targetPosition, float rotationSpeed, bool isAnybodyAround)
        {
            _characterRotation = characterRotation;
            _rotationSpeed = rotationSpeed;
            _isAnybodyAround = isAnybodyAround;
            _targetPosition = targetPosition;
        }

        public bool CanExecute() => _isAnybodyAround;

        public virtual UniTask Execute()
        {
            _characterRotation.RotateTowards(_targetPosition, _rotationSpeed);
            return UniTask.CompletedTask;
        }
    }

    public class ResetPositionCommand : ICommand
    {
        private readonly IMovable _characterMovement;
        private readonly IRotate _characterRotation;

        public ResetPositionCommand(IMovable characterMovement, IRotate characterRotation)
        {
            _characterMovement = characterMovement;
            _characterRotation = characterRotation;
        }

        public byte Priority => 0;

        public bool CanExecute()
        {
            return true;
        }

        public UniTask Execute()
        {
            _characterMovement.ResetPosition();
            _characterRotation.ResetRotation();
            return UniTask.CompletedTask;
        }
    }
}