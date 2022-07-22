using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class InvTrans
    {
        public int TransNum { get; set; } = 0;
        public Guid? InvBasicId { get; set; } = null;
        public Guid? InvSerialId { get; set; } = null;
        public Guid? ItemsId { get; set; } = null;
        public string Source { get; set; }
        public Guid? RefId { get; set; } = null;
        public Guid? FromLocationId { get; set; } = null;
        public Guid? ToLocationId { get; set; } = null;
        public double TransQty { get; set; }
        public string ItemNumber { get; set; }
        public string Rev { get; set; }
        public int BoxNum { get; set; }
        public string lineWeight { get; set; }
        public string CreatedBy { get; set; }
    }



}
