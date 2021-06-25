using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BankSafe.Tests
{
    public class BankVaultTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Category("Constructor")]
        public void ConstructorShouldWorkProperly()
        {
            var expectedResult = new Dictionary<string, Item>
            {
                {"A1", null},
                {"A2", null},
                {"A3", null},
                {"A4", null},
                {"B1", null},
                {"B2", null},
                {"B3", null},
                {"B4", null},
                {"C1", null},
                {"C2", null},
                {"C3", null},
                {"C4", null},
            };

            var actualResult = new BankVault().VaultCells;

            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("Add")]
        public void MethodAddShouldThrowExceptionIfGivenCellDoesNotExist()
        {
            BankVault bankVault = new BankVault();

            Assert.Throws<ArgumentException>(()
                => bankVault.AddItem("Cell", null));
        }

        [Test]
        [Category("Add")]
        public void MethodAddShouldThrowExceptionIfGivenKeyValueIsTaken()
        {
            BankVault bankVault = new BankVault();

            bankVault.AddItem("A3", new Item("Owner", "Id"));

            Assert.Throws<ArgumentException>(()
                => bankVault.AddItem("A3", new Item("Owner2", "Id2")));
        }

        [Test]
        [Category("Add")]
        public void MethodAddShouldThrowExceptionIfSameValueIsGiven()
        {
            BankVault bankVault = new BankVault();

            bankVault.AddItem("A3", new Item("Owner", "Id"));

            Assert.Throws<InvalidOperationException>(()
                => bankVault.AddItem("A1", new Item("Owner", "Id")));
        }

        [Test]
        [Category("Add")]
        public void MethodAddShouldReturnCorrectStringResultIfConditionsAreCorrect()
        {
            BankVault bankVault = new BankVault();

            string expectedResult = $"Item:Id saved successfully!";
            string actualResult = bankVault.AddItem("A3", new Item("Owner", "Id"));

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("Remove")]
        public void MethodRemoveShouldThrowExceptionIfGivenCellDoesNotExist()
        {
            BankVault bankVault = new BankVault();

            Assert.Throws<ArgumentException>(()
                => bankVault.RemoveItem("Cell", null));
        }

        [Test]
        [Category("Remove")]
        public void MethodRemoveShouldThrowExceptionIfGivenItemDoesNotExistInGivenCell()
        {
            BankVault bankVault = new BankVault();

            Item item = new Item("Owner", "Id");
            Item itemToRemove = new Item("Owner2", "Id2");

            bankVault.AddItem("A3", item);

            Assert.Throws<ArgumentException>(()
                => bankVault.RemoveItem("A3", itemToRemove));
        }

        [Test]
        [Category("Remove")]
        public void MethodRemoveShouldRemoveTheCellValueByMakingItNull()
        {
            BankVault bankVault = new BankVault();

            Item item = new Item("Owner", "Id");

            bankVault.AddItem("A3", item);
            bankVault.RemoveItem("A3", item);

            Item expectedResult = null;
            var actualResult = bankVault.VaultCells["A3"];

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("Remove")]
        public void MethodRemoveShouldReturnCorrectStringResultIfConditionsAreCorrect()
        {
            BankVault bankVault = new BankVault();
            Item item = new Item("Owner", "Id");

            bankVault.AddItem("A3", item);

            string expectedResult = $"Remove item:Id successfully!";
            string actualResult = bankVault.RemoveItem("A3", item);

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}