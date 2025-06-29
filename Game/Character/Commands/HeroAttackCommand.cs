using Cysharp.Threading.Tasks;
using Game.Core.Commands.Interfaces;
using Game.GamePlay.Character.Base.Attack.Interfaces;
using Game.GamePlay.Character.Base.Movement.Interfaces;
using UnityEngine;

namespace Game.GamePlay.Character.Commands
{
    public class HeroAttackCommand : ICommand
    {
        private readonly IAttack _attack;
        private readonly bool _isAnybodyAround;
        private readonly IMovable _characterMovement;
        public byte Priority => 2;

        public HeroAttackCommand(IAttack attack, bool isAnybodyAround, IMovable characterMovement)
        {
            _attack = attack;
            _isAnybodyAround = isAnybodyAround;
            _characterMovement = characterMovement;
        }

        public bool CanExecute()
        {
            return _isAnybodyAround && _attack.CanAttack();
        }

        public UniTask Execute()
        {
            _characterMovement.ForbidMoving();
            Debug.Log("Attacking");
            _attack.TryAttack();
            return UniTask.CompletedTask;
        }
    }
}