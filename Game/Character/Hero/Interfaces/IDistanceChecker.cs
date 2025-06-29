using Game.Core.Factories.Interfaces;
using UnityEngine;

namespace Game.GamePlay.Character.Hero.Interfaces
{
    public interface IDistanceChecker
    {
        bool IsAnybodyAround { get; }
        Transform GetCurrentClosestTargetTransform();
        IShared GetCurrentClosestTargetShared();
        void Reset();
    }
}