using Game.Core.Factories.Interfaces;
using Game.GamePlay.Character.Base.Attack.Damage;

namespace Game.GamePlay.Character.Base.DamageCalculators.Interfaces
{
    public interface IDamageCalculator
    {
        float CalculateDamage(float damageDealt, IShared attacker, ICombatTarget defender);
        float CalculateExplosionDamage(HitData damageData, IShared attacker, IShared defender);
        
    }
}