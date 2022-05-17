using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Inventory.Models
{
    public class LstInventoryStatus
    {
        public LstInventoryStatus()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public double OnHand { get; set; }
        public double NotAvailable { get; set; }
        public double Allocated { get; set; }
        public double Available { get; set; }

    }
}
