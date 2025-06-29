using UI.Factories;
using UI.Interfaces;
using UI.Providers;
using UI.Services;
using UnityEngine;
using Zenject;

namespace UI.Installers
{
    public class UiInstaller : MonoInstaller
    {
        [SerializeField] private UIParentProvider parentProvider;

        public override void InstallBindings()
        {
            
            Container.Bind(typeof(IViewModelProvider<>))
                .To(typeof(ViewModelProvider<>))
                .AsSingle()
                .WithArguments(true);
            
            Container.Bind<IViewModelFactory>().To<ViewModelFactory>().AsSingle();

            Container.Bind<IUIParentProvider>().To<UIParentProvider>().FromInstance(parentProvider).AsSingle();
            Container.Bind<IUIFactory>().To<GameUIFactory>().AsSingle();
            Container.Bind<IUIService>().To<UIService>().AsSingle();
            Container.Bind<IUIStateService>().To<UIStateService>().AsSingle();
            Container.BindInterfacesAndSelfTo<UIRegistrar>().AsSingle();
        }
    }
}