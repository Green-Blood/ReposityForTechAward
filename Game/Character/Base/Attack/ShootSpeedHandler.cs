using Game.Core.StaticData;
using Game.GamePlay.Character.Base.Attack.Interfaces;
using Systems.StatSystem;
using Systems.StatSystem.Interfaces;
using Systems.StatSystem.StatTypes;
using UnityEngine;

namespace Game.GamePlay.Character.Base.Attack
{
    public class ShootSpeedHandler : IAttackWaiter
    {
        private float _attackSpeedModifier;
        private float _delayTime;
        private float _attackCooldown;

        private readonly Attribute _attackSpeed;
        private readonly float _baseAttackCooldown;
        private readonly float _shootDelay;

        public ShootSpeedHandler(IStatCollection statsCollection, CastleStaticData castleStaticData)
        {
            _attackSpeed = statsCollection.TryGetStat<Attribute>(StatType.AttackSpeed);
            _baseAttackCooldown = castleStaticData.BaseAttackCooldown;

            UpdateAttackCooldown();
            ResetDelayTime();
        }
        private void ResetDelayTime() => _delayTime = Time.time + _attackCooldown;
        private bool CanShoot() => Time.time > _delayTime;
        public bool IsAttackCooldownFinished()
        {
            if (!CanShoot()) return false;
            ResetDelayTime();
            UpdateAttackCooldown();
            return true;
        }

        public void UpdateAttackCooldown() => _attackCooldown = _baseAttackCooldown / (_attackSpeed.StatBaseValue / (1 + _attackSpeed.StatModifierValue));
    }
}
