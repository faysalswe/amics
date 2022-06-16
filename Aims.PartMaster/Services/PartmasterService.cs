using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aims.Core.Models;
using Aims.PartMaster.Models;
using Microsoft.EntityFrameworkCore;

namespace Aims.Core.Services
{
    public interface IPartmasterService
    {
        LstItemDetails LoadPartmaster(string itemNumber, string rev);
        List<LstItemsBom> LoadItemsBom(string parentItemId);
        List<LstItemsPO> LoadItemsPO(string parentId);
        LstBomCount ItemsBomCount(string parentId);
        LstMessage ItemNumDelete(string itemNo, string rev);
        LstMessage ItemNumDetailsAddUpdate(LstItemDetails item);
        List<LstViewLocation> ViewLocation(string itemsId, string secUsersId);
        List<LstViewLocationWh> ViewLocationWarehouse(string itemsId, string secUsersId, string warehouse);
        LstMessage BomGridDetailsUpdation(List<LstBomGridItems> LstBomGridItems);
        List<LstInquiry> InquiryDetails(string itemnum, string desc, string lotno, string serial, string tag, string location, string action, string user, string er, string mdatIn);
        List<LstSerial> ViewSerial(string itemsId, string secUsersId);
        List<LstSerial> ViewSerialWarehouse(string itemsId, string secUsersId, string warehouse);
        List<LstSerial> ViewSerialSerNo(string itemsId, string secUsersId, string warehouse, string serNo);
        List<LstSerial> ViewSerialTagNo(string itemsId, string secUsersId, string warehouse, string tagNo);
    }
    public class PartmasterService: IPartmasterService
    {
        private readonly AmicsDbContext _amicsDbContext;
        public PartmasterService(AmicsDbContext aimsDbContext)
        {
            _amicsDbContext = aimsDbContext;
        }

        /// <summary>
        /// API Service to get Partmaster details
        /// </summary>
        /// <param name="itemnumber">itemnumber</param>  
        /// <param name="rev">rev</param>    
        public LstItemDetails LoadPartmaster(string itemNumber, string rev)
        {            
            var itemNum = string.IsNullOrEmpty(itemNumber) ? string.Empty : itemNumber;
            var revDef = string.IsNullOrEmpty(rev) ? "-" : rev;
            
            //The code above should resolve the error.
            var itemresult = _amicsDbContext.LstItemDetails
                .FromSqlRaw($"exec sp_webservice_load_partmaster5 @item ='{itemNum}',@rev = '{revDef}'").AsEnumerable().FirstOrDefault();

            return itemresult;
        }

        /// <summary>
        /// API Service to get BOM details
        /// </summary>
        /// <param name="parentId">Parent Item Id</param>          
        public List<LstItemsBom> LoadItemsBom(string parentId)
        {
            var itemsId = string.IsNullOrEmpty(parentId) ? Guid.Empty : new Guid(parentId.ToString());
            
            var bomItemresult = _amicsDbContext.LstItemsBom
                .FromSqlRaw($"exec sp_view_items_bom5 @parentid ='{itemsId}'").ToList();

            return bomItemresult;
        }

        /// <summary>
        /// API Service to get PO details
        /// </summary>
        /// <param name="parentId">Parent Item Id</param>          
        public List<LstItemsPO> LoadItemsPO(string parentId)
        {
            var itemsId = string.IsNullOrEmpty(parentId) ? Guid.Empty : new Guid(parentId.ToString());

            var poItemresult = _amicsDbContext.LstItemsPO
                .FromSqlRaw($"exec sp_view_items_po5 @parentid ='{itemsId}'").ToList();

            return poItemresult;
        }

        /// <summary>
        /// API Service to check whether Item BOM is exist or not in database
        /// </summary>
        /// <param name="parentId">Parent Item Id</param>          
        public LstBomCount ItemsBomCount(string parentId)
        {
            var itemsId = string.IsNullOrEmpty(parentId) ? Guid.Empty : new Guid(parentId.ToString());

            var bomexist = _amicsDbContext.LstBomCount.FromSqlRaw($"exec amics_sp_itembom_exist @parentid ='{itemsId}'").AsEnumerable().FirstOrDefault();

            return bomexist;
        }

        /// <summary>
        /// API Service for deletion of Item Num details, column 'flag_delete' is update with 1 in the table 
        /// list items(item num deleted) if return message is null. Message will appear if item num is used in some other tables, so item num can't delete.
        /// </summary>
        /// <param name="itemNo">Item Number</param> 
        /// <param name="Rev">Rev</param> 
        public LstMessage ItemNumDelete(string itemNo, string rev)
        {
            var itemNum = string.IsNullOrEmpty(itemNo) ? string.Empty : itemNo;            
            var revDef = string.IsNullOrEmpty(rev) ? "-" : rev;

            var deleteMsg = _amicsDbContext.LstMessage.FromSqlRaw($"exec sp_delete_list_items5 @item='{itemNum}',@rev='{revDef}'").AsEnumerable().FirstOrDefault();

            return deleteMsg;
        }

