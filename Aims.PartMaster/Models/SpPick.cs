using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class SpPick
    {

        public DateTime? PickTransdate { get; set; }
        public Guid? PickSourcesrefId { get; set; } = null;
        public string PickMiscReason { get; set; }
        public string PickMiscRef { get; set; }
        public string PickMiscSource { get; set; }
        public string PickShipvia { get; set; }
        public string PickSource { get; set; }
        public string PickShipCharge { get; set; }
        public string PickTrackingNum { get; set; }
        public string PickNotes { get; set; }
        public string PickPackNote { get; set; }
        public string PickInvoiceNote { get; set; }
        public decimal PickSalesTax { get; set; }
        public DateTime? PickShipdate { get; set; }
        public string PickUser { get; set; }
        public int PickTransnum { get; set; }
        public int PickSetnum { get; set; }
        public string PickWarehouse { get; set; }
        public Guid? PickOriginalReceiptsid { get; set; } = null;
        public Guid? PickItemId { get; set; } = null;
        public string PickQty { get; set; }


    }

}
