using Aims.Core.Models;
using Aims.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Linq;

namespace Aims.PartMaster.Services
{
    public interface ISearchService
    {
        List<AimcsSpLookUp> CommonLookup(FieldNameSearch fieldName, string search_col1, string search_col2);
        List<LstWarehouse> WarehouseLookup(string searchWarehouse, string warehouseId);
        List<LstLocaton> LocationLookup(string searchLocation, string warehouseId, string locationId);
        List<LstItemSearch> ItemNumberSearch(string itemnumber, string description, string itemtype, string itemcode, string itemclass);
        List<LstItemType> ItemTypeLookup(string searchItemtype, string itemtypeId);
        List<LstItemClass> ItemClassLookup(string searchItemclass, string itemclassId);
        List<LstItemCode> ItemCodeLookup(string searchItemcodes, string itemcodeId);
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
            var whId = new Guid(warehouseId.ToString());
            //var result = _amicsDbContext.LstWarehouses
            //    .FromSqlInterpolated($"select id,warehouse from list_warehouses where trim(warehouse) like  {"%" + searchWarehouse + "%"} order by warehouse")
            //    .ToList();
            var whresult = _amicsDbContext.LstWarehouses
                .FromSqlRaw($"exec amics_sp_warehouse_lookup @whid ='{whId}',@warehouse = '{searchWarehouse}'")
                .ToList();

            return whresult;
        }

        public List<LstLocaton> LocationLookup(string searchLocation,string warehouseId, string locationId)
        {
            var whId = new Guid(warehouseId.ToString());
            var locId = new Guid(locationId.ToString());
            //var result = _amicsDbContext.LstLocations
            //    .FromSqlInterpolated($"select id,location,isnull(list_locations.invalid,0) as invalid,isnull(list_locations.sequenceno,'') as sequenceno,isnull(list_locations.route,'') as route from list_locations where warehousesid={whouseId} and location like  {"%" + searchLocation + "%"} and isnull(flag_delete,0)=0 order by location")
            //    .ToList();
            var locresult = _amicsDbContext.LstLocations
                .FromSqlRaw($"exec amics_sp_location_lookup @whid='{whId}',@locationId='{locId}',@location='{searchLocation}'")
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

        public List<LstItemType> ItemTypeLookup(string searchItemtype, string itemtypeId)
        {
            var itmtypeId = new Guid(itemtypeId.ToString());
          
            var result = _amicsDbContext.LstItemTypes
                .FromSqlRaw($"exec amics_sp_itemtype_lookup @id ='{itmtypeId}',@itemtype = '{searchItemtype}'")
                .ToList();

            return result;
        }

        public List<LstItemClass> ItemClassLookup(string searchItemclass, string itemclassId)
        {
            var itmclassId = new Guid(itemclassId.ToString());

            var itemclsresult = _amicsDbContext.LstItemClasses
                .FromSqlRaw($"exec amics_sp_itemclass_lookup @id ='{itmclassId}',@itemclass = '{searchItemclass}'")
                .ToList();

            return itemclsresult;
        }

        public List<LstItemCode> ItemCodeLookup(string searchItemcodes, string itemcodeId)
        {
            var itmcodeId = new Guid(itemcodeId.ToString());

            var itmcodresult = _amicsDbContext.LstItemCodes
                .FromSqlRaw($"exec amics_sp_itemcode_lookup @id ='{itmcodeId}',@itemcode = '{searchItemcodes}'")
                .ToList();

            return itmcodresult;
        }
    }

}
