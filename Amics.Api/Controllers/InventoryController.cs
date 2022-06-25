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


        /// <summary>
        /// API Route Controller to get details of trans action log, options parametes FromDate,ToDate and Reason.
        /// </summary>        
        /// <param name="FromDate">01/25/2020</param> 
        /// <param name="ToDate">01/25/2022</param> 
        /// <param name="Reason">MISC REC,MISC PICK</param> 

        [HttpGet, Route("getTransLog")]        
        public IList<LstTransLog> GetTransLog([FromQuery] string FromDate, string ToDate, string Reason)
        {
            var resultTransLog = _inventoryService.TransLog(FromDate, ToDate, Reason);
            return resultTransLog;
        }

        /// <summary>
        /// API Route Controller to get List next numbers for receiving.
        /// </summary>        
     
        [HttpGet, Route("getTransNumberRec")]
        public TransNextNum GetTransNumberRec()
        {
            var resultTransLog = _inventoryService.TransNumberRec();
            return resultTransLog;
        }


        /// <summary>
        /// API Route Controller for execute receipt stored procedure and increase the quantity
        /// </summary>        
        [HttpPost, Route("UpdateReceipt")]
        public LstMessage UpdateInvReceipt([FromBody] InvReceipts InvReceipts)
        {
            var BomGridUpdate = _inventoryService.UpdateInvReceipt(InvReceipts);

            return BomGridUpdate;
        }

        /// <summary>
        /// API Route Controller for execute receipt stored procedure and increase the quantity
        /// </summary>        
        [HttpPost, Route("InsertInvSerLot")]
        public LstMessage InsertInvSerLot([FromBody] List<InvSerLot> InvSetLot)
        {
            var InvSerLot = _inventoryService.InsertInvSerLot(InvSetLot);

            return InvSerLot;
        }

    }
}
