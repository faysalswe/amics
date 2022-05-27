using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstLocaton
    {     
        public Guid Id { get; set; }
        public string Location { get; set; }
        public string Invalid { get; set; }
        public string SequenceNo { get; set; }
        public string Route { get; set; }
    }
}
