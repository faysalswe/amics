using Aims.PartMaster.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aims.PartMaster.Services;
using Aims.Core.Services;
using Aims.Core.Models;

namespace Amics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PartmasterController : ControllerBase
    {
        private readonly IPartmasterService _partMastService;
        private readonly ILogger<PartmasterController> _logger;
        public PartmasterController(IPartmasterService partMastService, ILogger<PartmasterController> logger)
        {
            _partMastService = partMastService;
            _logger = logger;
        }

        /// <summary>
        /// API Route Controller to get Partmaster details for parent form, pass Item number and rev as parameter.
        /// </summary>
        /// <param name="itemnumber">itemnumber</param>  
        /// <param name="rev">rev</param>      
        [HttpGet, Route("")]
        public LstItemDetails GetPartmasterDetails([FromQuery] string itemnumber, [FromQuery] string rev)
        {
            var resultPMInfo = _partMastService.LoadPartmaster(itemnumber, rev);

            return resultPMInfo;
        }

        /// <summary>
        /// API Route Controller to get Partmaster BOM details, pass itemsId as parameter.
        /// </summary>
        /// <param name="itemsId">Items Id</param>          
        [HttpGet, Route("BomDetails")]
        public IList<LstItemsBom> GetItemsBom([FromQuery] string itemsId)
        {
            var resultBomInfo = _partMastService.LoadItemsBom(itemsId);

            return resultBomInfo;
        }

        /// <summary>
        /// API Route Controller to get Partmaster PO details for grid, pass itemsId as parameter.
        /// </summary>
        /// <param name="itemsId">Items Id</param>          
        [HttpGet, Route("PODetails")]
        public IList<LstItemsPO> GetItemsPO([FromQuery] string itemsId)
        {
            var resultPOInfo = _partMastService.LoadItemsPO(itemsId);

            return resultPOInfo;
        }

        /// <summary>
        /// API Route Controller to get Partmaster PO details for grid, pass itemsId as parameter.
        /// </summary>
        /// <param name="itemsId">Items Id</param>          
        [HttpGet, Route("BOMCount")]
        public LstBomCount GetItemsBomCount([FromQuery] string itemsId)
        {
            var dataExist = _partMastService.ItemsBomCount(itemsId);

            return dataExist;
        }

        /// <summary>
        /// API Route Controller for deletion of Item Num details, column 'flag_delete' is update with 1 in the table 
        /// list items(item num deleted) if return message is null. Message will appear if item num is used in some other tables, so item num can't delete.
        /// </summary>
        /// <param name="itemnum">Item Number</param>          
        /// /// <param name="rev">Rev</param>          
        [HttpDelete, Route("")]
        public LstMessage ItemDetailsDelete([FromQuery] string itemnum, [FromQuery] string rev)
        {
            var dataExist = _partMastService.ItemNumDelete(itemnum,rev);

            return dataExist;
        }

        /// <summary>
        /// API Route Controller to get Partmaster PO details for grid, pass itemsId as parameter.
        /// </summary>
        /// <param name="itemsId">Items Id</param>          
        [HttpPost, Route("")]
        public LstMessage ItemDetailsAddUpdate([FromQuery] string id, [FromQuery] string itemNumber,[FromQuery] string rev,[FromQuery] string description, [FromQuery] string salesDescription, [FromQuery] string PurchaseDescription, [FromQuery] string invtypeidv, [FromQuery] string itemtypeidv, [FromQuery] string itemclassidv, [FromQuery] string itemcodeidv, string uomid, [FromQuery] decimal conversion, [FromQuery] decimal cost,decimal markup, [FromQuery] decimal price, [FromQuery] decimal price2, [FromQuery] decimal price3, [FromQuery] decimal weight, [FromQuery] int buyitem, [FromQuery] int obsolete, [FromQuery] string notes, [FromQuery] decimal minimum, [FromQuery] decimal maximum, [FromQuery] decimal leadtime, [FromQuery] string warehouseidv, [FromQuery] string locationsidv, [FromQuery] string glsales, [FromQuery] string glinv, [FromQuery] string glcogs, [FromQuery] string dwgno, [FromQuery] string user1, [FromQuery] string user2, [FromQuery] decimal user3, [FromQuery] int userbit, [FromQuery] int userbit2, [FromQuery] int userbit3)
        {
            var itemUpdate = _partMastService.ItemNumDetailsAddUpdate(id, itemNumber, rev, description, salesDescription, PurchaseDescription, invtypeidv, itemtypeidv, itemclassidv, itemcodeidv, uomid, conversion,
                cost, markup, price, price2, price3, weight, buyitem, obsolete, notes, minimum, maximum, leadtime, warehouseidv, locationsidv, glsales, glinv, glcogs, dwgno, user1, user2, user3, userbit, userbit2, userbit3);

            return itemUpdate;
        }
    }
}
