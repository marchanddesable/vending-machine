using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineSystem
{
    public class Recipe
    {
        FeePercentage _feesPercentToAdd;
        private List<Ingredient> _recipeIngredients;
        private decimal _recipeCostOfGoods = 0;
        private decimal _recipeSalePrice = 0;
        public string RecipeId { get; set; }
        public string RecipeName { get; set; }

        public decimal RecipeTotalCostOfGoods
        {
            get
            {
                if (_recipeCostOfGoods == 0)
                    _recipeCostOfGoods = GetRecipeTotalCostOfCoods();

                return _recipeCostOfGoods;
            }
        }

        public decimal RecipeSalePrice
        {
            get
            {
                if (_recipeSalePrice == 0)
                    _recipeSalePrice = GetRecipeSalePrice();

                return _recipeSalePrice;
            }
        }

        private decimal GetRecipeTotalCostOfCoods()
        {
            var recipeCostOfGoods = _recipeIngredients.Where(o => o.Quantity > 0 && o.Product != null)
                                                      .Sum(p => p.Product.ProductPrice * p.Quantity);

            return recipeCostOfGoods;
        }

        private decimal GetRecipeSalePrice()
        {
            decimal cost = GetRecipeTotalCostOfCoods();

            return cost + ((cost * _feesPercentToAdd.Fee) / 100);
        }

        public Recipe(string recipeName, FeePercentage feesPercentToAdd)
        {
            if (string.IsNullOrWhiteSpace(recipeName))
                throw new Exception($"Recipe: recipeName cannot be null");

            _recipeIngredients =  new List<Ingredient>();

            RecipeName = recipeName;
            RecipeId = "recipe-" + Guid.NewGuid().ToString();

            _feesPercentToAdd = feesPercentToAdd;
        }

        public List<Ingredient> GetRecipeIngredients()
        {
            return _recipeIngredients;
        }

        public bool VerifyRecipeQuantityAvailability(int recipeQuantityRequiered, bool raiseExceptionIfOutOfStock = true)
        {
            bool quantityIsAvailable = false;

            foreach(var ingredient in _recipeIngredients)
            {
                quantityIsAvailable = ingredient.CheckQuantityAvailability(ingredient.Quantity * recipeQuantityRequiered);

                if (!quantityIsAvailable && raiseExceptionIfOutOfStock)
                {
                    throw (new OutOfStockIngredientException(RecipeName, ingredient.Product.ProductName));
                }
            }

            return quantityIsAvailable;
        }

        public void AddNewIngredient(Product product, int quantity)
        {
            if (product == null || string.IsNullOrWhiteSpace(product.ProductName))
                throw new Exception("AddNewIngredient: product incorrect");

            var newIngredient = new Ingredient(product, quantity);
    
            _recipeIngredients.Add(newIngredient);
        }

        public bool AddIngredients(List<Ingredient> ingredients)
        {
            bool isAdded = false;

            if (ingredients == null || !ingredients.Any())
                throw new Exception("AddNewIngredient: The ingredients list cannot be empty");

            _recipeIngredients.AddRange(ingredients);

            return isAdded;
        }

        public void RemoveIngredient(string ingredientId)
        {
            if (string.IsNullOrWhiteSpace(ingredientId))
                throw new Exception("AddNewIngredient: ingredientId cannot be empty");

            _recipeIngredients.RemoveAll(p => p.IngredientId == ingredientId);
        }

      

    }

}
