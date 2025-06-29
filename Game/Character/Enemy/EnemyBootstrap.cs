using Cysharp.Threading.Tasks;
using Extensions.ExtensionMethods;
using Game.Core.Commands.Interfaces;
using Game.Core.StaticData;
using Game.GamePlay.Character.Base.Attack.Interfaces;
using Game.GamePlay.Character.Base.Health.Interfaces;
using Game.GamePlay.Character.Base.Movement.Interfaces;
using Game.GamePlay.Character.Commands;
using Game.GamePlay.Character.Hero;
using Game.GamePlay.Character.Hero.Interfaces;
using Game.GamePlay.Character.Other.Interfaces;
using Systems.Interfaces;
using Systems.StatSystem.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.GamePlay.Character.Enemy
{
    public partial class EnemyBootstrap : CharacterBootstrap
    {
        private IMovable _enemyMovement;
        private IAttack _enemyAttack;
        private IDistanceChecker _distanceChecker;
        private IRotate _enemyRotation;
        private IMovableAnimator _movableAnimator;
        private ICommandInvoker _commandInvoker;

        private Transform _currentTarget;

        [Inject]
        public void Construct(IDie enemyDeath, IMovable enemyMovement, IAttack enemyAttack,
            IDistanceChecker characterCount, IRotate enemyRotation, IMovableAnimator movableAnimator,
            IPrefabData unitPrefabData, ICommandInvoker commandInvoker, ICharacterView characterView,
            IDamageable unitHealth, IStatCollection statCollection, EnemyStaticData enemyStaticData)
        {
            UnitDeath = enemyDeath;
            UnitObject = gameObject;
            UnitName = unitPrefabData.Name;
            CharacterView = characterView;
            UnitHealth = unitHealth;
            StatCollection = statCollection;
            UnitStaticData = enemyStaticData;

            _enemyMovement = enemyMovement;
            _enemyAttack = enemyAttack;
            _distanceChecker = characterCount;
            _enemyRotation = enemyRotation;
            _movableAnimator = movableAnimator;
            _commandInvoker = commandInvoker;


            characterView.Reactions?.ForEach(x => x.Register(this));

            UpdateObservable =
                Observable.Interval(System.TimeSpan.FromSeconds(0.1f))
                    .Where(_ => CanUpdate && UnitObject.activeSelf && !UnitDeath.IsDead.Value)
                    .SelectMany(_ => CharacterUpdate().ToObservable())
                    .Subscribe(_ =>
                               {
                                   // Additional logic if needed after CharacterUpdateAsync completes
                               });
        }

        protected override async UniTask CharacterUpdate()
        {
            await base.CharacterUpdate();

            _movableAnimator.SetMovableAnimatorSpeed();

            CheckTarget();

            _commandInvoker.AddCommand(new MoveCommand(_enemyMovement, _currentTarget.position.ToFloat3()));
            _commandInvoker.AddCommand(new EnemyRotateCommand(_enemyRotation, _currentTarget.position, rotationSpeed));
            _commandInvoker.AddCommand(new EnemyAttackCommand(_enemyAttack, _enemyMovement));
            // _commandInvoker.AddCommand(new TargetAbilityCommand());

            await _commandInvoker.ExecuteCommands();
        }

        private void CheckTarget()
        {
            var target = _distanceChecker.GetCurrentClosestTargetTransform();

            if(target == null)
            {
                Debug.LogWarning($"[{UnitName}] No valid target found. Stopping enemy.");
                _enemyMovement.ForbidMoving();
                return;
            }

            if(target == _currentTarget) return;
            _currentTarget = target;
            _enemyMovement.AllowMoving();
        }

        public override void Stop()
        {
            base.Stop();
            _enemyMovement.ForbidMoving();
            _movableAnimator.SetMovableAnimatorSpeed();
        }

        public override void Pause()
        {
            base.Pause();
            _enemyMovement.ForbidMoving();
            _movableAnimator.SetMovableAnimatorSpeed();
        }

        public override void Reset()
        {
            base.Reset();
            _enemyMovement.ForbidMoving();
            _commandInvoker.ClearCommands();
            _commandInvoker.AddCommand(new HealToMaxCommand(StatCollection));
            _commandInvoker.ExecuteCommands().Forget();
            CheckTarget();
            _enemyMovement.AllowMoving();
            _movableAnimator.SetMovableAnimatorSpeed();
        }

        public override void OnObjectSpawn()
        {
            base.OnObjectSpawn();
            _enemyMovement.ForbidMoving();
            UnitDeath.Init();
            _commandInvoker.ClearCommands();
            _commandInvoker.AddCommand(new HealToMaxCommand(StatCollection));
            _commandInvoker.ExecuteCommands().Forget();
            CheckTarget();
            _enemyMovement.AllowMoving();
            _movableAnimator.SetMovableAnimatorSpeed();
        }

        public override void Resume()
        {
            base.Resume();
            _enemyMovement.AllowMoving();
            _movableAnimator.SetMovableAnimatorSpeed();
        }
    }
}