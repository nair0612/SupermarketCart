using System;
using System.Collections.Generic;
using ShoppingCartCheckout.Models;

namespace ShoppingCartCheckout.Services
{
    public class Basket
    {
        private readonly Dictionary<char, priceRules> _priceRules;
        private readonly Dictionary<char, int> _cart;

        public Basket(Dictionary<char, priceRules> priceRules)
        {
            _priceRules = priceRules ?? throw new ArgumentNullException(nameof(priceRules));
            _cart = new Dictionary<char, int>();
        }

        public void ItemScan(char item)
        {
            try 
            {
                if (_cart.ContainsKey(item))
                {
                    _cart[item]++;
                }
                else
                {
                    _cart[item] = 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error has occurred: {ex.Message}");
            }
        }

        public int GetPrice()
        {
            int total = 0;
            try
            {
                foreach (var item in _cart)
                {
                    if (_priceRules.ContainsKey(item.Key))
                    {
                        var rule = _priceRules[item.Key];
                        if (rule.SpecialQuantity.HasValue && rule.SpecialPrice.HasValue)
                        {
                            int specialSets = item.Value / rule.SpecialQuantity.Value;
                            total += specialSets * rule.SpecialPrice.Value;
                            total += (item.Value % rule.SpecialQuantity.Value) * rule.UnitPrice;
                        }
                        else
                        {
                            total += item.Value * rule.UnitPrice;
                        }
                    }
                }
                return total;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error has occurred: {ex.Message}");
                return -1;
            }
        }

        public void RemoveItem(char item)
        {
            if (_cart.ContainsKey(item))
            {
                _cart[item]--;
                if (_cart[item] <= 0)
                {
                    _cart.Remove(item);
                }
            }
            else
            {
                Console.WriteLine($"Warning: Attempted to remove item '{item}' which does not exist in the cart.");
            }
        }

        public void Clear()
        {
            _cart.Clear();
        }
    }
}

