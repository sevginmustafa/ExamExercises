using NUnit.Framework;
using System;
using TheRace;

namespace TheRace.Tests
{
    public class RaceEntryTests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Category("UnitDriverNameProperty")]
        public void PropertyNameShouldThrowExceptionIfNameIsNull()
        {
            //Arrange - Act - Assert
            Assert.Throws<ArgumentNullException>(()
               => new UnitDriver(null, new UnitCar("BMW", 160, 3000)));
        }

        [Test]
        [Category("AddDriver")]
        public void MethodAddDriverShouldThrowExceptionIfNullIsGiven()
        {
            //Arrange
            RaceEntry raceEntry = new RaceEntry();
            UnitDriver unitDriver = null;

            //Act - Assert
            Assert.Throws<InvalidOperationException>(()
               => raceEntry.AddDriver(unitDriver));
        }

        [Test]
        [Category("AddDriver")]
        public void MethodAddDriverShouldThrowExceptionIfExistingDriverIsGiven()
        {
            //Arrange
            RaceEntry raceEntry = new RaceEntry();
            UnitDriver unitDriver = new UnitDriver("Pesho", new UnitCar("BMW", 160, 3000));

            //Act 
            raceEntry.AddDriver(unitDriver);

            //Assert
            Assert.Throws<InvalidOperationException>(()
               => raceEntry.AddDriver(unitDriver));
        }

        [Test]
        [Category("AddDriver")]
        public void MethodAddDriverShouldIncreaseCount()
        {
            //Arrange
            RaceEntry raceEntry = new RaceEntry();
            UnitDriver unitDriver = new UnitDriver("Pesho", new UnitCar("BMW", 160, 3000));

            //Act 
            raceEntry.AddDriver(unitDriver);
            int expectedResult = 1;
            int actualResult = raceEntry.Counter;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("AddDriver")]
        public void MethodAddDriverShouldReturnProperString()
        {
            //Arrange
            RaceEntry raceEntry = new RaceEntry();
            UnitDriver unitDriver = new UnitDriver("Pesho", new UnitCar("BMW", 160, 3000));

            //Act - Assert
            string expectedResult = "Driver Pesho added in race.";
            string actualResult = raceEntry.AddDriver(unitDriver);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("CalculateAverageHorsePower")]
        public void MethodCalculateAverageHorsePowerShouldThrowExceptionIfCountIsLessThan2()
        {
            //Arrange
            RaceEntry raceEntry = new RaceEntry();
            UnitDriver unitDriver = new UnitDriver("Pesho", new UnitCar("BMW", 160, 3000));

            //Act 
            raceEntry.AddDriver(unitDriver);

            //Assert
            Assert.Throws<InvalidOperationException>(()
                => raceEntry.CalculateAverageHorsePower());
        }

        [Test]
        [Category("CalculateAverageHorsePower")]
        public void MethodCalculateAverageHorsePowerShouldReturnCorrectAverageValue()
        {
            //Arrange
            RaceEntry raceEntry = new RaceEntry();
            UnitDriver unitDriver = new UnitDriver("Pesho", new UnitCar("BMW", 160, 3000)); 
            UnitDriver unitDriver2 = new UnitDriver("Gosho", new UnitCar("Audi", 140, 2500));

            //Act 
            raceEntry.AddDriver(unitDriver);
            raceEntry.AddDriver(unitDriver2);

            double expectedResult = 150;
            double actualResult = raceEntry.CalculateAverageHorsePower();

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}