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
        LstMessage ItemNumDetailsAddUpdate(string id, string itemNumber, string rev, string description, string salesDescription, string PurchaseDescription, string invtypeidv, string itemtypeidv, string itemclassidv, string itemcodeidv, string uomid, decimal cost, decimal markup, decimal price, decimal price2, decimal price3, decimal weight, int buyitem, int obsolete, string notes, decimal leadtime, string warehouseidv, string locationsidv, string glsales, string glinv, string glcogs, string dwgno, string user1, string user2, decimal user3, string user4, string user5, string user6, string user7, string user8);
        List<LstViewLocation> ViewLocation(string itemsId, string secUsersId);
        List<LstViewLocationWh> ViewLocationWarehouse(string itemsId, string secUsersId, string warehouse);
        LstMessage BomGridDetailsUpdation(List<LstBomGridItems> LstBomGridItems);
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
        /// <param name="itemNo">Item Number</param> 
        /// <param name="Rev">Rev</param> 
        public LstMessage ItemNumDetailsAddUpdate(string id, string itemNumber, string rev, string description, string salesDescription, string PurchaseDescription, string invtypeidv, string itemtypeidv, string itemclassidv, string itemcodeidv, string uomid, decimal cost, decimal markup, decimal price, decimal price2, decimal price3, decimal weight, int buyitem, int obsolete, string notes,decimal leadtime, string warehouseidv, string locationsidv, string glsales, string glinv, string glcogs, string dwgno, string user1, string user2, decimal user3, string user4, string user5, string user6, string user7, string user8)
        {
            int actionFlag = 0;
            if (id == null || id == "")
                actionFlag = 1;
            else
                actionFlag = 2;

            var itemsid = string.IsNullOrEmpty(id) ? Guid.Empty : new Guid(id.ToString());           
            var uomsid = string.IsNullOrEmpty(uomid) ? Guid.Empty : new Guid(uomid.ToString());
                                    
            var itemNumUpdate = _amicsDbContext.LstMessage.FromSqlRaw($"exec sp_maintain_partmaster25 @actionflag='{actionFlag}',@id='{itemsid}',@itemNumber='{itemNumber}', @rev='{rev}', @description='{description}', @salesDescription='{salesDescription}', @PurchaseDescription='{PurchaseDescription}', @invtypeidv='{invtypeidv}', @itemtypeidv='{itemtypeidv}', @itemclassidv='{itemclassidv}', @itemcodeidv='{itemcodeidv}', @uomid='{uomsid}', @cost='{cost}', @markup='{markup}', @price='{price}', @price2= '{price2}', @price3= '{price3}', @weight='{weight}', @buyitem ='{buyitem}', @obsolete = '{obsolete}',@notes = '{notes}', @leadtime= '{leadtime}', @warehouseidv= '{warehouseidv}',@locationsidv='{locationsidv}',@glsales='{glsales}',@glinv='{glinv}',@glcogs='{glcogs}',@dwgno = '{dwgno}', @user1 = '{user1}', @user2 = '{user2}', @user3 = '{user3}', @user4 = '{user4}',  @user5 = '{user5}', @user6 = '{user6}', @user7 = '{user7}', @user8 = '{user8}'").AsEnumerable().FirstOrDefault();
            
            return itemNumUpdate;
        }

        /// <summary>
        /// API Service for Add/Update of Bom Grid Item details
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

    }
}
