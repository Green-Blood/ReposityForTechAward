using System;
using System.Collections.Generic;
using Game.Core.Factories.Interfaces;

namespace UI.Interfaces
{
    public interface IUIRegistrar : IObjectRegistrar
    {
        void Register<TView>(TView view) where TView : IView;
        TView Get<TView>() where TView : IView;
        IView Get(Type viewType);
        bool IsRegistered<TView>() where TView : IView;
        void Unregister<TView>() where TView : IView;
        IEnumerable<Type> GetAllRegisteredTypes();
    }
}