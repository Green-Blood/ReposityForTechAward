using Extensions.Enums.Types;
using Game.Core.Factories.Interfaces;
using Game.GamePlay.Character.Base.Attack.Damage;
using Game.GamePlay.Character.Base.Health.Interfaces;
using JetBrains.Annotations;
using MoreMountains.Feedbacks;
using UniRx;
using UnityEngine;

namespace Game.GamePlay.Character.Base.Health.Reaction_Observers
{
    public class DamageFeedbackReactionObserver : MonoBehaviour, IReact
    {
        [SerializeField] [CanBeNull] private MMF_Player damageFeedback;
        [SerializeField] [CanBeNull] private MMF_Player criticalDamageFeedback;

        private readonly CompositeDisposable _disposables = new();

        private IShared _unit;

        public void Register(IShared character)
        {
            _unit = character;

            if(damageFeedback != null)
            {
                _unit.UnitHealth.OnDamageTaken.Subscribe(ReactToDamage).AddTo(_disposables);
            }
        }

        private void ReactToDamage(HitData hitData)
        {
            switch (hitData.DamageType)
            {
                case DamageType.Physical:
                    React();
                    break;
                case DamageType.CriticalPhysical:
                    ReactToCriticalDamage();
                    break;
            }
        }

        public void React()
        {
            damageFeedback?.PlayFeedbacks();
        }

        private void ReactToCriticalDamage()
        {
            criticalDamageFeedback?.PlayFeedbacks();
        }

        private void OnDestroy()
        {
            _disposables?.Dispose();
        }
    }
}