using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstBomGridItems
    {
        public int ActionFlag { get; set; }
        public string Id { get; set; }
        public string Parent_ItemsId { get; set; }
        public string Child_ItemsId { get; set; }
        public int LineNum { get; set; }
        public string Quantity { get; set; }
        public string Ref { get; set; }
        public string Comments { get; set; }
        public string Createdby { get; set; }
        public string FindNo { get; set; }
    }
}
