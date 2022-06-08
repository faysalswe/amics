using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstItemsPO
    {
        public Guid Id { get; set; }
        public Guid PomainId { get; set; }
        public string Pomain { get; set; }
        public Int64 Linenum { get; set; }
        public Int16 Poline { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Received { get; set; }
        public string Somain { get; set; }
        public string Trans_date { get; set; }
        public string Supplier { get; set; }
        public string P10 { get; set; }
        public string P11 { get; set; }
        public string P12 { get; set; }
        public string P13 { get; set; }
        public string P14 { get; set; }
        public string P15 { get; set; }
        public string P16 { get; set; }
        public string P17 { get; set; }
        public string P18 { get; set; }
        public string P19 { get; set; }
        public string P20 { get; set; }
    }
}
