namespace Robots.Tests
{
    using System;
    using NUnit.Framework;

    public class RobotsTests
    {
        [Test]
        [Category("PropertyCapacity")]
        public void PropertyCapacityShouldThrowExceptionIfBelowZero()
        {
            //Arrange - Act - Assert
            Assert.Throws<ArgumentException>(()
                => new RobotManager(-10));
        }

        [Test]
        [Category("Add")]
        public void MethodAddShouldThrowExceptionIfRobotExists()
        {
            //Arrange
            RobotManager robotManager = new RobotManager(10);
            Robot robot = new Robot("Name", 100);

            //Act
            robotManager.Add(robot);

            //Assert
            Assert.Throws<InvalidOperationException>(()
                => robotManager.Add(robot));
        }

        [Test]
        [Category("Add")]
        public void MethodAddShouldThrowExceptionIfRobotsAddedAreMoreThanCapacity()
        {
            //Arrange
            RobotManager robotManager = new RobotManager(1);
            Robot robot = new Robot("Name", 100);

            //Act
            robotManager.Add(robot);

            //Assert
            Assert.Throws<InvalidOperationException>(()
                => robotManager.Add(new Robot("Name2",200)));
        }

        [Test]
        [Category("Add")]
        public void MethodAddShouldIncreaseCountIfSuccessful()
        {
            //Arrange
            RobotManager robotManager = new RobotManager(1);
            Robot robot = new Robot("Name", 100);

            //Act
            robotManager.Add(robot);
            int expectedResult = 1;
            int actualResult = robotManager.Count;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("Remove")]
        public void MethodRemoveShouldThrowExceptionIfRobotDoesNotExist()
        {
            //Arrange
            RobotManager robotManager = new RobotManager(10);
            Robot robot = new Robot("Name", 100);

            //Act - Assert
            Assert.Throws<InvalidOperationException>(()
                => robotManager.Remove("Name"));
        }

        [Test]
        [Category("Remove")]
        public void MethodRemoveShouldDecreaseCountIfSuccessful()
        {
            //Arrange
            RobotManager robotManager = new RobotManager(1);
            Robot robot = new Robot("Name", 100);

            //Act
            robotManager.Add(robot);
            robotManager.Remove("Name");
            int expectedResult = 0;
            int actualResult = robotManager.Count;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("Work")]
        public void MethodWorkShouldThrowExceptionIfRobotDoesNotExist()
        {
            //Arrange
            RobotManager robotManager = new RobotManager(1);
            Robot robot = new Robot("Name", 100);

            //Act
            robotManager.Add(robot);

            //Assert
            Assert.Throws<InvalidOperationException>(()
               => robotManager.Work("Name2", "Job", 100));
        }

        [Test]
        [Category("Work")]
        public void MethodWorkShouldThrowExceptionIfRobotBatteryIsLessThanGivenBatteryUsage()
        {
            //Arrange
            RobotManager robotManager = new RobotManager(1);
            Robot robot = new Robot("Name", 100);

            //Act
            robotManager.Add(robot);

            //Assert
            Assert.Throws<InvalidOperationException>(()
               => robotManager.Work("Name", "Job", 200));
        }

        [Test]
        [Category("Work")]
        public void MethodWorkShouldDecreaseBatteryIfSuccessful()
        {
            //Arrange
            RobotManager robotManager = new RobotManager(1);
            Robot robot = new Robot("Name", 100);

            //Act
            robotManager.Add(robot);
            robotManager.Work("Name", "Job", 90);
            int expectedResult = 10;
            int actualResult = robot.Battery;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("Charge")]
        public void MethodChargeShouldThrowExceptionIfRobotDoesNotExist()
        {
            //Arrange
            RobotManager robotManager = new RobotManager(1);
            Robot robot = new Robot("Name", 100);

            //Act
            robotManager.Add(robot);

            //Assert
            Assert.Throws<InvalidOperationException>(()
               => robotManager.Charge("Name2"));
        }

        [Test]
        [Category("Charge")]
        public void MethodChargeShouldIncreaseBatteryIfSuccessful()
        {
            //Arrange
            RobotManager robotManager = new RobotManager(1);
            Robot robot = new Robot("Name", 100);

            //Act
            robotManager.Add(robot);
            robotManager.Work("Name", "Job", 90);
            robotManager.Charge("Name");
            int expectedResult = 100;
            int actualResult = robot.Battery;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
