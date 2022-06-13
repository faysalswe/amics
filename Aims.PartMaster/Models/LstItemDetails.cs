using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string User1 { get; set; }
        public string User2 { get; set; }
        public decimal User3 { get; set; }
        public string User4 { get; set; }
        public string User5 { get; set; }
        public string User6 { get; set; }
        public string User7 { get; set; }
        public string User8 { get; set; }
        public string User9 { get; set; }
        public string User10 { get; set; }
        public string User11 { get; set; }
        public string User12 { get; set; }
        public string User13 { get; set; }
        public string User14 { get; set; }
        public string User15 { get; set; }

        [NotMapped]
        public Guid? uomid { get; set; }

    }
}
