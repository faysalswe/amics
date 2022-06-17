using System;

namespace Amics.Api.Model
{
    public class InquiryRequest
    {
        public InquiryActionType Action { get; set; }
        public string SearchText { get; set; }
        public Guid User { get; set; }
    }
}
