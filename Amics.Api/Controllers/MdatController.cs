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
        /// <param name="LstMdat">LstMdat</param>         
        [HttpPost, Route("UpdateMdatOutDetails")]
        public LstMessage UpdateMdatOutDetails([FromBody] LstMdat lstMdat)
        {
            var mdatUpdateResult = _mdatService.MdatOutUpdateDetails(lstMdat);

            return mdatUpdateResult;
        }

        [HttpGet, Route("Somain")]
        public IList<LstErLookup> GetSomainLookUp([FromQuery] string searchSomain, [FromQuery] string somainId, [FromQuery] string statusId)
        {
            var resultSomain = _mdatService.SomainLookup(searchSomain, somainId, statusId);

            return resultSomain;
        }

        [HttpGet, Route("Status")]
        public IList<LstMdatStatusLookup> GetStatusLookUp([FromQuery] string searchStatus, [FromQuery] string statusId)
        {
            var resultStatus = _mdatService.StatusLookup(searchStatus, statusId);

            return resultStatus;
        }
    }
}
