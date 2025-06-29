using UI.Interfaces;
using Zenject;

namespace UI.Factories
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly DiContainer _container;

        public ViewModelFactory(DiContainer container)
        {
            _container = container;
        }

        public TViewModel Create<TViewModel>(params object[] args) where TViewModel : IViewModel
        {
            return _container.Instantiate<TViewModel>(args);
        }
    }
}