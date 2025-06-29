using System;
using System.Linq;
using Extensions.Enums.Types;
using Extensions.ExtensionMethods;
using Extensions.UnityUtils.Scripts;
using Game.Core.Factories.Interfaces;
using Game.Core.Interfaces;
using Game.Core.StaticData;
using Game.GamePlay.Character.Base.Attack.Damage;
using Game.GamePlay.Character.Base.Attack.Interfaces;
using Game.GamePlay.Character.Base.Health;
using Game.GamePlay.Character.Other.Interfaces;
using Game.GamePlay.Observers;
using Systems.StatSystem;
using Systems.StatSystem.Interfaces;
using Systems.StatSystem.StatTypes;
using UnityEngine;

namespace Game.GamePlay.Character.Base.Attack
{
    public class UnitHitChecker : IHitChecker
    {
        private readonly IShared _shared;
        private readonly float _cleavage;
        private readonly float _effectiveDistance;
        private readonly LayerMask _layerMask;
        private readonly Transform _startTransform;
        private readonly Stat _damage;
        private readonly Collider[] _hits;
        private readonly DamageType _ordinaryAttackType;
        private readonly int _maxHitQuantity;

        public UnitHitChecker(
            AttackData data,
            ITransformView characterView,
            IAnimatorView animatorView,
            IStatCollection statCollection,
            IShared shared
        )
        {
            _shared = shared;
            _cleavage = data.Cleavage;
            _effectiveDistance = data.EffectiveDistance;
            _layerMask = data.LayerMask;
            _ordinaryAttackType = data.HitType;
            _startTransform = characterView.TransformView;

            _damage = statCollection.TryGetStat<Stat>(StatType.Damage);
            animatorView.AnimationEventReceiver.OnAnimationEvent += OnAnimationEventReceived;

            _maxHitQuantity = (int)statCollection.TryGetStat<Stat>(StatType.HitQuantity).StatValue;
            _hits = new Collider[_maxHitQuantity];
        }

        private void OnAnimationEventReceived(AnimationEventType eventType)
        {
            if(eventType != AnimationEventType.OnAttack) return;

            Hit(out var hits);

            if(hits.Length <= 0)
            {
                return;
            }

            foreach (var hit in hits)
            {
                PerformDamage(hit);
            }
        }

        private void PerformDamage(Collider hitCollider)
        {
            if(!hitCollider.TryGetComponent<CollisionObserver>(out var target))
                return;

            target.Do(x => x.HitCollisionWith(new HitData
                                              {
                                                  DamageType = _ordinaryAttackType, DamageDealt = _damage.StatValue
                                              },
                                              _shared));
        }

        public void Hit(out Collider[] colliders)
        {
            Array.Clear(_hits, 0, _hits.Length);
            int hitCount = Physics.OverlapSphereNonAlloc(
                GetStartPoint(),
                _cleavage,
                _hits,
                _layerMask
            );

            colliders = ToClampedArray(_hits, hitCount, _maxHitQuantity);
        }

        private Vector3 GetStartPoint()
        {
            var pos = _startTransform.position;
            return new Vector3(pos.x, pos.y + 0.5f, pos.z) + _startTransform.forward * _effectiveDistance;
        }

        private Collider[] ToClampedArray(Collider[] source, int totalFound, int maxCount)
        {
            int finalCount = Mathf.Min(totalFound, maxCount);

            if(finalCount <= 0)
                return Array.Empty<Collider>();

            return new Collider[finalCount].With(dest =>
                                                 {
                                                     for (int i = 0; i < finalCount; i++)
                                                     {
                                                         dest[i] = source[i];
                                                     }
                                                 });
        }
    }
}