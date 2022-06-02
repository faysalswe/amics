using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstUom
    {     
        public Guid Id { get; set; }
        public string Uom { get; set; }
        public string PurchasingUom { get; set; }
        public decimal Factor { get; set; }
    }
}
