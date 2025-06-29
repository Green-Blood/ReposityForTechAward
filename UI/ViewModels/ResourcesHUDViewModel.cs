using Systems.Currency;
using Systems.Currency.Interfaces;
using UI.Configs;
using UniRx;

namespace UI.ViewModels
{
    public class ResourcesHUDViewModel : ViewModelBase
    {
        public ReactiveProperty<Currency> Gold { get; }
        public ReactiveProperty<Currency> Diamonds { get; }
        public ReactiveProperty<Currency> Energy { get; }

        private readonly UIAnimationData _animationData;
        private readonly ICurrencyService _currencyService;
        
        public ResourcesHUDViewModel(UIAnimationData animationData, ICurrencyService currencyService)
        {
            _animationData = animationData;
            _currencyService = currencyService;
            Gold = new ReactiveProperty<Currency>(_currencyService.GetCurrency(CurrencyType.Gold));
            Diamonds = new ReactiveProperty<Currency>(_currencyService.GetCurrency(CurrencyType.Diamond));
            Energy = new ReactiveProperty<Currency>(_currencyService.GetCurrency(CurrencyType.Energy));

            _currencyService.GetCurrencyObservable(CurrencyType.Gold).Subscribe(model => Gold.Value = model);
            _currencyService.GetCurrencyObservable(CurrencyType.Diamond).Subscribe(model => Diamonds.Value = model);
            _currencyService.GetCurrencyObservable(CurrencyType.Energy).Subscribe(model => Energy.Value = model);
        }
        
    }
}