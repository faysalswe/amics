using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.PartMaster.Models
{    
    /// <summary>
    /// Summary description for LstObjListItems
    /// </summary>
    public class ListItems
    {
        public ListItems()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string Id { get; set; }
        public string ItemNumber { get; set; }
        public string Rev { get; set; }
        public string DwgNo { get; set; }

        public string Description { get; set; }
        public string SalesDescription { get; set; }
        public string PurchaseDescription { get; set; }

        public string ItemType { get; set; }
        public string ItemCode { get; set; }
        public string ItemClass { get; set; }

        public double? Cost { get; set; }
        public double? Markup { get; set; }
        public double? Price { get; set; }
        public double? Price2 { get; set; }
        public double? Price3 { get; set; }

        public double Price4 { get; set; }
        public double Price5 { get; set; }
        public double Price6 { get; set; }
        public double Price7 { get; set; }
        public double Price8 { get; set; }
        public double Price9 { get; set; }
        public double Price10 { get; set; }

        public double? Conversion { get; set; }
        public double? LeadTime { get; set; }
        public double? Minimum { get; set; }
        public double? Maximum { get; set; }

        public string Uom { get; set; }
        public string PurchasingUom { get; set; }

        public string GLSales { get; set; }
        public string GLInv { get; set; }
        public string GLCOGS { get; set; }

        public bool? UserBit { get; set; }
        public bool? BuyItem { get; set; }
        public bool? Obsolete { get; set; }
        public bool? Taa { get; set; }
        //public bool CreatePO { get; set; }



        public string Warehouse { get; set; }
        public string Location { get; set; }
        public string InvType { get; set; }
        public double Quantity { get; set; }
        public string Notes { get; set; }
        public string Ref { get; set; }
      //  public string Comments { get; set; }
       // public double ExtCost { get; set; }
        public int LineNum { get; set; }

       // public int ActionFlag { get; set; }
        public string ParentItemNumber { get; set; }
        public string ParentRev { get; set; }
       // public string BomId { get; set; }
        public string ReasonCode { get; set; }
        public double Weight { get; set; }

        //public string Coststring1 { get; set; }
        //public string Coststring2 { get; set; }
        //public string Coststring { get; set; }
        //public string Coststring4 { get; set; }

        //public string ExtCostString { get; set; }
        public string Qtystring { get; set; }

        public string user1 { get; set; }
        public string user2 { get; set; }
        public double user3 { get; set; }
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

        public string supplierId { get; set; }
        public string suppliersItemsId { get; set; }
        public string supplierItem { get; set; }

        public double supplierQty { get; set; }


        public string project { get; set; }


        public string Pricestring { get; set; }
        public string Pricestring2 { get; set; }
        public string Pricestring3 { get; set; }
        public string Pricestring4 { get; set; }
        public string Pricestring5 { get; set; }
        public string Pricestring6 { get; set; }
        public string Pricestring7 { get; set; }
        public string Pricestring8 { get; set; }
        public string Pricestring9 { get; set; }
        public string Pricestring10 { get; set; }

      //  public string ItemsId { get; set; }

        public string onhand { get; set; }
        public string allotedqty { get; set; }
        public string availableqty { get; set; }
        public string notavailableqty { get; set; }
        public string shortage { get; set; }



    }
}
