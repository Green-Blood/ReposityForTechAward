using UniRx;

namespace Game.GamePlay.Character.Base.Health.Interfaces
{
    public interface IDie
    {
        void Die();
        void Init();
        ReactiveProperty<bool> IsDead { get; }
        void Revive();
    }
}