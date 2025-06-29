using System;
using Game.Core.Factories.Interfaces;
using Game.GamePlay.Character.Base.Attack.Damage;
using Game.GamePlay.Character.Base.DamageCalculators.Interfaces;
using Systems.StatSystem.StatTypes;
using UnityEngine;
using Attribute = Systems.StatSystem.StatTypes.Attribute;

namespace Game.GamePlay.Character.Base.DamageCalculators
{
    public class PhysicalDamageCalculator : IDamageCalculator
    {
        public float CalculateDamage(float damageDealt, IShared attacker, ICombatTarget defender)
        {
            var physicalResistance = defender.StatCollection.CreateOrGetStat<Attribute>(StatType.PhysicalResistance).StatValue;
            var damage = damageDealt * (1 - physicalResistance / 100f);
            return MathF.Max(damage, 0);
        }

        public float CalculateExplosionDamage(HitData damageData, IShared attacker, IShared defender)
        {
            if(damageData.ExplosionData == null) return 0;

            var explosionCenter = damageData.ExplosionData.Value.ExplosionCenter;
            float explosionRadius = damageData.ExplosionData.Value.ExplosionRadius;
            float explosionResistance = defender.StatCollection.CreateOrGetStat<Attribute>(StatType.PhysicalResistance).StatValue;

            float distanceToCenter = Vector3.Distance(explosionCenter, defender.CharacterView.TransformView.position);
            float damageFalloff = Mathf.Clamp01(1 - (distanceToCenter / explosionRadius));
            // TODO Should I take it into some kind of settings?  
            damageFalloff = Mathf.Clamp(Mathf.Pow(damageFalloff, 0.5f), 0.15f, 1f);// Adjust 0.5 for a gentler falloff + minimum damage is 15% 

            float finalDamage = damageData.DamageDealt * damageFalloff * (1 - explosionResistance / 100f);
            return Mathf.Max(finalDamage, 0);
        }
    }
}