using Game.Core.Commands;
using Game.Core.Commands.Interfaces;
using Game.Core.Factories;
using Game.Core.Factories.Interfaces;
using Game.Core.StaticData;
using Game.GamePlay.AttackZone;
using Game.GamePlay.AttackZone.Interfaces;
using Game.GamePlay.Castle;
using Game.GamePlay.Character.Base.Attack;
using Game.GamePlay.Character.Base.Attack.Interfaces;
using Game.GamePlay.Character.Base.Health;
using Game.GamePlay.Character.Base.Health.Interfaces;
using Game.GamePlay.Projectiles;
using Systems.StatSystem.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
    public class CastleInstaller : Installer<CastleStaticData, CastleInstaller>
    {
        [Inject] private GameObject _castlePrefab;
        [Inject] private CastleStaticData _castleStaticData;

        private CastleView _castleView;

        public override void InstallBindings()
        {
            _castleView = Container.InstantiatePrefabForComponent<CastleView>(_castlePrefab);

            Container.BindInterfacesAndSelfTo<CastleView>().FromInstance(_castleView).AsSingle();
            Container.BindInterfacesAndSelfTo<CastleStaticData>().FromInstance(_castleStaticData).AsSingle();

            Container.BindInterfacesAndSelfTo<CastleObjectsSpawner>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CollisionHandler>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<CastleProjectileRegistrar>().AsSingle();


            Container.Bind<IAttackZoneStrategyFactory>().To<AttackZoneStrategyFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectileSelector>().AsSingle();
            Container.Bind<IAttackZoneIndicatorService>().To<AttackZoneIndicatorService>().AsSingle();
            Container.Bind<ICommandInvoker>().To<CommandInvoker>().AsSingle();

            Container.BindFactory<Catapult, Catapult.Factory>().AsSingle();

            Container.Bind<IStatCollection>().To<DefaultStatCollection>().AsSingle();
            Container.Bind<IDie>().To<CastleDeath>().AsSingle().NonLazy();
            Container.Bind<IDamageable>().To<UnitDamageReceiver>().AsSingle();
            Container.Bind<IAttackWaiter>().To<ShootSpeedHandler>().AsSingle();

            Container.Bind<CastleBootstrap>().FromNewComponentOn(_castleView.gameObject).AsSingle();
            Container.Bind<IShared>().To<CastleBootstrap>().FromResolve();
        }
    }
}