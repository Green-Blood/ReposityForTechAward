using Extensions.Console.Interfaces;
using Game.Core.StaticData;
using Systems.StatSystem;
using Systems.StatSystem.StatTypes;

namespace Game.GamePlay.Character.Enemy
{
    public class DefaultEnemyStatsCollection : StatCollection 
    {
        private readonly EnemyStaticData _enemyStaticData;

        public DefaultEnemyStatsCollection(EnemyStaticData enemyStaticData, IDebugLogger logger) : base(logger)
        {
            _enemyStaticData = enemyStaticData;
            ConfigureStats();
        }

        public override string GetStatOwner(StatType type) => GetStat(type) == null ? "No owner" : _enemyStaticData.Name;
        protected sealed override void ConfigureStats()
        {
            // var health = CreateOrGetStat<Vital>(StatType.Health);
            // health.StatName = "Health";

            // health.StatBaseValue = _enemyStaticData.Health;
            // health.StatCurrentValue = health.StatBaseValue;
            //
            // var damage = CreateOrGetStat<Vital>(StatType.Damage);
            // damage.StatName = "Damage";
            // damage.StatBaseValue = _enemyStaticData.AttackDamage;
            // damage.StatCurrentValue = damage.StatBaseValue;
            //
            // var attackSpeed = CreateOrGetStat<Vital>(StatType.AttackSpeed);
            // attackSpeed.StatName = "AttackSpeed";
            // attackSpeed.StatBaseValue = _enemyStaticData.AttackCooldown;
            // attackSpeed.StatCurrentValue = damage.StatBaseValue;
            //
            // var attackRange = CreateOrGetStat<Vital>(StatType.AttackRange);
            // attackRange.StatName = "AttackRange";
            // attackRange.StatBaseValue = _enemyStaticData.AttackRange;
            // attackRange.StatCurrentValue = attackRange.StatBaseValue;
        }
    }
}