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
    public class ConfigController : ControllerBase
    {
        private readonly IConfigService _configService;
        private readonly ILogger<ConfigController> _logger;
        public ConfigController(IConfigService configService, ILogger<ConfigController> logger)
        {
            _configService = configService;
            _logger = logger;
        }


        /// <summary>
        /// API Route Controller for get Company Options. Use this options for show or hide the fields or set default request globally 
        /// </summary>
        /// <param name="OptionId">Integer value as a parameter.</param>
        /// <param name="ScreenName">Get options by screen name if it is global, parameter value should be 'GENERAL' </param>

        [HttpGet, Route("GetCompanyOptions")]
        public IList<LstCompanyOptions> GetCompanyOptions([FromQuery] decimal OptionId, [FromQuery] string ScreenName)
        {
            var companyOptions = _configService.CompanyOptions(OptionId, ScreenName);
            return companyOptions;
        }

        /// <summary>
        /// API Route Controller to get list field properties information, pass labelNum as parameter.
        /// </summary>        
        /// <param name="labelNum">Label Number</param>           
        [HttpGet, Route("Label")]
        public IList<LstFieldProperties> GetListFieldProperties(string labelNum)
        {
            var fieldPropResult = _configService.LoadFieldProperties(labelNum);

            return fieldPropResult;
        }

        /// <summary>
        /// API Route Controller to get Message num & Message text from list_messages table, messagetext ie. message num or message text can pass as parameter to get specific messages 
        /// </summary>
        /// <param name="messagetext">Message text as a parameter.</param>
        [HttpGet, Route("GetMessages")]
        public IList<LstMessagetext> GetMessages([FromQuery] string messagetext)
        {
            var messages = _configService.ListMessages(messagetext);
            return messages;
        }
    }
}
