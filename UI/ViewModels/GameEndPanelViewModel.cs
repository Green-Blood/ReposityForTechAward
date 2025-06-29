using System;
using Cysharp.Threading.Tasks;
using Game.Core.Interfaces;
using UI.Configs;
using UniRx;

namespace UI.ViewModels
{
    public class GameEndPanelViewModel : ViewModelBase
    {
        public ReactiveCommand OnContinueClicked { get; } = new();
        public ReactiveCommand OnContinueWithGemsClicked { get; } = new();
        public ReactiveCommand OnCloseButtonClicked { get; } = new();

        private readonly IRestartService _restartService;
        private readonly UIAnimationData _animationData;

        public GameEndPanelViewModel(UIAnimationData animationData, IRestartService restartService)
        {
            _restartService = restartService;
            _animationData = animationData;

            OnContinueClicked.Subscribe(Continue).AddTo(Disposables);
        }

        private async void Continue(Unit unit)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_animationData.FadeOutAnimation.Delay + _animationData.FadeOutAnimation.Duration));
            await _restartService.Restart();
        }

        public override void Dispose()
        {
            OnContinueClicked?.Dispose();
            OnContinueWithGemsClicked?.Dispose();
            OnCloseButtonClicked?.Dispose();
            Disposables?.Dispose();
        }
    }
}