using System.Collections.Generic;
using Extensions.Console.Interfaces;
using Systems.Currency.Interfaces;
using UniRx;
using UnityEngine;

namespace Systems.Currency
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IDebugLogger _logger;
        private readonly Dictionary<CurrencyType, ReactiveProperty<Currency>> _currencies;
        private readonly object _lock = new();

        public CurrencyService(IDebugLogger logger)
        {
            _logger = logger;
            _currencies = new Dictionary<CurrencyType, ReactiveProperty<Currency>>
            {
                { CurrencyType.Gold, new ReactiveProperty<Currency>(new Currency(0)) },
                { CurrencyType.Diamond, new ReactiveProperty<Currency>(new Currency(0)) },
                { CurrencyType.Energy, new ReactiveProperty<Currency>(new Currency(0, 100)) }
            };
        }
        
        public void SetCurrency(CurrencyType type, float amount, float? max = null)
        {
            lock (_lock)
            {
                _logger.Log($"Set {amount} {type}");

                if(!_currencies.ContainsKey(type))
                {
                    _currencies.Add(type, new ReactiveProperty<Currency>(new Currency(0)));
                }

                if(max.HasValue)
                {
                    amount = Mathf.Min(amount, max.Value);
                }

                if(amount < 0)
                {
                    _currencies[type].Value.Current = 0;
                    _logger.LogWarning($"Set {amount} {type} to 0");
                    return;
                }

                _currencies[type].Value = new Currency(amount, max);
            }
        }

        public IReadOnlyReactiveProperty<Currency> GetCurrencyObservable(CurrencyType type)
        {
            lock (_lock)
            {
                return _currencies[type];
            }
        }

        public Currency GetCurrency(CurrencyType type)
        {
            lock (_lock)
            {
                return _currencies.TryGetValue(type, out var currency) ? currency.Value : null;
            }
        }

        public void AddCurrency(CurrencyType type, float amount)
        {
            lock (_lock)
            {
                if(!_currencies.ContainsKey(type))
                {
                    _currencies.Add(type, new ReactiveProperty<Currency>(new Currency(0)));
                }

                var currentCurrency = _currencies[type].Value;
                float newAmount = currentCurrency.Current + amount;

                // Check for max value
                if(currentCurrency.HasMax && currentCurrency.Max != null)
                {
                    newAmount = Mathf.Min(newAmount, currentCurrency.Max.Value);
                }

                _currencies[type].Value = new Currency(newAmount, currentCurrency.Max);
                var logMessage = currentCurrency.HasMax
                    ? $"Added {amount} {type}. New balance: {_currencies[type].Value.Current}/{currentCurrency.Max}"
                    : $"Added {amount} {type}. New balance: {_currencies[type].Value.Current}";

                _logger.Log(logMessage);
            }
        }

        public bool SpendCurrency(CurrencyType type, float amount)
        {
            lock (_lock)
            {
                if(!_currencies.TryGetValue(type, out var currency))
                {
                    _logger.LogWarning($"Currency {type} not found.");
                    return false;
                }

                var currentCurrency = currency.Value;

                if(currentCurrency.Current >= amount)
                {
                    _currencies[type].Value = new Currency(currentCurrency.Current - amount, currentCurrency.Max);
                    _logger.Log(currentCurrency.HasMax
                                    ? $"Spent {amount} {type}. Remaining balance: {_currencies[type].Value.Current}/{currentCurrency.Max}"
                                    : $"Spent {amount} {type}. Remaining balance: {_currencies[type].Value.Current}");
                    return true;
                }

                _logger.LogWarning($"Not enough {type} to spend.");
                return false;
            }
        }

        public Dictionary<CurrencyType, Currency> GetAllCurrencies()
        {
            lock (_lock)
            {
                var snapshot = new Dictionary<CurrencyType, Currency>();

                foreach (var currency in _currencies)
                {
                    snapshot[currency.Key] = currency.Value.Value;
                }

                return snapshot;
            }
        }
    }
}