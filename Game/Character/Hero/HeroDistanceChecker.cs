using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core.Factories.Interfaces;
using Game.Core.Interfaces;
using Game.GamePlay.Character.Enemy;
using Game.GamePlay.Character.Hero.Interfaces;
using Game.GamePlay.Character.Other.Interfaces;
using Game.GamePlay.Observers;
using UniRx;
using UnityEngine;

namespace Game.GamePlay.Character.Hero
{
    public class HeroDistanceChecker : IDisposable, IDistanceChecker
    {
        private readonly TriggerObserver _triggerObserver;
        private readonly HashSet<EnemyBootstrap> _enemies;
        public bool IsAnybodyAround { get; private set; }

        public HeroDistanceChecker(ICharacterView characterView)
        {
            _triggerObserver = characterView.TriggerObserver;

            _enemies = new HashSet<EnemyBootstrap>();

            _triggerObserver.TriggerEnter += OnTriggerEnter;
            _triggerObserver.TriggerExit += OnTriggerExit;
        }

        public Transform GetCurrentClosestTargetTransform() =>
            IsEnemyStillExist() ? _enemies.First().transform : null;

        public IShared GetCurrentClosestTargetShared() => IsEnemyStillExist() ? _enemies.First() : null;
        public void Reset()
        {
            _enemies.Clear();
            IsAnybodyAround = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!other.TryGetComponent<EnemyBootstrap>(out var enemy)) return;

            _enemies.Add(enemy);
            IsAnybodyAround = true;

            enemy.UnitDeath.IsDead.Subscribe(isDead => RemoveFromEnemyList(enemy, isDead));
        }

        private void OnTriggerExit(Collider other)
        {
            if(!other.TryGetComponent<EnemyBootstrap>(out var enemy)) return;

            RemoveFromEnemyList(enemy);
        }

        private void RemoveFromEnemyList(EnemyBootstrap enemy)
        {
            if(!_enemies.Contains(enemy)) return;
            _enemies.Remove(enemy);

            if(!IsEnemyStillExist()) IsAnybodyAround = false;
        }

        private void RemoveFromEnemyList(EnemyBootstrap enemy, bool isDead)
        {
            if(!_enemies.Contains(enemy)) return;
            if(!isDead) return;
            _enemies.Remove(enemy);

            if(!IsEnemyStillExist()) IsAnybodyAround = false;
        }

        private bool IsEnemyStillExist() => _enemies.Count != 0;

        public void Dispose()
        {
            _triggerObserver.TriggerEnter -= OnTriggerEnter;
            _triggerObserver.TriggerExit -= OnTriggerExit;
        }
    }
}