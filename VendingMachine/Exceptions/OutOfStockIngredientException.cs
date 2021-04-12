using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineSystem
{
    public class OutOfStockIngredientException : Exception
    {
        public string RecipeName { get; set; }
        public string ProductName { get; set; }
        public OutOfStockIngredientException(string recipeName, string productName) : base(String.Format("Not enough [{0}], remaining to create the [{1}] recipe", productName, recipeName))
        {
            RecipeName = recipeName;
            ProductName = productName;
        }
    }

}
