using Microsoft.AspNetCore.Mvc;
using Aims.Core.Models;
using Aims.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
namespace Amics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;
        private readonly ILogger<ContractController> _logger;

        public ContractController(IContractService contractService, ILogger<ContractController> logger)
        {
            _contractService = contractService;
            _logger = logger;
        }

        /// <summary>
        /// API Route Controller to get Contract details when search button is clicked 
        /// </summary>
        /// <param name="contract">contract</param>
        /// <param name="project">project</param>
        [HttpGet, Route("GetContractSearch")]
        public IList<LstContracts> LoadContracts([FromQuery] string contract, [FromQuery] string project)
        {
            var searchResult = _contractService.LoadContracts(contract, project);
            return searchResult;
        }

        /// <summary>
        /// API Route Controller to populate Project details for selected Contract Id
        /// </summary>
        /// <param name="contract">contract</param>
        /// <param name="project">project</param>
        [HttpGet, Route("GetProjects")]
        public IList<LstProjects> LoadProjects([FromQuery] string contractId)
        {
            var projResult = _contractService.LoadProjects(contractId);
            return projResult;
        }

        /// <summary>
        /// API Route Controller to update Contract details 
        /// </summary>
        /// <param name="lstContracts">lstContracts</param>
        /// <param name="userName">userName</param>
        [HttpPost, Route("UpdateContracts")]
        public LstMessage UpdateContracts([FromBody] List<LstContracts> lstContracts)
        {
            var conUpdateRes = _contractService.UpdateContracts(lstContracts);
            return conUpdateRes;
        }

        /// <summary>
        /// API Route Controller to update Project details 
        /// </summary>
        /// <param name="lstProjects">lstProjects</param>
        /// <param name="userName">userName</param>
        [HttpPost, Route("UpdateProjects")]
        public LstMessage UpdateProjects([FromBody] List<LstProjects> lstProjects)
        {
            var projResult = _contractService.UpdateProjects(lstProjects);
            return projResult;
        }
    }
}
