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
using Game.GamePlay.Character.Hero;
using Game.GamePlay.Character.Hero.Interfaces;
using Game.GamePlay.Character.Other;
using Systems.StatSystem;
using Systems.StatSystem.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
    public class MeleeHeroInstaller : Installer<HeroStaticData, MeleeHeroInstaller>
    {
        [Inject] private GameObject _heroPrefab;
        [Inject] private HeroStaticData _heroStaticData;
        private CharacterView _characterView;

        public override void InstallBindings()
        {
            _characterView = Container.InstantiatePrefabForComponent<CharacterView>(_heroPrefab);

            Container.BindInterfacesAndSelfTo<CharacterView>().FromInstance(_characterView).AsSingle();
            Container.BindInterfacesAndSelfTo<HeroStaticData>().FromInstance(_heroStaticData).AsSingle();
            Container.Bind<IStatCollection>().To<DefaultStatCollection>().AsSingle();

            Container.Bind<HealthBar>().FromComponentInNewPrefab(_heroStaticData.HealthBarPrefabPrefab).UnderTransform(_characterView.TransformView).AsSingle().NonLazy();

            Container.Bind<IDistanceChecker>().To<HeroDistanceChecker>().AsSingle().NonLazy();
            Container.Bind<IMovable>().To<GroundMovement>().AsSingle();
            Container.Bind<IMovableAnimator>().To<MovableAnimation>().AsSingle();
            Container.Bind<IRotate>().To<GroundRotation>().AsSingle();

            Container.Bind<IDamageable>().To<UnitDamageReceiver>().AsSingle();
            Container.Bind<IHitChecker>().To<UnitHitChecker>().AsSingle().WithArguments(_heroStaticData.AttackData).NonLazy();
            Container.Bind<IDie>().To<CharacterDeath>().AsSingle();
            Container.Bind<IRevivableInTime>().To<RevivableInTimeCharacter>().AsSingle();

            Container.Bind<IAttackAnimation>().To<SwordsmanAttackAnimation>().AsSingle();
            Container.Bind<IAttackRangeCheck>().To<AttackRangeChecker>().AsSingle();
            Container.BindInterfacesAndSelfTo<AttackSpeedHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<CollisionHandler>().AsSingle().NonLazy();
            Container.Bind<IAttack>().To<MeleeAttack>().AsSingle();
            Container.Bind<ICommandInvoker>().To<CommandInvoker>().AsSingle();


            Container.Bind<HeroBootstrap>().FromNewComponentOn(_characterView.gameObject).AsSingle();
            Container.Bind<IShared>().To<HeroBootstrap>().FromResolve();
        }
    }
}