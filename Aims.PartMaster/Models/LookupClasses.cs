using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstReasonCodes
    {     
        public Guid Id { get; set; }
        public string Reason { get; set; }        
    }

    public class LstCompanyOptions
    {
        public Guid Id { get; set; }
        public decimal OptionId { get; set; }
        public string Description { get; set; }
        public bool YesOrNo { get; set; }
        public decimal OptionValue { get; set; }       
    }


}
