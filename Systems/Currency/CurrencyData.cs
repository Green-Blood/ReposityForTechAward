using System;

namespace Systems.Currency
{
    [Serializable]
    public class CurrencyData
    {
        public float Current;
        public float? Max;  
        public CurrencyData(float current, float? max = null)
        {
            Current = current;
            Max = max;
        }
    }
}