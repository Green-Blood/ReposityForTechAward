using Game.GamePlay.Character.Base.Attack.Interfaces;
using Game.GamePlay.Character.Base.Movement.Interfaces;

namespace Game.GamePlay.Character.Base.Attack
{
    public class MeleeAttack : IAttack
    {
        private readonly IAttackAnimation _attackAnimation;
        private readonly IAttackRangeCheck _attackRangeCheck;
        private readonly IAttackWaiter _attackWaiter;
        private readonly IMovable _characterMovement;

        public MeleeAttack(IAttackAnimation attackAnimation, IAttackRangeCheck attackRangeCheck,
                           IAttackWaiter attackWaiter, IMovable characterMovement)
        {
            _attackAnimation = attackAnimation;
            _attackRangeCheck = attackRangeCheck;
            _attackWaiter = attackWaiter;
            _characterMovement = characterMovement;
        }

        public bool CanAttack()
        {
            if (_attackRangeCheck.IsInAttackRange())
            {
                return true;
            }
            _characterMovement.AllowMoving();
            return false;
        }

        public void TryAttack()
        {
            if (_attackWaiter.IsAttackCooldownFinished())
            {
                _attackAnimation.PlayAttackAnimation();
            }
        }
    }
}
