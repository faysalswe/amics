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

        [HttpGet, Route("CompanyOptions")]
        public IList<LstCompanyOption> GetCompanyOptions()
        {

            var companyOptResult = _partMasterService.LoadCompanyOptions();

            return companyOptResult;
        }

        [HttpGet, Route("ListFieldProperties")]
        public IList<LstFieldProperties> GetListFieldProperties(string labelNum)
        {
            var fieldPropResult = _partMasterService.LoadFieldProperties(labelNum);

            return fieldPropResult;
        }
    }
}
