using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstShipments
    {
        public string SoMain { get; set; }
        public string SoLinesId { get; set; }
        public string ItemsId { get; set; }
        public string CustomerId { get; set; }

        public string ItemNumber { get; set; }
        public string Rev { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string ShippedQuantity { get; set; }
        public string OutStandingQuantity { get; set; }

        public string PackList { get; set; }
        public string LineNum { get; set; }
        public string ItemType { get; set; }
        public string ShipDate { get; set; }
        public string CustPo { get; set; }
        public string EstDate { get; set; }
        public string Weight { get; set; }
        public string Obsoleted { get; set; }

        public string LotOrSn { get; set; }
        public string ColorOrTag { get; set; }
        public string expDate { get; set; }
        public string PackNotes { get; set; }
        public string InvoiceNotes { get; set; }

        public string ShipVia { get; set; }
        public string Terms { get; set; }
        public string ShipChargers { get; set; }
        public string TrackNumber { get; set; }

        public string PostInvoice { get; set; }
        public string ShippedBy { get; set; }

        public string BillToName { get; set; }
        public string BillToAddress1 { get; set; }
        public string BillToAddress2 { get; set; }
        public string BillToAddress3 { get; set; }
        public string BillToAddress4 { get; set; }
        public string BillToAddress5 { get; set; }
        public string BillToAddress6 { get; set; }
        public string ShipToName { get; set; }
        public string ShipToAddress1 { get; set; }
        public string ShipToAddress2 { get; set; }
        public string ShipToAddress3 { get; set; }
        public string ShipToAddress4 { get; set; }
        public string ShipToAddress5 { get; set; }
        public string ShipToAddress6 { get; set; }
        public string MdatOut { get; set; }
    }
}
