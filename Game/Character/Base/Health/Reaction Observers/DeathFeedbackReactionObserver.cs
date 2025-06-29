using System;
using Game.Core.Factories.Interfaces;
using Game.GamePlay.Character.Base.Health.Interfaces;
using JetBrains.Annotations;
using MoreMountains.Feedbacks;
using UniRx;
using UnityEngine;

namespace Game.GamePlay.Character.Base.Health.Reaction_Observers
{
    public class DeathFeedbackReactionObserver : MonoBehaviour, IReact, IDisposable
    {
        [SerializeField] [CanBeNull] private MMF_Player deathFeedback;

        private readonly CompositeDisposable _disposables = new();

        public void Register(IShared character)
        {
            if(deathFeedback != null)
            {
                character.UnitDeath.IsDead.Subscribe(ReactToDeath).AddTo(_disposables);
            }
        }

        private void ReactToDeath(bool isDead)
        {
            if(isDead)
            {
                React();
            }
        }

        public void UnregisterFromReaction()
        {
            _disposables?.Dispose();
        }

        public void React()
        {
            deathFeedback?.PlayFeedbacks();
            
        }

        public void Dispose()
        {
            UnregisterFromReaction();
        }
    }
}