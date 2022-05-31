﻿using Aims.Core.Models;
using Aims.PartMaster.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Amics.Api.Controllers
{
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")] 
    [ApiController]
    [ApiVersion("1.0")]
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
        public IList<LstLocaton> GetLocationLookUp([FromQuery] string searchLocation, [FromQuery] string warehouseId, [FromQuery] string locationId )
        {
          
            var resultLoc = _partMasterService.LocationLookup(searchLocation, warehouseId, locationId);

            return resultLoc;
        }

        [HttpGet, Route("ItemTypeLookup")]
        public IList<LstItemType> GetItemTypeSearch([FromQuery] string itemtype, [FromQuery] string itemtypeId)
        {

            var resultItemtype = _partMasterService.ItemTypeLookup(itemtype, itemtypeId);

            return resultItemtype;
        }

        [HttpGet, Route("ItemClass")]
        public IList<LstItemClass> GetItemClassSearch([FromQuery] string itemclass, [FromQuery] string itemclassId)
        {

            var resultItemclass = _partMasterService.ItemClassLookup(itemclass, itemclassId);

            return resultItemclass;
        }

        [HttpGet, Route("ItemCode")]
        public IList<LstItemCode> GetItemCodeSearch([FromQuery] string itemcode, [FromQuery] string itemcodeId)
        {

            var resultItemCode = _partMasterService.ItemCodeLookup(itemcode, itemcodeId);

            return resultItemCode;
        }

        [HttpGet, Route("ItemNumber")]
        public IList<LstItemSearch> GetItemNumberSearch([FromQuery] string itemnumber, [FromQuery] string description, [FromQuery] string itemtype, [FromQuery] string itemcode, [FromQuery] string itemclass)
        {

            var itemSearchResult = _partMasterService.ItemNumberSearch(itemnumber, description, itemtype, itemcode, itemclass);

            return itemSearchResult;
        }
    }
}