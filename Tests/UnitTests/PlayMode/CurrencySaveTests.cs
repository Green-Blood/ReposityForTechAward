using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Extensions.Console.Interfaces;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Systems.Currency;
using Systems.Currency.Interfaces;
using Systems.SaveSystem;
using Systems.SaveSystem.Interfaces;
using UnityEngine.TestTools;

namespace Tests.UnitTests.PlayMode
{
    [TestFixture]
    public class CurrencySaveTests
    {
        [UnityTest]
        public IEnumerator When_SaveCurrenciesCalled_Then_StoresWithCorrectKey()
            => UniTask.ToCoroutine(async () =>
                                   {
                                       // Arrange
                                       var mockCurrencyService = Substitute.For<ICurrencyService>();
                                       var mockSaveService = Substitute.For<ISaveService>();
                                       var mockLogger = Substitute.For<IDebugLogger>();
                                       var currencySaveLoadService = new CurrencySaveLoadService(mockCurrencyService, mockSaveService, mockLogger);

                                       var currencies = new Dictionary<CurrencyType, Currency>();
                                       currencies.Add(CurrencyType.Energy, new Currency(200f, 100f));
                                       currencies.Add(CurrencyType.Diamond, new Currency(50f));
                                       currencies.Add(CurrencyType.Gold, new Currency(100f));

                                       mockCurrencyService.GetAllCurrencies().Returns(currencies);

                                       // Act
                                       currencySaveLoadService.SaveCurrencies();

                                       // Assert
                                       await mockSaveService.Received(1).Save(Arg.Any<string>(), Arg.Any<Dictionary<CurrencyType, CurrencyData>>());
                                       var expectedLogMessage = "All currencies saved with current and max values: Energy: 200/100, Diamond: 50, Gold: 100";

                                       mockLogger.Received(1).Log(expectedLogMessage, "LogSave");
                                   });

        [UnityTest]
        public IEnumerator When_NoDataSaved_Then_LoadCurrencies_ReturnsDefaultValues()
            => UniTask.ToCoroutine(async () =>
                                   {
                                       // Arrange
                                       var mockCurrencyService = Substitute.For<ICurrencyService>();
                                       var mockSaveService = Substitute.For<ISaveService>();
                                       var mockLogger = Substitute.For<IDebugLogger>();
                                       var currencySaveLoadService = new CurrencySaveLoadService(mockCurrencyService, mockSaveService, mockLogger);

                                       mockSaveService.Load(Arg.Any<string>(), Arg.Any<Dictionary<CurrencyType, CurrencyData>>())
                                           .Returns(UniTask.FromResult(default(Dictionary<CurrencyType, CurrencyData>)));

                                       // Act
                                       var result = await currencySaveLoadService.LoadCurrencies();

                                       // Assert
                                       result[CurrencyType.Gold].Value.Current.Should().Be(0f);
                                       result[CurrencyType.Diamond].Value.Current.Should().Be(0f);
                                       result[CurrencyType.Energy].Value.Current.Should().Be(0f);
                                       mockLogger.Received(1).Log("No saved currencies found. Initialized all currencies with default values.", "LoadCurrencies");
                                       mockLogger.Received(1).Log("All currencies with current and max values loaded.", "LoadCurrencies");
                                   });

        [UnityTest]
        public IEnumerator When_SaveCurrencyCalled_Then_StoresWithUniqueKey()
            => UniTask.ToCoroutine(async () =>
                                   {
                                       // Arrange
                                       var mockCurrencyService = Substitute.For<ICurrencyService>();
                                       var mockSaveService = Substitute.For<ISaveService>();
                                       var mockLogger = Substitute.For<IDebugLogger>();
                                       var currencySaveLoadService = new CurrencySaveLoadService(mockCurrencyService, mockSaveService, mockLogger);

                                       CurrencyType currencyType = CurrencyType.Gold;
                                       var currency = new Currency(100f);

                                       mockCurrencyService.GetCurrency(currencyType).Returns(currency);

                                       // Act
                                       currencySaveLoadService.SaveCurrency(currencyType);

                                       // Assert
                                       await mockSaveService.Received(1).Save($"Currencies_{currencyType}", Arg.Any<CurrencyData>());
                                       mockLogger.Received(1).Log($"{currencyType} currency saved: {currency.Current}/{currency.Max}", "SaveCurrency");
                                   });

