using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Extensions.Console.Interfaces;
using FluentAssertions;
using Meta.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Systems.SaveSystem;
using Systems.SaveSystem.Interfaces;
using UnityEngine.TestTools;

namespace Tests.UnitTests.PlayMode
{
    [TestFixture]
    public class ConflictResolutionServiceTests
    {
        private ISaveService _mockLocalSaveService;
        private ISaveService _mockCloudSaveService;
        private IDataHashingService _mockHashingService;
        private IDebugLogger _mockLogger;
        private DataConflictResolutionService _conflictResolutionService;

        [SetUp]
        public void SetUp()
        {
            _mockLocalSaveService = Substitute.For<ISaveService>();
            _mockCloudSaveService = Substitute.For<ISaveService>();
            _mockHashingService = Substitute.For<IDataHashingService>();
            _mockLogger = Substitute.For<IDebugLogger>();

            _conflictResolutionService = new DataConflictResolutionService(
                _mockLocalSaveService,
                _mockCloudSaveService,
                _mockLogger,
                _mockHashingService
            );
        }

        [UnityTest]
        public IEnumerator When_ResolveConflictIsCalled_Then_ReturnMostRecentData()
            => UniTask.ToCoroutine(async () =>
                                   {
                                       // Arrange
                                       var key = "testKey";
                                       var defaultValue = "defaultData";
                                       var localData = new SaveData<string>("localData", DateTime.UtcNow.AddMinutes(-5), "localHash");
                                       var cloudData = new SaveData<string>("cloudData", DateTime.UtcNow, "cloudHash");

                                       // Mock hashing service to simulate tampering check (both valid)
                                       _mockHashingService.IsDataTampered(localData.Data, localData.Hash).Returns(false);
                                       _mockHashingService.IsDataTampered(cloudData.Data, cloudData.Hash).Returns(false);

                                       // Act
                                       var result = await _conflictResolutionService.ResolveConflict(key, defaultValue, localData, cloudData);

                                       // Assert: Cloud data is more recent, so local save should be updated with cloud data
                                       await _mockLocalSaveService.Received(1).Save(key, cloudData);
                                       await _mockCloudSaveService.DidNotReceive().Save(key, Arg.Any<SaveData<string>>());

                                       result.Should().Be(cloudData.Data);

                                       // Modify local data to be more recent
                                       localData = new SaveData<string>("localData", DateTime.UtcNow.AddMinutes(5), "localHash");

                                       // Act again with local data more recent
                                       result = await _conflictResolutionService.ResolveConflict(key, defaultValue, localData, cloudData);

                                       // Assert: Local data is now used, cloud save should be updated with local data
                                       await _mockCloudSaveService.Received(1).Save(key, localData);
                                       await _mockLocalSaveService.Received(1).Save(key, cloudData); // Local save for cloud data should be called only once
                                       result.Should().Be(localData.Data);
                                   });

        [UnityTest]
        public IEnumerator When_DataIsInvalid_Then_ResolveConflictHandlesCorrectly ()
            => UniTask.ToCoroutine(async () =>
                                   {
                                       // Arrange
                                       var key = "testKey";
                                       var defaultValue = "defaultData";
                                       var localData = new SaveData<string>("localData", DateTime.UtcNow, "localHash");
                                       var cloudData = new SaveData<string>("cloudData", DateTime.UtcNow, "cloudHash");

                                       // Setup: Only cloud data is valid (local data tampered)
                                       _mockHashingService.IsDataTampered(localData.Data, localData.Hash).Returns(true);
                                       _mockHashingService.IsDataTampered(cloudData.Data, cloudData.Hash).Returns(false);

                                       // Act
                                       var result = await _conflictResolutionService.ResolveConflict(key, defaultValue, localData, cloudData);

                                       // Assert: Cloud data is used since local is invalid
                                       result.Should().Be(cloudData.Data);
                                       await _mockLocalSaveService.Received(1).Save(key, cloudData);
                                       
                                       // Reset calls before testing the next case
                                       _mockLocalSaveService.ClearReceivedCalls();
                                       _mockCloudSaveService.ClearReceivedCalls();
                                       
                                       // Setup: Both data are invalid
                                       _mockHashingService.IsDataTampered(cloudData.Data, cloudData.Hash).Returns(true);

                                       // Act: Test when both data are invalid
                                       result = await _conflictResolutionService.ResolveConflict(key, defaultValue, localData, cloudData);

                                       // Assert: Ensure no save operation is called and default value is returned
                                       result.Should().Be(defaultValue);
                                       await _mockLocalSaveService.DidNotReceive().Save(key, Arg.Any<SaveData<string>>());
                                       await _mockCloudSaveService.DidNotReceive().Save(key, Arg.Any<SaveData<string>>());
                                   });
    }
}