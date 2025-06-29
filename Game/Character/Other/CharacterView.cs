using System.Collections.Generic;
using Extensions.UnityUtils.Scripts;
using Game.GamePlay.Character.Base.Health.Interfaces;
using Game.GamePlay.Character.Other.Interfaces;
using Game.GamePlay.Observers;
using ProjectDawn.Navigation.Hybrid;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game.GamePlay.Character.Other
{
    public class CharacterView : SerializedMonoBehaviour, ICharacterView
    {
        [SerializeField] private Animator animator;
        [SerializeField] private AgentAuthoring agent;
        [SerializeField] private Collider characterCollider;
        [SerializeField] private Rigidbody characterRigidBody;
        [SerializeField] private TriggerObserver heroDistanceTriggerObserver;
        [SerializeField] private AnimationEventReceiver animationEventReceiver;
        [SerializeField] private CollisionObserver collisionObserver;
        [SerializeField] private TriggerObserver impactObserver;
        [SerializeField] private Transform launchPoint;
        [OdinSerialize] private List<IReact> reactions;
        
        public AgentAuthoring Agent => agent;
        public Animator Animator => animator;
        public Transform TransformView => transform;
        public Rigidbody CharacterRigidBody => characterRigidBody;
        public Collider Collider => characterCollider; 
        public TriggerObserver TriggerObserver => heroDistanceTriggerObserver;
        public AnimationEventReceiver AnimationEventReceiver => animationEventReceiver;
        public CollisionObserver CollisionObserver => collisionObserver;
        public TriggerObserver ImpactObserver => impactObserver;
        public Transform LaunchPoint => launchPoint;
        public List<IReact> Reactions
        {
            get => reactions;
            set => reactions = value;
        }
    }
}