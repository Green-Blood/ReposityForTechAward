using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using UI.Configs;
using UI.Interfaces;
using UI.ViewModels;
using UniRx;
using UnityEngine;
using TextBlock = Nova.TextBlock;

namespace UI.Views
{
    public class CastleHealthView : BaseView, IView<CastleHealthViewModel>
    {
        [SerializeField] private TextBlock healthText;
        [SerializeField] private MMF_Player damageFeedback;

        // TODO Change this to provider 
        [SerializeField] private UIAnimationData uiAnimationData;
        private CastleHealthViewModel _viewModel;

        public void Initialize(CastleHealthViewModel castleHealthViewModel)
        {
            _viewModel = castleHealthViewModel;

            _viewModel.CurrentHealth
                .Subscribe(_ => UpdateHealthUI())
                .AddTo(this);

            _viewModel.MaxHealth
                .Subscribe(_ => UpdateHealthUI())
                .AddTo(this);
        }

        protected override void BeforeShow()
        {
            ShowCommandsBuilder.AddFade(UiBlock, uiAnimationData.FadeInAnimation, true);
        }

        protected override void BeforeHide()
        {
            HideCommandsBuilder.AddFade(UiBlock, uiAnimationData.FadeOutAnimation, true);
        }

        private void UpdateHealthUI()
        {
            healthText.Text = $"{(int)_viewModel.CurrentHealth.Value} / {(int)_viewModel.MaxHealth.Value}";
            damageFeedback.PlayFeedbacks();
        }
    }
}