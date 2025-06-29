using Extensions.UnityUtils.Scripts;
using UnityEngine;

namespace Game.GamePlay.Character.Other.Interfaces
{
    public interface IAnimatorView
    {
        Animator Animator { get; }
        AnimationEventReceiver AnimationEventReceiver { get; }
        
    }
}