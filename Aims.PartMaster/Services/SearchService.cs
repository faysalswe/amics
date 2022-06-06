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
using Aims.PartMaster.Models;

namespace Aims.PartMaster.Services
{
    public interface ISearchService
    {        
        List<LstWarehouse> WarehouseLookup(string searchWarehouse, string warehouseId);
        List<LstLocaton> LocationLookup(string searchLocation, string warehouseId, string locationId);
        List<LstItemSearch> ItemNumberSearch(string itemnumber, string description, string itemtype, string itemcode, string itemclass);
        List<LstItemType> ItemTypeLookup(string itemtypeId, string searchItemtype);
        List<LstItemClass> ItemClassLookup(string itemclassId, string searchItemclass);
        List<LstItemCode> ItemCodeLookup(string itemcodeId, string searchItemcodes);
        List<LstUom> UomLookup(string uomId, string uomRef);
        //  List<ListItems> LoadSelectedItemNum(string itemnumber, string rev);      
        List<LstFieldProperties> LoadFieldProperties(string labelNum);
    }

    public class SearchService:ISearchService
    {
        private readonly AmicsDbContext _amicsDbContext;
        public SearchService(AmicsDbContext aimsDbContext)
        {
            _amicsDbContext = aimsDbContext;
        }
        /// <summary>
        /// API Service to get Id, Warehouse details, returns all the records if parameters are null.
        /// </summary>
        /// <param name="searchWarehouse">Locationr</param>  
        /// <param name="warehouseId">Warehouse Id</param>         
        public List<LstWarehouse> WarehouseLookup(string searchWarehouse, string warehouseId)
        {            
            var whId = string.IsNullOrEmpty(warehouseId) ? Guid.Empty : new Guid(warehouseId.ToString());
            var name = string.IsNullOrEmpty(searchWarehouse) ? string.Empty : searchWarehouse;
             
            var whresult = _amicsDbContext.LstWarehouses
                .FromSqlRaw($"exec amics_sp_warehouse_lookup @whid ='{whId}',@warehouse = '{name}'")
                .ToList();

            return whresult;
        }


        /// <summary>
        /// API Service to get Location(warehouseId, id,location, invalid,sequenceNo,route) details, returns all the records if parameters are null and warehouse Id is must to pass as parameter to get specific location.    
        /// </summary>
        /// <param name="searchLocation">Locationr</param>  
        /// <param name="warehouseId">Warehouse Id</param> 
        /// <param name="locationId">Location Id</param> 
        public List<LstLocaton> LocationLookup(string searchLocation,string warehouseId, string locationId)
        { 
            var whId = string.IsNullOrEmpty(warehouseId) ? Guid.Empty : new Guid(warehouseId.ToString());
            var locId = string.IsNullOrEmpty(locationId)? Guid.Empty:  new Guid(locationId.ToString());
            var location = string.IsNullOrEmpty(searchLocation) ? string.Empty : searchLocation;
             
            var locresult = _amicsDbContext.LstLocations
                .FromSqlRaw($"exec amics_sp_location_lookup @whid='{whId}',@locid='{locId}',@location='{location}'")
                .ToList();

            return locresult;
        }


        /// <summary>
        /// API Service to get Item Type details, returns all the records if parameters are null        
        /// </summary>
        /// <param name="itemtypeId">Item Type Idr</param>  
        /// <param name="searchItemtype">Item Type</param> 
        public List<LstItemType> ItemTypeLookup(string itemtypeId, string searchItemtype)
        {           
            var itmtypeId = string.IsNullOrEmpty(itemtypeId) ? Guid.Empty : new Guid(itemtypeId.ToString());
            string itemtype = string.IsNullOrEmpty(searchItemtype) ? string.Empty : searchItemtype;
            
            var result = _amicsDbContext.LstItemTypes
                .FromSqlRaw($"exec amics_sp_itemtype_lookup @id ='{itmtypeId}',@itemtype = '{itemtype}'")
                .ToList();

            return result;
        }

