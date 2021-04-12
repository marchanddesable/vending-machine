using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineSystem
{
    public class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int NumberOfUnits { get; private set; } = 1;

        public Product(string productName, decimal productPrice, int numberOfUnits = 1)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new Exception($"Product: productName cannot be null");

            if (productPrice < 0)
                throw new Exception($"Product: productPrice cannot be negative");

            ProductName = productName;
            ProductPrice = productPrice;
            ProductId = "product-" + Guid.NewGuid().ToString();
            NumberOfUnits = numberOfUnits;
        }

        public bool SubstractFromInventory(int quantity)
        {
            bool result = false;

            if (NumberOfUnits >= quantity)
            {
                NumberOfUnits -= quantity;
                result = true;
            }

            return result;
        }
    }

}
