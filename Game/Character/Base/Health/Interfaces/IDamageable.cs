using Game.GamePlay.Character.Base.Attack.Damage;
using UniRx;

namespace Game.GamePlay.Character.Base.Health.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(HitData hitData, float damage);
        ReactiveCommand<HitData> OnDamageTaken { get; }
    }
}