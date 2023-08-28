using System;
using System.Collections.Generic;
using ShoppingCartCheckout.Models;
using ShoppingCartCheckout.Services;
using NUnit.Framework;

namespace ShoppingCartCheckout.Tests
{
    public class BasketTests
    {
        private Dictionary<char, priceRules> _priceRules;

        [SetUp]
        public void Setup()
        {
            _priceRules = new Dictionary<char, priceRules>
            {
                { 'A', new priceRules { UnitPrice = 50, SpecialQuantity = 3, SpecialPrice = 130 } },
                { 'B', new priceRules { UnitPrice = 30, SpecialQuantity = 2, SpecialPrice = 45 } },
                { 'C', new priceRules { UnitPrice = 20 } },
                { 'D', new priceRules { UnitPrice = 15 } }
            };
        }

        [Test]
        public void TestPricing_Threshold()
        {
            var basket = new Basket(_priceRules);
            basket.ItemScan('A');
            basket.ItemScan('A');
            basket.ItemScan('A');
            basket.ItemScan('A'); // One above the special price threshold

            var expectedPrice = 130 + 50; 
            var actualPrice = basket.GetPrice();

            Console.WriteLine($"Expected Price for Threshold: {expectedPrice}");
            Console.WriteLine($"Actual Price for Threshold: {actualPrice}");
            Assert.AreEqual(expectedPrice, actualPrice);
        }

        [Test]
        public void TestPricing_Invalid()
        {
            var basket = new Basket(_priceRules);
            basket.ItemScan('E'); // Invalid item

            var expectedPrice = 0; // Assuming invalid items are ignored
            var actualPrice = basket.GetPrice();

            Console.WriteLine($"Expected Price for Invalid Item: {expectedPrice}");
            Console.WriteLine($"Actual Price for Invalid Item: {actualPrice}");
            Assert.AreEqual(expectedPrice, actualPrice);
        }

        [Test]
        public void TestPricing_Remove()
        {
            var basket = new Basket(_priceRules);
            basket.ItemScan('A');
            basket.ItemScan('A');
            basket.ItemScan('A');
            basket.RemoveItem('A'); // Remove one 'A'

            var expectedPrice = 100; // Price for two 'A's
            var actualPrice = basket.GetPrice();

            Console.WriteLine($"Expected Price for Removing Item: {expectedPrice}");
            Console.WriteLine($"Actual Removing Item: {actualPrice}");
            Assert.AreEqual(expectedPrice, actualPrice);
        }

        [Test]
        public void TestPricing_Clear()
        {
            var basket = new Basket(_priceRules);
            basket.ItemScan('A');
            basket.ItemScan('B');
            basket.Clear(); // Clear the basket

            var expectedPrice = 0; 
            var actualPrice = basket.GetPrice();

            Console.WriteLine($"Expected Price after Clearing: {expectedPrice}");
            Console.WriteLine($"Actual Price after Clearing: {actualPrice}");
            Assert.AreEqual(expectedPrice, actualPrice);
        }
    }
}
