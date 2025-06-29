using Extensions.Enums.Types;
using Game.Core.Factories;
using Game.Core.Factories.Interfaces;
using Game.Core.Interfaces;
using Game.Core.Providers;
using Game.Core.Services;
using Game.GamePlay.Character.Hero.Interfaces;
using Game.GamePlay.WaveSystem;
using Game.GamePlay.WaveSystem.Interfaces;
using Game.Object_Pooler;
using Game.Object_Pooler.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.Shared
{
    public class MenuInstaller : MonoInstaller
    {
        [SerializeField] private ObjectPooler objectPooler;
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


            Container.Bind<IStatFactory>().To<DefaultStatFactory>().AsSingle();

            Container.Bind<IInitializable>().To<MenuInitializer>().AsSingle();
        }
    }
}