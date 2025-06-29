using Extensions.Console.Interfaces;
using Game.Core.StaticData;
using Systems.StatSystem;
using Systems.StatSystem.StatTypes;

namespace Game.GamePlay.Character.Hero
{
    public class DefaultHeroStatsCollection : StatCollection
    {
        private readonly HeroStaticData _heroStaticData;

        public DefaultHeroStatsCollection(HeroStaticData heroStaticData, IDebugLogger logger) : base(logger)
        {
            _heroStaticData = heroStaticData;
            ConfigureStats();
        }
        protected sealed override void ConfigureStats()
        {
            // var health = CreateOrGetStat<Vital>(StatType.Health);
            // health.StatName = "Health";
            //
            // health.StatBaseValue = _heroStaticData.Health;
            // health.StatCurrentValue = health.StatBaseValue;
            //
            // var damage = CreateOrGetStat<Vital>(StatType.Damage);
            // damage.StatName = "Damage";
            // damage.StatBaseValue = _heroStaticData.AttackDamage;
            // damage.StatCurrentValue = damage.StatBaseValue;
            //
            // var attackSpeed = CreateOrGetStat<Vital>(StatType.AttackSpeed);
            // attackSpeed.StatName = "AttackSpeed";
            // attackSpeed.StatBaseValue = _heroStaticData.AttackCooldown;
            // attackSpeed.StatCurrentValue = attackSpeed.StatBaseValue;
            //
            // var attackRange = CreateOrGetStat<Vital>(StatType.AttackRange);
            // attackRange.StatName = "AttackRange";
            // attackRange.StatBaseValue = _heroStaticData.AttackRange;
            // attackRange.StatCurrentValue = attackRange.StatBaseValue;

        }
    }
}