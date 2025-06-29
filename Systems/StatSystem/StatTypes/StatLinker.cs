using System;
using Systems.StatSystem.Interfaces;

namespace Systems.StatSystem.StatTypes
{
    public abstract class StatLinker : IStatValueChange
    {
        public Stat Stat { get; private set; }
        public abstract int Value { get; }
        public event EventHandler OnValueChange;

        protected StatLinker(Stat stat)
        {
            Stat = stat;
            if (Stat is IStatValueChange iValueChange)
                iValueChange.OnValueChange += OnLinkedStatValueChange;
        }

        private void OnLinkedStatValueChange(object sender, EventArgs eventArgs) =>
            OnValueChange?.Invoke(this, eventArgs);
    }
}