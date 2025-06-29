namespace Systems.StatSystem.Modifiers.StatModifiers
{
    public class StatModifierTotalPercent : StatModifier
    {
        public override int Order => 3;
        public override float ApplyModifier(float statValue, float modValue) => (int)(statValue * modValue);
        
        public StatModifierTotalPercent(float value) : base(value) { }
        public StatModifierTotalPercent(float value, bool stacks) : base(value, stacks) { }
    }
}