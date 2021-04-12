using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineSystem
{
    public class InvalidQuantityException : Exception
    {
        public int Quantity {get; set;}
        public InvalidQuantityException(int quantity) : base(String.Format("This quantity is invalid: {0}", quantity))
        {
            Quantity = quantity;
        }
    }

}
