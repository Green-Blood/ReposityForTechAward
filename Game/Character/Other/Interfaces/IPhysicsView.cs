using Game.GamePlay.Observers;
using UnityEngine;

namespace Game.GamePlay.Character.Other.Interfaces
{
    public interface IPhysicsView
    {
        Rigidbody CharacterRigidBody { get; }
        Collider Collider { get; }
        TriggerObserver TriggerObserver { get; }
        CollisionObserver CollisionObserver { get; }
        TriggerObserver ImpactObserver { get; }
        Transform LaunchPoint { get; }
    }
}