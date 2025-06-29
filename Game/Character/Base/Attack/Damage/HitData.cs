using Extensions.Enums.Types;
using UnityEngine;

namespace Game.GamePlay.Character.Base.Attack.Damage
{
    public struct HitData
    {
        public float DamageDealt;
        public DamageType DamageType;
        public ExplosionData? ExplosionData;
    }
}
