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
        LstMessage ItemNumDetailsAddUpdate(int actionFlag, string id, string itemNumber, string rev, string description, string salesDescription, string PurchaseDescription, string invtypeidv, string itemtypeidv, string itemclassidv, string itemcodeidv, string uomid, decimal conversion, decimal cost, decimal markup, decimal price, decimal price2, decimal price3, decimal weight, bool reorder, int buyitem, int obsolete, bool cyclecount, string notes, decimal minimum, decimal maximum, decimal leadtime, string warehouseidv, string locationsidv, string glsales, string glinv, string glcogs, string dwgno, string user1, string user2, decimal user3, int userbit, int userbit2, int userbit3);
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
        /// list items(item num deleted) if return message is null. Message will appear if item num is used in some other tables, so item num can't delete.
        /// </summary>
        /// <param name="itemNo">Item Number</param> 
        /// <param name="Rev">Rev</param> 
        public LstMessage ItemNumDetailsAddUpdate(int actionFlag, string id, string itemNumber, string rev, string description, string salesDescription, string PurchaseDescription, string invtypeidv, string itemtypeidv, string itemclassidv, string itemcodeidv, string uomid, decimal conversion, decimal cost, decimal markup, decimal price, decimal price2, decimal price3, decimal weight, bool reorder, int buyitem, int obsolete,bool cyclecount, string notes, decimal minimum, decimal maximum, decimal leadtime, string warehouseidv, string locationsidv, string glsales, string glinv, string glcogs, string dwgno, string user1, string user2, decimal user3, int userbit, int userbit2, int userbit3)
        {
            var itemsid = (dynamic)null;
            var itemNumUpdate = (dynamic)null;
            if (actionFlag == 2)
                itemsid = string.IsNullOrEmpty(id) ? Guid.Empty : new Guid(id.ToString());           

            var uomsid = string.IsNullOrEmpty(uomid) ? Guid.Empty : new Guid(uomid.ToString());

            if (actionFlag == 2)
                itemNumUpdate = _amicsDbContext.LstMessage.FromSqlRaw($"exec sp_maintain_partmaster25 @actionFlag='{actionFlag}', @id='{itemsid}',@itemNumber='{itemNumber}', @rev='{rev}', @description='{description}', @salesDescription='{salesDescription}', @PurchaseDescription='{PurchaseDescription}', @invtypeidv='{invtypeidv}', @itemtypeidv='{itemtypeidv}', @itemclassidv='{itemclassidv}', @itemcodeidv='{itemcodeidv}', @uomid='{uomsid}', @conversion='{conversion}',@cost='{cost}', @markup='{markup}', @price='{price}', @price2= '{price2}', @price3= '{price3}', @weight='{weight}', @reorder ='{reorder}', @buyitem ='{buyitem}', @obsolete = '{obsolete}', @cyclecount = '{cyclecount}', @notes = '{notes}', @minimum = '{minimum}', @maximum = '{maximum}',@leadtime= '{leadtime}', @warehouseidv= '{warehouseidv}',@locationsidv='{locationsidv}',@glsales='{glsales}',@glinv='{glinv}',@glcogs='{glcogs}',@dwgno = '{dwgno}', @user1 = '{user1}', @user2 = '{user2}', @user3 = '{user3}', @userbit = '{userbit}', @userbit2 = '{userbit2}', @userbit3 = '{userbit3}'").AsEnumerable().FirstOrDefault();
            else
                itemNumUpdate = _amicsDbContext.LstMessage.FromSqlRaw($"exec sp_maintain_partmaster25 @actionFlag='{actionFlag}', @itemNumber='{itemNumber}', @rev='{rev}', @description='{description}', @salesDescription='{salesDescription}', @PurchaseDescription='{PurchaseDescription}', @invtypeidv='{invtypeidv}', @itemtypeidv='{itemtypeidv}', @itemclassidv='{itemclassidv}', @itemcodeidv='{itemcodeidv}', @uomid='{uomsid}', @conversion='{conversion}',@cost='{cost}', @markup='{markup}', @price='{price}', @price2= '{price2}', @price3= '{price3}', @weight='{weight}', @reorder ='{reorder}', @buyitem ='{buyitem}', @obsolete = '{obsolete}', @cyclecount = '{cyclecount}', @notes = '{notes}', @minimum = '{minimum}', @maximum = '{maximum}',@leadtime= '{leadtime}', @warehouseidv= '{warehouseidv}',@locationsidv='{locationsidv}',@glsales='{glsales}',@glinv='{glinv}',@glcogs='{glcogs}',@dwgno = '{dwgno}', @user1 = '{user1}', @user2 = '{user2}', @user3 = '{user3}', @userbit = '{userbit}', @userbit2 = '{userbit2}', @userbit3 = '{userbit3}'").AsEnumerable().FirstOrDefault();

            return itemNumUpdate;
        }

    }
}
