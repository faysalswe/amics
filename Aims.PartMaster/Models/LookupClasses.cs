using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    public partial class InvStatus
    {
        [Key]
        public string Pn { get; set; }
        public string Descr { get; set; }
        public decimal? Allocated { get; set; }
        public decimal? Avail { get; set; }
        public decimal? Notavail { get; set; }
        public decimal? Total { get; set; }


    }


    public partial class LstDefaultsValues
    {
        public Guid Id { get; set; }
        public string FormName { get; set; }
        public string TextFields { get; set; }
        public string Value { get; set; }      

    }

    public class LstErLookup
    {
        public Guid Id { get; set; }
        public string SoMain { get; set; }      
    }

    public class LstMdatStatusLookup
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }


}
