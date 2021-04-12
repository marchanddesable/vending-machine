using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineSystem
{
    public class RecipeIdNotFoundException : Exception
    {
        public string RecipeId {get; set;}

        public RecipeIdNotFoundException(string recipeId) : base(String.Format("Recipe Id not found {0}", recipeId))
        {
            RecipeId = recipeId;
        }

    }

}
