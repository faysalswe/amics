using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstItemsBom
    {
        public Guid Id { get; set; }
        public Guid Itemsid_Parent { get; set; }
        public Guid Itemsid_Child { get; set; }
        public string ItemNumber { get; set; }
        public string Rev { get; set; }        
        public string Description { get; set; }
        public decimal? Quantity { get; set; }
        public string uomref { get; set; }
        public string Ref { get; set; }
        public string Comments { get; set; }
        public string FindNo { get; set; }
        public int LineNum { get; set; }
        public decimal? Cost { get; set; }
        public decimal? ExtCost { get; set; }
        public string Itemtype { get; set; }
    }
}
