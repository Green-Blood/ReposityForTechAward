using System;
using Game.Core.Factories.Interfaces;
using Systems.StatSystem.StatTypes;
using UI.Configs;
using UI.Interfaces;
using UniRx;

namespace UI.ViewModels
{
    public class CastleHealthViewModel : ViewModelBase
    {
        public ReactiveProperty<float> CurrentHealth { get; private set; }
        public ReactiveProperty<float> MaxHealth { get; private set; }
        
        private readonly Vital _healthStat;
        
        public CastleHealthViewModel(IShared shared, UIAnimationData animationData)
        {
            _healthStat = shared.StatCollection.GetStat<Vital>(StatType.Health);

            CurrentHealth = new ReactiveProperty<float>(_healthStat.StatCurrentValue);
            MaxHealth = new ReactiveProperty<float>(_healthStat.StatValue);

            _healthStat.OnCurrentValueChange += (_, _) => CurrentHealth.Value = _healthStat.StatCurrentValue;

            _healthStat.OnValueChange += (_, _) => MaxHealth.Value = _healthStat.StatValue;
        }

        public override void Dispose()
        {
            // Clean up the reactive properties
            CurrentHealth.Dispose();
            MaxHealth.Dispose();
        }
    }
}