using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aims.Core.Models
{ 
    public partial class OutValidateSerTag
    {       
        public Guid itemsid { get; set; } 
        public Guid serialid { get; set; }
        public Guid locationsid { get; set; }
        public string itemnumber { get; set; }
        public string rev { get; set; }
        public string serno { get; set; }
        public string tagno { get; set; }
    }

    public partial class InputValidateSerTag
    {
        public string itemsid { get; set; }      
        public string sertag { get; set; }
        public string option { get; set; }
    }
}
