using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using VendingMachineSystem;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        //I use a static var for demo purpose.
        //In a real scenario I would probably use something else (like a cache for example)
        //because static vars are not garbage collected and might create memory leaks if the object becomes too big
        //it can also create concurrent access issues
        public static VendingMachine machine;

        private CultureInfo culture;

        public HomeController()
        {
            if (machine == null)
                InitMachine();

            culture = new CultureInfo("fr-FR");
        }

        private void InitMachine()
        {
            if (machine != null)
                return;

            machine = new VendingMachine();

            //The Vending Machine updates the stock after each purchase.
            //If the quantity of a product becomes too low for a particular recipe, the purchase of the recipe will be denied.
            int numberOfUnits = 5;

            machine.ProductManager.AddProductToMachine("Café", 1, numberOfUnits);
            machine.ProductManager.AddProductToMachine("Sucre", (decimal)0.1, numberOfUnits);
            machine.ProductManager.AddProductToMachine("Crème", (decimal)0.5, numberOfUnits);
            machine.ProductManager.AddProductToMachine("Thé", 2, numberOfUnits);
            machine.ProductManager.AddProductToMachine("Eau", (decimal)0.2, numberOfUnits);
            machine.ProductManager.AddProductToMachine("Chocolat", 1, numberOfUnits);
            machine.ProductManager.AddProductToMachine("Lait", (decimal)0.4, numberOfUnits);

            machine.RecipeManager.CreateRecipe("Expresso", new Dictionary<string, int>()
            {
                { "Café", 1 },
                { "Eau", 1 },
            });

            machine.RecipeManager.CreateRecipe("Allongé", new Dictionary<string, int>()
            {
                { "Café", 1 },
                { "Eau", 2 },
            });

            machine.RecipeManager.CreateRecipe("Capuccino", new Dictionary<string, int>()
            {
                { "Café", 1 },
                { "Chocolat", 1 },
                { "Eau", 1 },
                { "Crème", 1 },
            });

            machine.RecipeManager.CreateRecipe("Chocolat", new Dictionary<string, int>()
            {
                { "Chocolat", 3 },
                { "Lait", 2 },
                { "Eau", 1 },
                { "Sucre", 1 },
            });

            machine.RecipeManager.CreateRecipe("The", new Dictionary<string, int>()
            {
                { "Thé", 1 },
                { "Eau", 2 }
            });
        }

        public IActionResult Index()
        {
            List<Recipe> recipes = machine.GetAllRecipes();
            List<Product> products = machine.GetAllProducts();

            var recipeLst = recipes.Select(p => new RecipeViewModel()
            {
                RecipeId = p.RecipeId,
                RecipeName = p.RecipeName,
                RecipeSalePrice = p.RecipeSalePrice.ToString("C", culture),
                RecipeTotalCostOfGoods = p.RecipeTotalCostOfGoods.ToString("C", culture),
                Ingredients = p.GetRecipeIngredients().Select(o => new RecipeIngredientsViewModel()
                {
                    Id = o.IngredientId,
                    Name = o.Product.ProductName,
                    Quantity = o.Quantity
                }).ToList()
            }).ToList();

            var productLst = products.Select(p => new ProductViewModel()
            {
                Id = p.ProductId,
                Name = p.ProductName,
                UnitsRemaining = p.NumberOfUnits,
                Price = p.ProductPrice.ToString("C", culture),
            }).ToList();

            ViewBag.Products = productLst;

            return View(recipeLst);
        }

        [Route("reset")]
        public IActionResult Reset()
        {
            machine = null;
            InitMachine();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("api/buy")]
        public PurchaseResultViewModel BuyRecipe(string recipeid)
        {
            var res = new PurchaseResultViewModel();

            if (!string.IsNullOrWhiteSpace(recipeid))
            {
                string msg = string.Empty;

                if (machine.TryCreateNewOrder(recipeid, 1, ref msg))
                {
                    List<Product> products = machine.GetAllProducts();

                    res.Succeeded = true;

                    var rcp = machine.GetRecipe(recipeid);

                    res .Recipe = new RecipeViewModel() 
                    { 
                        RecipeId = rcp.RecipeId, 
                        RecipeName = rcp.RecipeName, 
                        RecipeSalePrice = rcp.RecipeSalePrice.ToString("C", culture), 
                        RecipeTotalCostOfGoods = rcp.RecipeTotalCostOfGoods.ToString("C", culture),
                        Ingredients = rcp.GetRecipeIngredients().Select(o => new RecipeIngredientsViewModel()
                        {
                            Id = o.Product.ProductId,
                            Name = o.Product.ProductName,
                            Price = o.Product.ProductPrice.ToString("C", culture),
                            RemainingUnits = products.FirstOrDefault(p => p.ProductId == o.Product.ProductId).NumberOfUnits
                        }).ToList()
                    };
                }

                res.Message = msg;

            }

            return res;
        }

        [HttpGet]
        [Route("api/recipes")]
        public IEnumerable<RecipeViewModel> Recipes()
        {
            List<Recipe> recipes = machine.GetAllRecipes();

            var lst = recipes.Select(p => new RecipeViewModel()
            {
                RecipeId = p.RecipeId,
                RecipeName = p.RecipeName,
                RecipeSalePrice = p.RecipeSalePrice.ToString("C", culture),
                RecipeTotalCostOfGoods = p.RecipeTotalCostOfGoods.ToString("C", culture),
                Ingredients = p.GetRecipeIngredients().Select(o => new RecipeIngredientsViewModel()
                {
                    Id = o.IngredientId,
                    Name = o.Product.ProductName,
                    Quantity = o.Quantity
                }).ToList()
            }).ToList();

            return lst;
        }

        [HttpGet]
        [Route("api/products")]
        public IEnumerable<ProductViewModel> Products()
        {
            List<Product> products = machine.GetAllProducts();

            var lst = products.Select(p => new ProductViewModel()
            {
                Id = p.ProductId,
                Name = p.ProductName,
                UnitsRemaining = p.NumberOfUnits,
                Price = p.ProductPrice.ToString("C", culture),
            }).ToList();

            return lst;
        }


    

    }
}