        [UnityTest]
        public IEnumerator When_NoCurrenciesSaved_Then_LoadCurrencies_ReturnsDefaultValues()
            => UniTask.ToCoroutine(async () =>
                                   {
                                       // Arrange
                                       var mockCurrencyService = Substitute.For<ICurrencyService>();
                                       var mockSaveService = Substitute.For<ISaveService>();
                                       var mockLogger = Substitute.For<IDebugLogger>();
                                       var currencySaveLoadService = new CurrencySaveLoadService(mockCurrencyService, mockSaveService, mockLogger);

                                       Dictionary<CurrencyType, CurrencyData> savedCurrencies = null;
                                       mockSaveService.Load(Arg.Any<string>(), Arg.Any<Dictionary<CurrencyType, CurrencyData>>())
                                           .Returns(UniTask.FromResult(savedCurrencies));

                                       // Act
                                       var loadedCurrencies = await currencySaveLoadService.LoadCurrencies();

                                       // Assert
                                       loadedCurrencies[CurrencyType.Gold].Value.Current.Should().Be(0f);
                                       loadedCurrencies[CurrencyType.Diamond].Value.Current.Should().Be(0f);
                                       loadedCurrencies[CurrencyType.Energy].Value.Current.Should().Be(0f);
                                       mockLogger.Received(1).Log("No saved currencies found. Initialized all currencies with default values.", "LoadCurrencies");
                                       mockLogger.Received(1).Log("All currencies with current and max values loaded.", "LoadCurrencies");
                                   });

        [UnityTest]
        public IEnumerator When_LoadCurrencyCalled_Then_ReturnsCorrectAmount()
            => UniTask.ToCoroutine(async () =>
                                   {
                                       // Arrange
                                       var mockCurrencyService = Substitute.For<ICurrencyService>();
                                       var mockSaveService = Substitute.For<ISaveService>();
                                       var mockLogger = Substitute.For<IDebugLogger>();
                                       var currencySaveLoadService = new CurrencySaveLoadService(mockCurrencyService, mockSaveService, mockLogger);

                                       var loadedCurrencies = new Dictionary<CurrencyType, CurrencyData>
                                       {
                                           {
                                               CurrencyType.Gold, new CurrencyData(100f)
                                           },
                                           {
                                               CurrencyType.Diamond, new CurrencyData(50f)
                                           },
                                           {
                                               CurrencyType.Energy, new CurrencyData(200f, 100f)
                                           }
                                       };

                                       mockSaveService.Load("Currencies", Arg.Any<Dictionary<CurrencyType, CurrencyData>>())
                                           .Returns(UniTask.FromResult(loadedCurrencies));

                                       // Act
                                       var result = await currencySaveLoadService.LoadCurrencies();

                                       // Assert
                                       result[CurrencyType.Gold].Value.Current.Should().Be(100f);
                                       result[CurrencyType.Diamond].Value.Current.Should().Be(50f);
                                       result[CurrencyType.Energy].Value.Current.Should().Be(200f);
                                       mockLogger.Received(1).Log("All currencies with current and max values loaded.", "LoadCurrencies");
                                   });

        [UnityTest]
        public IEnumerator When_SaveCurrencyCalledWithInvalidType_Then_ShouldNotThrowError()
            => UniTask.ToCoroutine(async () =>
                                   {
                                       // Arrange
                                       var mockCurrencyService = Substitute.For<ICurrencyService>();
                                       var mockSaveService = Substitute.For<ISaveService>();
                                       var mockLogger = Substitute.For<IDebugLogger>();
                                       var currencySaveLoadService = new CurrencySaveLoadService(mockCurrencyService, mockSaveService, mockLogger);

                                       CurrencyType invalidCurrencyType = (CurrencyType)99; // Invalid currency type

                                       // Act & Assert
                                       currencySaveLoadService.Invoking(x => x.SaveCurrency(invalidCurrencyType))
                                           .Should().NotThrow();
                                       await UniTask.CompletedTask;
                                   });

