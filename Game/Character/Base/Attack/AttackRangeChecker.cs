using Game.GamePlay.Character.Base.Attack.Interfaces;
using Game.GamePlay.Character.Other.Interfaces;
using ProjectDawn.Navigation;
using ProjectDawn.Navigation.Hybrid;
using Systems.StatSystem.Interfaces;
using Systems.StatSystem.StatTypes;

namespace Game.GamePlay.Character.Base.Attack
{
    public class AttackRangeChecker : IAttackRangeCheck
    {
        private readonly AgentAuthoring _agent;
        private readonly float _attackRange;

        public AttackRangeChecker(IAgentView characterView, IStatCollection statCollection)
        {
            _agent = characterView.Agent;
            _attackRange = statCollection.TryGetStat<Stat>(StatType.AttackRange).StatValue;
            SetStoppingDistance(_agent.EntityLocomotion);
        }

        public bool IsInAttackRange()
        {
            float remainingDistance = _agent.EntityBody.RemainingDistance;
            return remainingDistance != 0 && IsInAttackRange(remainingDistance, _attackRange);
        }

        private static bool IsInAttackRange(float remainingDistance, float attackRange) => remainingDistance <= attackRange;

        private void SetStoppingDistance(AgentLocomotion agent)
        {
            agent.StoppingDistance = _attackRange;
            _agent.EntityLocomotion = agent;
        }
    }
}