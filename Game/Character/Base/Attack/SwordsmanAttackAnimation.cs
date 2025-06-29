using Game.Core.Interfaces;
using Game.GamePlay.Character.Base.Attack.Interfaces;
using Game.GamePlay.Character.Other.Interfaces;
using UnityEngine;

namespace Game.GamePlay.Character.Base.Attack
{
    public class SwordsmanAttackAnimation : IAttackAnimation
    {
        private readonly Animator _animator;
        private readonly IRandomService _randomService;

        public SwordsmanAttackAnimation(IAnimatorView animatorView, IRandomService randomService)
        {
            _animator = animatorView.Animator;
            _randomService = randomService;
        }

        public void PlayAttackAnimation()
        {
            int attackVariant = _randomService.Next(0, 3);

            _animator.SetInteger(AnimatorTexts.AttackVariant, attackVariant);
            _animator.SetTrigger(AnimatorTexts.Attack);
        }
    }
}