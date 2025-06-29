using System;
using Extensions.Console.Interfaces;
using Game.GamePlay.Character.Base.Attack.Damage;
using Game.GamePlay.Character.Base.Health.Interfaces;
using Systems.StatSystem.Interfaces;
using Systems.StatSystem.StatTypes;
using UniRx;

namespace Game.GamePlay.Character.Base.Health
{
    public class UnitDamageReceiver : IDamageable
    {
        private readonly Vital _health;
        private readonly IDie _characterDeath;
        private readonly IDebugLogger _logger;

        public ReactiveCommand<HitData> OnDamageTaken { get; } = new ReactiveCommand<HitData>();

        public UnitDamageReceiver(IDie death, IStatCollection characterStatCollection, IDebugLogger logger)
        {
            _characterDeath = death ?? throw new ArgumentNullException(nameof(death));
            _logger = logger;
            _health = characterStatCollection.TryGetStat<Vital>(StatType.Health);
        }

        public void TakeDamage(HitData hitData, float damage)
        {
            if(damage < 0)
            {
                _logger.LogWarning("Damage can't be negative");
                return;
            }

            _health.StatCurrentValue -= damage;
            _logger.Log("Health is " + _health.StatCurrentValue);
            if(_health.StatCurrentValue <= 0) _characterDeath.Die();

            OnDamageTaken.Execute(hitData);
        }
    }
}