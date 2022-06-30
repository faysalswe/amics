using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aims.Core.Models
{
    public class LstTransLog
    {
        [Key]
        public Guid id { get; set; }
        public Guid Invtransid { get; set; }
        public string Itemnumber { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Ref { get; set; }
        public decimal? Quantity { get; set; }    
        public string TransDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string SerNo { get; set; }
        public string TagNo { get; set; }
        public string LotNo { get; set; }
        public string Warehouse { get; set; }
        public string Location { get; set; }
        public decimal? Cost { get; set; }
        public string Notes { get; set; }  
    }
}
