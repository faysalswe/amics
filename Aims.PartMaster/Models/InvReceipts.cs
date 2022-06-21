using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Aims.Core.Models
{
   
    public class InvReceipts
    {     
        public Guid SourcesRefId { get; set; }
        public string Source { get; set; }
        public Guid? ExtendedId { get; set; }
        public string Warehouse { get; set; }
        public string Location { get; set; }
        public string ItemNumber { get; set; }
        public string Rev { get; set; }
        public decimal Cost { get; set; } = 0;
        public decimal Quantity { get; set; } = 0;
        public string MiscReason { get; set; }
        public string MiscRef { get; set; }
        public string MiscSource { get; set; }
        public string Notes { get; set; }
        public string TransDate { get; set; }
        public int TransNum { get; set; } = 0;
        public string PoType { get; set; }
        public string RecAccount { get; set; }
        public string RecPackList { get; set; }
        public bool LicPlatFlage { get; set; }=false;
        public int ReceiverNum { get; set; } = 0;
        public string User1 { get; set; }
        public string User2 { get; set; }
        
    }
}
