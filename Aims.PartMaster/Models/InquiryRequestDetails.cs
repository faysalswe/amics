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
        public InquiryActionType Action { get; set; } = InquiryActionType.PartMaster;
        public string User { get; set; } = "";
        public string ER { get; set; } = "";
        public string MDATIn { get; set; } = "";

    }
}

public enum  InquiryActionType
{
    PartMaster = 1,
    ER = 2,
    Location = 3,
    Description = 4,
    Serial = 5,
    Tag = 6,
    MdatIn = 7,
}
