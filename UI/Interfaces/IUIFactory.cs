using Cysharp.Threading.Tasks;
using Game.Core.Factories.Interfaces;
using Game.Core.Interfaces;
using Meta.Interfaces;
using UI.Configs;
using UnityEngine;

namespace UI.Interfaces
{
    public interface IUIFactory : IServicePreloader
    {
        UniTask<TView> CreateUIElement<TView, TViewModel>(
            IViewModelProvider<TViewModel> viewModelProvider,
            IShared unit = null,
            Transform under = null,
            UIAnimationData animationData = null
            )
            where TView : MonoBehaviour, IView<TViewModel>
            where TViewModel : IViewModel;
    }
}