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
       
    }
}
