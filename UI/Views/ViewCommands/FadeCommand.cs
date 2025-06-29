using Cysharp.Threading.Tasks;
using DG.Tweening;
using Extensions.ExtensionMethods;
using Game.Core.Commands.Interfaces;
using Nova;

namespace UI.Views.ViewCommands
{
    public class FadeCommand : ICommand
    {
        private readonly UIBlock _viewObject;
        private readonly float _startAlpha;
        private readonly float _endAlpha;
        private readonly float _duration;
        private readonly Ease _ease;
        private readonly float _delay;
        private readonly bool _shouldWait;

        public FadeCommand(UIBlock uiBlock, float startAlpha, float endAlpha, float duration, Ease ease, float delay = 0, bool shouldWait = false)
        {
            _viewObject = uiBlock;
            _startAlpha = startAlpha;
            _endAlpha = endAlpha;
            _duration = duration;
            _ease = ease;
            _delay = delay;
            _shouldWait = shouldWait;
        }

        public byte Priority { get; } = 2;

        public bool CanExecute() => _viewObject.isActiveAndEnabled;

        public async UniTask Execute()
        {
            var viewObjectColor = _viewObject.Color;
            viewObjectColor.a = _startAlpha;
            _viewObject.Color = viewObjectColor;

            var fadeTweener = _viewObject.DOFade(_endAlpha, _duration).SetEase(_ease).SetDelay(_delay);

            if(_shouldWait)
            {
                await fadeTweener.AwaitForComplete();
            }
        }
    }
}