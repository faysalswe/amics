using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aims.Core.Models
{
    public class LstUserAccess
    {
        [Key]
        public string Id { get; set; }
        public string accessId { get; set; }
        public string userId { get; set; }
        public string access { get; set; }
        public int readOnly { get; set; }
        public int actionFlag { get; set; }
        public int OnTheFly { get; set; }
        public string module { get; set; }
    }
}
