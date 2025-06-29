using Extensions.Console;
using Extensions.Console.Interfaces;
using FluentAssertions;
using Game.Core.Interfaces;
using Game.Core.Services;
using Game.Core.StaticData;
using Moq;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
namespace Tests.EditMode
{
    public class LevelTests
    {
        [Test]
        public void WhenCurrentLevelIsLoaded_ThenIsNotNull()
        {
            // Arrange
            var staticDataService = new StaticDataService();
            staticDataService.Load();
            var levelService = new LevelService(staticDataService, new DebugLogger());
            levelService.Warmup();

            // Act
            var result = levelService.GetCurrentLevel();

            // Assert
            result.Should().NotBeNull();
        }
        [Test]
        public void  WhenLevelIsNull_ThenReturnsError()
        {
            // Arrange
            var staticDataServiceMock = Substitute.For<IStaticDataService>();
            var loggerMock = Substitute.For<IDebugLogger>();
            var levelService = new LevelService(staticDataServiceMock, loggerMock);
            LevelStaticData expectedLevel = null;
            string expectedErrorMessage = "Current level is null";

            // Act
            levelService.Warmup();
            staticDataServiceMock.ForLevel(1).Returns(expectedLevel);
            var result = levelService.GetCurrentLevel();

            // Assert
            loggerMock.Received().LogError(expectedErrorMessage, "GetCurrentLevel");
            result.Should().BeNull();
        }
        [Test]
        public void WhenWarmedUp_ThenCurrentLevelIsNotNull()
        {
            // Arrange
            var staticDataServiceMock = Substitute.For<IStaticDataService>();
            var logger = Substitute.For<IDebugLogger>();
            var levelService = new LevelService(staticDataServiceMock, logger);
            LevelStaticData expectedLevel = null;
            string expectedErrorMessage = $"Level 2 not found";

            // Act
            staticDataServiceMock.ForLevel(2).Returns(expectedLevel);
            levelService.WarmUp(2);

            // Assert
            logger.Received().LogError(expectedErrorMessage, "IsLevelValid");

        }
        [Test]
        public void WhenWarmedUpAlready_ThenWarmingUpIsSkipped()
        {
            // Arrange
            var staticDataServiceMock = Substitute.For<IStaticDataService>();
            var logger = Substitute.For<IDebugLogger>();
            var levelService = new LevelService(staticDataServiceMock, logger);
            var expectedLevel = ScriptableObject.CreateInstance<LevelStaticData>();
            string expectedErrorMessage = $"Current level is already assigned";

            // Act
            staticDataServiceMock.ForLevel(1).Returns(expectedLevel);
            levelService.WarmUp(1);
            levelService.WarmUp(1);

            // Assert
            logger.Received().LogWarning(expectedErrorMessage, "IsLevelValid");

        }
    }
}
