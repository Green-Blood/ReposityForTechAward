using Extensions.ExtensionMethods.Even_More_Extensions.RX;
using UI.Interfaces;
using UI.ViewModels;
using UniRx;
using UnityEngine;

namespace UI.Views.Currency
{
    public class ResourcesHUDView : BaseView, IView<ResourcesHUDViewModel>
    {
        [SerializeField] private CurrencyView goldView;
        [SerializeField] private CurrencyView diamondView;
        [SerializeField] private CurrencyView energyView;
        
        private ResourcesHUDViewModel _viewModel;

        public void Initialize(ResourcesHUDViewModel viewModel)
        {
            _viewModel = viewModel;
            
            _viewModel.Gold.Subscribe(gold => goldView.UpdateAmount((int)gold.Current, gold.HasMax ? (int?)gold.Max : null)).AddTo(this);
            _viewModel.Diamonds.Subscribe(diamonds => diamondView.UpdateAmount((int)diamonds.Current, diamonds.HasMax ? (int?)diamonds.Max : null)).AddTo(this);
            _viewModel.Energy.Subscribe(energy => energyView.UpdateAmount((int)energy.Current, energy.HasMax ? (int?)energy.Max : null)).AddTo(this);
        }
    }
}
