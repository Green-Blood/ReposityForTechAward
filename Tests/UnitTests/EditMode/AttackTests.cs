using FluentAssertions;
using Game.GamePlay.Character.Base.Attack;
using Game.GamePlay.Character.Other.Interfaces;
using NSubstitute;
using NUnit.Framework;
using ProjectDawn.Navigation.Hybrid;
using Systems.StatSystem;
using Systems.StatSystem.Interfaces;
using Systems.StatSystem.StatTypes;
using UnityEngine;

namespace UnitTests.EditMode
{
    public class AttackTests
    {
        [Test]
        public void WhenIsInAttackRange_ThenAttack()
        {
            // // Arrange  
            // //TODO Fix this 
            // var agentViewMock = Substitute.For<IAgentView>();
            // // var agentMock = new GameObject().AddComponent<AgentAuthoring>();
            // // var agentMock = Substitute.For<AgentAuthoring>()
            //
            // agentViewMock.Agent.EntityBody.RemainingDistance.Returns(5);
            // // agentMock.EntityBody.RemainingDistance.Returns(5);
            //
            // IStatCollection statCollection = Substitute.For<IStatCollection>();
            //
            // CreateAttackRangeMock(statCollection, StatType.AttackRange, 5);
            //
            //
            // // Act
            // var attackRangeChecker = new AttackRangeChecker(agentViewMock, statCollection);
            //
            // // Assert
            // attackRangeChecker.IsInAttackRange().Should().BeTrue();
        }

        [Test]
        public void WhenAttackCooldownIsFinished_ThenAttack()
        {
            // Arrange
            // IAttackWaiter attackSpeedHandler = new AttackSpeedHandler(AttackSpeedHandler);



            // Act

            // Assert
            // attackSpeedHandler.IsAttackCooldownFinished().Should().BeTrue();
        }

        private static Vital CreateAttackRangeMock(IStatCollection statCollectionMock, StatType attackRange, float statBaseValue)
        {
            var attackRangeMock = new Vital
            {
                StatBaseValue = statBaseValue
            };
            statCollectionMock.CreateOrGetStat<Vital>(attackRange).Returns(attackRangeMock);
            statCollectionMock.TryGetStat<Vital>(attackRange).Returns(attackRangeMock);
            attackRangeMock.StatCurrentValue = attackRangeMock.StatBaseValue;
            return attackRangeMock;
        }
    }
}
