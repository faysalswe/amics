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
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;
        private readonly ILogger<ShipmentController> _logger;
        public ShipmentController(IShipmentService shipmentService, ILogger<ShipmentController> logger)
        {
            _shipmentService = shipmentService;
            _logger = logger;
        }

        /// <summary>
        /// API Route Controller to get search results of Project Id and ER details from below parameters for Shipment
        /// </summary>
        /// <param name="projectId">Project Id</param>                  
        /// <param name="projectName">Project Name</param>
        /// <param name="er">ER</param>
        /// <param name="budget">Budget Authority</param>                  
        /// <param name="user">user</param>                  
        [HttpGet, Route("ShipmentSearch")]
        public IList<LstChangeLocSearch> ShipmentSearch([FromQuery] string projectId, [FromQuery] string projectName, [FromQuery] string er, [FromQuery] string budget, [FromQuery] string user)
        {
            var vwChgLocSearchResult = _shipmentService.ShipmentSearch(projectId, projectName, er, budget, user);

            return vwChgLocSearchResult;
        }

        /// <summary>
        /// API Route Controller to get SO with Itemnum details(Parent Form) for given parameter ProjectId or SoMain
        /// </summary>
        /// <param name="projectId">Project Id</param>   
        /// <param name="somain">SO Main</param>   
        [HttpGet, Route("ShipmentViewByERProject")]
        public IList<LstChangeLocSearch> ShipmentView([FromQuery] string projectId, [FromQuery] string somain)
        {
            var vwChgLocViewResult = _shipmentService.ShipmentViewByProjER(projectId, somain);

            return vwChgLocViewResult;
        }

        /// <summary>
        /// API Route Controller to get Shipment view details(Left table) for Serial/Basic
        /// </summary>
        /// <param name="somain">So Main</param>   
        /// <param name="itemnumber">Item Number</param>   
        /// <param name="userId">User Id</param>   
        /// <param name="soLinesId">So Lines Id</param>           
        /// /// <param name="invType">InvType(Basic/Serial)</param>           
        [HttpGet, Route("ShipmentViewDetails")]
        public IList<LstChangeLocSearch> ShipmentViewDetails([FromQuery] string somain, [FromQuery] string itemnumber, [FromQuery] string userId, [FromQuery] string soLinesId, [FromQuery] string invType)
        {
            var vwChgLocViewResult = _shipmentService.ShipmentViewDetails(somain, itemnumber, userId, soLinesId, invType);

            return vwChgLocViewResult;
        }

        /// <summary>
        /// API Controller to select picked Basic/Serial items and populate it in the right table
        /// </summary>
        /// <param name="itemsId">itemsId</param>    
        /// <param name="username">UserName</param>    
        /// <param name="solinesId">solinesId</param>    
        /// <param name="invType">invType</param>    
        [HttpGet, Route("ShipmentSelectedItems")]
        public List<LstChangeLocSearch> ShipmentSelectedItems([FromQuery] string itemsId, [FromQuery] string username, [FromQuery] string solinesId, [FromQuery] string invType)
        {
            var chgLocTransItemsResult = _shipmentService.ShipmentViewSelectedDetails(itemsId, username, solinesId, invType);

            return chgLocTransItemsResult;
        }

        /// <summary>
        /// API Route Controller to clear the data in the table inv_pick_ship on page load.
        /// </summary>
        /// <param name="userName">UserName</param>                   
        [HttpPost, Route("DeleteInvPickShip")]
        public LstMessage DeleteInvPickShip([FromBody] UserInfo User)
        {
            var delPickShipResult = _shipmentService.DeleteInvPickShip(User.UserName);

            return delPickShipResult;
        }

        /// <summary>
        /// API Route Controller to check pick items exist in inv_pick_ship table and also checks available quantity
        /// in the inv_serial/inv_basic table, if not exists, update invserialid/invbasicid, transqty, solinesid details into inv_pick_ship table
        /// </summary>
        /// <param name="LstChgLocTransItems">LstChgLocTransItems</param>                  
        [HttpPost, Route("UpdateInvPickShip")]
        public LstMessage UpdateInvPickShip([FromBody] List<LstChgLocTransItems> lstchgloc)
        {
            var pickShipResult = _shipmentService.UpdateDelInvPickShip(lstchgloc);

            return pickShipResult;
        }

        /// <summary>
        /// API Route Controller to insert the picked items data in the translog/translogsn/inv_soship/inv_pick tables and update qty 
        /// for corresponding invbasic/invserial tables
        /// This method InvPickShipCount() gets count from the table inv_pick_ship for username
        /// This SP amics_sp_api_shipment_ship used to ship the items.
        /// </summary>
        /// <param name="userName">UserName</param>   
        /// <param name="mdatout">To mdatout</param>           
        [HttpGet, Route("UpdateShipment")]
        public LstMessage UpdateShipment([FromQuery] string userName, [FromQuery] string mdatout)
        {
            var updChangeLocResult = _shipmentService.UpdateShipment(userName, mdatout);

            return updChangeLocResult;
        }
        //public class UserInfo
        //{
        //    public string UserName { get; set; }
        //}

    }
}
