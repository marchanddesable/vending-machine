using System;
using Xunit;

using VendingMachineSystem;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineTest
{
    public class VendingMachineTest
    {
        [Fact]
        public void Products_Should_Be_Added_To_Machine()
        {
            var machine = new VendingMachine();

            int expectedTotalProducts = 7;

            int numberOfUnits = 5;

            machine.ProductManager.AddProductToMachine("Café", 1, numberOfUnits);
            machine.ProductManager.AddProductToMachine("Sucre", (decimal)0.1, numberOfUnits);
            machine.ProductManager.AddProductToMachine("Crème", (decimal)0.5, numberOfUnits);
            machine.ProductManager.AddProductToMachine("Thé", 2, numberOfUnits);
            machine.ProductManager.AddProductToMachine("Eau", (decimal)0.2, numberOfUnits);
            machine.ProductManager.AddProductToMachine("Chocolat", 1, numberOfUnits);
            machine.ProductManager.AddProductToMachine("Lait", (decimal)0.4, numberOfUnits);

            var totalProducts = machine.GetAllProducts().Count();

            Assert.Equal(expectedTotalProducts, totalProducts);

        }

        [Fact]
        public void Recipes_Should_Be_Created()
        {
            var machine = new VendingMachine();

            int expectedTotalRecipes = 5;

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

            var totalRecipes = machine.GetAllRecipes().Count();

            Assert.Equal(expectedTotalRecipes, totalRecipes);

        }

        [Fact]
        public void Recipe_Capuccino_Should_Return_The_Right_Final_Price()
        {
            var machine = new VendingMachine();

            decimal recipePriceWith30PercentMargin = (decimal)3.51;

            int numberOfUnits = 5;

            machine.ProductManager.AddProductToMachine("Café", 1, numberOfUnits);
            machine.ProductManager.AddProductToMachine("Crème", (decimal)0.5, numberOfUnits);
            machine.ProductManager.AddProductToMachine("Eau", (decimal)0.2, numberOfUnits);
            machine.ProductManager.AddProductToMachine("Chocolat", 1, numberOfUnits);

            machine.RecipeManager.CreateRecipe("Capuccino", new Dictionary<string, int>()
            {
                { "Café", 1 },
                { "Chocolat", 1 },
                { "Eau", 1 },
                { "Crème", 1 },
            });

            var recipe = machine.GetAllRecipes().FirstOrDefault();

            Assert.Equal(recipePriceWith30PercentMargin, recipe.RecipeSalePrice);

        }

        [Fact]
        public void Recipe_Expresso_Should_Return_The_Right_Final_Price()
        {
            var machine = new VendingMachine();

            decimal recipePriceWith30PercentMargin = (decimal)1.56;

            int numberOfUnits = 5;

            machine.ProductManager.AddProductToMachine("Café", 1, numberOfUnits);
            machine.ProductManager.AddProductToMachine("Eau", (decimal)0.2, numberOfUnits);

            machine.RecipeManager.CreateRecipe("Expresso", new Dictionary<string, int>()
            {
                { "Café", 1 },
                { "Eau", 1 },
            });

            var recipe = machine.GetAllRecipes().FirstOrDefault();

            Assert.Equal(recipePriceWith30PercentMargin, recipe.RecipeSalePrice);

        }

        [Fact]
        public void Purchase_Should_Succeed()
        {
            var machine = new VendingMachine();

            int numberOfUnits = 5;

            machine.ProductManager.AddProductToMachine("Café", 1, numberOfUnits);
            machine.ProductManager.AddProductToMachine("Eau", (decimal)0.2, numberOfUnits);

            machine.RecipeManager.CreateRecipe("Expresso", new Dictionary<string, int>()
            {
                { "Café", 1 },
                { "Eau", 1 },
            });

            var recipe = machine.GetAllRecipes().FirstOrDefault();

            string msg = string.Empty;
            var purchaseSuceeded = machine.TryCreateNewOrder(recipe.RecipeId, 1, ref msg);

            Assert.True(purchaseSuceeded);

        }

        [Fact]
        public void Purchase_Should_Fail()
        {
            var machine = new VendingMachine();

            int numberOfUnits = 1; //will fail because 2 Eau are needed for Allongé recipe

            machine.ProductManager.AddProductToMachine("Café", 1, numberOfUnits);
            machine.ProductManager.AddProductToMachine("Eau", (decimal)0.2, numberOfUnits);

            machine.RecipeManager.CreateRecipe("Allongé", new Dictionary<string, int>()
            {
                { "Café", 1 },
                { "Eau", 2 },
            });

            var recipe = machine.GetAllRecipes().FirstOrDefault();

            string msg = string.Empty;
            var purchaseSuceeded = machine.TryCreateNewOrder(recipe.RecipeId, 1, ref msg);

            Assert.True(purchaseSuceeded);

        }

    }
}
