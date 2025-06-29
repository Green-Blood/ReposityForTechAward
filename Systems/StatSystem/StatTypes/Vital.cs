using System;
using Systems.StatSystem.Interfaces;

namespace Systems.StatSystem.StatTypes
{
    [Serializable]
    public class Vital : Attribute, IStatCurrentValueChange
    {
        private float _statCurrentValue;
        public event EventHandler OnCurrentValueChange;
        public float StatCurrentValue
        {
            get
            {
                if (_statCurrentValue > StatValue) _statCurrentValue = StatValue;
                else if(_statCurrentValue < 0) _statCurrentValue = 0;
                return _statCurrentValue;
            }
            set
            {
                if (Math.Abs(_statCurrentValue - value) < 0.01f) return;
                _statCurrentValue = value;
                TriggerCurrentValueChange();

            }
        }

        public void SetCurrentValueToMax() => StatCurrentValue = StatValue;

        private void TriggerCurrentValueChange() => OnCurrentValueChange?.Invoke(this, EventArgs.Empty);
       
    }
}