using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineSystem
{
    public interface IProductManager
    {
        void AddProductToMachine(string productName, decimal costOfGood, int numberOfUnits = 5);
        bool RemoveProduct(string productId, ref string resultMessage);
        string GetProductIdByName(string productName);
    }
}