        /// <summary>
        /// API Service for Add/Update of Item details
        /// Insert/Update the Item details from the Parent form into list_items table. If id is null, data will be added in the table. Data will be updated if id is not null. 
        /// </summary>
        /// <param name="LstItemDetails">Item Details</param>         
        public LstMessage ItemNumDetailsAddUpdate(LstItemDetails item)
        {
            int actionFlag = 0;
            if (item.Id == null || item.Id == Guid.Empty)
                actionFlag = 1;
            else
                actionFlag = 2;

            var itemsid = (item.Id == null || item.Id == Guid.Empty) ? Guid.Empty : item.Id;          
            var uomsid = (item.Uomid == null || item.Uomid == Guid.Empty) ? Guid.Empty : item.Uomid;

            var itemNumUpdate = _amicsDbContext.LstMessage.FromSqlRaw($"exec sp_maintain_partmaster25 @actionflag='{actionFlag}',@id='{itemsid}',@itemNumber='{item.ItemNumber}', @rev='{item.Rev}', @description='{item.Description}', @salesDescription='{item.SalesDescription}', @PurchaseDescription='{item.PurchaseDescription}', @invtypeidv='{item.InvType}', @itemtypeidv='{item.ItemType}', @itemclassidv='{item.ItemClass}', @itemcodeidv='{item.ItemCode}', @uomid='{item.Uomid}', @cost='{item.Cost}', @markup='{item.Markup}', @price='{item.Price}', @price2= '{item.Price2}', @price3= '{item.Price3}', @weight='{item.Weight}', @buyitem ='{item.BuyItem}', @obsolete = '{item.Obsolete}',@notes = '{item.Notes}', @leadtime= '{item.LeadTime}', @warehouseidv= '{item.Warehouse}',@locationsidv='{item.Location}',@glsales='{item.GLSales}',@glinv='{item.GLInv}',@glcogs='{item.GLCOGS}',@dwgno = '{item.DwgNo}', @user1 = '{item.User1}', @user2 = '{item.User2}', @user3 = '{item.User3}', @user4 = '{item.User4}',  @user5 = '{item.User5}', @user6 = '{item.User6}', @user7 = '{item.User7}', @user8 = '{item.User8}'").AsEnumerable().FirstOrDefault();
            
            return itemNumUpdate;
        }

        /// <summary>        
        /// API Service for Insert/Update/Delete Bom Item details in the items_bom table
        /// </summary>
        /// <param name="LstBomGridItems">Bom Item details</param>         
        public LstMessage BomGridDetailsUpdation(List<LstBomGridItems> LstBomGridItems)
        {
            var bomUpdate = (dynamic)null;

            foreach (var bomItem in LstBomGridItems)
            {
                var bomItemsid = string.IsNullOrEmpty(bomItem.Id) ? Guid.Empty : new Guid(bomItem.Id.ToString());                              

                bomUpdate = _amicsDbContext.LstMessage.FromSqlRaw($"exec sp_maintain_item_bom5 @actionflag='{bomItem.ActionFlag}',@id='{bomItemsid}',@itemsid_parent='{bomItem.Parent_ItemsId}',@itemsid_child='{bomItem.Child_ItemsId}',@linenum='{bomItem.LineNum}',@quantity='{bomItem.Quantity}',@ref='{bomItem.Ref}',@comments='{bomItem.Comments}',@createdby='{bomItem.Createdby}',@findno='{bomItem.FindNo}'").AsEnumerable().FirstOrDefault();
            }           
            return bomUpdate;
        }

        /// <summary>
        /// API Service to get warehouse,location, somain, quantity & name details using below sql function        
        /// </summary>
        /// <param name="itemsId">Items Id</param> 
        /// <param name="secUsersId">Sec_Users Id</param> 
        public List<LstViewLocation> ViewLocation(string itemsId, string secUsersId)
        {
            var itmId = string.IsNullOrEmpty(itemsId) ? string.Empty : itemsId;
            var usersId = string.IsNullOrEmpty(secUsersId) ? string.Empty : secUsersId;
                        
            var viewLocResult = _amicsDbContext.LstViewLocation.FromSqlRaw($"select * from fn_view_location_summary_essex('{itmId}','{usersId}')").ToList();
            
            return viewLocResult;
        }
        /// <summary>
        /// API Service to get location, somain, quantity & name details for given Warehouse
        /// </summary>
        /// <param name="itemsId">Items Id</param> 
        /// <param name="secUsersId">Sec_Users Id</param> 
        /// <param name="warehouse">Warehouse</param> 
        public List<LstViewLocationWh> ViewLocationWarehouse(string itemsId, string secUsersId, string warehouse)
        {
            var itmId = string.IsNullOrEmpty(itemsId) ? string.Empty : itemsId;
            var usersId = string.IsNullOrEmpty(secUsersId) ? string.Empty : secUsersId;

            var viewLocResult = _amicsDbContext.LstViewLocationWh.FromSqlRaw($"select * from fn_view_location_summary_whs_essex('{itmId}','{usersId}','{warehouse}')").ToList();

            return viewLocResult;
        }

