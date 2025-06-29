using System;
using System.Collections.Generic;
using System.Linq;
using Extensions.Console.Interfaces;
using Systems.StatSystem.Interfaces;
using Systems.StatSystem.Modifiers;
using Systems.StatSystem.StatTypes;

namespace Systems.StatSystem
{
    public abstract class StatCollection : IStatCollection
    {
        private readonly IDebugLogger _logger;
        private readonly Dictionary<StatType, List<Stat>> _statTypeDictionary;
        private readonly Dictionary<string, List<Stat>> _statNameDictionary;

        protected Dictionary<StatType, List<Stat>> StatsByType => _statTypeDictionary;
        protected Dictionary<string, List<Stat>> StatsByName => _statNameDictionary;

        protected StatCollection(IDebugLogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _statTypeDictionary = new Dictionary<StatType, List<Stat>>(Enum.GetValues(typeof(StatType)).Length);
            _statNameDictionary = new Dictionary<string, List<Stat>>(StringComparer.OrdinalIgnoreCase);
        }

        protected abstract void ConfigureStats();

        #region BasicStat Accessors

        public T TryGetStat<T>(StatType type) where T : Stat
        {
            var stat = GetFirstStatByType(type) as T;
            if (stat == null)
                _logger?.LogWarning($"Stat of type '{type}' not found.");
            return stat;
        }

        public T TryGetStat<T>(string statName) where T : Stat
        {
            var stat = GetFirstStatByName(statName) as T;
            if (stat == null)
                _logger?.LogError($"Stat with name '{statName}' not found.");
            return stat;
        }

        public T TryGetStat<T>(StatType type, string statName) where T : Stat
        {
            var stat = GetStatByTypeAndName(type, statName) as T;
            if (stat == null)
                _logger?.LogError($"Stat of type '{type}' with name '{statName}' not found.");
            return stat;
        }

        protected void AddOrUpdateStat(StatType type, Stat stat)
        {
            if (stat == null)
                throw new ArgumentNullException(nameof(stat));

            if (!StatsByType.ContainsKey(type))
                StatsByType[type] = new List<Stat>();

            if (!StatsByType[type].Contains(stat))
                StatsByType[type].Add(stat);
            else
                _logger?.LogWarning(
                    $"Duplicate stat detected: Type '{type}', Name '{stat.StatName}'. Ensure that your design accounts for unique combinations.");


            if (!string.IsNullOrEmpty(stat.StatName))
            {
                if (!StatsByName.ContainsKey(stat.StatName))
                    StatsByName[stat.StatName] = new List<Stat>();

                if (!StatsByName[stat.StatName].Contains(stat))
                    StatsByName[stat.StatName].Add(stat);
                else
                    _logger?.LogWarning(
                        $"Duplicate stat detected: Type '{type}', Name '{stat.StatName}'. Ensure that your design accounts for unique combinations.");
            }
        }

        /// <summary>
        /// Retrieves the first stat matching a given StatType. Useful for situations where you expect a single stat of that type.
        /// </summary>
        /// <param name="type">The StatType to search for.</param>
        /// <returns>The first matching stat, or null if none found.</returns>
        private Stat GetFirstStatByType(StatType type)
        {
            if (StatsByType.TryGetValue(type, out var stats) && stats.Count > 0)
                return stats[0];

            return null;
        }

        private Stat GetFirstStatByName(string statName)
        {
            if (string.IsNullOrEmpty(statName))
                throw new ArgumentNullException(nameof(statName));

            if (StatsByName.TryGetValue(statName, out var stats) && stats.Count > 0)
                return stats[0];

            return null;
        }

        private Stat GetStatByTypeAndName(StatType type, string statName)
        {
            if (string.IsNullOrEmpty(statName))
                throw new ArgumentNullException(nameof(statName));

            return StatsByType.TryGetValue(type, out var statsByType)
                ? statsByType.FirstOrDefault(stat => stat.StatName == statName)
                : null;
        }

        public T CreateOrGetStat<T>(StatType type) where T : Stat => (GetStat<T>(type) ?? CreateStat<T>(type)) as T;

