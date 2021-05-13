using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Store.Tests
{
    public class StoreManagerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Category("PropertyProducts")]
        public void PropertyProductsShouldReturnCollectionAsReadOnly()
        {
            //Arrange
            StoreManager storeManager = new StoreManager();
            Product product = new Product("Product", 20, 50);

            //Act 
            storeManager.AddProduct(product);
            IReadOnlyCollection<Product> expectedResult = new List<Product>() { product }.AsReadOnly();
            IReadOnlyCollection<Product> actualResult = storeManager.Products;

            //Assert
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("AddProduct")]
        public void MethodAddProductShouldThrowExceptionIfNullProductIsGiven()
        {
            //Arrange
            StoreManager storeManager = new StoreManager();
            Product product = null;

            //Act - Assert
            Assert.Throws<ArgumentNullException>(()
                => storeManager.AddProduct(product));
        }

        [Test]
        [Category("AddProduct")]
        [TestCase(-20)]
        [TestCase(0)]
        public void MethodAddProductShouldThrowExceptionIfProductQuantityIsZeroOrBelow(int quantity)
        {
            //Arrange
            StoreManager storeManager = new StoreManager();
            Product product = new Product("Product", quantity, 50);

            //Act - Assert
            Assert.Throws<ArgumentException>(()
                => storeManager.AddProduct(product));
        }

        [Test]
        [Category("AddProduct")]
        public void MethodAddProductShouldIncreaseCountIfProperProductIsGiven()
        {
            //Arrange
            StoreManager storeManager = new StoreManager();
            Product product = new Product("Product", 20, 50);

            //Act 
            storeManager.AddProduct(product);
            int expectedResult = 1;
            int actualResult = storeManager.Count;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("BuyProduct")]
        public void MethodBuyProductShouldThrowExceptionIfProductWithGivenNameIsNull()
        {
            //Arrange
            StoreManager storeManager = new StoreManager();
            Product product = new Product("Product", 20, 50);

            //Act 
            storeManager.AddProduct(product);

            //Assert
            Assert.Throws<ArgumentNullException>(()
                => storeManager.BuyProduct("Product2", 20));
        }

        [Test]
        [Category("BuyProduct")]
        public void MethodBuyProductShouldThrowExceptionIfDesiredQuantityIsMoreThanActualQuantity()
        {
            //Arrange
            StoreManager storeManager = new StoreManager();
            Product product = new Product("Product", 20, 50);

            //Act 
            storeManager.AddProduct(product);

            //Assert
            Assert.Throws<ArgumentException>(()
                => storeManager.BuyProduct("Product", 30));
        }

        [Test]
        [Category("BuyProduct")]
        public void MethodBuyProductShouldDecreaseProductQuantityIfCorrectDataIsGiven()
        {
            //Arrange
            StoreManager storeManager = new StoreManager();
            Product product = new Product("Product", 20, 50);

            //Act 
            storeManager.AddProduct(product);
            storeManager.BuyProduct("Product", 10);
            int expectedResult = 10;
            int actualResult = product.Quantity;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("BuyProduct")]
        public void MethodBuyProductShouldCalculateAndReturnCorrectFinalPrice()
        {
            //Arrange
            StoreManager storeManager = new StoreManager();
            Product product = new Product("Product", 20, 50);

            //Act 
            storeManager.AddProduct(product);
            decimal expectedResult = 500;
            decimal actualResult = storeManager.BuyProduct("Product", 10);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("GetTheMostExpensiveProduct")]
        public void MethodGetTheMostExpensiveProductShouldReturnMostExpensiveProduct()
        {
            //Arrange
            StoreManager storeManager = new StoreManager();
            Product product = new Product("Product", 20, 50);
            Product product2 = new Product("Product2", 20, 100);

            //Act 
            storeManager.AddProduct(product);
            storeManager.AddProduct(product2);
            Product expectedResult = product2;
            Product actualResult = storeManager.GetTheMostExpensiveProduct();

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("GetTheMostExpensiveProduct")]
        public void MethodGetTheMostExpensiveProductShouldReturnTheFirstAddedProductIfPricesAreEqual()
        {
            //Arrange
            StoreManager storeManager = new StoreManager();
            Product product = new Product("Product", 20, 50);
            Product product2 = new Product("Product2", 20, 100);
            Product product3 = new Product("Product2", 20, 100);

            //Act 
            storeManager.AddProduct(product);
            storeManager.AddProduct(product2);
            Product expectedResult = product2;
            Product actualResult = storeManager.GetTheMostExpensiveProduct();

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("GetTheMostExpensiveProduct")]
        public void MethodGetTheMostExpensiveProductShouldReturnNullIfStoreManagerIsEmpty()
        {
            //Arrange
            StoreManager storeManager = new StoreManager();

            //Act 
            Product expectedResult = null;
            Product actualResult = storeManager.GetTheMostExpensiveProduct();

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}