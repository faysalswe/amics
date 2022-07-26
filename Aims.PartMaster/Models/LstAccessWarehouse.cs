using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstAccessWarehouse
    {
        public string Id { get; set; }
        public string WarehouseId { get; set; }
        public string userId { get; set; }
        public string Warehouse { get; set; }
        public int readOnly { get; set; }
        public int actionFlag { get; set; }
    }
}
