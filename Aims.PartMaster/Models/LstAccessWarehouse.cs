using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstAccessWarehouse
    {
        public string Id { get; set; }
        public string WarehouseId { get; set; }
        public string UserId { get; set; }
        public string Warehouse { get; set; }
        public int ReadOnly { get; set; }
        public int ActionFlag { get; set; }
        public string Createdby { get; set; }
    }

}
