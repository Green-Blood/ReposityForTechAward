using System;
using UnityEngine;

namespace Systems.StatSystem.StatTypes
{
    [Serializable]
    public abstract class Stat
    {
        [field: SerializeField, HideInInspector] public string StatName { get; set; } = "Stat";
        [field: SerializeField, HideInInspector] public virtual float StatBaseValue { get; set; } = 100;
        public virtual float StatValue => StatBaseValue;

        protected Stat()
        {
        }

        protected Stat(string statName, int statBaseValue)
        {
            StatName = statName;
            StatBaseValue = statBaseValue;
        }
    }
}