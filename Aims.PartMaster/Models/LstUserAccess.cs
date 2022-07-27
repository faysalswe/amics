using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aims.Core.Models
{
    public class LstUserAccess
    {
        public int ActionFlag { get; set; }
        public string Id { get; set; }
        public string AccessId { get; set; }
        public string UserId { get; set; }
        public string Access { get; set; }
        public int ReadOnly { get; set; }        
        public int OnTheFly { get; set; }
        public string Module { get; set; }
        public string Createdby { get; set; }
    }
}
