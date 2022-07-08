using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstMdat
    {
        public Guid Id { get; set; }
        public int ActionFlag { get; set; }
        public string MdatNum { get; set; }        
        public string Somain { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int Packlistnum { get; set; }
        public string Submitted_date { get; set; }
        public string Approved_date { get; set; }
        public string Shipped_date { get; set; }
        public string Cancelled_date { get; set; }
        public string Createdby { get; set; }
      
    }
}
