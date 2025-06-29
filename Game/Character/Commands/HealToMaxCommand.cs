using Cysharp.Threading.Tasks;
using Game.Core.Commands.Interfaces;
using Systems.StatSystem.Interfaces;
using Systems.StatSystem.StatTypes;

namespace Game.GamePlay.Character.Commands
{
    public class HealToMaxCommand : ICommand
    {
        private readonly IStatCollection _statCollection;
        public byte Priority => 0;

        public HealToMaxCommand(IStatCollection statCollection)
        {
            _statCollection = statCollection;
        }

        public bool CanExecute()
        {
            return _statCollection.TryGetStat<Vital>(StatType.Health).StatCurrentValue < 
                   _statCollection.TryGetStat<Vital>(StatType.Health).StatValue;
        }

        public UniTask Execute()
        {
            _statCollection.TryGetStat<Vital>(StatType.Health).SetCurrentValueToMax();
            return default;
        }
    }
}