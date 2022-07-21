using Aims.Core.Models;
using Aims.PartMaster.Models;
using Aims.PartMaster.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Amics.Api.Controllers
{
    [Route("api/[controller]")] 
    [ApiController] 
    [Authorize]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _partMasterService;
        private readonly ILogger<SearchController> _logger;
        public SearchController(ISearchService partMasterService, ILogger<SearchController> logger)
        {
            _partMasterService = partMasterService;
            _logger = logger;
        }
        
        /// <summary>
        /// API Route Controller to get Warehouse details, pass warehouse Id and warehouse as parameter.
        /// </summary>
        /// <param name="searchWarehouse">Locationr</param>  
        /// <param name="warehouseId">Warehouse Id</param>      
        [HttpGet, Route("Warehouse")]
        public IList<LstWarehouse> GetWarehouseLookUp([FromQuery] string searchWarehouse, [FromQuery] string warehouseId)
        {
            var resultWh = _partMasterService.WarehouseLookup(searchWarehouse, warehouseId);

            return resultWh;
        }

        /// <summary>
        /// API Route Controller to  Location details, pass warehouse Id, location Id and Location as parameter.
        /// </summary>        
        /// <param name="warehouseId">Warehouse Id</param> 
        /// <param name="searchLocation">Location</param> 
        /// <param name="locationId">location Id</param> 
        [HttpGet, Route("Location")]
        public IList<LstLocaton> GetLocationLookUp([FromQuery] string warehouseId, [FromQuery] string searchLocation="",[FromQuery] string locationId = null)
        {
          
            var resultLoc = _partMasterService.LocationLookup(searchLocation, warehouseId, locationId);

            return resultLoc;
        }

        /// <summary>
        /// API Route Controller to get Itemtype details, pass itemtypeId and itemtype as parameter.
        /// </summary>        
        /// <param name="itemtypeId">itemtype Id</param> 
        /// <param name="itemtype">itemtype</param>         
        [HttpGet, Route("ItemType")]
        public IList<LstItemType> GetItemTypeSearch([FromQuery] string itemtypeId, [FromQuery] string itemtype)
        {

            var resultItemtype = _partMasterService.ItemTypeLookup(itemtypeId, itemtype);

            return resultItemtype;
        }

        /// <summary>
        /// API Route Controller to get ItemClass details, pass itemclassId and itemclass as parameter.
        /// </summary>        
        /// <param name="itemclassId">itemclass Id</param> 
        /// <param name="itemclass">itemclass</param>     
        [HttpGet, Route("ItemClass")]
        public IList<LstItemClass> GetItemClassSearch([FromQuery] string itemclassId, [FromQuery] string itemclass)
        {

            var resultItemclass = _partMasterService.ItemClassLookup(itemclassId, itemclass);

            return resultItemclass;
        }

        /// <summary>
        /// API Route Controller to get ItemCode details, pass itemcodeId and itemcode as parameter.
        /// </summary>        
        /// <param name="itemcodeId">itemcode Id</param> 
        /// <param name="itemcode">itemcodeId</param>     
        [HttpGet, Route("ItemCode")]
        public IList<LstItemCode> GetItemCodeSearch([FromQuery] string itemcodeId, [FromQuery] string itemcode)
        {

            var resultItemCode = _partMasterService.ItemCodeLookup(itemcodeId, itemcode);

            return resultItemCode;
        }

        /// <summary>
        /// API Route Controller to get UOM details, pass uomId and uomRef as parameter.
        /// </summary>        
        /// <param name="uomId">uom Id</param> 
        /// <param name="uomRef">uomRef</param>     
        [HttpGet, Route("Uom")]
        public IList<LstUom> GetUomSearch([FromQuery] string uomId, [FromQuery] string uomRef)
        {

            var resultUom = _partMasterService.UomLookup(uomId, uomRef);

            return resultUom;
        }

        /// <summary>
        /// API Route Controller to get Item details for partmaster search, pass itemnumber,description,itemtype,itemcode and itemclass as parameter.
        /// </summary>        
        /// <param name="itemnumber">Item Number</param>  
        /// <param name="description">Descriptionr</param>   
        /// <param name="itemtype">Itemtype</param>   
        /// <param name="itemcode">Itemcoder</param>   
        /// <param name="itemclass">Itemclass</param>    
        [HttpGet, Route("ItemNumber")]
        public IList<LstItemSearch> GetItemNumberSearch([FromQuery] string itemnumber, [FromQuery] string description, [FromQuery] string itemtype, [FromQuery] string itemcode, [FromQuery] string itemclass)
        {
            var itemSearchResult = _partMasterService.ItemNumberSearch(itemnumber, description, itemtype, itemcode, itemclass);

            return itemSearchResult;
        }

        /// <summary>
        /// API Route Controller for get item's information, Must pass ItemsId or ItemNumber as parameter Rev is optional
        /// </summary>
        /// <param name="ItemsId">The ItemsId of the data.</param>
        /// <param name="ItemNumber">The ItemNumber of the data.</param>
        /// <param name="Rev">The Rev of the data. Default is '-'</param>

        [HttpGet, Route("GetItemInfo")]
        public IList<LstItemInfo> GetItemInfo([FromQuery] string ItemsId, [FromQuery] string ItemNumber, [FromQuery] string Rev)
        {
            var itemInfo = _partMasterService.ItemInfo(ItemsId, ItemNumber,Rev);
            return itemInfo;
        }


        /// <summary>
        /// API Route Controller for get list of reason codes for INCREASE or DECREASE
        /// </summary>
        /// <param name="ReasonCode">ReasonCode for the transaction.</param>
        /// <param name="CodeFor">CodeFor is type of transaction must 'INCREASE' or 'DECREASE' as a parameter.</param>


        [HttpGet, Route("GetReasonCode")]  
        public IList<LstReasonCodes> GetReasonCode([FromQuery] string ReasonCode, [FromQuery] string CodeFor)
        {
            var resCode = _partMasterService.ReasonCodes(ReasonCode, CodeFor);
            return resCode;
        }
    }
}
