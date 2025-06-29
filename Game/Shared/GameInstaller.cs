using System.Linq;
using System.Reflection;
using Extensions.Enums.Types;
using Game.Core.AbilitySystem.Interfaces;
using Game.Core.Factories;
using Game.Core.Factories.Interfaces;
using Game.Core.Interfaces;
using Game.Core.Providers;
using Game.Core.Services;
using Game.Core.StaticData;
using Game.GamePlay.Castle;
using Game.GamePlay.Character.Enemy;
using Game.GamePlay.Character.Hero;
using Game.GamePlay.Character.Hero.Interfaces;
using Game.GamePlay.Projectiles;
using Game.GamePlay.WaveSystem;
using Game.GamePlay.WaveSystem.Interfaces;
using Game.Installers;
using Game.Object_Pooler;
using Game.Object_Pooler.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.Shared
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private ObjectPooler objectPooler;
        [SerializeField] private SpawnPoints spawnPoints;
        [SerializeField] private SpawnPoints heroSpawnPoints;
        [SerializeField] private ParentsProvider parentsProvider;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CameraProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<ParentsProvider>().FromInstance(parentsProvider).AsSingle();
            Container.BindInterfacesAndSelfTo<StoppableEntityService>().AsSingle();
            Container.BindInterfacesAndSelfTo<RestartService>().AsSingle();

            Container.Bind<IObjectPooler>().FromInstance(objectPooler).AsSingle();
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
            Container.Bind<ILevelService>().To<LevelService>().AsSingle();

            // UI 
            // Container.Bind<IUIFactory>().To<GameUIFactory>().AsSingle();
            // Container.Bind<IUIService>().To<UIService>().AsSingle();
            // Container.Bind<IUIStateService>().To<UIStateService>().AsSingle();
            // Container.Bind<IBinderService>().To<BinderService>().AsSingle().NonLazy();
            // Container.BindInterfacesAndSelfTo<UIRegistrar>().AsSingle();
            
            // Bind stats 
            Container.Bind<IStatFactory>().To<DefaultStatFactory>().AsSingle();
            Container.Bind<IStatStorage>().To<ConcurrentStatStorage>().AsSingle();
            Container.Bind<IStatProvider>().To<StatCollectionProvider>().AsSingle();

            Container.Bind<IUnitFactory>().To<UnitFactory>().AsSingle();
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectileBehaviorFactory>().AsSingle();

            Container.Bind<IWaveSpawner>().To<EnemyWaveSpawner>().AsSingle().WithArguments(spawnPoints.GetSpawnPoints);
            Container.Bind<IHeroSpawner>().To<HeroSpawner>().AsSingle().WithArguments(heroSpawnPoints.GetSpawnPoints).NonLazy();

            BindMeleeHeroFactory();
            BindMeleeEnemyFactory();
            
            BindRangedEnemyFactory();
            BindRangedHeroFactory();
            
            BindCastleFactory();
            RegisterAllAbilityEffects();

            Container.Bind<IObjectRegistrar>().WithId(RegistrarTypes.Enemy).To<EnemyRegistrar>().AsSingle();
            Container.Bind<IObjectRegistrar>().WithId(RegistrarTypes.VFX).To<VFXRegistrar>().AsSingle();
            Container.Bind<IObjectRegistrar>().WithId(RegistrarTypes.CharacterProjectile).To<CharacterProjectileRegistar>().AsSingle();
            // TODO Add hero object registrar here when it is ready
            Container.Bind<IInitializable>().To<GameInitializer>().AsSingle();
        }

        private void RegisterAllAbilityEffects()
        {
            var effectTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IAbilityEffect).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var effectType in effectTypes)
            {
                Container.Bind(effectType).AsTransient();
            }
        }

        private void BindCastleFactory() =>
            Container.BindFactory<CastleStaticData, GameObject, CastleBootstrap, CastleBootstrap.Factory>()
                .FromSubContainerResolve()
                .ByInstaller<CastleInstaller>()
                .AsSingle();

        private void BindMeleeEnemyFactory() =>
            Container.BindFactory<EnemyStaticData, GameObject, EnemyBootstrap, EnemyBootstrap.MeleeFactory>()
                .FromSubContainerResolve()
                .ByInstaller<EnemyMeleeInstaller>()
                .AsSingle();
        private void BindRangedEnemyFactory() =>
            Container.BindFactory<EnemyStaticData, GameObject, EnemyBootstrap, EnemyBootstrap.RangedFactory>()
                .FromSubContainerResolve()
                .ByInstaller<EnemyRangedInstaller>()
                .AsSingle();

        private void BindMeleeHeroFactory() =>
            Container.BindFactory<HeroStaticData, GameObject, HeroBootstrap, HeroBootstrap.MeleeFactory>()
                .FromSubContainerResolve()
                .ByInstaller<MeleeHeroInstaller>()
                .AsSingle();
        private void BindRangedHeroFactory() =>
            Container.BindFactory<HeroStaticData, GameObject, HeroBootstrap, HeroBootstrap.RangedFactory>()
                .FromSubContainerResolve()
                .ByInstaller<RangedHeroInstaller>()
                .AsSingle();
    }
}