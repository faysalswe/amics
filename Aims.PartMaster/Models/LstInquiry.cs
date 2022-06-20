using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Aims.Core.Models
{   
    public class LstInquiry
    {
        [Key]
        public string Itemnumber { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Quantity { get; set; }
        public string Serial { get; set; }
        public string Lotno { get; set; } 
        public string Color { get; set; } 
        public int Lic_Plate { get; set; }
        public string Source { get; set; }
        public string Ref { get; set; }
        public string Cost { get; set; }
        public string Itemtype { get; set; }
        public string ER { get; set; } 
        public string Mdatin { get; set; }
    }
}
