using Game.Core.Factories.Interfaces;
using Game.Core.Providers;
using UI.Configs;

namespace UI.Interfaces
{
    public interface IViewModelProvider<out TViewModel> where TViewModel : IViewModel
    {
        TViewModel CreateViewModel(UIAnimationData animationData);
        TViewModel CreateViewModelForUnit(IShared unit, UIAnimationData animationProvider);
    }
}