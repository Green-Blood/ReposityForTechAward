using Extensions.Console.Interfaces;
using Extensions.Enums.Types;
using Extensions.ExtensionMethods;
using Extensions.UnityUtils.Scripts;
using Game.Core.Factories.Interfaces;
using Game.Core.StaticData;
using Game.GamePlay.Character.Base.Attack.Damage;
using Game.GamePlay.Character.Base.Attack.Interfaces;
using Game.GamePlay.Character.Hero.Interfaces;
using Game.GamePlay.Character.Other.Interfaces;
using Game.GamePlay.Projectiles;
using Game.Object_Pooler.Interfaces;
using Systems.StatSystem.StatTypes;
using UnityEngine;

namespace Game.GamePlay.Character.Base.Attack
{
    public class RangedHitChecker : IHitChecker
    {
        private readonly IDistanceChecker _distanceChecker;
        private readonly IDebugLogger _logger;
        private readonly IObjectPooler _objectPooler;
        private readonly IShared _shared;

        private readonly LayerMask _layerMask;
        private readonly Transform _startTransform;
        private readonly Collider[] _hits;
        private readonly DamageType _ordinaryAttackType;

        private readonly int _maxHitQuantity;
        private readonly float _cleavage;
        private readonly float _effectiveDistance;

        private Vector3 _launchPointPosition;
        private IShared _target;

        public RangedHitChecker(
            IDistanceChecker distanceChecker,
            IAnimatorView animatorView,
            IShared shared,
            IObjectPooler objectPooler,
            IDebugLogger logger
        )
        {
            _distanceChecker = distanceChecker;
            _objectPooler = objectPooler;
            _logger = logger;
            _shared = shared;

            animatorView.AnimationEventReceiver.OnAnimationEvent += OnAnimationEventReceived;
            _launchPointPosition = _shared.CharacterView.LaunchPoint.position;
        }

        private async void OnAnimationEventReceived(AnimationEventType eventType)
        {
            if(eventType != AnimationEventType.OnAttack) return;

            _target = _distanceChecker.GetCurrentClosestTargetShared();
            if(_target == null) return;


            if(DetermineProjectileData(projectileTag: out var projectileTag, attackData: out var attackData)) return;


            var targetPosition = _target.CharacterView.ImpactObserver.transform.position;
            _launchPointPosition = _shared.CharacterView.LaunchPoint.position;

            var projectileObject = _objectPooler.SpawnFromPool(projectileTag, _launchPointPosition, Quaternion.identity).GetComponent<AProjectile>();
            projectileObject.Initialize(_shared, attackData.LayerMask);
            await projectileObject.Launch(_launchPointPosition, targetPosition);

            DealDamage(attackData);
        }

        private bool DetermineProjectileData(out Tag projectileTag, out AttackData attackData)
        {
            switch (_shared.UnitStaticData)
            {
                case HeroStaticData { IsRanged: true } hero:
                    projectileTag = hero.ProjectileTag;
                    attackData = hero.AttackData;
                    break;
                case EnemyStaticData { IsRanged: true } enemy:
                    projectileTag = enemy.ProjectileTag;
                    attackData = enemy.attackData;

                    break;
                default:
                    projectileTag = null;
                    attackData = null;
                    break;
            }


            if(projectileTag is not null) return false;
            _logger.LogError($"Projectile tag on {_shared.UnitName} is null");
            return true;
        }

        private void DealDamage(AttackData attackData)
        {
            var targetObserver = _target.CharacterView.CollisionObserver;
            targetObserver.Do(x => x.HitCollisionWith(
                                  new HitData
                                  {
                                      DamageType = attackData.HitType,
                                      // TODO ADD PROJECTILE"S OWN DAMAGE!
                                      DamageDealt = _shared.StatCollection.CreateOrGetStat<Attribute>(StatType.Damage).StatValue
                                  },
                                  _shared));
        }

        public void Hit(out Collider[] colliders)
        {
            colliders = new Collider[]
                { };
        }
    }
}