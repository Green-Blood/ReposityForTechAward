using System;
using Cysharp.Threading.Tasks;
using Game.Core.Factories.Interfaces;
using Game.Core.Interfaces;
using Game.Core.StaticData;
using Game.GamePlay.Character.Base.Health.Interfaces;
using Game.GamePlay.Character.Other.Interfaces;
using Game.Object_Pooler.Interfaces;
using Systems.StatSystem;
using Systems.StatSystem.Interfaces;
using UnityEngine;

namespace Game.GamePlay.Character.Hero
{
    public abstract class CharacterBootstrap : MonoBehaviour, IShared, IPooledObject, IStoppable
    {
        [SerializeField] protected float rotationSpeed = 25f;
        public virtual string UnitName { get; protected set; }
        public virtual GameObject UnitObject { get; protected set; }
        public virtual IDie UnitDeath { get; protected set; }
        public IDamageable UnitHealth { get; protected set; }
        public IStatCollection StatCollection { get; protected set; }
        public ICharacterView CharacterView { get; protected set; }
        public UnitStaticData UnitStaticData { get; protected set; }
        public virtual StatDescription[] StatDescriptions { get; protected set; }

        protected IDisposable UpdateObservable;
        protected bool CanUpdate;

        protected virtual async UniTask CharacterUpdate()
        {
            await UniTask.CompletedTask;
        }

        public virtual void OnObjectSpawn() => CanUpdate = true;
        public virtual void OnObjectDeSpawn() => CanUpdate = false;
        protected virtual void OnDestroy() => UpdateObservable?.Dispose();
        public virtual void Stop() => CanUpdate = false;
        public virtual void Pause() => CanUpdate = false;
        public virtual void Resume() => CanUpdate = true;
        public virtual void Reset() { }
    }
}