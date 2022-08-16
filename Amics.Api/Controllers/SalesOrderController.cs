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
    public class SalesOrderController : ControllerBase
    {
        private readonly ISalesOrderService _salesOrderService;
        private readonly ILogger<SalesOrderController> _logger;
        public SalesOrderController(ISalesOrderService salesOrderService, ILogger<SalesOrderController> logger)
        {
            _salesOrderService = salesOrderService;
            _logger = logger;
        }

        // <summary>
        /// API Route Controller to get search results of Project Id and ER details from below parameters
        /// </summary>                         
        [HttpGet, Route("SoSearchOnOpen")]
        public IList<LstSoMain> SalesOrderSearchOnOpen()
        {
            var soSearchResult = _salesOrderService.SalesOrderSearchOnOpen();

            return soSearchResult;
        }

        // <summary>
        /// API Route Controller to get search results of Project Id and ER details from below parameters
        /// </summary>   
        ///  <param name="soMain">soMain</param>                  
        /// <param name="status">status</param>                  
        /// <param name="requestor">requestor</param>                  
        /// <param name="projectId">projectId</param>                  
        /// <param name="projectName">projectName</param>                  
        /// <param name="mdatIn">mdatIn</param>  
        [HttpGet, Route("SoSearch")]         
        public IList<LstSoMain> SalesOrderSearch([FromQuery] string soMain, [FromQuery] string status, [FromQuery] string requestor, [FromQuery] string projectId, [FromQuery] string projectName, [FromQuery] string mdatIn)
        {
            var soSearchRes = _salesOrderService.SalesOrderSearch(soMain, status, requestor, projectId, projectName, mdatIn);

            return soSearchRes;
        }

        // <summary>
        /// API Route Controller to get So parent details 
        /// </summary>                        
        ///  <param name="soMainId">soMainId</param>   
        [HttpGet, Route("SalesOrderMainView")]
        public LstSoMain SalesOrderMainView([FromQuery] string soMainId)
        {
            var soViewResult = _salesOrderService.SalesOrderMainView(soMainId);

            return soViewResult;
        }

        // <summary>
        /// API Route Controller to get So parent details 
        /// </summary>   
        /// <param name="soMain">soMain</param> 
        [HttpGet, Route("SoLinesView")]
        public List<LstSoLines> SoLinesView([FromQuery] string soMain)
        {
            var soLinesViewResult = _salesOrderService.SoLinesView(soMain);

            return soLinesViewResult;
        }

        // <summary>
        /// API Route Controller to get So parent details 
        /// </summary>     
        ///  <param name="Project">Project</param>
        ///  <param name="Status">Status</param>
        ///  <param name="Shipvia">Shipvia</param>
        [HttpGet, Route("ValidateSO")]
        public string ValidateSO([FromQuery] string Project, [FromQuery] string Status, [FromQuery] string Shipvia)
        {
            var sovalidResult = _salesOrderService.ValidateSO(Project, Status, Shipvia);

            return sovalidResult;
        }
        // <summary>
        /// API Route Controller to insert/update/delete So main details
        /// </summary> 
        /// <param name="LstSoMain">LstSoMain</param>
        /// <param name="userName">userName</param>
        [HttpPost, Route("SoMainUpdate")]
        public LstMessage UpdateSoMain([FromBody] LstSoMain somain)
        {
            var soUpdateResult = _salesOrderService.UpdateSoMain(somain);

            return soUpdateResult;
        }

        // <summary>
        /// API Route Controller to get So parent details 
        /// </summary>  
        /// <param name=" List<LstSoLines> solines"> List<LstSoLines> solines</param>
        /// <param name="userName">userName</param>
        [HttpPost, Route("SoLinesUpdate")]       
        public LstMessage UpdateSoLines([FromBody] List<LstSoLines> solines)
        {
            var solinesUpdate = _salesOrderService.UpdateSoLines(solines);

            return solinesUpdate;
        }

        // <summary>
        /// API Route Controller to populate so lines details in the top grid of Create Project Transfer
        /// </summary>  
        /// <param name="soMain">soMain</param>        
        [HttpGet, Route("CreateProjectTransferView")]
        public List<LstSoLines> CreateProjectTransferView([FromQuery] string soMain)
        {
            var lstSoLines = _salesOrderService.LoadProjTransferView(soMain);

            return lstSoLines;
        }

        // <summary>
        /// API Route Controller to populate so main warehouse, items details in the bottom grid of Create Project Transfer
        /// </summary>          
        /// <param name="soMain">soMain</param>
        /// <param name="itemNumber">itemNumber</param>
        /// <param name="userId">userId</param>
        [HttpGet, Route("CreateProjTransInvTypeView")]
        public List<LstChangeLocSearch> CreateProjTransferInvTypeView([FromQuery] string soMain, [FromQuery] string itemNumber, [FromQuery] string userId)
        {
            var lstProjTrans = _salesOrderService.ProjTransferInvTypeView(soMain, itemNumber, userId);

            return lstProjTrans;
        }

        // <summary>
        /// API Route Controller to execute project transfer in Create Project Transfer
        /// </summary>  
        /// <param name=" List<LstProjectTransData>">LstProjectTransData</param>        
        [HttpPost, Route("ProjectTransUpdate")]
        public LstMessage ProjectTransUpdate([FromBody] List<LstProjectTransData> projTransData)
        {
            var projTransUpdate = _salesOrderService.ProjTransferUpdate(projTransData);

            return projTransUpdate;
        }

        /// <summary>
        /// API Service to view 'From project' data using parameters SoMain and Actionflag is 0
        /// To populate records for Top of the grid of View Project Transfer
        /// <param name="soMain">soMain</param>       
        /// </summary>            
        [HttpGet, Route("ViewFromProjectTransfer")]
        public List<LstInvSerLot> ViewFromProjectTransfer([FromQuery] string soMain)
        {
            var lstProjTrans = _salesOrderService.ViewFromProjTransfer(soMain);

            return lstProjTrans;
        }

        /// <summary>
        /// API Service to view 'To project' data using parameters SoMain and Actionflag is 1
        /// To populate records for bottom of the grid of View Project Transfer
        /// <param name="soMain">soMain</param>       
        /// </summary>         
        [HttpGet, Route("ViewToProjectTransfer")]
        public List<LstInvSerLot> ViewToProjectTransfer([FromQuery] string soMain)
        {
            var lstProjTrans = _salesOrderService.ViewToProjTransfer(soMain);

            return lstProjTrans;
        }

        /// <summary>
        /// API Controller to populate records in the popup window on clicking the grid row 
        /// Click row from Top of the grid, transoption "ERIN"
        /// Click row from Top of the grid, transoption "EROUT"
        /// <param name="itemsid">itemsid</param>       
        /// <param name="somainid">somainid</param>       
        /// <param name="tosomainid">tosomainid</param>       
        /// <param name="invtype">invtype</param>       
        /// <param name="itemno">itemno</param>       
        /// <param name="transoption">transoption</param>       
        ///</summary>  
        [HttpGet, Route("ViewProjTransferRowClicked")]
        public List<LstInvSerLot> ProjTransferRowClicked([FromQuery] string itemsid, [FromQuery] string somainid, [FromQuery] string tosomainid, [FromQuery] string invtype, [FromQuery] string itemno, [FromQuery] string transoption)
        {         
            var lstProjTrans = _salesOrderService.ViewProjTransferRowClicked(itemsid, somainid, tosomainid,  invtype, itemno, transoption);

            return lstProjTrans;
        }
    }
}
