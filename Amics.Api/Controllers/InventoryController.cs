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
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;
        public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }


        /// <summary>
        /// API Route Controller to get Inventory Status of an item, pass ItemsGUID and UsersGUID as parameters.
        /// </summary>        
        /// <param name="ItemsId">items guid</param> 
        /// <param name="SecUserId">sec users guid</param>     
        [HttpGet, Route("getInventoryStatus")]
        public  InvStatus GetInventoryStatus([FromQuery] string ItemsId, [FromQuery] string SecUserId)
        {

            var resultItemCode = _inventoryService.InventoryStatus(ItemsId, SecUserId);

            return resultItemCode;
        }

        /// <summary>
        /// API Route Controller to get DefaultValues for the screen or the form, pass FormName as optional parameter.
        /// </summary>        
        /// <param name="FormName">string</param> 

        [HttpGet, Route("getDefaultValues")]
        public IList<LstDefaultsValues> GetDefaultValues([FromQuery] string FormName)
        {

            var resultItemCode = _inventoryService.DefaultValues(FormName);

            return resultItemCode;
        }

        /// <summary>
        /// API Route Controller to get sales order or ER number an item, must pass ItemsGUID, optional somain as parameters.
        /// </summary>        
        /// <param name="ItemsId">items guid</param> 
        /// <param name="SoMain">perfix so main</param> 

        [HttpGet, Route("getErLookup")]
        public IList<LstErLookup> GetErLookup([FromQuery] string ItemsId, string SoMain)
        {
            var resultItemCode = _inventoryService.ErLookup(ItemsId, SoMain);
            return resultItemCode;
        }
        

    }
}
