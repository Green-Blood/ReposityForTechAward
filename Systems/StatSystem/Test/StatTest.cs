using System;
using Systems.StatSystem.StatCollections;
using Systems.StatSystem.StatTypes;
using UnityEngine;

namespace Systems.StatSystem.Test
{
    public class StatTest : MonoBehaviour
    {
        private StatCollection _statCollection;
        private void Start()
        {
            _statCollection = new DefaultStats(null);
            var health = _statCollection.TryGetStat<Vital>(StatType.Health);
            health.OnValueChange += OnStatValueChange;
            
            DisplayStatValues();
            
            // _statCollection.GetStat<Attribute>(StatType.Vitality).Scale(15);
            DisplayStatValues();
        }

        private void OnStatValueChange(object sender, EventArgs e)
        {
            var changedStat = (Vital)sender;
            if (changedStat == null) return;
            Debug.Log($"Vital {changedStat.StatName}'s OnStatValueChange event triggered");
            changedStat.StatCurrentValue = changedStat.StatValue;
        }


        private void ForEachEnum<T>(Action<T> action)
        {
            if (action == null) return;
            var statTypes = Enum.GetValues(typeof(T));
            foreach (var statType in statTypes)
            {
                action((T)statType);
            }
        }
 
        private void DisplayStatValues() {
            ForEachEnum<StatType>((statType) => {
                var stat = _statCollection.GetStat(statType);
                if (stat != null) {
                    Debug.Log($"Stat {stat.StatName}'s value is {stat.StatValue}");
                }
            });
        }
    }
}