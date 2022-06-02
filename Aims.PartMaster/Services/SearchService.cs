using Aims.Core.Models;
using Aims.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Linq;
using Amics.Core.utils;

namespace Aims.PartMaster.Services
{
    public interface ISearchService
    {
        List<AimcsSpLookUp> CommonLookup(FieldNameSearch fieldName, string search_col1, string search_col2);
        List<LstWarehouse> WarehouseLookup(string searchWarehouse, string warehouseId);
        List<LstLocaton> LocationLookup(string searchLocation, string warehouseId, string locationId);
        List<LstItemSearch> ItemNumberSearch(string itemnumber, string description, string itemtype, string itemcode, string itemclass);
        List<LstItemType> ItemTypeLookup(string itemtypeId, string searchItemtype);
        List<LstItemClass> ItemClassLookup(string itemclassId, string searchItemclass);
        List<LstItemCode> ItemCodeLookup(string itemcodeId, string searchItemcodes);
        List<LstUom> UomLookup(string uomId, string uomRef);
    }

    public class SearchService:ISearchService
    {
        private readonly AmicsDbContext _amicsDbContext;
        public SearchService(AmicsDbContext aimsDbContext)
        {
            _amicsDbContext = aimsDbContext;
        }

        public List<AimcsSpLookUp> CommonLookup(FieldNameSearch fieldName, string search_col1, string search_col2 ) 
        { 
            var result = _amicsDbContext.AmicsSpLookups
                .FromSqlRaw($"exec amicsmvc_sp_lookup @fieldname='{fieldName.GetEnumDescription()}',@search_col1='{search_col1}',@search_col2='{search_col2 }'")
                .ToList< AimcsSpLookUp>();

            return result;
        }

        public List<LstWarehouse> WarehouseLookup(string searchWarehouse, string warehouseId)
        {
            var whId = string.IsNullOrEmpty(warehouseId) ? Guid.Empty : new Guid(warehouseId.ToString());
            var name = string.IsNullOrEmpty(searchWarehouse) ? string.Empty : searchWarehouse;
             
            var whresult = _amicsDbContext.LstWarehouses
                .FromSqlRaw($"exec amics_sp_warehouse_lookup @whid ='{whId}',@warehouse = '{name}'")
                .ToList();

            return whresult;
        }

        public List<LstLocaton> LocationLookup(string searchLocation,string warehouseId, string locationId)
        {
            if (string.IsNullOrEmpty(warehouseId))
            {
                throw new BusinessRuleException("WarehouseId cannot be null or empty");
            } 
            var whId = new Guid(warehouseId.ToString());
            var locId = string.IsNullOrEmpty(locationId)? Guid.Empty:  new Guid(locationId.ToString());
            var location = string.IsNullOrEmpty(searchLocation) ? string.Empty : searchLocation;
             
            var locresult = _amicsDbContext.LstLocations
                .FromSqlRaw($"exec amics_sp_location_lookup @whid='{whId}',@locid='{locId}',@location='{location}'")
                .ToList();

            return locresult;
        }


        public List<LstItemSearch> ItemNumberSearch(string itemnumber, string description, string itemtype, string itemcode, string itemclass)
        {          
            var searchresult = _amicsDbContext.LstItemSearchs
                .FromSqlRaw($"exec sp_webservice_search_items5 @item='{itemnumber}',@description='{description}',@itemtype='{itemtype }',@itemclass='{itemclass }',@itemcode='{itemcode }'")
                .ToList<LstItemSearch>();

            return searchresult;
        }

        public List<LstItemType> ItemTypeLookup(string itemtypeId, string searchItemtype)
        {           
            var itmtypeId = string.IsNullOrEmpty(itemtypeId) ? Guid.Empty : new Guid(itemtypeId.ToString());
            string itemtype = string.IsNullOrEmpty(searchItemtype) ? string.Empty : searchItemtype;
            
            var result = _amicsDbContext.LstItemTypes
                .FromSqlRaw($"exec amics_sp_itemtype_lookup @id ='{itmtypeId}',@itemtype = '{itemtype}'")
                .ToList();

            return result;
        }

        public List<LstItemClass> ItemClassLookup(string itemclassId, string searchItemclass)
        {          
            var itmclassId = string.IsNullOrEmpty(itemclassId) ? Guid.Empty : new Guid(itemclassId.ToString());
            string itemClass = string.IsNullOrEmpty(searchItemclass) ? string.Empty : searchItemclass;

            var itemclsresult = _amicsDbContext.LstItemClasses
                .FromSqlRaw($"exec amics_sp_itemclass_lookup @id ='{itmclassId}',@itemclass = '{itemClass}'")
                .ToList();

            return itemclsresult;
        }

        public List<LstItemCode> ItemCodeLookup(string itemcodeId,string searchItemcodes)
        {          
            var itmcodeId = string.IsNullOrEmpty(itemcodeId) ? Guid.Empty : new Guid(itemcodeId.ToString());
            string itemCode = string.IsNullOrEmpty(searchItemcodes) ? string.Empty : searchItemcodes;

            var itmcodresult = _amicsDbContext.LstItemCodes
                .FromSqlRaw($"exec amics_sp_itemcode_lookup @id ='{itmcodeId}',@itemcode = '{itemCode}'")
                .ToList();

            return itmcodresult;
        }

        public List<LstUom> UomLookup(string uomId, string uomRef)
        {
            var uomRefId = string.IsNullOrEmpty(uomId) ? Guid.Empty : new Guid(uomId.ToString());
            string uomRefVal = string.IsNullOrEmpty(uomRef) ? string.Empty : uomRef;

            var result = _amicsDbContext.LstUoms
                .FromSqlRaw($"exec amics_sp_uom_lookup @id ='{uomRefId}',@uomref = '{uomRefVal}'")
                .ToList();

            return result;
        }

    }

}
