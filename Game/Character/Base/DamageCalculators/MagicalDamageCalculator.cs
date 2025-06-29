using Game.Core.Factories.Interfaces;
using Game.GamePlay.Character.Base.Attack.Damage;
using Game.GamePlay.Character.Base.DamageCalculators.Interfaces;
using Systems.StatSystem.StatTypes;
using UnityEngine;

namespace Game.GamePlay.Character.Base.DamageCalculators
{
    public class MagicalDamageCalculator : IDamageCalculator
    {
        public float CalculateDamage(float damageDealt, IShared attacker, ICombatTarget defender)
        {
            var magicalResistance = defender.StatCollection.CreateOrGetStat<Attribute>(StatType.MagicalResistance).StatValue;
            var damage = damageDealt * (1 - magicalResistance / 100f);
            return Mathf.Max(damage, 0);
        }

        public float CalculateExplosionDamage(HitData damageData, IShared attacker, IShared defender)
        {
           return CalculateDamage(damageData.DamageDealt, attacker, defender);
        }
    }
}