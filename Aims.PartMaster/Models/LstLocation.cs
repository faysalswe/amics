using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstLocaton
    {     
        public Guid Id { get; set; }
        public string Location { get; set; }
        public bool Invalid { get; set; }
        public short SequenceNo { get; set; }
        public short Route { get; set; }
    }
}
