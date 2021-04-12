using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineSystem
{
    public class FeePercentage
    {
        public int Fee { get; }

        public FeePercentage(int feePercent)
        {
            Fee = feePercent;
        }
    }

}
