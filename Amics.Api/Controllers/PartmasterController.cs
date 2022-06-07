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
        public IList<LstItemDetails> GetPartmasterDetails([FromQuery] string itemnumber, [FromQuery] string rev)
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

    }
}
