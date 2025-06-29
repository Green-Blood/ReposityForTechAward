namespace Systems.StatSystem.Modifiers.StatModifiers
{
    public class StatModifierTotalAdd : StatModifier
    {
        public override int Order => 4;
        public override float ApplyModifier(float statValue, float modValue) => (int)(modValue);
        
        public StatModifierTotalAdd(float value) : base(value) { }
        public StatModifierTotalAdd(float value, bool stacks) : base(value, stacks) { }
    }
}