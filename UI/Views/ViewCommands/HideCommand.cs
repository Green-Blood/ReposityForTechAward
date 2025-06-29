using Cysharp.Threading.Tasks;
using Game.Core.Commands.Interfaces;
using Nova;

namespace UI.Views.ViewCommands
{
    public class HideCommand : ICommand
    {
        private readonly UIBlock _uiBlock;

        public HideCommand(UIBlock uiBlock)
        {
            _uiBlock = uiBlock;
        }

        public byte Priority { get; } = 1;

        public bool CanExecute()
        {
            return _uiBlock.isActiveAndEnabled && _uiBlock.Visible;
        }

        public UniTask Execute()
        {
            _uiBlock.Visible = false;
            return UniTask.CompletedTask;
        }
    }
}