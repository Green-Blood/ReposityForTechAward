using Extensions.Console;
using Extensions.Console.Interfaces;
using Extensions.Enums.Types;
using Game.Core.Interfaces;
using Game.Core.Loaders;
using Game.Core.Providers;
using Game.Core.Services;
using Game.Core.StateMachines;
using Game.MainMenu;
using Meta;
using Meta.Auth;
using Meta.Interfaces;
using Systems.Currency;
using Systems.SaveSystem;
using Systems.SaveSystem.Interfaces;
using Systems.TimeSystem;
using UI.Interfaces;
using UI.Providers;
using Zenject;

namespace Game.Shared
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ILoader>().To<SceneLoader>().FromNew().AsSingle();
            Container.Bind<StateMachine>().FromNew().AsSingle();
            
            Container.Bind<IRandomService>().To<RandomService>().AsSingle();
            
            // Bind Providers
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.Bind<IAddressProvider>().To<UIAddressProvider>().AsSingle().NonLazy();
            Container.Bind<IAnimationProvider>().To<UIAnimationProvider>().AsSingle().NonLazy();
            
            // Bind Network services
            Container.BindInterfacesAndSelfTo<UnityAuthService>().AsSingle().NonLazy();
            Container.Bind<INetworkStatusService>().To<NetworkStatusService>().AsSingle();
            
            // Bind Save Services
            Container.Bind<ISaveService>().To<HybridSaveService>().AsSingle();
            Container.Bind<ISaveService>().WithId(DataSource.Local).To<LocalSaveService>().AsSingle();
            Container.Bind<ISaveService>().WithId(DataSource.Cloud).To<CloudSave>().AsSingle();
            Container.Bind<IDataLoader>().WithId(DataSource.Local).To<LocalDataLoader>().AsSingle();
            Container.Bind<IDataLoader>().WithId(DataSource.Cloud).To<CloudDataLoader>().AsSingle();
            Container.Bind<IDataHashingService>().To<DataHashingService>().AsSingle();
            Container.Bind<IConflictResolutionService>().To<DataConflictResolutionService>().AsSingle();
            Container.Bind<ITimeService>().To<TimeService>().AsSingle();
            
            //Bind Currency services
            Container.BindInterfacesAndSelfTo<CurrencyService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CurrencySaveLoadService>().AsSingle();
            Container.BindInterfacesAndSelfTo<CurrencySaveMonitor>().AsSingle().NonLazy();
            
            // Bind Hero Services
            Container.BindInterfacesAndSelfTo<HeroLoadoutService>().AsSingle();
            Container.BindInterfacesAndSelfTo<HeroLoadoutSaveLoadService>().AsSingle();
            
            

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            BindDebugServices();
#else
            BindReleaseServices();
#endif
        }

        private void BindReleaseServices()
        {
            Container.Bind<IDebugLogger>().To<ReleaseLogger>().AsSingle();
            Container.Bind<IDebugMenu>().To<ReleaseDebugMenu>().AsSingle();
        }

        private void BindDebugServices()
        {
            Container.Bind<IDebugLogger>().To<DebugLogger>().AsSingle();
            Container.Bind<IDebugMenu>().To<DebugMenu>().AsSingle();
        }
    }
}