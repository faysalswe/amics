using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstProjectTransData
    {
        public double Cost { get; set; }
        public string Warehouse { get; set; }
        public string Location { get; set; }
        public double Quantity { get; set; }
        public double TransactionQuantity { get; set; }
        public string BasicId { get; set; }
        public string SerialId { get; set; }

        public string SerialNumber { get; set; }
        public string TagNumber { get; set; }
        public string LotNumber { get; set; }
        public string Color { get; set; }

        public string InvType { get; set; }
        public string Notes { get; set; }

        public string ItemNumber { get; set; }
        public string Rev { get; set; }
        public string ItemsId { get; set; }
        public string LocationsId { get; set; }

        public string Description { get; set; }
        public string SourcesRefId { get; set; }
        public string Coststring { get; set; }

        public string Qtystring { get; set; }
        public string ToWarehouse { get; set; }
        public string ToLocation { get; set; }

        public string Comments { get; set; }

        public string userName { get; set; }
        public string soMain { get; set; }
        public string toSoMain { get; set; }
        public string fromSoLine { get; set; }
        public string toSoLine { get; set; }

        public string projId { get; set; }

        public string toSolinesId { get; set; }
    }
}
