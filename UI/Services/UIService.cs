using System;
using UI.Interfaces;

namespace UI.Services
{
    public class UIService : IUIService
    {
        private readonly IUIRegistrar _uiRegistrar;

        public UIService(IUIRegistrar uiRegistrar)
        {
            _uiRegistrar = uiRegistrar;
        }

        public void Show<TView>() where TView : IView
        {
            if (_uiRegistrar.IsRegistered<TView>())
            {
                var view = _uiRegistrar.Get<TView>();
                view.Show();
            }
            else
            {
                throw new InvalidOperationException($"{typeof(TView).Name} is not registered.");
            }
        }

        public void Hide<TView>() where TView : IView
        {
            if (_uiRegistrar.IsRegistered<TView>())
            {
                var view = _uiRegistrar.Get<TView>();
                view.Hide();
            }
            else
            {
                throw new InvalidOperationException($"{typeof(TView).Name} is not registered.");
            }
        }

        public void HideAll()
        {
            foreach (var viewType in _uiRegistrar.GetAllRegisteredTypes())
            {
                var view = _uiRegistrar.Get(viewType);
                view.Hide();
            }
        }
    }
}