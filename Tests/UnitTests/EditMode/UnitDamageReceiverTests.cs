using Extensions.Console.Interfaces;
using FluentAssertions;
using Game.Core.StaticData;
using Game.GamePlay.Character.Base.Attack.Damage;
using Game.GamePlay.Character.Base.Health;
using Game.GamePlay.Character.Base.Health.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Systems.StatSystem;
using Systems.StatSystem.Interfaces;
using Systems.StatSystem.StatTypes;

namespace Tests.EditMode
{
    public class UnitDamageReceiverTests
    {
        [Test]
        public void WhenCharacterTakesDamage_ThenHealthDecreases()
        {
            // Arrange
            IStatCollection statCollectionMock = Substitute.For<IStatCollection>();

            var healthStat = CreateHealthStat(statCollectionMock);

            var unitDeathMock = Substitute.For<IDie>();
            var logger = Substitute.For<IDebugLogger>();
            var damageReceiver = new UnitDamageReceiver(unitDeathMock, statCollectionMock, logger);

            // Act
            damageReceiver.TakeDamage(new HitData(),20);
            // Assert
            healthStat.StatCurrentValue.Should().Be(80);
        }

        [Test]
        public void WhenCharacterHealthIsLessThan0_ThenCharacterDies()
        {
            // Arrange
            IStatCollection statCollectionMock = Substitute.For<IStatCollection>();

            CreateHealthStat(statCollectionMock);

            var unitDeathMock = Substitute.For<IDie>();
            var logger = Substitute.For<IDebugLogger>();
            var damageReceiver = new UnitDamageReceiver(unitDeathMock, statCollectionMock, logger);

            // Act
            damageReceiver.TakeDamage(new HitData(),120);
            // Assert
            unitDeathMock.Received(1).Die();
        }

        [Test]
        public void WhenCharacterReceivesMinusDamage_ThenExceptionWillOccur()
        {
            // Arrange
            IStatCollection statCollectionMock = Substitute.For<IStatCollection>();
            var healthStat = CreateHealthStat(statCollectionMock);

            var unitDeathMock = Substitute.For<IDie>();
            var logger = Substitute.For<IDebugLogger>();
            var damageReceiver = new UnitDamageReceiver(unitDeathMock, statCollectionMock, logger);

            // Act
            damageReceiver.TakeDamage( new HitData(),-120);

            // Assert
            healthStat.StatCurrentValue.Should().Be(100);
        }

        private static Vital CreateHealthStat(IStatCollection statCollectionMock)
        {
            var healthStat = new Vital
            {
                StatBaseValue = 100
            };
            statCollectionMock.CreateOrGetStat<Vital>(StatType.Health).Returns(healthStat);
            statCollectionMock.TryGetStat<Vital>(StatType.Health).Returns(healthStat);
            healthStat.StatCurrentValue = healthStat.StatBaseValue;
            return healthStat;
        }
    }
}
