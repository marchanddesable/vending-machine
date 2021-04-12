using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class RecipeViewModel
    {
        public string RecipeId { get; set; }
        public string RecipeName { get; set; }
        public string RecipeTotalCostOfGoods { get; set; }
        public string RecipeSalePrice { get; set; }

        public List<RecipeIngredientsViewModel> Ingredients { get; set; }
    }


}
