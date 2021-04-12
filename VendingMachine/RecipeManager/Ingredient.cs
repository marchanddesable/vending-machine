using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineSystem
{
    public class Ingredient
    {
        public string IngredientId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public Ingredient(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
            IngredientId = "ingredient-" + Guid.NewGuid().ToString();
        }

        public bool CheckQuantityAvailability(int requiredQuantity)
        {
            return (Product.NumberOfUnits >= requiredQuantity) ? true : false;
        }
    }

}
