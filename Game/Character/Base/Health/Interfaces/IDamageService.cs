using Game.Core.Factories.Interfaces;
using Game.Core.Interfaces;
using Game.GamePlay.Character.Base.Attack.Damage;
using Meta.Interfaces;

namespace Game.GamePlay.Character.Base.Health.Interfaces
{
    public interface IDamageService : IServicePreloader
    {
        void ApplyDamage(HitData hitData, IShared attacker, IShared receiver);
    }
}