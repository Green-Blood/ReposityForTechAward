namespace Systems.StatSystem.Modifiers.StatModifiers
{
    public class StatModifierBaseAdd : StatModifier
    {
        public override int Order => 2;
        public override float ApplyModifier(float statValue, float modValue) => (modValue);
        
        public StatModifierBaseAdd(float value) : base(value) { }
        public StatModifierBaseAdd(float value, bool stacks) : base(value, stacks) { }
    }
}