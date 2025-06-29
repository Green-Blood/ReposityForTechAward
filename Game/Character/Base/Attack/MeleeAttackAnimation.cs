using Extensions.ExtensionMethods;
using Game.GamePlay.Character.Base.Attack.Interfaces;
using Game.GamePlay.Character.Other.Interfaces;

namespace Game.GamePlay.Character.Base.Attack
{
    public class MeleeAttackAnimation : IAttackAnimation
    {
        private readonly IAnimatorView _animatorView;

        public MeleeAttackAnimation(IAnimatorView animatorView) => _animatorView = animatorView;
        public void PlayAttackAnimation() => _animatorView.Animator.SetTriggerSafe(AnimatorTexts.Attack);
    }
}