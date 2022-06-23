using System;

namespace Amics.Api.Model
{
    public class InquiryRequest
    {
        public InquiryActionType Action { get; set; }
        public string SearchText { get; set; }
        public string User { get; set; }
    }
}
