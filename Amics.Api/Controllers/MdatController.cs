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
using Amics.Api.Model;

namespace Amics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MdatController : ControllerBase
    {
        private readonly IMdatService _mdatService;
        private readonly ILogger<MdatController> logger;
        public MdatController(IMdatService mdatService, ILogger<MdatController> logger)
        {
            _mdatService = mdatService;
            this.logger = logger;
        }
                
        /// <summary>
        /// API Route Controller to get Mdat search result from parameter mdatNum/er/packlistNum/status
        /// </summary>
        /// <param name="mdatNum">mdatNum</param>  
        /// <param name="er">er</param> 
        /// <param name="packlistNum">packlistNum</param> 
        /// <param name="status">status</param> 
        [HttpGet, Route("GetMdatOutSearchDetails")]
        public List<LstMdat> GetMdatOutSearchDetails([FromQuery] string mdatNum, [FromQuery] string er,[FromQuery] string packlistNum, [FromQuery] string status)
        {
            var mdatSearchResult = _mdatService.MdatOutSearch(mdatNum, er, packlistNum, status);

            return mdatSearchResult;
        }
        /// <summary>
        /// API Route Controller to get Mdat view details on clicking Mdat number in the Search result
        /// </summary>
        /// <param name="mdatNum">mdatNum</param>         
        [HttpGet, Route("GetMdatOutViewDetails")]
        public LstMdat GetMdatOutViewDetails([FromQuery] string mdatNum)
        {
            var mdatViewResult = _mdatService.MdatOutViewDetails(mdatNum);

            return mdatViewResult;
        }
        /// <summary>
        /// API Route Controller to insert/update/delete Mdat details in the table inv_mdat_out
        /// </summary>
        /// <param name="actionFlag">ActionFlag</param>  
        /// <param name="id">Id</param>  
        /// <param name="mdatNum">Mdat Out Number</param>  
        /// <param name="somain">Somain</param> 
        /// <param name="status">status</param> 
        /// <param name="submittedDate">submittedDate</param>
        /// <param name="approvedDate">approvedDate</param>
        /// <param name="shippedDate">shippedDate</param>
        /// <param name="cancelledDate">cancelledDate</param>
        /// <param name="createdBy">createdBy</param>      
        [HttpPost, Route("UpdateMdatOutDetails")]
        public string UpdateMdatOutDetails([FromBody] LstMdat lstMdat)
        {
            var mdatUpdateResult = _mdatService.MdatOutUpdateDetails(lstMdat);

            return mdatUpdateResult;
        }
    }
}
