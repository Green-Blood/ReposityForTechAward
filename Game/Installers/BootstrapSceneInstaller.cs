using Meta.Auth;
using Meta.Interfaces;
using Zenject;
namespace Game.Installers
{
    public class BootstrapSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
           
            Container.Bind<IServiceInitializer>().To<UnityServiceInitializer>().AsSingle().NonLazy();
        }
    }
}