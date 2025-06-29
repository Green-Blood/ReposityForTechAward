using DG.Tweening;
using Extensions.ExtensionMethods;
using Extensions.Helpers;
using MoreMountains.Feedbacks;
using Nova;
using Systems.Currency;
using UnityEngine;

namespace UI.Views.Currency
{
    public class CurrencyView : BaseView
    {
        [SerializeField] private CurrencyType currencyType;
        [SerializeField] private MMF_Player changeValueFeedback;
        [SerializeField] private TextBlock currencyAmountText;

        private int _currentAmount;
        private int? _maxAmount;

        public void UpdateAmount(int targetAmount, int? maxAmount = null)
        {
            _maxAmount = maxAmount;
            DOTween.To(() => _currentAmount, SetAmount, targetAmount, 0.5f)
                .SetEase(Ease.OutQuad);
            changeValueFeedback?.PlayFeedbacks();
        }

        private void SetAmount(int amount)
        {
            _currentAmount = amount;
            currencyAmountText.Do(x => x.Text = NumberFormatter.FormatNumber(_currentAmount), when: _maxAmount == null);
            currencyAmountText.Do(x => x.Text = NumberFormatter.FormatCurrentMax(_currentAmount, _maxAmount ?? 0), when: _maxAmount != null);
        }
    }
}