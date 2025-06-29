using FluentAssertions;
using Game.Core.Factories.Interfaces;
using Game.GamePlay.Character.Base.Attack.Damage;
using Game.GamePlay.Character.Base.DamageCalculators;
using NSubstitute;
using NUnit.Framework;
using Systems.StatSystem.Interfaces;
using Systems.StatSystem.StatTypes;

namespace Tests.UnitTests.EditMode
{
    public class DamageTests
    {
        [Test]
        public void When_PhysicalDamageDealt_Then_CalculateCorrectFinalDamageAmount ()
        {
            var hitData = new HitData { DamageDealt = 100 };
            var attacker = Substitute.For<IShared>();
            var defender = Substitute.For<IShared>();
            var statCollection = Substitute.For<IStatCollection>();
            var physicalResistance = Substitute.For<Attribute>();

            physicalResistance.StatValue.Returns(10);
            statCollection.CreateOrGetStat<Attribute>(StatType.PhysicalResistance)
                .ReturnsForAnyArgs(physicalResistance);
            defender.StatCollection.Returns(statCollection);

            var calculator = new PhysicalDamageCalculator();
            var damage = calculator.CalculateDamage(hitData.DamageDealt, attacker, defender);
            
            damage.Should().Be(90);
        }
        [Test]
        public void When_PhysicalDamageIsNegative_Then_CalculateCorrectFinalDamageAmount ()
        {
            var hitData = new HitData { DamageDealt = -100 };
            var attacker = Substitute.For<IShared>();
            var defender = Substitute.For<IShared>();
            var statCollection = Substitute.For<IStatCollection>();
            var physicalResistance = Substitute.For<Attribute>();

            physicalResistance.StatValue.Returns(10);
            statCollection.CreateOrGetStat<Attribute>(StatType.PhysicalResistance)
                .ReturnsForAnyArgs(physicalResistance);
            defender.StatCollection.Returns(statCollection);

            var calculator = new PhysicalDamageCalculator();
            var damage = calculator.CalculateDamage(hitData.DamageDealt, attacker, defender);
            
            damage.Should().Be(0);
        }
        [Test]
        public void When_MagicalDamageDealt_Then_CalculateCorrectFinalDamageAmount ()
        {
            var hitData = new HitData { DamageDealt = 100 };
            var attacker = Substitute.For<IShared>();
            var defender = Substitute.For<IShared>();
            var statCollection = Substitute.For<IStatCollection>();
            var magicalResistance = Substitute.For<Attribute>();

            magicalResistance.StatValue.Returns(10);
            statCollection.CreateOrGetStat<Attribute>(StatType.MagicalResistance)
                .ReturnsForAnyArgs(magicalResistance);
            defender.StatCollection.Returns(statCollection);

            var calculator = new MagicalDamageCalculator();
            var damage = calculator.CalculateDamage(hitData.DamageDealt, attacker, defender);
            
            damage.Should().Be(90);
        }
        [Test]
        public void When_MagicalDamageIsNegative_Then_CalculateCorrectFinalDamageAmount ()
        {
            var hitData = new HitData { DamageDealt = -100 };
            var attacker = Substitute.For<IShared>();
            var defender = Substitute.For<IShared>();
            var statCollection = Substitute.For<IStatCollection>();
            var magicalResistance = Substitute.For<Attribute>();

            magicalResistance.StatValue.Returns(10);
            statCollection.CreateOrGetStat<Attribute>(StatType.MagicalResistance)
                .ReturnsForAnyArgs(magicalResistance);
            defender.StatCollection.Returns(statCollection);

            var calculator = new MagicalDamageCalculator();
            var damage = calculator.CalculateDamage(hitData.DamageDealt, attacker, defender);
            
            damage.Should().Be(0);
        }
    }
}