using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineSystem
{

    public interface IVendingMachine
    {
        ProductManager ProductManager { get; }
        RecipeManager RecipeManager { get; }

        List<Recipe> GetAllRecipes();
        Recipe GetRecipe(string recipeId);
        bool TryCreateNewOrder(string recipeId, int requiredQuantity, ref string resultMessage);
    }


}
