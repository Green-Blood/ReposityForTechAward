using Game.GamePlay.Character.Base.Character_UI;
using Systems.StatSystem.Interfaces;
using Systems.StatSystem.StatTypes;

namespace Game.GamePlay.Character.Base.Health
{
    public sealed class TestHealthBar : HealthBar
    {
        public void Construct(IStatCollection characterStatCollection)
        {
            Health = characterStatCollection.TryGetStat<Vital>(StatType.Health);
            Health.OnCurrentValueChange += OnHealthChangedAction;
        }
    }
}