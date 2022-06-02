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

        [HttpGet, Route("CommonLookup")]
        public IList<AimcsSpLookUp> GetLookUpData([FromQuery]FieldNameSearch fieldName, [FromQuery] string search_col1, [FromQuery] string search_col2)
        {
            var result = _partMasterService.CommonLookup(fieldName, search_col1, search_col2);

            return result;

        }

        [HttpGet, Route("Warehouse")]
        public IList<LstWarehouse> GetWarehouseLookUp([FromQuery] string searchWarehouse, [FromQuery] string warehouseId)
        {
            var resultWh = _partMasterService.WarehouseLookup(searchWarehouse, warehouseId);

            return resultWh;
        }

        [HttpGet, Route("Location")]
        public IList<LstLocaton> GetLocationLookUp([FromQuery] string warehouseId, [FromQuery] string searchLocation="",[FromQuery] string locationId = null)
        {
          
            var resultLoc = _partMasterService.LocationLookup(searchLocation, warehouseId, locationId);

            return resultLoc;
        }

        [HttpGet, Route("ItemType")]
        public IList<LstItemType> GetItemTypeSearch([FromQuery] string itemtypeId, [FromQuery] string itemtype)
        {

            var resultItemtype = _partMasterService.ItemTypeLookup(itemtypeId, itemtype);

            return resultItemtype;
        }

        [HttpGet, Route("ItemClass")]
        public IList<LstItemClass> GetItemClassSearch([FromQuery] string itemclassId, [FromQuery] string itemclass)
        {

            var resultItemclass = _partMasterService.ItemClassLookup(itemclassId, itemclass);

            return resultItemclass;
        }

        [HttpGet, Route("ItemCode")]
        public IList<LstItemCode> GetItemCodeSearch([FromQuery] string itemcodeId, [FromQuery] string itemcode)
        {

            var resultItemCode = _partMasterService.ItemCodeLookup(itemcodeId, itemcode);

            return resultItemCode;
        }

        [HttpGet, Route("Uom")]
        public IList<LstUom> GetUomSearch([FromQuery] string uomId, [FromQuery] string uomRef)
        {

            var resultUom = _partMasterService.UomLookup(uomId, uomRef);

            return resultUom;
        }

        [HttpGet, Route("ItemNumber")]
        public IList<LstItemSearch> GetItemNumberSearch([FromQuery] string itemnumber, [FromQuery] string description, [FromQuery] string itemtype, [FromQuery] string itemcode, [FromQuery] string itemclass)
        {

            var itemSearchResult = _partMasterService.ItemNumberSearch(itemnumber, description, itemtype, itemcode, itemclass);

            return itemSearchResult;
        }

        /// <summary>
        /// API for get item's information, Must pass ItemsId or ItemNumber as parameter Rev is optional
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
        /// API for get list of reason codes for INCREASE or DECREASE
        /// </summary>
        /// <param name="ReasonCode">ReasonCode for the transaction.</param>
        /// <param name="CodeFor">CodeFor is type of transaction must 'INCREASE' or 'DECREASE' as a parameter.</param>
      

        [HttpGet, Route("GetReasonCode")]  
        public IList<LstReasonCodes> GetReasonCode([FromQuery] string ReasonCode, [FromQuery] string CodeFor)
        {
            var resCode = _partMasterService.ReasonCodes(ReasonCode, CodeFor);
            return resCode;
        }


        /// <summary>
        /// API for get Company Options. Use this options for show or hide the fields or set default request globally 
        /// </summary>
        /// <param name="OptionId">Integer value as a parameter.</param>
        /// <param name="ScreenName">Get options by screen name if it is global parameter value should be 'GENERAL' </param>

        [HttpGet, Route("GetCompanyOptions")]
        public IList<LstCompanyOptions> GetCompanyOptions([FromQuery] decimal OptionId, [FromQuery] string ScreenName)
        {
            var companyOptions = _partMasterService.CompanyOptions(OptionId, ScreenName);
            return companyOptions;
        }

    }
}
