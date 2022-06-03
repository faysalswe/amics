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
        List<AimcsSpLookUp> CommonLookup(FieldNameSearch fieldName, string search_col1, string search_col2);
        List<LstWarehouse> WarehouseLookup(string searchWarehouse, string warehouseId);
        List<LstLocaton> LocationLookup(string searchLocation, string warehouseId, string locationId);
        List<LstItemSearch> ItemNumberSearch(string itemnumber, string description, string itemtype, string itemcode, string itemclass);
        List<LstItemType> ItemTypeLookup(string itemtypeId, string searchItemtype);
        List<LstItemClass> ItemClassLookup(string itemclassId, string searchItemclass);
        List<LstItemCode> ItemCodeLookup(string itemcodeId, string searchItemcodes);
        List<LstUom> UomLookup(string uomId, string uomRef);
        //  List<ListItems> LoadSelectedItemNum(string itemnumber, string rev);


        /// <summary>
        /// Interface for get item's information, Must pass ItemsId or ItemNumber as parameter Rev is optional
        /// </summary>
        /// <param name="ItemsId">The ItemsId of the data.</param>
        /// <param name="ItemNumber">The ItemNumber of the data.</param>
        /// <param name="Rev">The Rev of the data. Default is '-'</param>

        List<LstItemInfo> ItemInfo(string ItemsId, string ItemNumber, string Rev);


        /// <summary>
        /// Interface for get list of reason codes for INCREASE or DECREASE
        /// </summary>
        /// <param name="ReasonCode">ReasonCode for the transaction.</param>
        /// <param name="CodeFor">CodeFor is type of transaction must 'INCREASE' or 'DECREASE' as a parameter.</param>
        List<LstReasonCodes> ReasonCodes(string ReasonCode, string CodeFor);


        /// <summary>
        /// Interface  for get Company Options. Use this options for show or hide the fields or set default request globally 
        /// </summary>
        /// <param name="OptionId">Integer value as a parameter.</param>
        /// <param name="ScreenName">Get options by screen name if it is global parameter value should be 'GENERAL' </param>
        List<LstCompanyOptions> CompanyOptions(decimal OptionId, string ScreenName);

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
            var whId = string.IsNullOrEmpty(warehouseId) ? Guid.Empty : new Guid(warehouseId.ToString());
            var locId = string.IsNullOrEmpty(locationId)? Guid.Empty:  new Guid(locationId.ToString());
            var location = string.IsNullOrEmpty(searchLocation) ? string.Empty : searchLocation;
             
            var locresult = _amicsDbContext.LstLocations
                .FromSqlRaw($"exec amics_sp_location_lookup @whid='{whId}',@locid='{locId}',@location='{location}'")
                .ToList();

            return locresult;
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
        /// API Service for get item's information, Must pass ItemsId or ItemNumber as parameter Rev is optional
        /// </summary>
        /// <param name="ItemsId">The ItemsId of the data.</param>
        /// <param name="ItemNumber">The ItemNumber of the data.</param>
        /// <param name="Rev">The Rev of the data. Default is '-'</param>

        public List<LstItemInfo> ItemInfo(string ItemsId, string ItemNumber, string Rev)
        {

            var itemsGuId = string.IsNullOrEmpty(ItemsId) ? Guid.Empty : new Guid(ItemsId.ToString());
            var itemNo = string.IsNullOrEmpty(ItemNumber) ? string.Empty : ItemNumber;
            var rev = string.IsNullOrEmpty(Rev) ? string.Empty : Rev;
          
            var searchResult = _amicsDbContext.LstItemsInfo
                .FromSqlRaw($"exec sp_webapi_get_iteminfo @item='{itemNo}',@rev='{rev}',@itemsid='{itemsGuId}'")
                .ToList<LstItemInfo>();

            return searchResult;
        }

        /// <summary>
        /// API Service for get list of reason codes for INCREASE or DECREASE
        /// </summary>
        /// <param name="ReasonCode">ReasonCode for the transaction.</param>
        /// <param name="CodeFor">CodeFor is type of transaction must 'INCREASE' or 'DECREASE' as a parameter.</param>

        public List<LstReasonCodes> ReasonCodes(string ReasonCode, string CodeFor)
        {
             
            var resCode = string.IsNullOrEmpty(ReasonCode) ? string.Empty : ReasonCode;
            var resCodeFor = string.IsNullOrEmpty(CodeFor) ? string.Empty : CodeFor;

            var searchResult = _amicsDbContext.ListReasonCodes
                .FromSqlRaw($"exec sp_webapi_get_reasoncode @reasoncode='{resCode}',@codefor='{resCodeFor}'")
                .ToList<LstReasonCodes>();

            return searchResult;
        }
        /// <summary>
        /// API Service for get Company Options. Use this options for show or hide the fields or set default request globally 
        /// </summary>
        /// <param name="OptionId">Integer value as a parameter.</param>
        /// <param name="ScreenName">Get options by screen name if it is global parameter value should be 'GENERAL' </param>

        public List<LstCompanyOptions> CompanyOptions(decimal OptionId, string ScreenName)
         {            
          
            var screenName = string.IsNullOrEmpty(ScreenName) ? string.Empty : ScreenName;

        var searchResult = _amicsDbContext.ListCompanyOptions
            .FromSqlRaw($"exec sp_webapi_get_list_company_options @optionid={OptionId},@screenname='{screenName}'")
            .ToList<LstCompanyOptions>();

            return searchResult;
        }

}
}
