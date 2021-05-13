using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Computers.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Category("PropertyComputers")]
        public void PropertyComputersShouldReturnCollectionAsReadOnly()
        {
            //Arrange
            ComputerManager computerManager = new ComputerManager();
            Computer computer = new Computer("Manufacturer", "Model", 1000);

            //Act
            computerManager.AddComputer(computer);
            IReadOnlyCollection<Computer> expectedResult = new List<Computer>() { computer }.AsReadOnly();
            IReadOnlyCollection<Computer> actualResult = computerManager.Computers;

            //Assert
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("AddComputer")]
        public void MethodAddComputerShouldThrowExceptionIfComputerIsNull()
        {
            //Arrange
            ComputerManager computerManager = new ComputerManager();
            Computer computer = null;

            //Act - Assert 
            Assert.Throws<ArgumentNullException>(()
                => computerManager.AddComputer(computer));
        }

        [Test]
        [Category("AddComputer")]
        public void MethodAddComputerShouldThrowExceptionIfComputerAlreadyExists()
        {
            //Arrange
            ComputerManager computerManager = new ComputerManager();
            Computer computer = new Computer("Manufacturer", "Model", 1000);

            //Act
            computerManager.AddComputer(computer);

            //Assert
            Assert.Throws<ArgumentException>(()
                => computerManager.AddComputer(computer));
        }

        [Test]
        [Category("AddComputer")]
        public void MethodAddComputerShouldIncreaseCollectionCount()
        {
            //Arrange
            ComputerManager computerManager = new ComputerManager();
            Computer computer = new Computer("Manufacturer", "Model", 1000);

            //Act
            computerManager.AddComputer(computer);
            int expectedResult = 1;
            int actualResult = computerManager.Count;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("RemoveComputer")]
        [TestCase(null, "Model")]
        [TestCase("Manufacturer", null)]
        public void MethodRemoveComputerShouldThrowExceptionIfGivenVariableIsNull(string manufacturer, string model)
        {
            //Arrange
            ComputerManager computerManager = new ComputerManager();
            Computer computer = new Computer("Manufacturer", "Model", 1000);

            //Act
            computerManager.AddComputer(computer);

            //Assert
            Assert.Throws<ArgumentNullException>(()
                => computerManager.RemoveComputer(manufacturer, model));
        }

        [Test]
        [Category("RemoveComputer")]
        public void MethodRemoveComputerShouldThrowExceptionIfCollectionIsEmpty()
        {
            //Arrange
            ComputerManager computerManager = new ComputerManager();

            //Act - Assert 
            Assert.Throws<ArgumentException>(()
                => computerManager.RemoveComputer("Manufacturer", "Model"));
        }

        [Test]
        [Category("RemoveComputer")]
        public void MethodRemoveComputerShouldDecreaseCollectionCount()
        {
            //Arrange
            ComputerManager computerManager = new ComputerManager();
            Computer computer = new Computer("Manufacturer", "Model", 1000);

            //Act
            computerManager.AddComputer(computer);
            computerManager.RemoveComputer("Manufacturer", "Model");
            int expectedResult = 0;
            int actualResult = computerManager.Count;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("RemoveComputer")]
        public void MethodRemoveComputerShouldReturnTheRemovedComputer()
        {
            //Arrange
            ComputerManager computerManager = new ComputerManager();
            Computer computer = new Computer("Manufacturer", "Model", 1000);

            //Act
            computerManager.AddComputer(computer);
            Computer expectedResult = computer;
            Computer actualResult = computerManager.RemoveComputer("Manufacturer", "Model");

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("GetComputer")]
        [TestCase(null,"Model")]
        [TestCase("Manufacturer",null)]
        public void MethodGetComputerShouldThrowExceptionIfGivenVariableIsNull(string manufacturer,string model)
        {
            //Arrange
            ComputerManager computerManager = new ComputerManager();
            Computer computer = new Computer("Manufacturer", "Model", 1000);

            //Act
            computerManager.AddComputer(computer);

            //Assert
            Assert.Throws<ArgumentNullException>(()
                => computerManager.GetComputer(manufacturer, model));
        }

        [Test]
        [Category("GetComputer")]
        public void MethodGetComputerShouldThrowExceptionIfGivenComputerDoesNotExist()
        {
            //Arrange
            ComputerManager computerManager = new ComputerManager();
            Computer computer = new Computer("Manufacturer", "Model", 1000);

            //Act
            computerManager.AddComputer(computer);

            //Assert
            Assert.Throws<ArgumentException>(()
                => computerManager.GetComputer("Manufacturer2", "Model"));
        }

        [Test]
        [Category("GetComputer")]
        public void MethodGetComputerShouldReturnTheCorrectComputer()
        {
            //Arrange
            ComputerManager computerManager = new ComputerManager();
            Computer computer = new Computer("Manufacturer", "Model", 1000);

            //Act
            computerManager.AddComputer(computer);
            Computer expectedResult = computer;
            Computer actualResult = computerManager.GetComputer("Manufacturer", "Model");

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("GetComputersByManufacturer")]
        public void MethodGetComputersByManufacturerShouldThrowExceptionIfManufacturerIsNull()
        {
            //Arrange
            ComputerManager computerManager = new ComputerManager();
            Computer computer = new Computer("Manufacturer", "Model", 1000);

            //Act
            computerManager.AddComputer(computer);

            //Assert
            Assert.Throws<ArgumentNullException>(()
                => computerManager.GetComputersByManufacturer(null));
        }

        [Test]
        [Category("GetComputersByManufacturer")]
        public void MethodGetComputersByManufacturerShouldReturnTheCorrectComputers()
        {
            //Arrange
            ComputerManager computerManager = new ComputerManager();
            Computer computer = new Computer("Manufacturer", "Model", 1000);
            Computer computer2 = new Computer("Manufacturer", "Model2", 1000);

            //Act
            computerManager.AddComputer(computer);
            computerManager.AddComputer(computer2);
            ICollection<Computer> expectedResult = new List<Computer>() { computer, computer2 };
            ICollection<Computer> actualResult = computerManager.GetComputersByManufacturer("Manufacturer");

            //Assert
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("GetComputersByManufacturer")]
        public void MethodGetComputersByManufacturerShouldReturnEmtyCollectionIfThereAreNoSatisfyingCondition()
        {
            //Arrange
            ComputerManager computerManager = new ComputerManager();
            Computer computer = new Computer("Manufacturer", "Model", 1000);

            //Act
            computerManager.AddComputer(computer);
            ICollection<Computer> expectedResult = new List<Computer>();
            ICollection<Computer> actualResult = computerManager.GetComputersByManufacturer("Manufacturer2");

            //Assert
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }
    }
}