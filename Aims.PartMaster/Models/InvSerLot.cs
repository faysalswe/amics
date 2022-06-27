using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class InvSerLot
    {         
        public int Transnum { get; set; }
        public string SerNo { get; set; }
        public string TagNo { get; set; }
        public string LotNo { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public decimal Qty { get; set; }
        public string CreatedBy { get; set; }     
        public DateTime? ExpDate { get; set; }
    }

}
