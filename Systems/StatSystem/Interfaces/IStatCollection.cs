using System.Collections.Generic;
using Systems.StatSystem.Modifiers;
using Systems.StatSystem.StatTypes;

namespace Systems.StatSystem.Interfaces
{
    public interface IStatCollection
    {
        T GetStat<T>(StatType type) where T : Stat;
        T GetStat<T>(string statName) where T : Stat;
        T TryGetStat<T>(StatType type) where T : Stat;
        T TryGetStat<T>(string statName) where T : Stat;
        T TryGetStat<T>(StatType type, string statName) where T : Stat;
        T CreateOrGetStat<T>(StatType type) where T : Stat;
        List<Stat> GetAllStatsByType(StatType type);
        List<Stat> GetAllStatsByName(string statName);
        T CreateStat<T>(StatType statType) where T : Stat;
        void RemoveStat(StatType type, Stat stat);
        void AddStatModifier(StatType target, StatModifier modifier, bool update = false);
        void RemoveStatModifier(StatType target, StatModifier mod, bool update = false);
        public void ClearStatModifiers(bool update = false);
        public void ClearStatModifier(StatType target, bool update = false);
        public void UpdateStatModifiers();
        public void UpdateStatModifer(StatType target);
        public void ScaleStatCollection(int level);
        public void ScaleStat(StatType target, int level);
        public string GetStatOwner(StatType type);
    }
}