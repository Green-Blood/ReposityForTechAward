using System;
using Game.GamePlay.Character.Base.Attack.Interfaces;
using Systems.StatSystem;
using Systems.StatSystem.Interfaces;
using Systems.StatSystem.StatTypes;
using UniRx;
using Attribute = Systems.StatSystem.StatTypes.Attribute;

namespace Game.GamePlay.Character.Base.Attack
{
    public class AttackSpeedHandler : IAttackWaiter, IDisposable
    {
        private readonly Attribute _attackSpeed;
        private float _attackSpeedModifier;
        private float _attackCooldown;
        private bool _isAttackCooldownFinished = true;
        private IDisposable _attackCooldownDisposable;
        private const float BaseAttackCooldown = 0.25f;

        public AttackSpeedHandler(IStatCollection statsCollection)
        {
            _attackSpeed = statsCollection.TryGetStat<Attribute>(StatType.AttackSpeed);
            _attackCooldown = BaseAttackCooldown;
            _attackCooldown = CalculateAttackCooldown();
        }

        public bool IsAttackCooldownFinished()
        {
            if (!_isAttackCooldownFinished)
                return false;
            
            StartAttackCooldown();
            return true;
        }
        private void StartAttackCooldown()
        {
            _isAttackCooldownFinished = false;
            _attackCooldown = CalculateAttackCooldown();
            _attackCooldownDisposable = Observable.Timer(TimeSpan.FromSeconds(_attackCooldown)).Subscribe(_ =>
            {
                _isAttackCooldownFinished = true;
            });
        }

        private float CalculateAttackCooldown() => _attackSpeed.StatBaseValue / (1 + _attackSpeed.StatModifierValue);

        public void UpdateAttackCooldown() => throw new System.NotImplementedException();
        public void Dispose()
        {
            _attackCooldownDisposable?.Dispose();
        }
    }
}
