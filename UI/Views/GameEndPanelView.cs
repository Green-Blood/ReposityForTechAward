using System;
using Cysharp.Threading.Tasks;
using Extensions.ExtensionMethods.Even_More_Extensions.RX;
using NovaSamples.UIControls;
using UI.Configs;
using UI.Interfaces;
using UI.ViewModels;
using UniRx;
using UnityEngine;

namespace UI.Views
{
    public class GameEndPanelView : BaseView, IView<GameEndPanelViewModel>
    {
        [SerializeField] private ButtonAnimation closeButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button continueWithGemsButton;

        // TODO move it to Provider
        [SerializeField] private float delay = 2.5f;

        // TODO Change this to provider 
        [SerializeField] private UIAnimationData uiAnimationData;
        private GameEndPanelViewModel _viewModel;

        public void Initialize(GameEndPanelViewModel viewModel)
        {
            _viewModel = viewModel;


            continueButton.OnClicked
                .AsObservable()
                .BindTo(_viewModel.OnContinueClicked)
                .AddTo(this);

            continueWithGemsButton.OnClicked
                .AsObservable()
                .BindTo(_viewModel.OnContinueWithGemsClicked)
                .AddTo(this);

            closeButton.Button.OnClicked
                .AsObservable()
                .BindTo(_viewModel.OnCloseButtonClicked)
                .AddTo(this);

            _viewModel.OnContinueClicked.Subscribe(OnContinueClicked).AddTo(this);
        }

        protected override void BeforeShow()
        {
            ShowCommandsBuilder.AddFade(UiBlock, uiAnimationData.FadeInAnimation, true);
            closeButton.ToggleButton();
        }

        protected override async void AfterShow()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: destroyCancellationToken);
            closeButton.ToggleButton();
        }

        protected override void BeforeHide()
        {
            HideCommandsBuilder.AddFade(UiBlock, uiAnimationData.FadeOutAnimation, true);
        }

        protected override void AfterHide()
        {
            gameObject.SetActive(false);
        }

        private async void OnContinueClicked(Unit _) => await Hide();
    }
}