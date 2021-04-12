using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class RecipeIngredientsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int RemainingUnits { get; set; }
        public string Price { get; set; }

    }
}
