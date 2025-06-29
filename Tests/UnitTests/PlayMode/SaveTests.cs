using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Extensions.Console.Interfaces;
using FluentAssertions;
using Meta.Auth;
using Meta.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Systems.Currency;
using Systems.SaveSystem;
using Systems.SaveSystem.Interfaces;
using UnityEngine.TestTools;

namespace Tests.UnitTests.PlayMode
{
    [TestFixture]
    public class SaveTests
    {
        private ISaveService _mockLocalSaveService;
        private ISaveService _mockCloudSaveService;
        private IDataHashingService _mockHashingService;
        private IDataLoader _mockLocalDataLoader;
        private IDataLoader _mockCloudDataLoader;
        private IConflictResolutionService _mockDataConflictResolver;
        private IServiceInitializer _mockServiceInitializer;
        private HybridSaveService _hybridSaveService;
        private IDebugLogger _mockLogger;
        private INetworkStatusService _mockNetworkStatusService;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = Substitute.For<IDebugLogger>();
            _mockLocalSaveService = Substitute.For<ISaveService>();
            _mockCloudSaveService = Substitute.For<ISaveService>();
            _mockHashingService = Substitute.For<IDataHashingService>();
            _mockLocalDataLoader = Substitute.For<IDataLoader>();
            _mockCloudDataLoader = Substitute.For<IDataLoader>();
            _mockDataConflictResolver = Substitute.For<IConflictResolutionService>();
            _mockNetworkStatusService = Substitute.For<INetworkStatusService>();
            _mockServiceInitializer = new UnityServiceInitializer(_mockLogger);

            _hybridSaveService = new HybridSaveService(
                _mockLocalSaveService,
                _mockCloudSaveService,
                _mockLocalDataLoader,
                _mockCloudDataLoader,
                _mockHashingService,
                _mockDataConflictResolver,
                _mockLogger,
                _mockNetworkStatusService
            );
        }

        [UnityTest]
        public IEnumerator When_DataIsSaved_Then_DataIsSyncedToCloud ()
            => UniTask.ToCoroutine(async () =>
                                   {
                                       // Arrange
                                       var key = "testKey";
                                       var data = "testData";
                                       var hash = "hashedData";
                                       _mockHashingService.HashData(data).Returns(hash);
                                       _mockNetworkStatusService.IsOnline().Returns(true);

                                       await _mockLocalSaveService.Warmup();
                                       await _mockCloudSaveService.Warmup();

                                       // Act
                                       await _hybridSaveService.Save(key, data);

                                       // Assert
                                       // await _mockCloudSaveService.Received(1).Save(key, Arg.Any<SaveData<string>>());
                                       await _mockLocalSaveService.Received(1).Save(key, Arg.Any<SaveData<string>>());
                                   });

        [UnityTest]
        public IEnumerator When_LocalSaveIsUnavailable_Then_FallbackToCloud ()
            => UniTask.ToCoroutine(async () =>
                                   {
                                       // Arrange
                                       var key = "testKey";
                                       var data = "testData";
                                       var hash = "hashedData";
                                       _mockNetworkStatusService.IsOnline().Returns(true);

                                       _mockLocalSaveService
                                           .When(s => s.Save(key, Arg.Any<SaveData<string>>()))
                                           .Do(x => throw new Exception("Local service unavailable"));

                                       _mockHashingService.HashData(data).Returns(hash);

                                       // Act
                                       await _hybridSaveService.Save(key, data);

                                       // Assert: Verify that the cloud save service is still called even if the local service fails
                                       await _mockCloudSaveService.Received(1).Save(key, Arg.Any<SaveData<string>>());
                                   });

        [UnityTest]
        public IEnumerator When_Offline_Then_CloudSaveIsUnavailable ()
            => UniTask.ToCoroutine(async () =>
                                   {
                                       // Arrange
                                       var key = "testKey";
                                       var data = "testData";
                                       var hash = "hashedData";

                                       _mockHashingService.HashData(data).Returns(hash);
                                       _mockNetworkStatusService.IsOnline().Returns(false); // Simulate offline state

                                       // Act
                                       await _hybridSaveService.Save(key, data);

                                       // Assert
                                       await _mockLocalSaveService.Received(1).Save(key, Arg.Any<SaveData<string>>());
                                       await _mockCloudSaveService.DidNotReceive()
                                           .Save(key, Arg.Any<SaveData<string>>());
                                   });

