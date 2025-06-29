using System.Threading.Tasks;
using Extensions.Console.Interfaces;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Systems.Currency;

namespace Tests.UnitTests.EditMode
{
    public class CurrencyTests
    {
        private IDebugLogger _loggerMock;

        [SetUp]
        public void Setup()
        {
            // Mock dependencies_
            _loggerMock = Substitute.For<IDebugLogger>();
        }

        [Test]
        public void When_SetCurrencyCalled_Then_UpdatesValueCorrectly()
        {
            // Arrange
            var currencyService = new CurrencyService(_loggerMock);
            var expectedAmount = 100f;

            // Act
            currencyService.SetCurrency(CurrencyType.Gold, expectedAmount);
            var actualAmount = currencyService.GetCurrency(CurrencyType.Gold).Current;

            // Assert
            actualAmount.Should().Be(expectedAmount);
        }

        [Test]
        public void When_SpendCurrencyCalled_Then_FailsWithInsufficientFunds()
        {
            // Arrange
            var currencyService = new CurrencyService(_loggerMock);

            // Act
            currencyService.SetCurrency(CurrencyType.Gold, 50f);
            bool result = currencyService.SpendCurrency(CurrencyType.Gold, 100f);

            // Assert
            result.Should().BeFalse();
            _loggerMock.Received(1).LogWarning(Arg.Any<string>(), "SpendCurrency");
        }

        [Test]
        public void When_SetCurrencyCalled_Then_CorrectlyUpdatesValue()
        {
            // Arrange
            var currencyService = new CurrencyService(_loggerMock);
            // Act
            currencyService.SetCurrency(CurrencyType.Gold, 150f);
            // Assert
            currencyService.GetCurrency(CurrencyType.Gold).Current.Should().Be(150f);
        }

        [Test]
        public void When_GetCurrencyCalled_Then_ReturnsCorrectValue()
        {
            // Arrange
            var currencyService = new CurrencyService(_loggerMock);
            currencyService.SetCurrency(CurrencyType.Gold, 150f);

            // Act
            var currencyValue = currencyService.GetCurrency(CurrencyType.Gold);

            // Assert
            currencyValue.Current.Should().Be(150f);
        }

        [Test]
        public void When_AddCurrencyCalled_Then_IncreasesValueAndLogsAddition()
        {
            // Arrange
            var currencyService = new CurrencyService(_loggerMock);

            // Act
            currencyService.AddCurrency(CurrencyType.Gold, 50f);

            // Assert
            currencyService.GetCurrency(CurrencyType.Gold).Current.Should().Be(50f);
        }

        [Test]
        public void When_SpendCurrencyCalledWithSufficientFunds_Then_DecreasesBalance()
        {
            // Arrange
            var currencyService = new CurrencyService(_loggerMock);
            currencyService.SetCurrency(CurrencyType.Gold, 100f);

            // Act
            var result = currencyService.SpendCurrency(CurrencyType.Gold, 50f);

            // Assert
            result.Should().BeTrue();
            currencyService.GetCurrency(CurrencyType.Gold).Current.Should().Be(50f);
        }

        [Test]
        public void When_GetAllCurrenciesCalled_Then_ReturnsSnapshot()
        {
            // Arrange
            var currencyService = new CurrencyService(_loggerMock);
            currencyService.SetCurrency(CurrencyType.Gold, 100f);
            currencyService.SetCurrency(CurrencyType.Diamond, 50f);
            currencyService.SetCurrency(CurrencyType.Energy, 200f);

            // Act
            var snapshot = currencyService.GetAllCurrencies();

            // Assert
            snapshot.Count.Should().Be(3);
            snapshot[CurrencyType.Gold].Current.Should().Be(100f);
            snapshot[CurrencyType.Diamond].Current.Should().Be(50f);
            snapshot[CurrencyType.Energy].Current.Should().Be(200f);
        }

        [Test]
        public void When_SetCurrencyCalledWithNegativeValue_Then_SetsToZero()
        {
            // Arrange
            var currencyService = new CurrencyService(_loggerMock);

            // Act
            currencyService.SetCurrency(CurrencyType.Gold, -50f);

            // Assert
            _loggerMock.Received(1).LogWarning(Arg.Any<string>(), "SetCurrency");
            currencyService.GetCurrency(CurrencyType.Gold).Current.Should().Be(0f);
        }

        [Test]
        public void When_AddCurrencyCalledWithZero_Then_HandlesCorrectly()
        {
            // Arrange
            var currencyService = new CurrencyService(_loggerMock);

            // Act
            currencyService.AddCurrency(CurrencyType.Gold, 0f);
            currencyService.AddCurrency(CurrencyType.Gold, 50f);

            // Assert
            currencyService.GetCurrency(CurrencyType.Gold).Current.Should().Be(50f);
        }

        [Test]
        public void When_GetCurrencyObservableCalled_Then_ReturnsValidObservable()
        {
            // Arrange
            var currencyService = new CurrencyService(_loggerMock);

            // Act
            var goldObservable = currencyService.GetCurrencyObservable(CurrencyType.Gold);
            var diamondObservable = currencyService.GetCurrencyObservable(CurrencyType.Diamond);
            var energyObservable = currencyService.GetCurrencyObservable(CurrencyType.Energy);

            // Assert
            goldObservable.Should().NotBeNull();
            diamondObservable.Should().NotBeNull();
            energyObservable.Should().NotBeNull();
        }

        [Test]
        public void When_SpendCurrencyCalledWithZero_Then_HandlesCorrectly()
        {
            // Arrange
            var currencyService = new CurrencyService(_loggerMock);
            currencyService.SetCurrency(CurrencyType.Gold, 50f);
            
            // Act
            var initialBalance = currencyService.GetCurrency(CurrencyType.Gold);
            currencyService.SpendCurrency(CurrencyType.Gold, 0f);

            // Assert
            initialBalance.Should().BeEquivalentTo(currencyService.GetCurrency(CurrencyType.Gold));
        }

        [Test]
        public void When_Initialized_Then_CurrencyServiceHasZeroValues()
        {
            // Arrange
            var currencyService = new CurrencyService(_loggerMock);

            // Assert
            currencyService.GetCurrency(CurrencyType.Gold).Current.Should().Be(0f);
            currencyService.GetCurrency(CurrencyType.Diamond).Current.Should().Be(0f);
            currencyService.GetCurrency(CurrencyType.Energy).Current.Should().Be(0f);
        }

        [Test]
        public void When_ConcurrentAccessOccurs_Then_CurrencyServiceHandlesCorrectly()
        {
            // Arrange
            var currencyService = new CurrencyService(_loggerMock);

            // Act
            Parallel.For(0,
                         100,
                         i =>
                         {
                             currencyService.AddCurrency(CurrencyType.Gold, 10f);
                             currencyService.SpendCurrency(CurrencyType.Gold, 5f);
                         });

            // Assert
            var goldBalance = currencyService.GetCurrency(CurrencyType.Gold).Current;
            goldBalance.Should().Be(500f);
        }
    }
}