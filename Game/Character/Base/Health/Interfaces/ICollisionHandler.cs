using Game.Core.Factories.Interfaces;
using Game.GamePlay.Character.Base.Attack.Damage;

namespace Game.GamePlay.Character.Base.Health.Interfaces
{
    public interface ICollisionHandler
    {
        void Handle(HitData hitData, IShared attacker);
    }
}