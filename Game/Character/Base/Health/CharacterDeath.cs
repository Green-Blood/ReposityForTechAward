using Extensions.ExtensionMethods;
using Game.GamePlay.Character.Base.Health.Interfaces;
using Game.GamePlay.Character.Other.Interfaces;
using ProjectDawn.Navigation.Hybrid;
using UniRx;
using UnityEngine;

namespace Game.GamePlay.Character.Base.Health
{
    public class CharacterDeath : IDie
    {
        private readonly Animator _animator;
        private readonly Collider _collider;
        private readonly AgentAuthoring _agent;
        // private readonly Rigidbody _rigidBody;

        public ReactiveProperty<bool> IsDead { get; private set; } = new();

        public CharacterDeath(ICharacterView characterView)
        {
            _animator = characterView.Animator;
            _collider = characterView.Collider;
            _agent = characterView.Agent;
            //_rigidBody = characterView.CharacterRigidBody;
            Init();
        }

        public void Init()
        {
            _agent.enabled = true;
            _collider.enabled = true;
            // _rigidBody.WakeUp();
            IsDead.Value = false;
        }

        public void Revive()
        {
            _animator.SetTriggerSafe(AnimatorTexts.ReviveTrigger);
            Init();
        }

        public void Die()
        {
            _animator.SetTriggerSafe(AnimatorTexts.DieTrigger);
            _collider.enabled = false;
            _agent.EntityBody.Stop();
            // _rigidBody.Sleep();
            IsDead.Value = true;
        }
    }
}