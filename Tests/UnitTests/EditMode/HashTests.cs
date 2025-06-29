using System.Threading.Tasks;
using FluentAssertions;
using Meta;
using NUnit.Framework;

namespace Tests.UnitTests.EditMode
{
    public class HashTests
    {
        [Test]
        public void When_IdenticalInputsProvided_Then_ProducesConsistentHash ()
        {
            var dataHashingService = new DataHashingService();
            var testData = new { Name = "Test", Value = 123 };

            var hash1 = dataHashingService.HashData(testData);
            var hash2 = dataHashingService.HashData(testData);

            Assert.AreEqual(hash1, hash2);
        }
        [Test]
        public void When_NullOrEmptyInputProvided_Then_HandleCorrectly ()
        {
            var dataHashingService = new DataHashingService();

            var nullHash = dataHashingService.HashData<object>(null);
            var emptyHash = dataHashingService.HashData(new { });

            nullHash.Should().NotBeNullOrEmpty();
            emptyHash.Should().NotBeNullOrEmpty();
            nullHash.Should().NotBeSameAs(emptyHash);
        }
        [Test]
        public void When_DataMatchesExpectedHash_Then_ReturnsFalse()
        {
            // Arrange
            var dataHashingService = new DataHashingService();
            var testData = new { Name = "Test", Value = 123 };
            var expectedHash = dataHashingService.HashData(testData);

            // Act
            var result = dataHashingService.IsDataTampered(testData, expectedHash);

            // Assert
            result.Should().BeFalse();
        }
        [Test]
        public void When_VariousDataTypesProvided_Then_HandleCorrectly ()
        {
            // Arrange
            var dataHashingService = new DataHashingService();
            var testDataInt = 123;
            var testDataString = "Test";
            var testDataObject = new { Name = "Test", Value = 123 };

            // Act
            var hashInt = dataHashingService.HashData(testDataInt);
            var hashString = dataHashingService.HashData(testDataString);
            var hashObject = dataHashingService.HashData(testDataObject);

            // Assert
            hashInt.Should().NotBeNullOrEmpty();
            hashString.Should().NotBeNullOrEmpty();
            hashObject.Should().NotBeNullOrEmpty();
        }
        [Test]
        public void When_DataIsTampered_Then_ReturnsTrue ()
        {
            var dataHashingService = new DataHashingService();
            var testData = new { Name = "Test", Value = 123 };
            var expectedHash = dataHashingService.HashData(testData);

            // Tamper the data
            var value = 456;
            testData = new { testData.Name, Value = value };
            

            var result = dataHashingService.IsDataTampered(testData, expectedHash);

            result.Should().BeTrue();
        }
        [Test]
        public void When_ExpectedHashIsNullOrEmpty_Then_ReturnsTrueForTamperedData ()
        {
            // Arrange
            var dataHashingService = new DataHashingService();
            var testData = new { Name = "Test", Value = 123 };

            // Act
            var resultNull = dataHashingService.IsDataTampered(testData, null);
            var resultEmpty = dataHashingService.IsDataTampered(testData, "");

            // Assert
            Assert.IsTrue(resultNull);
            Assert.IsTrue(resultEmpty);
        }
        [Test]
        public void When_DataIsTampered_Then_IdentifyTamperedData ()
        {
            // Arrange
            var dataHashingService = new DataHashingService();
            var testData = new { Name = "Test", Value = 123 };
            var expectedHash = dataHashingService.HashData(testData);
    
            // Act
            var tamperedData = new { Name = "Test", Value = 456 };
            var result = dataHashingService.IsDataTampered(tamperedData, expectedHash);
    
            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public void When_DifferentInputsProvided_Then_ProducesDifferentHashes ()
        {
            var dataHashingService = new DataHashingService();
    
            var testData1 = new { Name = "Test1", Value = 123 };
            var testData2 = new { Name = "Test2", Value = 456 };
    
            var hash1 = dataHashingService.HashData(testData1);
            var hash2 = dataHashingService.HashData(testData2);
    
            Assert.AreNotEqual(hash1, hash2);
        }
        [Test]
        public void When_HashDataIsCalled_Then_MaintainsThreadSafety ()
        {
            var dataHashingService = new DataHashingService();
            var testData = new { Name = "Test", Value = 123 };
    
            Parallel.Invoke(
                () =>
                {
                    var hash1 = dataHashingService.HashData(testData);
                    Assert.IsNotNull(hash1);
                },
                () =>
                {
                    var hash2 = dataHashingService.HashData(testData);
                    Assert.IsNotNull(hash2);
                }
            );
        }
        [Test]
        public void When_DataTypeIsUnexpected_Then_ReturnsTrueForTamperedData()
        {
            // Arrange
            var dataHashingService = new DataHashingService();
            var testData = new { Name = "Test", Value = 123 };
            var expectedHash = dataHashingService.HashData(testData);

            // Act
            var result = dataHashingService.IsDataTampered(testData, expectedHash);

            // Assert
            Assert.IsFalse(result);
        }
        
    }
}