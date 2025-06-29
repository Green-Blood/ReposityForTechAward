using Game.Core.Factories.Interfaces;
using UI.Configs;
using UI.Interfaces;

namespace UI.Providers
{
    public class ViewModelProvider<TViewModel> : IViewModelProvider<TViewModel> where TViewModel : IViewModel
    {
        private TViewModel _cachedViewModel;

        private readonly bool _useCaching;
        private readonly IViewModelFactory _viewModelFactory;

        public ViewModelProvider(IViewModelFactory viewModelFactory, bool useCaching)
        {
            _viewModelFactory = viewModelFactory;
            _useCaching = useCaching;
        }

        public TViewModel CreateViewModel(UIAnimationData animationData)
        {
            if(_useCaching && !Equals(_cachedViewModel, default(TViewModel)))
            {
                return _cachedViewModel;
            }

            _cachedViewModel = _viewModelFactory.Create<TViewModel>(animationData);
            return _cachedViewModel;
        }

        public TViewModel CreateViewModelForUnit(IShared unit, UIAnimationData animationProvider)
        {
            if(_useCaching && !Equals(_cachedViewModel, default(TViewModel)))
            {
                return _cachedViewModel;
            }

            _cachedViewModel = _viewModelFactory.Create<TViewModel>(unit, animationProvider);
            return _cachedViewModel;
        }
    }
}