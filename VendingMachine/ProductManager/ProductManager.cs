using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineSystem
{
    public class ProductManager : IProductManager
    {
        protected Dictionary<string, Product> _products = new Dictionary<string, Product>();

        public Dictionary<string, Product> GetProductList()
        {
            return _products;
        }
        public void AddProductToMachine(string productName, decimal costOfGood, int numberOfUnits = 5)
        {
            var produt = new Product(productName, costOfGood, numberOfUnits);
            _products.Add(produt.ProductId, produt);
        }
        public bool RemoveProduct(string productId, ref string resultMessage)
        {
            bool isRemoved = false;

            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new Exception("RemoveProduct: productId cannot be empty");
            }

            if (!_products.ContainsKey(productId))
            {
                throw new Exception($"RemoveProduct: _products does not contain the key {productId}");
            }

            isRemoved = _products.Remove(productId);

            return isRemoved;
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
