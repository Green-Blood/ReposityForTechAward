using Extensions.Console.Interfaces;
using Systems.StatSystem.Modifiers.StatModifiers;
using Systems.StatSystem.StatTypes;

namespace Systems.StatSystem.StatCollections
{
    public class DefaultStats : StatCollection
    {
        public DefaultStats(IDebugLogger logger) : base(logger)
        {
        }
        protected override void ConfigureStats()
        {
            // var stamina = CreateOrGetStat<Attribute>(StatType.Vitality);
            // stamina.StatName = "Stamina";
            // stamina.StatBaseValue = 10;

            // var defence = CreateOrGetStat<Attribute>(StatType.Defence);
            // defence.StatName = "Defence";
            // defence.StatBaseValue = 5;
            
            var health = CreateOrGetStat<Vital>(StatType.Health);
            health.StatName = "Health";
            health.StatBaseValue = 100;
            // health.AddLinker(new StatLinkerBasic(CreateOrGetStat<Attribute>(StatType.Vitality), 10f));
            // health.UpdateLinkers();
            health.AddModifier(new StatModifierBasePercent(1f));
            health.AddModifier(new StatModifierBaseAdd(50f));
            health.AddModifier(new StatModifierTotalPercent(1f, false));
            health.UpdateModifiers();

            // var attack = CreateOrGetStat<Stat>(StatType.Attack); 
            // TODO Do i need name? 
            // attack.StatName = "Attack";
            // attack.StatBaseValue = 10;
            
            

        }
    }
}