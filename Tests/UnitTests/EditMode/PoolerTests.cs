using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Extensions.Console.Interfaces;
using FluentAssertions;
using Game.Core.Interfaces;
using Game.Object_Pooler;
using Game.Object_Pooler.Interfaces;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Tests.UnitTests.EditMode
{
    public class PoolerTests
    {
        private ObjectPooler _objectPooler;
        private IAssetProvider _assetProvider;
        private DiContainer _container;
        private IDebugLogger _logger;

        [SetUp]
        public void SetUp()
        {
            // Mock dependencies
            _assetProvider = Substitute.For<IAssetProvider>();
            _container = Substitute.For<DiContainer>();
            _logger = Substitute.For<IDebugLogger>();

            // Initialize ObjectPooler
            _objectPooler = new GameObject().AddComponent<ObjectPooler>();
            _objectPooler.Construct(_assetProvider, _container, _logger);
        }

        [Test]
        public async Task When_WarmUpCalled_Then_ShouldInitializePools ()
        {
            // Arrange
            var enemyTag = ScriptableObject.CreateInstance<Tag>();
            var pooledObjectData = new ObjectPooler.PooledObjectData { name = "Enemy", tag = enemyTag };
            var assetReference = Substitute.For<AssetReference>();
            var pool = new ObjectPooler.Pool
            {
                pooledObjectData = pooledObjectData,
                reference = assetReference,
                size = 5
            };
            _objectPooler.pools = new List<ObjectPooler.Pool> { pool };

            var prefab = new GameObject("EnemyPrefab");
            _assetProvider.Load<GameObject>(assetReference).Returns(UniTask.FromResult(prefab));

            // Act
            await _objectPooler.WarmUp();

            // Assert
            _objectPooler.GetPoolSize(pooledObjectData).Should().Be(5);
        }
        [Test]
        public async Task When_SpawnFromPoolCalled_Then_ShouldSpawnInactiveObject ()
        {
            // Arrange
            var enemyTag = ScriptableObject.CreateInstance<Tag>();
            var pooledObjectData = new ObjectPooler.PooledObjectData { name = "Enemy", tag = enemyTag };
            var assetReference = Substitute.For<AssetReference>();
            var pool = new ObjectPooler.Pool
            {
                pooledObjectData = pooledObjectData,
                reference = assetReference,
                size = 5
            };
            _objectPooler.pools = new List<ObjectPooler.Pool> { pool };

            var prefab = new GameObject("EnemyPrefab");
            _assetProvider.Load<GameObject>(assetReference).Returns(UniTask.FromResult(prefab));

            await _objectPooler.WarmUp();

            // Act
            var spawnedObject = _objectPooler.SpawnFromPool(enemyTag, "Enemy", Vector3.zero, Quaternion.identity);

            // Assert
            spawnedObject.Should().NotBeNull();
            spawnedObject.activeSelf.Should().BeTrue();
        }
        [Test]
        public async Task When_PoolDepleted_Then_ShouldExpandPoolAndSpawnObject()
        {
            // Arrange
            var enemyTag = ScriptableObject.CreateInstance<Tag>();
            var pooledObjectData = new ObjectPooler.PooledObjectData { name = "Enemy", tag = enemyTag };
            var assetReference = Substitute.For<AssetReference>();
            var pool = new ObjectPooler.Pool
            {
                pooledObjectData = pooledObjectData,
                reference = assetReference,
                size = 1
            };

            _objectPooler.pools = new List<ObjectPooler.Pool> { pool };

            var prefab = new GameObject("EnemyPrefab");
            _assetProvider.Load<GameObject>(assetReference).Returns(UniTask.FromResult(prefab));

            
            await _objectPooler.WarmUp();
            // Set up the pool with one object, which will be depleted.
            _objectPooler.AddToPool(pooledObjectData, new Queue<GameObject>(new[] { prefab }));

            // Act
            var firstSpawned = _objectPooler.SpawnFromPool(enemyTag, "Enemy", Vector3.zero, Quaternion.identity);
            firstSpawned.SetActive(true); // Simulate object being used.
            var secondSpawned = _objectPooler.SpawnFromPool(enemyTag, "Enemy", Vector3.zero, Quaternion.identity);

            // Assert
            firstSpawned.Should().NotBeNull();
            secondSpawned.Should().NotBeNull();
            
            _objectPooler.GetPoolSize(pooledObjectData).Should().Be(4);
            _logger.Received().LogWarning(Arg.Is<string>(s => s.Contains("Pool depleted. Expanding the pool to 4 objects.")),"SpawnFromQueue");
            secondSpawned.SetActive(true); // Simulate object being used.
            
            var thirdSpawned = _objectPooler.SpawnFromPool(enemyTag, "Enemy", Vector3.zero, Quaternion.identity);
            thirdSpawned.Should().NotBeNull();
            _objectPooler.GetPoolSize(pooledObjectData).Should().Be(4);
            
            var fourthSpawned = _objectPooler.SpawnFromPool(enemyTag, "Enemy", Vector3.zero, Quaternion.identity);
            fourthSpawned.Should().NotBeNull();
            
            _objectPooler.GetPoolSize(pooledObjectData).Should().Be(8);
            _logger.Received().LogWarning(Arg.Is<string>(s => s.Contains("Pool depleted. Expanding the pool to 8 objects.")),"SpawnFromQueue");
        }
        [Test]
        public void When_DespawnCalled_Then_ShouldDeactivateObject ()
        {
            // Arrange
            var prefab = new GameObject("EnemyPrefab");
            var pooledObject = Substitute.For<IPooledObject>();

            // Act
            _objectPooler.Despawn(prefab, pooledObject);

            // Assert
            prefab.activeSelf.Should().BeFalse();
            pooledObject.Received().OnObjectDeSpawn();
        }
    }
}
