using System;
using System.Collections.Generic;
using Systems.StatSystem.Interfaces;
using Systems.StatSystem.Modifiers;
using UnityEngine;

namespace Systems.StatSystem.StatTypes
{
    [Serializable]
    public class Attribute : StatModifiable, IStatScalable, IStatLinkable
    {
        [field: SerializeField, HideInInspector]
        public int StatLevelValue { get; set; }

        [field: SerializeField, HideInInspector]
        public int StatLinkerValue { get; set; }

        public override float StatBaseValue => base.StatBaseValue + StatLevelValue + StatLinkerValue;

        private readonly List<StatLinker> _linkers;


        public Attribute() => _linkers = new List<StatLinker>();

        public virtual void Scale(int level)
        {
            StatLevelValue = level;
            TriggerValueChange();
        }

        public void AddLinker(StatLinker linker)
        {
            _linkers.Add(linker);
            linker.OnValueChange += OnLinkerValueChange;
        }

        public void RemoveLinker(StatLinker linker)
        {
            _linkers.Remove(linker);
            linker.OnValueChange -= OnLinkerValueChange;
        }

        public void ClearLinkers()
        {
            foreach (var linker in _linkers) linker.OnValueChange -= OnLinkerValueChange;
            _linkers.Clear();
        }

        public void UpdateLinkers()
        {
            StatLinkerValue = 0;
            foreach (var linker in _linkers) StatLinkerValue += linker.Value;
            TriggerValueChange();
        }

        private void OnLinkerValueChange(object sender, EventArgs e) => UpdateLinkers();
    }
}