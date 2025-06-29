namespace Systems.Currency
{
    public class Currency
    {
        public float Current { get; set; }
        public float? Max { get; set; } // Nullable max value

        public Currency(float current, float? max = null)
        {
            Current = current;
            Max = max;
        }

        public bool HasMax => Max.HasValue;
    }
}