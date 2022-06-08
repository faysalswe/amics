using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{    
    /// <summary>
    /// Summary description for LstObjListItems
    /// </summary>
    public class LstItemDetails
    {      

        public Guid Id { get; set; }
        public string ItemNumber { get; set; }
        public string Rev { get; set; }
        public string DwgNo { get; set; }

        public string Description { get; set; }
        public string SalesDescription { get; set; }
        public string PurchaseDescription { get; set; }

        public string ItemType { get; set; }
        public string ItemCode { get; set; }
        public string ItemClass { get; set; }

        public decimal Cost { get; set; }
        public decimal Markup { get; set; }
        public decimal Price { get; set; }
        public decimal Price2 { get; set; }
        public decimal Price3 { get; set; }
        public decimal Weight { get; set; }


        public decimal Conversion { get; set; }
        public decimal LeadTime { get; set; }
        public decimal Minimum { get; set; }
        public decimal Maximum { get; set; }

        public string Uomref { get; set; }
        public string Purchasing_Uom { get; set; }

        public string GLSales { get; set; }
        public string GLInv { get; set; }
        public string GLCOGS { get; set; }

        public bool? UserBit { get; set; }
        public bool? UserBit2 { get; set; }
        public string Notes { get; set; }
        public string Warehouse { get; set; }
        public string Location { get; set; }
        public bool? BuyItem { get; set; }
        public bool? Obsolete { get; set; }
        public string InvType { get; set; }
        public string Child_ItemNumber { get; set; }
        public string Child_Rev { get; set; }
        public string Child_Description { get; set; }
        public string Child_Uom { get; set; }
        public decimal? Child_Cost { get; set; }
        public double Quantity { get; set; }
        public int? LineNum { get; set; }
        public string user1 { get; set; }
        public string user2 { get; set; }
        public decimal user3 { get; set; }
        public string user4 { get; set; }
        public string user5 { get; set; }
        public string user6 { get; set; }
        public string user7 { get; set; }
        public string user8 { get; set; }
        public string user9 { get; set; }
        public string user10 { get; set; }
        public string user11 { get; set; }
        public string user12 { get; set; }
        public string user13 { get; set; }
        public string user14 { get; set; }
        public string user15 { get; set; }

    }
}
