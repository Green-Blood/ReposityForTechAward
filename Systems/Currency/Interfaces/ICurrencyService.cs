using System.Collections.Generic;
using UniRx;

namespace Systems.Currency.Interfaces
{
    public interface ICurrencyService 
    {
        Currency GetCurrency(CurrencyType type);
        void AddCurrency(CurrencyType type, float amount);
        void SetCurrency(CurrencyType type, float amount, float? max = null);
        IReadOnlyReactiveProperty<Currency> GetCurrencyObservable(CurrencyType type);
        Dictionary<CurrencyType, Currency> GetAllCurrencies();
        bool SpendCurrency(CurrencyType type, float amount);
    }
}