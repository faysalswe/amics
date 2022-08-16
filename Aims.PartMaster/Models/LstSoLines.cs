using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstSoLines
    {
        public string Id { get; set; }
        public string ActionFlag { get; set; }
        public string SoNumber { get; set; }
        public string CustomersItemsId { get; set; }
        public int Line { get; set; }
        public string ItemsId { get; set; }
        public string ItemNumber { get; set; }
        public string Rev { get; set; }
        public string Description { get; set; }
        public string Qty { get; set; }
        public string ShippedQty { get; set; }
        public string ShippedQtyStr { get; set; }
        public string Uom { get; set; }
        public double UnitCost { get; set; }
        public double Markup { get; set; }
        public double Cost { get; set; }
        public double ExtCost { get; set; }
        public string EstShipDate { get; set; }
        public string ShipDate { get; set; }
        public string WarrantyYears { get; set; }
        public string WarrantyDays { get; set; }
        public string CreatedBy { get; set; }
        public string SoLineId { get; set; }
        public string SoLinesNotes { get; set; }
        public string User1 { get; set; }
        public string CostStr { get; set; }
        public string CostStr1 { get; set; }
        public string CostStr2 { get; set; }
        public string CostStr4 { get; set; }
        public string ExtCostStr { get; set; }
        public string QtyStr { get; set; }
        public string MarkupStr { get; set; }
        public string Obsolete { get; set; }
        public string ItemType { get; set; }
        public string AvailQty { get; set; }
        public string MarkupChanged { get; set; }
    }
}
