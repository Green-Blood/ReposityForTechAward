using System;
#if UNITY_EDITOR
using Extensions.Editor;
#endif
using Sirenix.OdinInspector;
using Systems.StatSystem.StatTypes;
using UnityEngine;

namespace Systems.StatSystem
{
    [Serializable]
    [ShowOdinSerializedPropertiesInInspector]
    public class StatDescription
    {
        [SerializeReference, InlineProperty] private Stat stat;

#if UNITY_EDITOR
        //TODO Make search through Name or smth like that not Type 
        [SizedFoldoutGroup("Base Info", 0.87f, 0f, 0.97f)]
#endif
        [SerializeField, InlineProperty]
        private StatType statType;
#if UNITY_EDITOR
        [SizedFoldoutGroup("Base Info", 0.87f, 0f, 0.97f)]
#endif
        [ShowInInspector, InlineProperty]
        public string StatName
        {
            get => stat != null ? stat.StatName : string.Empty;
            set
            {
                if(stat != null) stat.StatName = value;
            }
        }

#if UNITY_EDITOR
        [SizedFoldoutGroup("Base Info", 0.87f, 0f, 0.97f)]
#endif
        [Tooltip("The base value before modifiers are applied.")]
        [ShowInInspector, InlineProperty]
        public float StatBaseValue
        {
            get => stat?.StatBaseValue ?? 0;
            set
            {
                if(stat != null) stat.StatBaseValue = value;
            }
        }
#if UNITY_EDITOR
        [SizedFoldoutGroup("Base Info", 0.87f, 0f, 0.97f)]
#endif
        [Title("Special Attributes"), SerializeField]
        private bool hasAttributes;
#if UNITY_EDITOR
        // TODO Add linkers implementation
        [SizedFoldoutGroup("Base Info", 0.87f, 0f, 0.97f)]
#endif
        [Tooltip("They're not added yet, should add them later.")]
        [SerializeField]
        private bool hasLinkers;
#if UNITY_EDITOR
        // Conditional property for Attribute-specific property
        [SizedFoldoutGroup("Base Info", 0.87f, 0f, 0.97f)]
#endif
        [Title("Special Attributes"), ShowInInspector, InlineProperty, ShowIf("IsAttributeTypeAndHasSpecialAttributes")]
        public int StatLevelValue
        {
            get => (stat as StatTypes.Attribute)?.StatLevelValue ?? 0;
            set
            {
                if(stat is StatTypes.Attribute attribute) attribute.StatLevelValue = value;
            }
        }

        public Stat Stat => stat;
        public StatType StatType => statType;


        public bool IsAttributeTypeAndHasSpecialAttributes() => stat is StatTypes.Attribute && hasAttributes;
        public bool IsAttributeTypeAndHasLinker() => stat is StatTypes.Attribute && hasLinkers;
    }
}