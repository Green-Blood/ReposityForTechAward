using Cysharp.Threading.Tasks;
using Game.Core.Commands.Interfaces;
using Nova;

namespace UI.Views.ViewCommands
{
    public class ShowCommand : ICommand
    {
        private readonly UIBlock _uiBlock;

        public ShowCommand(UIBlock uiBlock)
        {
            _uiBlock = uiBlock;
        }

        public byte Priority { get; } = 1;

        public bool CanExecute()
        {
            if(!_uiBlock.isActiveAndEnabled) return false;

            return !_uiBlock.Visible;
        }

        public UniTask Execute()
        {
            _uiBlock.Visible = true;
            return UniTask.CompletedTask;
        }
    }
}