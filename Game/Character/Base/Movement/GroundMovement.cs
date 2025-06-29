using System;
using Extensions.ExtensionMethods;
using Game.GamePlay.Character.Base.Health.Interfaces;
using Game.GamePlay.Character.Base.Movement.Interfaces;
using Game.GamePlay.Character.Other.Interfaces;
using ProjectDawn.Navigation.Hybrid;
using UniRx;
using Unity.Mathematics;
using UnityEngine;

namespace Game.GamePlay.Character.Base.Movement
{
    public class GroundMovement : IMovable, IDisposable
    {
        private readonly AgentAuthoring _characterAgent;
        private readonly Transform _characterTransform;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public Vector3 InitialPosition { get; }
        public bool CanMove { get; private set; } = true;

        public GroundMovement(ICharacterView characterView, IDie die)
        {
            _characterAgent = characterView.Agent;
            _characterTransform = characterView.TransformView;
            InitialPosition = characterView.TransformView.position.ToFloat3();
            die.IsDead.Subscribe(OnDead).AddTo(_disposable);
        }

        public void MoveTo(float3 destination)
        {
            if(!CanMove) return;
            if(HasNoDestination(destination)) _characterAgent.SetDestination(destination);
        }

        public void AllowMoving()
        {
            if(CanMove) return;
            CanMove = true;
        }

        public void ForbidMoving()
        {
            if(!CanMove) return;
            _characterAgent.EntityBody.Stop();
            CanMove = false;
        }

        public void ResetPosition()
        {
            _characterTransform.position = InitialPosition;
        }

        private void OnDead(bool value)
        {
            if(!value) ForbidMoving();
        }

        private bool HasNoDestination(float3 destination) =>
            !(destination - _characterAgent.EntityBody.Destination).Equals(float3.zero);

        public void Dispose() => _disposable?.Dispose();
    }
}