        public Stat CreateOrGetStat(Type statType, StatType typeEnum, string statName)
        {
            // Check if the type is assignable to Stat and has a parameterless constructor
            if (!typeof(Stat).IsAssignableFrom(statType) || statType.GetConstructor(Type.EmptyTypes) == null)
                throw new ArgumentException(
                    $"The type {statType.Name} must be a subclass of Stat and have a parameterless constructor.",
                    nameof(statType));

            var existingStat = GetStatByTypeAndName(typeEnum, statName);
            if (existingStat != null)
                return existingStat;

            var newStat = (Stat)Activator.CreateInstance(statType);
            newStat.StatName = statName;
            AddOrUpdateStat(typeEnum, newStat);
            return newStat;
        }

        public List<Stat> GetAllStatsByType(StatType type) => StatsByType.GetValueOrDefault(type);

        public List<Stat> GetAllStatsByName(string statName)
        {
            if (string.IsNullOrEmpty(statName))
                throw new ArgumentNullException(nameof(statName));

            return StatsByName.GetValueOrDefault(statName);
        }

        public Stat GetStat(StatType type) =>
            Contains(type) ? GetFirstStatByType(type) : null;

        public Stat GetStat(string statName)
        {
            if (string.IsNullOrEmpty(statName))
                throw new ArgumentNullException(nameof(statName));

            return Contains(statName) ? GetFirstStatByName(statName) : null;
        }

        public T GetStat<T>(StatType type) where T : Stat => GetStat(type) as T;
        public T GetStat<T>(string statName) where T : Stat => GetStat(statName) as T;

        public T CreateStat<T>(StatType statType) where T : Stat
        {
            var stat = Activator.CreateInstance<T>();
            AddOrUpdateStat(statType, stat); // Ensures the new stat is added properly to both dictionaries.
            return stat;
        }

        public void RemoveStat(StatType type, Stat stat)
        {
            if (stat == null)
                throw new ArgumentNullException(nameof(stat));

            if (StatsByType.ContainsKey(type))
                StatsByType[type].Remove(stat);

            if (!string.IsNullOrEmpty(stat.StatName) && StatsByName.ContainsKey(stat.StatName))
                StatsByName[stat.StatName].Remove(stat);
        }

        #endregion

        #region Modifiers

        public void AddStatModifier(StatType target, StatModifier modifier, bool update = false)
        {
            var modifiedStat = IsModifiable(target);
            modifiedStat?.AddModifier(modifier);
            if (update) modifiedStat?.UpdateModifiers();
        }

        public void RemoveStatModifier(StatType target, StatModifier mod, bool update = false)
        {
            var modStat = IsModifiable(target);
            modStat?.RemoveModifier(mod);
            if (update) modStat?.UpdateModifiers();
        }

        public void ClearStatModifiers(bool update = false)
        {
            foreach (var key in StatsByType.Keys)
            {
                ClearStatModifier(key, update);
            }
        }

        public void ClearStatModifier(StatType target, bool update = false)
        {
            var modStat = IsModifiable(target);
            modStat?.ClearModifiers();
            if (update) modStat?.UpdateModifiers();
        }

        public void UpdateStatModifiers()
        {
            foreach (var key in StatsByType.Keys)
                UpdateStatModifer(key);
        }

        public void UpdateStatModifer(StatType target)
        {
            var modStat = IsModifiable(target);
            modStat?.UpdateModifiers();
        }

        private IModifiable IsModifiable(StatType target)
        {
            if (Contains(target))
            {
                return GetStat(target) as IModifiable;
            }

            _logger?.LogError($"Stat '{target}' is either missing or not modifiable.");
            return null;
        }

        #endregion

        #region Scale

        public void ScaleStatCollection(int level)
        {
            foreach (var key in StatsByType.Keys)
                ScaleStat(key, level);
        }

        public void ScaleStat(StatType target, int level)
        {
            if (Contains(target) && GetStat(target) is IStatScalable stat)
            {
                stat.Scale(level);
            }
            else
            {
                _logger?.LogError($"Stat '{target}' is either missing or not scalable.");
            }
        }

        #endregion

        public virtual string GetStatOwner(StatType type)
        {
            return "Character";
        }

        private bool Contains(StatType type) => _statTypeDictionary.ContainsKey(type);

        private bool Contains(string statName)
        {
            if (string.IsNullOrEmpty(statName))
                throw new ArgumentNullException(nameof(statName));

            return StatsByName.ContainsKey(statName);
        }

        public bool Contains(StatType type, string statName)
        {
            if (string.IsNullOrEmpty(statName))
                throw new ArgumentNullException(nameof(statName));

            return GetStatByTypeAndName(type, statName) != null;
        }
    }
}