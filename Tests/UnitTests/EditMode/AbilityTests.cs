using System.Threading.Tasks;
using Extensions.ExtensionMethods;
using FluentAssertions;
using Game.Core.AbilitySystem;
using Game.Core.AbilitySystem.Interfaces;
using Game.Core.Factories.Interfaces;
using Game.Core.StaticData;
using Game.GamePlay.Character.Base;
using Game.GamePlay.Character.Other.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Systems.StatSystem.Interfaces;
using UnityEngine;

namespace Tests.UnitTests.EditMode
{
    public class AbilityTests
    {
        private readonly ICharacterView _characterView;
        private readonly IStatCollection _statCollection;
        private readonly IGameFactory _gameFactory;
        private readonly BaseAbility _baseAbility;
        public AbilityTests()
        {
            _characterView = Substitute.For<ICharacterView>();
            _statCollection = Substitute.For<IStatCollection>();
            _gameFactory = Substitute.For<IGameFactory>();

            _baseAbility = new BaseAbility(_characterView, _statCollection, _gameFactory);

        }

        [Test]
        public void When_AbilityIsInitialized_Then_ShouldHaveCorrectName()
        {
            // Arrange
            // var abilityData = ScriptableObject.CreateInstance<AbilityStaticData>();

            // Act & Assert
            // abilityData.Name.Should().Be("TestAbility");
        }
        
    }
}