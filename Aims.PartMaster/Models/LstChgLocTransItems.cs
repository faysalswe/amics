using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstChgLocTransItems
    {
        public string Id { get; set; }
        public int Action { get; set; }
        public string SoLinesId { get; set; }       
        public int AvailQuantity { get; set; }
        public int TransQuantity { get; set; }
        public string InvSerialId { get; set; }
        public string InvBasicId { get; set; }
        public string CreatedBy { get; set; }

    }
}
