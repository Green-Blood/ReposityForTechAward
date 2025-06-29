using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UI.Interfaces;

namespace UI.Factories
{
    public class UIRegistrar : IUIRegistrar
    {
        private readonly Dictionary<Type, object> _registeredViews = new();


        public UniTask Register()
        {
            return UniTask.CompletedTask;
        }

        public void Register<TView>(TView view) where TView : IView
        {
            var viewType = typeof(TView);

            if(!_registeredViews.TryAdd(viewType, view))
            {
                throw new InvalidOperationException($"{viewType.Name} is already registered.");
            }
            // Cast to the base interface
        }

        public TView Get<TView>() where TView : IView
        {
            var viewType = typeof(TView);

            if(_registeredViews.TryGetValue(viewType, out var view))
            {
                return (TView)view;
            }

            throw new InvalidOperationException($"{viewType.Name} is not registered.");
        }

        public IView Get(Type viewType)
        {
            if (_registeredViews.TryGetValue(viewType, out var view))
            {
                return view as IView;
            }

            throw new InvalidOperationException($"{viewType.Name} is not registered.");
        }

        public bool IsRegistered<TView>() where TView : IView
        {
            return _registeredViews.ContainsKey(typeof(TView));
        }

        public void Unregister<TView>() where TView : IView
        {
            var viewType = typeof(TView);

            if(_registeredViews.ContainsKey(viewType))
            {
                _registeredViews.Remove(viewType);
            }
        }

        public IEnumerable<Type> GetAllRegisteredTypes()
        {
            return _registeredViews.Keys;
        }
    }
}