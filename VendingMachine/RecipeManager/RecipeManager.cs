using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineSystem
{
    public class RecipeManager : IRecipeManager
    {
        private Dictionary<string, Recipe> _recipeList = new Dictionary<string, Recipe>();
        protected Dictionary<string, Product> _products;
        protected FeePercentage _percentFeesToAdd;


        public RecipeManager(Dictionary<string, Product> productList, FeePercentage Fee)
        {
            _percentFeesToAdd = Fee;

            _products = productList;
        }
        public List<Recipe> GetRecipeList()
        {
            return _recipeList.Select(p => p.Value).ToList();
        }

        public Recipe CreateRecipe(string recipeName, Dictionary<string, int> productNamesAndQuantities)
        {
            var rcp = new Recipe(recipeName, _percentFeesToAdd);

            foreach (var ingredient in productNamesAndQuantities)
            {
                string productId = GetProductIdByName(ingredient.Key);
                int productQuantity = ingredient.Value;

                if (!_products.ContainsKey(productId))
                    throw new Exception($"CreateRecipe: productId not found {productId}");

                rcp.AddNewIngredient(_products[productId], productQuantity);
            }

            _recipeList.Add(rcp.RecipeId, rcp);

            return rcp;
        }

        public bool RemoveRecipe(string receiptId, ref string resultMessage)
        {
            bool isRemoved = false;

            if (string.IsNullOrWhiteSpace(receiptId))
                throw new Exception($"RemoveRecipe: receiptId cannot be empty");

            if (_products.ContainsKey(receiptId))
                isRemoved = _recipeList.Remove(receiptId);

            return isRemoved;
        }
        public Recipe GetRecipe(string recipeId)
        {
            if (string.IsNullOrWhiteSpace(recipeId))
                throw new Exception($"GetRecipe: recipeId cannot be empty");

            if (_recipeList.ContainsKey(recipeId))
                return _recipeList[recipeId];

            return null;
        }

        public bool TryExtractRecipeQuantityOfInventory(string recipeId, int requiredQuantity, ref string resultMessage)
        {
            bool isExtracted = false;

            if (string.IsNullOrWhiteSpace(recipeId))
                throw new Exception($"TryExtractRecipeQuantityOfInventory: recipeId cannot be empty");

            if (!_recipeList.ContainsKey(recipeId))
                return false;

            if (requiredQuantity <= 0)
            {
                throw (new InvalidQuantityException(requiredQuantity));
            }

            bool raiseExceptionIfOutOfStock = true;

            bool quantityIsAvailable = _recipeList[recipeId].VerifyRecipeQuantityAvailability(requiredQuantity, raiseExceptionIfOutOfStock);

            if (quantityIsAvailable)
            {
                var ingredients = _recipeList[recipeId].GetRecipeIngredients();

                foreach (var item in ingredients)
                {
                    item.Product.SubstractFromInventory(item.Quantity * requiredQuantity);
                }

                isExtracted = true;
            }

            return isExtracted;
        }

        private decimal GetRecipeTotalCostOfCoods(string recipeId)
        {
            if (string.IsNullOrWhiteSpace(recipeId))
                throw new Exception($"GetRecipeTotalCostOfCoods: recipeId cannot be empty");

            if (!_recipeList.ContainsKey(recipeId))
                return 0;

            return _recipeList[recipeId].RecipeTotalCostOfGoods;
        }
        public string GetProductIdByName(string productName)
        {
            string productId = null;

            var item = _products.FirstOrDefault(p => p.Value.ProductName == productName);

            if (item.Value != null)
            {
                productId = item.Key;
            }

            return productId;
        }
    }

}
