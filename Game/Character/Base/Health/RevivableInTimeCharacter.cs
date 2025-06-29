using System;
using Game.GamePlay.Character.Base.Health.Interfaces;
using Systems.StatSystem.Interfaces;
using Systems.StatSystem.StatTypes;
using UniRx;
using Attribute = Systems.StatSystem.StatTypes.Attribute;

namespace Game.GamePlay.Character.Base.Health
{
    public class RevivableInTimeCharacter : IRevivableInTime, IDisposable
    {
        private readonly Attribute _reviveTimer;
        private readonly IDie _reviveableCharacterDeath;
        private readonly CompositeDisposable _disposables = new();
        public ReactiveCommand OnReviveFinished { get; private set; } = new();
        public ReactiveCommand<float> OnReviveStart { get; private set; } = new();

        public RevivableInTimeCharacter(IStatCollection statCollection, IDie reviveableCharacterDeath)
        {
            _reviveableCharacterDeath = reviveableCharacterDeath;
            _reviveTimer = statCollection.TryGetStat<Attribute>(StatType.ReviveTime);
            _reviveableCharacterDeath.IsDead.Subscribe(OnCharacterDied).AddTo(_disposables);
        }

        private void OnCharacterDied(bool isDead)
        {
            if(isDead)
            {
                StartReviveTimer();
            }
        }

        public void StartReviveTimer()
        {
            OnReviveStart?.Execute(_reviveTimer.StatValue);
            Observable.Timer(TimeSpan.FromSeconds(_reviveTimer.StatValue)).Subscribe(_ => FinishReviveTimer()).AddTo(_disposables);
        }

        public void FinishReviveTimer()
        {
            OnReviveFinished?.Execute();
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}