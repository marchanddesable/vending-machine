using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineSystem
{
    public class VendingMachine : IVendingMachine
    {
        private FeePercentage _feesPercentToAdd;

        private Dictionary<Type, string> UserMessages = new Dictionary<Type, string>()
        {
            { typeof(OutOfStockIngredientException), "{0}: Désolé, ce produit est en rupture de stock." },
            { typeof(InvalidQuantityException), "{0}: Quantité non autorisée" },
            { typeof(RecipeIdNotFoundException), "Product introuvable" },
            { typeof(string), "Merci pour votre achat! (Note: Le stock a été mis à jour)" }
        };

        public ProductManager ProductManager { get; private set; }
        public RecipeManager RecipeManager { get; private set; }

        public VendingMachine()
        {
            InitMachine(30);//30% fees by default
        }

        public VendingMachine(int feePercent)
        {
            if (feePercent < 0)
            {
                throw new Exception("VendingMachine: feePercent must be >= 0");
            }

            InitMachine(feePercent);
        }

        private void InitMachine(int feePercent)
        {
            _feesPercentToAdd = new FeePercentage(feePercent);

            ProductManager = new ProductManager();
            RecipeManager = new RecipeManager(ProductManager.GetProductList(), _feesPercentToAdd);
        }

        public List<Recipe> GetAllRecipes()
        {
            return RecipeManager.GetRecipeList();
        }

        public Recipe GetRecipe(string recipeId)
        {
            return RecipeManager.GetRecipe(recipeId);
        }
        public List<Product> GetAllProducts()
        {
            return ProductManager.GetProductList().Select(p => p.Value).ToList();
        }

        public bool TryCreateNewOrder(string recipeId, int requiredQuantity, ref string resultMessage)
        {
            bool succeeded = false;
            
            try
            {
                succeeded = RecipeManager.TryExtractRecipeQuantityOfInventory(recipeId, requiredQuantity, ref resultMessage);
                
                if (succeeded)
                {
                    resultMessage = UserMessages[typeof(string)];
                }
            }
            catch (OutOfStockIngredientException ex)
            {
                resultMessage = string.Format(UserMessages[typeof(OutOfStockIngredientException)], ex.RecipeName);
            }
            catch (InvalidQuantityException ex)
            {
                resultMessage = string.Format(UserMessages[typeof(InvalidQuantityException)], ex.Quantity);
            }
            catch (RecipeIdNotFoundException ex)
            {
                resultMessage = UserMessages[typeof(RecipeIdNotFoundException)];
            }

            return succeeded;
        }
    }

}
