using System;
using Cysharp.Threading.Tasks;
using Game.Core.Commands.Interfaces;
using Game.GamePlay.Character.Base.Movement.Interfaces;
using Unity.Mathematics;

namespace Game.GamePlay.Character.Commands
{
    public class MoveCommand : ICommand
    {
        private readonly IMovable _characterMovement;
        private readonly float3 _target;

        public byte Priority => 1;

        public MoveCommand(IMovable characterMovement, float3 target)
        {
            _characterMovement = characterMovement;
            _target = target;
        }

        public bool CanExecute() => _characterMovement.CanMove;

        public UniTask Execute()
        {
            _characterMovement.MoveTo(_target);

            return UniTask.CompletedTask;
        }
    }
}