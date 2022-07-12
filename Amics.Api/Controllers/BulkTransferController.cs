using Aims.PartMaster.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Aims.Core.Services;
using Aims.Core.Models;
using Amics.Api.Model;

namespace Amics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BulkTransferController : ControllerBase
    {
        private readonly IBulkTransferService _bulkTransferService;
        private readonly ILogger<BulkTransferController> logger;
        public BulkTransferController(IBulkTransferService bulkTransferService, ILogger<BulkTransferController> logger)
        {
            _bulkTransferService = bulkTransferService;
            this.logger = logger;
        }

        /// <summary>
        /// API Route Controller to check valid warehouse and location and returns locationid to get item details 
        /// </summary>
        /// <param name="warehouse">Warehouse</param>  
        /// <param name="location">Location</param> 
        [HttpGet, Route("ValidateLocation")]
        public string ValidateLocation([FromQuery] string warehouse, [FromQuery] string location)
        {
            var validLocationId = _bulkTransferService.ValidateLocation(warehouse, location);

            return validLocationId;
        }

        /// <summary>
        /// API Route Controller to get Item Numbeer details using warehouse and location for parent form view  
        /// </summary>
        /// <param name="warehouse">Warehouse</param>  
        /// <param name="location">Location</param> 
        [HttpGet, Route("BulkTransferView")]
        public List<LstBulkTransfer> BulkTransferView([FromQuery] string warehouse, [FromQuery] string location)
        {
            var bulkTransView = _bulkTransferService.BulkTransferItemDetails(warehouse, location);

            return bulkTransView;
        }

        /// <summary>
        /// API Route Controller to get Item Numbeer details using warehouse and location for parent form view  
        /// </summary>
        /// <param name="warehouse">Warehouse</param>  
        /// <param name="location">Location</param> 
        [HttpPost, Route("ExecuteBulkTransfer")]
        public string BulkTransferView([FromBody] LstBulkTransferUpdate bulkTransferUpdate)
        {
            var updateBulkTrans = _bulkTransferService.ExecuteBulkTransfer(bulkTransferUpdate);

            return updateBulkTrans;
        }

    }
}
