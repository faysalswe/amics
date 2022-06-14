using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aims.Core.Models
{
    public class LstViewLocation
    {
        [Key]
        public string Warehouse { get; set; }
        public string Location { get; set; }
        public string somain { get; set; }
        public string name { get; set; }
        public Int16 Quantity { get; set; }
    }
}
