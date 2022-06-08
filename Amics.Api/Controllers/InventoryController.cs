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
        /// API Route Controller for get Company Options. Use this options for show or hide the fields or set default request globally 
        /// </summary>
        /// <param name="OptionId">Integer value as a parameter.</param>
        /// <param name="ScreenName">Get options by screen name if it is global, parameter value should be 'GENERAL' </param>

        //[HttpGet, Route("GetCompanyOptions")]
        //public IList<LstCompanyOptions> GetCompanyOptions([FromQuery] decimal OptionId, [FromQuery] string ScreenName)
        //{
        //    var companyOptions = _partMasterService.CompanyOptions(OptionId, ScreenName);
        //    return companyOptions;
        //}

    }
}
