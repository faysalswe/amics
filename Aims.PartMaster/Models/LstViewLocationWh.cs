using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aims.Core.Models
{
    public class LstViewLocationWh
    {
        [Key]        
        public string Location { get; set; }
        public string Somain { get; set; }
        public string Name { get; set; }
        public Int16 Quantity { get; set; }
    }
}
