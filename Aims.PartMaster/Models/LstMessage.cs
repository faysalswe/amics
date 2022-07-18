using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aims.Core.Models
{ 
    public class LstMessage
    {
        [Key] 
        public string Message { get; set; }
        
    }
    
    public class LstPacklist
    {
        [Key]
        public string Packlist { get; set; }

    }

}
