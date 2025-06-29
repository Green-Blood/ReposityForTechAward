using Meta.Interfaces;
using UI.Enums;
using UnityEngine;

namespace UI.Interfaces
{
    public interface IUIParentProvider : IServicePreloader
    {
        Transform GetParent(UIParentType parentType);
    }
}