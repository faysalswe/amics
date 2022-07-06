using Aims.Core.Models;
using Aims.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Amics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChangeLocationController : ControllerBase
    {
        private readonly IChangeLocService _changeLocService;
        private readonly ILogger<ChangeLocationController> _logger;
        public ChangeLocationController(IChangeLocService changeLocService, ILogger<ChangeLocationController> logger)
        {
            _changeLocService = changeLocService;
            _logger = logger;
        }

        /// <summary>
        /// API Route Controller to get search results of Project Id and ER details from below parameters
        /// </summary>
        /// <param name="projectId">Project Id</param>                  
        /// <param name="projectName">Project Name</param>
        /// <param name="er">ER</param>
        /// <param name="budget">Budget Authority</param>                  
        /// <param name="user">user</param>                  
        [HttpGet, Route("ChangeLocSearch")]
        public IList<LstChangeLocSearch> ChangeLocSearch([FromQuery] string projectId,[FromQuery] string projectName,[FromQuery] string er,[FromQuery] string budget,[FromQuery] string user)
        {            
            var vwChgLocSearchResult = _changeLocService.ChangeLocSearch(projectId, projectName, er, budget, user);

            return vwChgLocSearchResult;
        }

        /// <summary>
        /// API Route Controller to get SO with itemnum details for given parameter ProjectId or SOMain
        /// </summary>
        /// <param name="projectId">Project Id</param>   
        /// <param name="somain">SO Main</param>   
        [HttpGet, Route("ChangeLocView")]
        public IList<LstChangeLocSearch> ChangeLocView([FromQuery] string projectId, [FromQuery] string somain)
        {
            var vwChgLocViewResult = _changeLocService.ChangeLocationView(projectId, somain);

            return vwChgLocViewResult;
        }

        /// <summary>
        /// API Route Controller to get Change Location view details for Serial/Basic
        /// </summary>
        /// <param name="somain">SO Main</param>   
        /// <param name="itemnumber">Item Number</param>   
        /// <param name="userId">User Id</param>   
        /// <param name="soLinesId">SO Lines Id</param>           
        [HttpGet, Route("ChangeLocViewDetails")]
        public IList<LstChangeLocSearch> ChangeLocViewDetails([FromQuery] string somain, [FromQuery] string itemnumber, [FromQuery] string userId, [FromQuery] string soLinesId, [FromQuery] string invType)
        {
            var vwChgLocViewResult = _changeLocService.ChangeLocViewDetails(somain, itemnumber, userId, soLinesId,invType);

            return vwChgLocViewResult;
        }
              
        /// <summary>
        /// API Route Controller to check pick items exist in inv_transfer_location table and also checks available quantity 
        /// from inv_serial/inv_basic table, update invserialid/invbasicid, transqty, solinesid details into inv_transfer_location table
        /// </summary>
        /// <param name="LstChgLocTransItems">LstChgLocTransItems</param>                  
        [HttpPost, Route("UpdateInvTransLoc")]
        public LstMessage UpdateTransloc([FromBody] List<LstChgLocTransItems> lstchgloc)
        {          
            var translocResult = _changeLocService.UpdateInvTransLocation(lstchgloc);

            return new LstMessage() { Message = translocResult };
        }

        /// <summary>
        /// API Service to transfer the pick items in translog and corresponding basic/serial tables
        /// This method ChangeLocationTransCount() gets count from the table inv_transfer_location for passing parameter 'username'
        /// This method GetTransLogNum() updates translognum+1 for translognum field in the table list_next_number and 
        /// fetch the translognum to insert into translog and update inv_transfer_location table
        /// Finally, calling this SP sp_essex_transfer5 to transfer the item, it updates the translog,invBasic/invSerial tables.
        /// This method GetTransDatefmTransNum() gets transaction date using translog number
        /// </summary>
        /// <param name="userName">UserName</param>   
        /// <param name="toWarehouse">To Warehouse</param>   
        /// <param name="toLocation">To Location</param>           
        [HttpPost, Route("UpdateChangeLoc")]
        public LstMessage UpdateChangeLoc(string userName, string toWarehouse, string toLocation)
        {
            var updChangeLocResult = _changeLocService.UpdateChangeLocation(userName, toWarehouse, toLocation);

            return updChangeLocResult;
        }

        /// <summary>
        /// API Route Controller to clear the data in the table inv_transfer_location when page load.
        /// </summary>
        /// <param name="userName">UserName</param>                   
        [HttpGet, Route("DeleteInvTransLoc")]
        public LstMessage DeleteInvTransLoc([FromQuery] string userName)
        {
            var delTransLocResult = _changeLocService.DeleteInvTransferLoc(userName);

            return delTransLocResult;
        }
    }
}
