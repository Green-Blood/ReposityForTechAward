using System;
using System.Collections.Generic;
using System.Linq;
using Systems.StatSystem.Interfaces;
using Systems.StatSystem.StatTypes;
using UnityEngine;

namespace Systems.StatSystem.Modifiers
{
    [Serializable]
    public class StatModifiable : Stat, IModifiable, IStatValueChange
    {
        [field: SerializeField, HideInInspector] public float StatModifierValue { get; private set; }

        public event EventHandler OnValueChange;

        private readonly List<StatModifier> _statModifiers;

        public override float StatValue => base.StatValue + StatModifierValue;

        public StatModifiable()
        {
            _statModifiers = new List<StatModifier>();
            StatModifierValue = 0f;
        }

        public StatModifiable(List<StatModifier> statModifiers, int statModifierValue)
        {
            _statModifiers = statModifiers;
            StatModifierValue = statModifierValue;
        }

        public void AddModifier(StatModifier modifier)
        {
            _statModifiers.Add(modifier);
            modifier.OnValueChange += OnModifierValueChange;
        }

        public void RemoveModifier(StatModifier modifier)
        {
            _statModifiers.Remove(modifier);
            modifier.OnValueChange -= OnModifierValueChange;
        }

        public void ClearModifiers()
        {
            foreach (var statModifier in _statModifiers)
            {
                statModifier.OnValueChange -= OnModifierValueChange;
            }

            _statModifiers.Clear();
        }

        public void UpdateModifiers()
        {
            StatModifierValue = 0f;
            var orderGroups = _statModifiers.OrderBy(modifier => modifier.Order)
                .GroupBy(modifier => modifier.Order);

            foreach (var group in orderGroups)
            {
                float sum = 0, max = 0;
                foreach (var modifier in group)
                {
                    if (modifier.Stacks == false)
                    {
                        if (modifier.Value > max)
                        {
                            max = modifier.Value;
                        }
                    }
                    else
                    {
                        sum += modifier.Value;
                    }

                    StatModifierValue +=
                        group.First().ApplyModifier(StatBaseValue + StatModifierValue, sum > max ? sum : max);
                }
            }
            TriggerValueChange();
        }

        protected void TriggerValueChange() => OnValueChange?.Invoke(this, null);

        private void OnModifierValueChange(object sender, EventArgs e) => UpdateModifiers();
    }
}