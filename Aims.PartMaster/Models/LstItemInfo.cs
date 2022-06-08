using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstItemInfo
    {     
        public Guid Id { get; set; }
        public string ItemNumber { get; set; }
        public string Rev { get; set; }
        public string Description { get; set; }
        public string Warehouse { get; set; }
        public string Location { get; set; }     
        public decimal Cost { get; set; }     
    }
}
