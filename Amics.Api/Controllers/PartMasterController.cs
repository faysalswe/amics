﻿using Aims.Core.Models;
using Aims.PartMaster.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Amics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartMasterController : ControllerBase
    {
        private readonly IPartMasterService _partMasterService;
        private readonly ILogger<PartMasterController> _logger;
        public PartMasterController(IPartMasterService partMasterService, ILogger<PartMasterController> logger)
        {
            _partMasterService = partMasterService;
            _logger = logger;
        }

        [HttpGet, Route("Lookup")]
        public IList<AimcsSpLookUp> GetLookUpData([FromQuery]FieldNameSearch fieldName, [FromQuery] string search_col1, [FromQuery] string search_col2)
        {
            var result = _partMasterService.CommonLookup(fieldName, search_col1, search_col2);

            return result;

        }
    }
}