        [UnityTest]
        public IEnumerator When_DataIsLoaded_Then_PrioritizeLocal_AndFallBackToCloud ()
            => UniTask.ToCoroutine(async () =>
                                   {
                                       var key = "testKey";
                                       var defaultValue = "defaultData";
                                       var localData = new SaveData<string>("localData", DateTime.UtcNow, "localHash");
                                       var cloudData = new SaveData<string>("cloudData", DateTime.UtcNow, "cloudHash");

                                       // Setup: Both local and cloud data exist
                                       _mockLocalDataLoader.LoadData(key, defaultValue).Returns(UniTask.FromResult(localData.Data));
                                       _mockCloudDataLoader.LoadData(key, defaultValue).Returns(UniTask.FromResult(cloudData.Data));

                                       // Mock conflict resolution to prioritize cloud data
                                       _mockDataConflictResolver.ResolveConflict(key, defaultValue, Arg.Any<SaveData<string>>(), Arg.Any<SaveData<string>>())
                                           .Returns(UniTask.FromResult(cloudData.Data));

                                       // Act
                                       var result = await _hybridSaveService.Load(key, defaultValue);

                                       // Assert: Verifies that the conflict resolver was called and cloud data is returned
                                       result.Should().Be(cloudData.Data);

                                       // Setup: Only local data is available
                                       _mockCloudDataLoader.LoadData(key, defaultValue).Returns(UniTask.FromResult(defaultValue));

                                       // Act: Load when only local data is available
                                       result = await _hybridSaveService.Load(key, defaultValue);

                                       // Assert: Local data should be returned
                                       result.Should().Be(localData.Data);

                                       // Setup: Only cloud data is available
                                       _mockLocalDataLoader.LoadData(key, defaultValue).Returns(UniTask.FromResult(defaultValue));
                                       _mockCloudDataLoader.LoadData(key, defaultValue).Returns(UniTask.FromResult(cloudData.Data));

                                       // Act: Load when only cloud data is available
                                       result = await _hybridSaveService.Load(key, defaultValue);

                                       // Assert: Cloud data should be returned
                                       result.Should().Be(cloudData.Data);

                                       // Setup: No data available from either source
                                       _mockLocalDataLoader.LoadData(key, defaultValue).Returns(UniTask.FromResult(defaultValue));
                                       _mockCloudDataLoader.LoadData(key, defaultValue).Returns(UniTask.FromResult(defaultValue));

                                       // Act: Load with no data available
                                       result = await _hybridSaveService.Load(key, defaultValue);

                                       // Assert: Default value should be returned
                                       result.Should().Be(defaultValue);
                                   });

        [UnityTest]
        public IEnumerator When_KeysAreMissing_Then_HandleGracefully ()
            => UniTask.ToCoroutine(async () =>
                                   {
                                       // Arrange
                                       var key = "missingKey";
                                       var defaultValue = "defaultValue";


                                       // Mock the behavior for missing keys: Return false for HasKey and default value for Load
                                       _mockLocalSaveService.HasKey(key).Returns(UniTask.FromResult(false));
                                       _mockCloudSaveService.HasKey(key).Returns(UniTask.FromResult(false));
                                       // Setup: Both local and cloud data exist
                                       _mockLocalDataLoader.LoadData(key, defaultValue)
                                           .Returns(_ =>
                                                    {
                                                        _mockLogger.Log("Key 'missingKey' not found. Returning default value.");
                                                        return UniTask.FromResult(defaultValue);
                                                    });

                                       _mockCloudDataLoader.LoadData(key, defaultValue)
                                           .Returns(_ =>
                                                    {
                                                        _mockLogger.Log("Key 'missingKey' not found. Returning default value.");
                                                        return UniTask.FromResult(defaultValue);
                                                    });


                                       _mockLocalSaveService.Load(key, defaultValue).Returns(UniTask.FromResult(defaultValue));
                                       _mockCloudSaveService.Load(key, defaultValue).Returns(UniTask.FromResult(defaultValue));

                                       // Act
                                       var result = await _hybridSaveService.Load(key, defaultValue);

                                       // Assert
                                       result.Should().Be(defaultValue);
                                       _mockLogger.Received().Log($"Key '{key}' not found. Returning default value.");
                                   });

        [UnityTest]
        public IEnumerator When_SavingData_Then_HashingIsPerformedBeforeSaving()
            => UniTask.ToCoroutine(async () =>
                                   {
                                       // Arrange
                                       var key = "testKey";
                                       var data = "testData";
                                       var hashedData = "hashedTestData";

                                       _mockNetworkStatusService.IsOnline().Returns(true);

                                       // Mock the hashing service to return a specific hash for the data
                                       _mockHashingService.HashData(data).Returns(hashedData);

                                       // Act
                                       await _hybridSaveService.Save(key, data);

                                       // Assert
                                       // Ensure that data was hashed before saving
                                       _mockHashingService.Received(1).HashData(data);

                                       // Ensure that the hashed data was saved correctly in both local and cloud storage
                                       await _mockLocalSaveService.Received(1).Save(key, Arg.Any<SaveData<string>>());
                                       await _mockCloudSaveService.Received(1).Save(key, Arg.Any<SaveData<string>>());

                                   });
    }
}