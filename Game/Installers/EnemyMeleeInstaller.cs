using Game.Core.Commands;
using Game.Core.Commands.Interfaces;
using Game.Core.Factories;
using Game.Core.Factories.Interfaces;
using Game.Core.StaticData;
using Game.GamePlay.Character.Base.Attack;
using Game.GamePlay.Character.Base.Attack.Interfaces;
using Game.GamePlay.Character.Base.Character_UI;
using Game.GamePlay.Character.Base.Health;
using Game.GamePlay.Character.Base.Health.Interfaces;
using Game.GamePlay.Character.Base.Movement;
using Game.GamePlay.Character.Base.Movement.Interfaces;
using Game.GamePlay.Character.Enemy;
using Game.GamePlay.Character.Hero.Interfaces;
using Game.GamePlay.Character.Other;
using Game.GamePlay.Character.Other.Interfaces;
using Game.GamePlay.Currency;
using Game.GamePlay.Currency.Interfaces;
using Systems.StatSystem;
using Systems.StatSystem.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
    public class EnemyMeleeInstaller : Installer<EnemyStaticData, EnemyMeleeInstaller>
    {
        [Inject] private GameObject _enemyPrefab;
        [Inject] private EnemyStaticData _enemyStaticData;
        private CharacterView _enemyView;

        public override void InstallBindings()
        {
            _enemyView = Container.InstantiatePrefabForComponent<CharacterView>(_enemyPrefab);

            Container.BindInterfacesAndSelfTo<CharacterView>().FromInstance(_enemyView).AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyStaticData>().FromInstance(_enemyStaticData).AsSingle();
            Container.Bind<HealthBar>().FromComponentInNewPrefab(_enemyStaticData.HealthBarPrefab).UnderTransform(_enemyView.TransformView).AsSingle().NonLazy();
            Container.Bind<IStatCollection>().To<DefaultStatCollection>().AsSingle();

            Container.Bind<IDistanceChecker>().To<EnemyDistanceChecker>().AsSingle();
            Container.Bind<IMovable>().To<GroundMovement>().AsSingle();
            Container.Bind<IMovableAnimator>().To<MovableAnimation>().AsSingle();
            Container.Bind<IRotate>().To<GroundRotation>().AsSingle();

            Container.Bind<IDamageable>().To<UnitDamageReceiver>().AsSingle();
            Container.Bind<IHitChecker>().To<UnitHitChecker>().AsSingle().WithArguments(_enemyStaticData.AttackData).NonLazy();
            Container.Bind<IDie>().To<CharacterDeath>().AsSingle();

            Container.Bind<IAttackAnimation>().To<MeleeAttackAnimation>().AsSingle();
            Container.Bind<IAttackRangeCheck>().To<AttackRangeChecker>().AsSingle();
            Container.BindInterfacesAndSelfTo<AttackSpeedHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<CollisionHandler>().AsSingle().NonLazy();
            Container.Bind<IAttack>().To<MeleeAttack>().AsSingle();
            Container.Bind<ICommandInvoker>().To<CommandInvoker>().AsSingle();
            Container.Bind<ICurrencyDrop>().To<EnemyCurrencyDrop>().AsSingle().NonLazy();


            Container.Bind<EnemyBootstrap>().FromNewComponentOn(_enemyView.gameObject).AsSingle().NonLazy();
            Container.Bind<IShared>().To<EnemyBootstrap>().FromResolve();
        }
    }
}