using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Core.Commands.Interfaces;

namespace UI.Views
{
    public class SequentialCommand : ICommand
    {
        private List<ICommand> _commands;

        public SequentialCommand(List<ICommand> commands)
        {
            _commands = commands;
        }

        public byte Priority => 0;

        public bool CanExecute()
        {
            var commands = _commands;

            foreach (var command in _commands.Where(command => !command.CanExecute()).ToList())
            {
                commands.Remove(command);
            }

            _commands = commands;

            return _commands.Count > 0;
        }

        public async UniTask Execute()
        {
            foreach (var command in _commands)
            {
                await command.Execute();
            }

            _commands.Clear();
        }
    }
}