        /// <summary>
        /// API Service to get Item Class details, returns all the records if parameters are null        
        /// </summary>
        /// <param name="itemclassId">Item Class Idr</param>  
        /// <param name="searchItemclass">Item Class</param> 
        public List<LstItemClass> ItemClassLookup(string itemclassId, string searchItemclass)
        {          
            var itmclassId = string.IsNullOrEmpty(itemclassId) ? Guid.Empty : new Guid(itemclassId.ToString());
            string itemClass = string.IsNullOrEmpty(searchItemclass) ? string.Empty : searchItemclass;

            var itemclsresult = _amicsDbContext.LstItemClasses
                .FromSqlRaw($"exec amics_sp_itemclass_lookup @id ='{itmclassId}',@itemclass = '{itemClass}'")
                .ToList();

            return itemclsresult;
        }

        /// <summary>
        /// API Service to get Item Code details, returns all the records if parameters are null        
        /// </summary>
        /// <param name="itemcodeId">Item Code Idr</param>  
        /// <param name="searchItemcodes">Item Code</param>       
        public List<LstItemCode> ItemCodeLookup(string itemcodeId,string searchItemcodes)
        {          
            var itmcodeId = string.IsNullOrEmpty(itemcodeId) ? Guid.Empty : new Guid(itemcodeId.ToString());
            string itemCode = string.IsNullOrEmpty(searchItemcodes) ? string.Empty : searchItemcodes;

            var itmcodresult = _amicsDbContext.LstItemCodes
                .FromSqlRaw($"exec amics_sp_itemcode_lookup @id ='{itmcodeId}',@itemcode = '{itemCode}'")
                .ToList();

            return itmcodresult;
        }

        /// <summary>
        /// API Service to get UOM details, returns all the records if parameters are null        
        /// </summary>
        /// <param name="uomId">Uom Idr</param>  
        /// <param name="uomRef">UomRef</param>           
        public List<LstUom> UomLookup(string uomId, string uomRef)
        {
            var uomRefId = string.IsNullOrEmpty(uomId) ? Guid.Empty : new Guid(uomId.ToString());
            string uomRefVal = string.IsNullOrEmpty(uomRef) ? string.Empty : uomRef;

            var result = _amicsDbContext.LstUoms
                .FromSqlRaw($"exec amics_sp_uom_lookup @id ='{uomRefId}',@uomref = '{uomRefVal}'")
                .ToList();

            return result;
        }

        /// <summary>
        /// API Service to get search result of Item No, Rev, Description details using below parameters, returns top 100 records from list_items table(order by createddate desc)
        /// if parameters are null
        /// </summary>
        /// <param name="itemnumber">Item Number</param>  
        /// <param name="description">Descriptionr</param>   
        /// <param name="itemtype">Itemtype</param>   
        /// <param name="itemcode">Itemcoder</param>   
        /// <param name="itemclass">Itemclass</param>   
        public List<LstItemSearch> ItemNumberSearch(string itemnumber, string description, string itemtype, string itemcode, string itemclass)
        {
            var itmnumber = string.IsNullOrEmpty(itemnumber) ? string.Empty : itemnumber;
            var desc = string.IsNullOrEmpty(description) ? string.Empty : description;
            var itmtype = string.IsNullOrEmpty(itemtype) ? string.Empty : itemtype;
            var itmcode = string.IsNullOrEmpty(itemcode) ? string.Empty : itemcode;
            var itmclass = string.IsNullOrEmpty(itemclass) ? string.Empty : itemclass;

            var searchresult = _amicsDbContext.LstItemSearchs
                .FromSqlRaw($"exec sp_webservice_search_items5 @item='{itmnumber}',@description='{desc}',@itemtype='{itmtype}',@itemclass='{itmclass}',@itemcode='{itmcode}'")
                .ToList<LstItemSearch>();

            return searchresult;
        }

        /// <summary>
        /// API Service to get My Label info from db, returns all the data if parameter is null. Label no can pass single or multiple number with comma separated.
        /// </summary>
        /// <param name="labelNum">Label Number</param>        
        public List<LstFieldProperties> LoadFieldProperties(string labelNum)
        {
            var optResult = _amicsDbContext.LstFieldProperties
                            .FromSqlRaw($"amics_sp_list_fieldproperties @labelnumber='{labelNum}'").ToList<LstFieldProperties>();

            return optResult;
        }
    }
}
