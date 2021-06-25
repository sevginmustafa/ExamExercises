namespace Computers.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    public class ComputerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Category("PropertyName")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void PropertyNameCannotBeNullEmptyOrWhiteSpaces(string name)
        {
            //Arrange - Act - Assert
            Assert.Throws<ArgumentNullException>(()
                => new Computer(name));
        }

        [Test]
        [Category("PropertyParts")]
        public void PropertyPartsShouldReturnCollectionAsReadOnly()
        {
            //Arrange
            Computer computer = new Computer("Name");
            Part part = new Part("Name", 100);

            //Act
            computer.AddPart(part);
            IReadOnlyCollection<Part> expectedResult = new List<Part>() { part }.AsReadOnly();
            IReadOnlyCollection<Part> actualResult = computer.Parts;

            //Assert
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("TotalPrice")]
        public void PropertyTotalPriceShouldReturnTotalSumOfParts()
        {
            //Arrange
            Computer computer = new Computer("Name");
            Part part = new Part("Name", 100);
            Part part2 = new Part("Name2", 100);

            //Act
            computer.AddPart(part);
            computer.AddPart(part2);
            decimal expectedResult = 200;
            decimal actualResult = computer.TotalPrice;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("AddPart")]
        public void MethodAddPartShouldThrowExceptionIdPartIsNull()
        {
            //Arrange
            Computer computer = new Computer("Name");
            Part part = null;

            //Act - Assert
            Assert.Throws<InvalidOperationException>(()
                => computer.AddPart(part));
        }

        [Test]
        [Category("AddPart")]
        public void MethodAddPartShouldIncreasePartCollectionCount()
        {
            //Arrange
            Computer computer = new Computer("Name");
            Part part = new Part("Name", 100);

            //Act
            computer.AddPart(part);
            int expectedResult = 1;
            int actualResult = computer.Parts.Count;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("RemovePart")]
        public void MethodRemovePartShouldReturnTrueIfSuccessful()
        {
            //Arrange
            Computer computer = new Computer("Name");
            Part part = new Part("Name", 100);

            //Act
            computer.AddPart(part);

            //Assert
            Assert.IsTrue(computer.RemovePart(part));
        }

        [Test]
        [Category("RemovePart")]
        public void MethodRemovePartShouldReturnFalseIfIsNotSuccessful()
        {
            //Arrange
            Computer computer = new Computer("Name");
            Part part = new Part("Name", 100);

            //Act
            computer.AddPart(part);

            //Assert
            Assert.IsFalse(computer.RemovePart(new Part("Name2", 100)));
        }

        [Test]
        [Category("GetPart")]
        public void MethodGetPartShouldReturnTheCorrectPart()
        {
            //Arrange
            Computer computer = new Computer("Name");
            Part part = new Part("Name", 100);

            //Act
            computer.AddPart(part);
            Part expectedResult = part;
            Part actualResult = computer.GetPart("Name");

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("GetPart")]
        public void MethodGetPartShouldReturnNullIfTherIsNoPartWithGivenName()
        {
            //Arrange
            Computer computer = new Computer("Name");
            Part part = new Part("Name", 100);

            //Act
            computer.AddPart(part);
            Part expectedResult = null;
            Part actualResult = computer.GetPart("Name2");

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}