using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstSerial
    {
        public Guid Id { get; set; }
        public string Pomain { get; set; }
        public string Serlot { get; set; }
        public string Tagcol { get; set; }
        public string Warehouse { get; set; }
        public string Location { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public string Expdate { get; set; }
        public string Color_model { get; set; }
        public string Invtype { get; set; }
        public string ActualSo { get; set; }
        public string CurrentSo { get; set; }
    }
}
