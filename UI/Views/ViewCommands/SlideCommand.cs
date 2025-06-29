using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Core.Commands.Interfaces;
using Nova;
using UnityEngine;

namespace UI.Views.ViewCommands
{
    public class SlideCommand : ICommand
    {
        private readonly UIBlock _uiBlock;
        private readonly Vector3 _startPos;
        private readonly Vector3 _endPos;
        private readonly float _duration;
        private readonly bool _shouldWait;
        private readonly Ease _ease;
        private readonly float _delay;

        public SlideCommand(UIBlock uiBlock, Vector3 startPos, Vector3 endPos, float duration, Ease ease = Ease.Linear, float delay = 0, bool shouldWait = false)
        {
            _uiBlock = uiBlock;
            _startPos = startPos;
            _endPos = endPos;
            _duration = duration;
            _ease = ease;
            _delay = delay;
            _shouldWait = shouldWait;
        }

        public byte Priority { get; } = 2;
       
        public bool CanExecute() => _uiBlock.isActiveAndEnabled;

        public async UniTask Execute()
        {
            _uiBlock.transform.localPosition = _startPos;

            var tweener = _uiBlock.transform.DOLocalMove(_endPos, _duration).SetEase(_ease).SetDelay(_delay);

            if(_shouldWait)
            {
                await tweener.AwaitForComplete();
            }
        }
    }
}