using Cysharp.Threading.Tasks;
using Extensions.Console.Interfaces;
using Game.Core.Commands.Interfaces;
using Game.GamePlay.Character.Base.Health.Interfaces;
using Systems.StatSystem.Interfaces;

namespace Game.GamePlay.Character.Commands
{
    public class ReviveCommand : ICommand
    {
        private readonly IDie _characterDeath;
        private readonly ICommandInvoker _commandInvoker;
        private readonly IStatCollection _statCollection;
        private readonly IDebugLogger _logger;
        public byte Priority => 0;

        public ReviveCommand(IDie characterDeath, ICommandInvoker commandInvoker, IStatCollection statCollection, IDebugLogger logger)
        {
            _characterDeath = characterDeath;
            _commandInvoker = commandInvoker;
            _statCollection = statCollection;
            _logger = logger;
        }

        public bool CanExecute()
        {
            return _characterDeath.IsDead.Value;
        }

        public UniTask Execute()
        {
            _commandInvoker.AddCommand(new HealToMaxCommand(_statCollection));
            _characterDeath.Revive();
            _logger.Log("Revived");
            return _commandInvoker.ExecuteCommands();
        }
    }
}