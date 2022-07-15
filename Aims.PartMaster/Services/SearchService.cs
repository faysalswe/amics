using Aims.Core.Models; 
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic; 
using System.Linq; 
using Aims.PartMaster.Models;
using System.Data;
using Microsoft.Data.SqlClient;

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
        /// <param name="searchWarehouse">Warehouse</param>  
        /// <param name="warehouseId">Warehouse Id</param>         
        public List<LstWarehouse> WarehouseLookup(string searchWarehouse, string warehouseId)
        {            
            var whId = string.IsNullOrEmpty(warehouseId) ? Guid.Empty : new Guid(warehouseId.ToString());
            var name = string.IsNullOrEmpty(searchWarehouse) ? string.Empty : searchWarehouse;
             
            var whresult = _amicsDbContext.LstWarehouses
                .FromSqlRaw($"exec amics_sp_api_warehouse_lookup @whid ='{whId}',@warehouse = '{name}'")
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
                .FromSqlRaw($"exec amics_sp_api_location_lookup @whid='{whId}',@locid='{locId}',@location='{location}'")
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
                .FromSqlRaw($"exec amics_sp_api_itemtype_lookup @id ='{itmtypeId}',@itemtype = '{itemtype}'")
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
                .FromSqlRaw($"exec amics_sp_api_itemclass_lookup @id ='{itmclassId}',@itemclass = '{itemClass}'")
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
                .FromSqlRaw($"exec amics_sp_api_itemcode_lookup @id ='{itmcodeId}',@itemcode = '{itemCode}'")
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
                .FromSqlRaw($"exec amics_sp_api_uom_lookup @id ='{uomRefId}',@uomref = '{uomRefVal}'")
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
            var itmnumber = (string.IsNullOrEmpty(itemnumber) || itemtype == "null") ? string.Empty : itemnumber;
            var desc = string.IsNullOrEmpty(description) ? string.Empty : description;
            var itmtype = (string.IsNullOrEmpty(itemtype)|| itemtype =="null") ? string.Empty : itemtype;
            var itmcode = (string.IsNullOrEmpty(itemcode) || itemtype == "null") ? string.Empty : itemcode;
            var itmclass = (string.IsNullOrEmpty(itemclass) || itemtype == "null")? string.Empty : itemclass;
            string strCurr = string.Empty;
         
            Utility util = new Utility();
            var listItems = new List<LstItemSearch>();

            //CompanyOptions CompOptions = (CompanyOptions)HttpContext.Current.Session["CompanyOptions"];

            //if (CompOptions != null)
            //{
            //    strCurr = util.ReturnZeros(CompOptions.DecimalsinCurrency);

            //}
            //else
            //{
            strCurr = util.ReturnZeros(2);

            //}
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand .CommandText = "amics_sp_api_search_items";
                    conn.Open(); 
                   sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@item", itmnumber));
                    sqlCommand.Parameters.Add(new SqlParameter("@description", desc));
                    sqlCommand.Parameters.Add(new SqlParameter("@itemtype", itmtype));
                    sqlCommand.Parameters.Add(new SqlParameter("@itemclass", itmclass));
                    sqlCommand.Parameters.Add(new SqlParameter("@itemcode", itmcode));
                 

                    var dataReader = sqlCommand.ExecuteReader();

                        while (dataReader.Read())
                        {
                             LstItemSearch ItemListings = new LstItemSearch();
                            ItemListings.Id = dataReader["id"].ToString().Trim();
                            ItemListings.ItemNumber = dataReader["itemnumber"].ToString().Trim();
                            ItemListings.Rev = dataReader["rev"].ToString().Trim();
                            ItemListings.Description = dataReader["description"].ToString().Trim();
                            ItemListings.ItemType = dataReader["itemtype"].ToString().Trim();
                            ItemListings.ItemClass = dataReader["itemclass"].ToString().Trim();
                            ItemListings.ItemCode = dataReader["itemcode"].ToString().Trim();
                            ItemListings.Uomref = dataReader["uomref"].ToString().Trim();
                            ItemListings.DwgNo = dataReader["dwgno"].ToString().Trim();

                            if ((dataReader["cost"] != DBNull.Value) || (dataReader["cost"].ToString() != ""))
                            { 
                                ItemListings.Cost = Convert.ToDecimal(String.Format("{0:0." + strCurr + "}", dataReader["cost"]));
                            }

                            if ((dataReader["conversion"] != DBNull.Value) || (dataReader["conversion"].ToString() != ""))
                            {
                                ItemListings.Conversion = Convert.ToDecimal(dataReader["conversion"]);
                            }
                        listItems.Add(ItemListings);
                        }
                    }
                catch (Exception ex)
                {
                    //Log.ErrorLog(ex.Message, "Search : SearchListItems");
                }
                finally
                {
                    conn.Close();
                } }

            return listItems;
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
                .FromSqlRaw($"exec amics_sp_api_get_iteminfo @item='{itemNo}',@rev='{rev}',@itemsid='{itemsGuId}'")
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
                .FromSqlRaw($"exec amics_sp_api_get_reasoncode @reasoncode='{resCode}',@codefor='{resCodeFor}'")
                .ToList<LstReasonCodes>();

            return searchResult;
        }
       
    }
}
