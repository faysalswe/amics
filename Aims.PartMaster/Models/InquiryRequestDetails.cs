using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class InquiryRequestDetails
    {
        public string ItemNumber { get; set; } = "";
        public string Description { get; set; } = "";
        public string LotNo { get; set; } = "";

        public string Serial { get; set; } = "";
        public string Tag { get; set; } = "";
        public string Location { get; set; } = "";
        public string Action { get; set; } = "";
        public string User { get; set; } = "";
        public string ER { get; set; } = "";
        public string MDATIn { get; set; } = "";

    }
}
