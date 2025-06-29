using System;
using Cysharp.Threading.Tasks;
using Extensions.Console.Interfaces;
using Extensions.ExtensionMethods;
using Game.Core.Commands.Interfaces;
using Game.Core.StaticData;
using Game.Core.StaticData.Interfaces;
using Game.GamePlay.Character.Base.Attack.Interfaces;
using Game.GamePlay.Character.Base.Health.Interfaces;
using Game.GamePlay.Character.Base.Movement.Interfaces;
using Game.GamePlay.Character.Commands;
using Game.GamePlay.Character.Hero.Interfaces;
using Game.GamePlay.Character.Other.Interfaces;
using Systems.Interfaces;
using Systems.StatSystem.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.GamePlay.Character.Hero
{
    public partial class HeroBootstrap : CharacterBootstrap
    {
        private IDistanceChecker _heroDistanceChecker;
        private IMovable _heroMovement;
        private IRotate _heroRotation;
        private IMovableAnimator _movableAnimator;
        private ICommandInvoker _commandInvoker;
        private IAttack _heroAttack;
        private IDebugLogger _logger;

        private Transform _currentTarget;

        [Inject]
        public void Construct(
            IDistanceChecker distanceChecker,
            IAttack attack,
            IMovable heroMovement,
            IRotate heroRotation,
            IDie heroDeath,
            IMovableAnimator movableAnimator,
            IPrefabData unitPrefabData,
            ICommandInvoker commandInvoker,
            IStatCollectionData statCollectionData,
            ICharacterView characterView,
            IDamageable unitHealth,
            IStatCollection statCollection,
            HeroStaticData heroStaticData,
            IRevivableInTime revivableInTime,
            IDebugLogger logger
        )
        {
            UnitName = unitPrefabData.Name;
            UnitObject = gameObject;
            StatDescriptions = statCollectionData.StatDescriptions;
            StatCollection = statCollection;
            CharacterView = characterView;
            UnitDeath = heroDeath;
            UnitHealth = unitHealth;
            UnitStaticData = heroStaticData;

            _heroDistanceChecker = distanceChecker;
            _heroAttack = attack;
            _heroMovement = heroMovement;
            _heroRotation = heroRotation;
            _movableAnimator = movableAnimator;
            _commandInvoker = commandInvoker;
            _logger = logger;

            characterView.Reactions?.ForEach(x => x.Register(this));
            revivableInTime.OnReviveFinished.Subscribe(OnReviveFinished).AddTo(this);

            UpdateObservable = Observable.Interval(TimeSpan.FromSeconds(0.1f))
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

            var currentTargetPosition = _currentTarget == null ? _heroMovement.InitialPosition : _currentTarget.position;

            _commandInvoker.AddCommand(new HeroAttackCommand(_heroAttack, _heroDistanceChecker.IsAnybodyAround, _heroMovement));
            _commandInvoker.AddCommand(new HeroRotateCommand(_heroRotation, currentTargetPosition, rotationSpeed, _heroDistanceChecker.IsAnybodyAround));
            _commandInvoker.AddCommand(new MoveCommand(_heroMovement, currentTargetPosition.ToFloat3()));
            // _commandInvoker.AddCommand(new AbilityCommand());

            await _commandInvoker.ExecuteCommands();
        }

        public override void Reset()
        {
            base.Reset();
            _commandInvoker.ClearCommands();
            _commandInvoker.AddCommand(new ResetPositionCommand(_heroMovement, _heroRotation));
            _heroDistanceChecker.Reset();
            Revive();
            CheckTarget();
        }

        private void OnReviveFinished(Unit _)
        {
            _commandInvoker.ClearCommands();
            Revive();
            CheckTarget();
        }

        private void Revive()
        {
            _commandInvoker.AddCommand(new ReviveCommand(UnitDeath, _commandInvoker, StatCollection, _logger));
            _commandInvoker.ExecuteCommands().Forget();
        }

        private void CheckTarget()
        {
            var target = _heroDistanceChecker.GetCurrentClosestTargetTransform();

            if(target == _currentTarget) return;
            _currentTarget = target;
            _heroMovement.AllowMoving();
        }
    }
}