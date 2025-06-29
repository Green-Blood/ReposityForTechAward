using Extensions.Console.Interfaces;
using FluentAssertions;
using Game.Core.Commands;
using Game.Core.Commands.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Tests.UnitTests.EditMode
{
    public class CommandInvokerTest
    {
        private IDebugLogger _logger;
        private CommandInvoker _commandInvoker;
        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For<IDebugLogger>();
            _commandInvoker = new CommandInvoker(_logger);
        }
        [Test]
        public void When_AddCommandInvoked_ThenEnqueuedSingleCommand()
        {
            // Arrange
            var commandMock = Substitute.For<ICommand>();

            // Act
            _commandInvoker.AddCommand(commandMock);

            // Assert
            _commandInvoker.Commands.Count.Should().Be(1);
        }

        [Test]
        public void WhenNullCommandIsAdded_ThenNullExceptionOccurs()
        {
            // Act 
            _commandInvoker.AddCommand(null);
            // Assert
            _logger.Received(1).LogError("Command is null", "AddCommand");
        }
        [Test]
        public  void When_ExecuteCommandsInvoked_ThenCommandsExecuted()
        {
            // Arrange
            var commandMock = Substitute.For<ICommand>();

            _commandInvoker.AddCommand(commandMock);
            commandMock.CanExecute().Returns(true);

            // Act
            _ = _commandInvoker.ExecuteCommands();

            // Assert
            commandMock.Received(1).Execute();
        }
        [Test]
        public void When_CommandsCleared_ThenQueueIsEmpty()
        {
            // Arrange
            var commandMock = Substitute.For<ICommand>();

            _commandInvoker.AddCommand(commandMock);

            // Act
            _commandInvoker.ClearCommands();

            // Assert
            _commandInvoker.Commands.Count.Should().Be(0);
        }
    }
}