        /// <summary>
        /// API Service to get Inquiry details based on the search
        /// </summary>
        /// <param name="itemnum">Items Id</param> 
        /// <param name="desc">Description</param> 
        /// <param name="lotno">Lot No</param> 
        /// <param name="serial">Serial No</param> 
        /// <param name="tag">Tag No</param> 
        /// <param name="location">Location</param> 
        /// <param name="action">Action</param> 
        /// <param name="er">ER</param> 
        public List<LstInquiry> InquiryDetails(string itemnum, string desc, string lotno,string serial, string tag, string location, string action, string user, string er, string mdatIn)
        {
            var inquiryResult = _amicsDbContext.LstInquiry.FromSqlRaw($"exec sp_inquiry5 @part='{itemnum}',@desc = '{desc}',@lotno='{lotno}',@location ='{location}',@action = '{action}', @serial='{serial}',@tag ='{tag}',@user = '{user}', @er = '{er}', @mdatIn='{mdatIn}'").ToList();
            
            return inquiryResult;
        }

        /// <summary>
        /// API Service to get warehouse,location, pomain, serial no, tag no, cost, quantity details using below sql function        
        /// </summary>
        /// <param name="itemsId">Items Id</param> 
        /// <param name="secUsersId">Sec_Users Id</param> 
        public List<LstSerial> ViewSerial(string itemsId, string secUsersId)
        {
            var itmId = string.IsNullOrEmpty(itemsId) ? string.Empty : itemsId;
            var usersId = string.IsNullOrEmpty(secUsersId) ? string.Empty : secUsersId;

            var viewLocResult = _amicsDbContext.LstSerial.FromSqlRaw($"select * from fn_view_location_detail('{itmId}','{usersId}') where quantity > 0").ToList();

            return viewLocResult;
        }

        /// <summary>
        /// API Service to get location, pomain,serial no, tag no, cost, quantity details for selected warehouse using below sql function           
        /// </summary>
        /// <param name="itemsId">Items Id</param> 
        /// <param name="secUsersId">Sec_Users Id</param> 
        public List<LstSerial> ViewSerialWarehouse(string itemsId, string secUsersId, string warehouse)
        {
            var itmId = string.IsNullOrEmpty(itemsId) ? string.Empty : itemsId;
            var usersId = string.IsNullOrEmpty(secUsersId) ? string.Empty : secUsersId;

            var viewLocResult = _amicsDbContext.LstSerial.FromSqlRaw($"select * from fn_view_location_detail('{itmId}','{usersId}') where warehouse='{warehouse}'").ToList();

            return viewLocResult;
        }

        /// <summary>
        /// API Service to get location, pomain,serial no, tag no, cost, quantity details for selected warehouse and serial no using below sql function           
        /// </summary>
        /// <param name="itemsId">Items Id</param> 
        /// <param name="secUsersId">Sec_Users Id</param> 
        public List<LstSerial> ViewSerialSerNo(string itemsId, string secUsersId, string warehouse, string serNo)
        {
            var serResult = (dynamic)null;
            var itmId = string.IsNullOrEmpty(itemsId) ? string.Empty : itemsId;
            var usersId = string.IsNullOrEmpty(secUsersId) ? string.Empty : secUsersId;

            if (warehouse == null || warehouse == "")
                serResult = _amicsDbContext.LstSerial.FromSqlRaw($"select * from fn_view_location_detail('{itmId}','{usersId}') where serlot like '{serNo}'").ToList();
            else
                serResult = _amicsDbContext.LstSerial.FromSqlRaw($"select * from fn_view_location_detail('{itmId}','{usersId}') where serlot like '{serNo}' and warehouse='{warehouse}'").ToList();

            return serResult;
        }

        /// <summary>
        /// API Service to get location, pomain,serial no, tag no, cost, quantity details for selected warehouse and tag no using below sql function           
        /// </summary>
        /// <param name="itemsId">Items Id</param> 
        /// <param name="secUsersId">Sec_Users Id</param> 
        /// <param name="warehouse">Warehouse</param> 
        /// <param name="tagNo">Tag No</param> 
        public List<LstSerial> ViewSerialTagNo(string itemsId, string secUsersId, string warehouse, string tagNo)
        {
            var tagResult = (dynamic)null;
            var itmId = string.IsNullOrEmpty(itemsId) ? string.Empty : itemsId;
            var usersId = string.IsNullOrEmpty(secUsersId) ? string.Empty : secUsersId;

            if (warehouse == null || warehouse == "")
                tagResult = _amicsDbContext.LstSerial.FromSqlRaw($"select * from fn_view_location_detail('{itmId}','{usersId}') where tagcol like '{tagNo}'").ToList();
            else
                tagResult = _amicsDbContext.LstSerial.FromSqlRaw($"select * from fn_view_location_detail('{itmId}','{usersId}') where tagcol like '{tagNo}' and warehouse='{warehouse}'").ToList();

            return tagResult;
        }
    }
}