        [UnityTest]
        public IEnumerator When_LoadCurrencyCalledWithInvalidType_Then_ReturnsDefaultValue()
            => UniTask.ToCoroutine(async () =>
                                   {
                                       // Arrange
                                       var mockCurrencyService = Substitute.For<ICurrencyService>();
                                       var mockSaveService = Substitute.For<ISaveService>();
                                       var mockLogger = Substitute.For<IDebugLogger>();
                                       var currencySaveLoadService = new CurrencySaveLoadService(mockCurrencyService, mockSaveService, mockLogger);

                                       CurrencyType invalidCurrencyType = (CurrencyType)99;

                                       // Act
                                       var result = await currencySaveLoadService.LoadCurrency(invalidCurrencyType);

                                       // Assert
                                       result.Current.Should().Be(0f);
                                       mockLogger.Received(1).Log("Unknown currency type encountered.", "LoadCurrency");
                                       mockLogger.Received(1).Log($"{invalidCurrencyType} not found. Initialized to default values.", "LoadCurrency");
                                       mockLogger.Received(1).Log($"{invalidCurrencyType} currency loaded: 0/", "LoadCurrency");
                                   });

        [UnityTest]
        public IEnumerator When_NullServices_Then_HandleGracefully()
            => UniTask.ToCoroutine(async () =>
                                   {
                                       // Arrange
                                       var mockCurrencyService = Substitute.For<ICurrencyService>();
                                       var mockSaveService = Substitute.For<ISaveService>();
                                       var mockLogger = Substitute.For<IDebugLogger>();
                                       var currencySaveLoadService = new CurrencySaveLoadService(mockCurrencyService, mockSaveService, mockLogger);

                                       mockCurrencyService.GetAllCurrencies().Returns((Dictionary<CurrencyType, Currency>)null);

                                       // Act & Assert
                                       currencySaveLoadService
                                           .Invoking(x => x.SaveCurrencies())
                                           .Should().NotThrow();
                                       
                                       mockLogger.Received(1).Log("No currencies available to save.", "SaveCurrencies");
                                       await UniTask.CompletedTask;
                                   });

        [UnityTest]
        public IEnumerator When_LoadCurrenciesCalled_Then_ReactivePropertiesInitialized()
            => UniTask.ToCoroutine(async () =>
                                   {
                                       // Arrange
                                       var mockCurrencyService = Substitute.For<ICurrencyService>();
                                       var mockSaveService = Substitute.For<ISaveService>();
                                       var mockLogger = Substitute.For<IDebugLogger>();
                                       var currencySaveLoadService = new CurrencySaveLoadService(mockCurrencyService, mockSaveService, mockLogger);

                                       var savedCurrencies = new Dictionary<CurrencyType, CurrencyData>
                                       {
                                           {
                                               CurrencyType.Gold, new CurrencyData(100f)
                                           },
                                           {
                                               CurrencyType.Diamond, new CurrencyData(50f)
                                           },
                                           {
                                               CurrencyType.Energy, new CurrencyData(200f, 100f)
                                           }
                                       };

                                       mockSaveService.Load("Currencies", Arg.Any<Dictionary<CurrencyType, CurrencyData>>())
                                           .Returns(UniTask.FromResult(savedCurrencies));


                                       // Act
                                       var loadedReactiveProperties = await currencySaveLoadService.LoadCurrencies();

                                       // Assert
                                       loadedReactiveProperties[CurrencyType.Gold].Value.Current.Should().Be(100f);
                                       loadedReactiveProperties[CurrencyType.Diamond].Value.Current.Should().Be(50f);
                                       loadedReactiveProperties[CurrencyType.Energy].Value.Current.Should().Be(200f);
                                       mockLogger.Received(1).Log("All currencies with current and max values loaded.", "LoadCurrencies");
                                   });
    }
}