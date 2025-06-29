using Systems.StatSystem.Modifiers;

namespace Systems.StatSystem.Interfaces
{
    public interface IModifiable
    {
        float StatModifierValue { get; }
     
        void AddModifier(StatModifier modifier);
        void RemoveModifier(StatModifier modifier);
        void ClearModifiers();
        void UpdateModifiers();
    }
}