using System;

namespace Systems.StatSystem.Modifiers
{
    public abstract class StatModifier
    {
        private float _value = 0;
        public event EventHandler OnValueChange;
        public abstract int Order { get; }
        public float Value
        {
            get => _value;
            set
            {
                if (Math.Abs(_value - value) < 0.01) return;
                _value = value;
                OnValueChange?.Invoke(this, null);
            }
        }
        public bool Stacks { get; set; }

        public StatModifier()
        {
            Value = 0;
            Stacks = true;
        }

        public StatModifier(float value)
        {
            Value = value;
            Stacks = true;
        }
        public StatModifier(float value, bool stacks)
        {
            Value = value;
            Stacks = stacks;
        }

        public abstract float ApplyModifier(float statValue, float modValue);
    }
}