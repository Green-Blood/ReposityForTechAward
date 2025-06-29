namespace Systems.StatSystem.StatTypes
{
    public class StatLinkerBasic : StatLinker
    {
        private float _ratio;

        public override int Value => (int)(Stat.StatValue * _ratio);

        public StatLinkerBasic(Stat stat, float ratio) : base(stat)
        {
            _ratio = ratio;
        }
    }
}