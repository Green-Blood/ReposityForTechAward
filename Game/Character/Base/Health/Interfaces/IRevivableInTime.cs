using UniRx;

namespace Game.GamePlay.Character.Base.Health.Interfaces
{
    public interface IRevivableInTime
    {
        ReactiveCommand<float> OnReviveStart { get; }
        ReactiveCommand OnReviveFinished { get; }
        void StartReviveTimer();
        void FinishReviveTimer();
    }
}