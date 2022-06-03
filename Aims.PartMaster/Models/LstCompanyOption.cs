using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstCompanyOption
    {
        public Guid Id { get; set; }
        public decimal OptionId { get; set; }
        public string Description { get; set; }
        public bool YesorNo { get; set; }
        public decimal OptionValue { get; set; }
    }
}
