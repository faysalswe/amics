using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstItemSearch
    {     
        public string Id { get; set; }
        public string ItemNumber { get; set; }
        public string Rev { get; set; }
        public string Description { get; set; }
        public string ItemType { get; set; }
        public string ItemCode { get; set; }
        public string ItemClass { get; set; }
        public string Uomref { get; set; }
        public decimal Cost { get; set; }
        public string DwgNo { get; set; }
        public decimal? Conversion { get; set; }        

    }
}
