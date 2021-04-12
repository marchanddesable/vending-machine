using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineSystem
{
    public interface IRecipeManager
    {
        List<Recipe> GetRecipeList();
        Recipe CreateRecipe(string recipeName, Dictionary<string, int> productNamesAndQuantities);
        bool RemoveRecipe(string receiptId, ref string resultMessage);
        Recipe GetRecipe(string recipeId);
        string GetProductIdByName(string productName);
        bool TryExtractRecipeQuantityOfInventory(string recipeId, int requiredQuantity, ref string resultMessage);
    }
}
