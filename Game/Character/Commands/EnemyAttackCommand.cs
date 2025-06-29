using Cysharp.Threading.Tasks;
using Game.Core.Commands.Interfaces;
using Game.GamePlay.Character.Base.Attack.Interfaces;
using Game.GamePlay.Character.Base.Movement.Interfaces;

namespace Game.GamePlay.Character.Commands
{
    public class EnemyAttackCommand : ICommand
    {
        private readonly IAttack _attack;
        private readonly IMovable _characterMovement;
        public byte Priority => 1;

        public EnemyAttackCommand(IAttack attack, IMovable characterMovement)
        {
            _characterMovement = characterMovement;
            _attack = attack;
        }

        public bool CanExecute()
        {
            return _attack.CanAttack();
        }

        public UniTask Execute()
        {
            _characterMovement.ForbidMoving();
            _attack.TryAttack();

            return UniTask.CompletedTask;
        }
    }
}