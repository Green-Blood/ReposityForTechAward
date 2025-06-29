using Systems.Currency.Interfaces;
using Systems.SaveSystem.Interfaces;
using UniRx;

namespace Systems.Currency
{
    public class CurrencySaveMonitor : ISaveMonitor
    {
        private readonly ICurrencyService _currencyService;
        private readonly ICurrencySave _currencySave;
        private float _bufferedGoldGains;

        // TODO Change to settings later
        private const float SaveThreshold = 500f;

        public CurrencySaveMonitor(ICurrencyService currencyService, ICurrencySave currencySave)
        {
            _currencyService = currencyService;
            _currencySave = currencySave;
        }
        
        public void StartMonitor()
        {
            _currencyService.GetCurrencyObservable(CurrencyType.Gold)
                .Subscribe(OnGoldCurrencyChanged);
        }

        private void OnGoldCurrencyChanged(Currency newValue)
        {
            _bufferedGoldGains += newValue.Current;

            if(_bufferedGoldGains >= SaveThreshold)
            {
                _currencySave.SaveCurrency(CurrencyType.Gold);
                _bufferedGoldGains = 0;
            }
        }
    }
}