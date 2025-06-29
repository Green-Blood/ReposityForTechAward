using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Extensions.Console.Interfaces;
using Extensions.Enums.Types;
using Game.Core.Factories.Interfaces;
using Game.GamePlay.Character.Base.Attack.Damage;
using Game.GamePlay.Character.Base.DamageCalculators.Interfaces;
using Game.GamePlay.Character.Base.Health.Interfaces;
using Zenject;

namespace Game.GamePlay.Character.Base.Health
{
    public class DamageService : IDamageService
    {
        private readonly IDebugLogger _logger;
        private readonly Dictionary<DamageType, IDamageCalculator> _damageStrategies = new();
        private readonly DiContainer _container;

        public DamageService(IDebugLogger logger, DiContainer container)
        {
            _logger = logger;
            _container = container;
        }

        public async UniTask Warmup()
        {
            foreach (DamageType damageType in Enum.GetValues(typeof(DamageType)))
            {
                try
                {
                    // Resolve the correct IDamageCalculator by damageType
                    var strategy = _container.ResolveId<IDamageCalculator>(damageType);
                    _damageStrategies[damageType] = strategy;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"No strategy found for DamageType: {damageType}. Exception: {ex.Message}");
                }
            }

            await UniTask.CompletedTask;
        }

        public void ApplyDamage(HitData hitData, IShared attacker, IShared receiver)
        {
            if (_damageStrategies.TryGetValue(hitData.DamageType, out var strategy))
            {
                var finalDamage = hitData.ExplosionData.HasValue
                    ? strategy.CalculateExplosionDamage(hitData, attacker, receiver)
                    : strategy.CalculateDamage(hitData.DamageDealt, attacker, receiver);
                
                receiver.UnitHealth.TakeDamage(hitData, finalDamage);
            }
            else
            {
                _logger.LogWarning($"No strategy found for damage type {hitData.DamageType}");
            }
        }

        public void CleanUp()
        {
            _damageStrategies.Clear();
        }
    }
}