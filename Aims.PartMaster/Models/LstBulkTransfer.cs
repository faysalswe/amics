using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstBulkTransfer
    {
        public string ItemNumber { get; set; }
        public string Rev { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
    }

    public class LstBulkTransferUpdate
    {
        public string WarehouseFrom { get; set; }
        public string LocationFrom { get; set; }
        public string WarehouseTo { get; set; }       
        public string LocationTo { get; set; }
        public string UserName { get; set; }
    }
}
