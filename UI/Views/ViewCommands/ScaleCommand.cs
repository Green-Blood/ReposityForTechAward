using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Core.Commands.Interfaces;
using Nova;
using UnityEngine;

namespace UI.Views.ViewCommands
{
    public class ScaleCommand : ICommand
    {
        private readonly UIBlock _uiBlock;
        private readonly Vector3 _startScale;
        private readonly Vector3 _endScale;
        private readonly float _duration;
        private readonly Ease _ease;
        private readonly float _delay;
        private readonly bool _shouldWait;

        public ScaleCommand(UIBlock uiBlock, Vector3 startScale, Vector3 endScale, float duration, Ease ease = Ease.Linear, float delay = 0, bool shouldWait = false)
        {
            _uiBlock = uiBlock;
            _startScale = startScale;
            _endScale = endScale;
            _duration = duration;
            _ease = ease;
            _delay = delay;
            _shouldWait = shouldWait;
        }

        public byte Priority { get; } = 2;
        public bool CanExecute() => _uiBlock.isActiveAndEnabled;

        public async UniTask Execute()
        {
            _uiBlock.transform.localScale = _startScale;
            var tweener = _uiBlock.transform.DOScale(_endScale, _duration).SetEase(_ease).SetDelay(_delay);

            if(_shouldWait)
            {
                await tweener.AwaitForComplete();
            }
        }
    }
}