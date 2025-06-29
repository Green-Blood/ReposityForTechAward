using Cysharp.Threading.Tasks;
using Game.Core.AbilitySystem.Interfaces;
using Game.Core.Commands.Interfaces;
using Game.Core.Factories.Interfaces;
namespace Game.GamePlay.Character.Commands
{
    public class TargetAbilityCommand : ICommand
    {
        private readonly ITargetAbility _targetAbility;
        private readonly IShared _owner;
        private readonly AbilityTarget _target;
        public byte Priority => 1;
        public TargetAbilityCommand(ITargetAbility targetAbility, AbilityTarget target, IShared owner)
        {
            _targetAbility = targetAbility;
            _target = target;
            _owner = owner;

        }
        public bool CanExecute()
        {
            return _targetAbility != null && _target.Target != null && _owner != null;
        }
        public async UniTask Execute()
        {
            await _targetAbility.Cast(_owner, _target);
        }
    }
}
