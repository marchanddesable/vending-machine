using System;

namespace WebApp.Models
{
    public class PurchaseResultViewModel
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public RecipeViewModel Recipe { get; set; }

    }
}
