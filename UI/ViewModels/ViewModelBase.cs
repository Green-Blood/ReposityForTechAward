using System;
using UI.Interfaces;
using UniRx;

namespace UI.ViewModels
{
    public class ViewModelBase : IViewModel, IDisposable
    {
        protected CompositeDisposable Disposables = new();
        public virtual void Dispose()
        {
            
        }
    }
}