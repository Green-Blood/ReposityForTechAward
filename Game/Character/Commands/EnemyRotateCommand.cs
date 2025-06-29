using Cysharp.Threading.Tasks;
using Game.Core.Commands.Interfaces;
using Game.GamePlay.Character.Base.Movement.Interfaces;
using UnityEngine;
namespace Game.GamePlay.Character.Commands
{
    public class EnemyRotateCommand : ICommand
    {
        private readonly Vector3 _targetPosition;
        private readonly float _rotationSpeed;
        private readonly IRotate _characterRotation;
        public byte Priority => 2;
        public EnemyRotateCommand(IRotate characterRotation, Vector3 targetPosition, float rotationSpeed)
        {
            _characterRotation = characterRotation;
            _rotationSpeed = rotationSpeed;
            _targetPosition = targetPosition;
        }

        public bool CanExecute()
        {
            return true;
        }
        public virtual UniTask Execute()
        {
            _characterRotation.RotateTowards(_targetPosition, _rotationSpeed);
            return UniTask.CompletedTask;
        }
    }
}
