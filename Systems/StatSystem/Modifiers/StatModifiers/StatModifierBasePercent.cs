namespace Systems.StatSystem.Modifiers.StatModifiers
{
    public class StatModifierBasePercent : StatModifier
    {
        public override int Order => 1;
        public override float ApplyModifier(float statValue, float modValue) => (int)(statValue * modValue);
        
        public StatModifierBasePercent(float value) : base(value) { }
        public StatModifierBasePercent(float value, bool stacks) : base(value, stacks) { }
    }
}