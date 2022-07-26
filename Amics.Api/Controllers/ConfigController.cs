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

        /// <summary>
        /// API Controller to get users list from sec_users table. Get all the users list if parameter is null
        /// </summary>
        /// <param name="userId">userId</param>    
        [HttpGet, Route("GetUserLst")]
        public IList<LstUser> GetUsersList([FromQuery] string userId)
        {
            var usersLstRes = _configService.GetUsersList(userId);
            return usersLstRes;
        }

        /// <summary>
        /// API Controller to get user's access from sec_access and sec_users_access table. Get all the user's access list if parameter is null
        /// </summary>
        /// <param name="userId">userId</param>    
        [HttpGet, Route("GetUserAccess")]
        public IList<LstUserAccess> GetSecUserAccess([FromQuery] string userId)
        {
            var userAccess = _configService.GetUserAccess(userId);
            return userAccess;
        }

        /// <summary>
        /// API Controller to get user's warehouse access from list_warehouse and sec_users_warehouses table. 
        /// Get all the warehouses if parameter is null 
        /// </summary>
        /// <param name="userId">userId</param>  
        [HttpGet, Route("GetUserWarehouseAccess")]
        public IList<LstAccessWarehouse> GetWarehouseAccess([FromQuery] string userId)
        {
            var whAccess = _configService.GetWarehouseAccess(userId);
            return whAccess;
        }

        /// <summary>
        /// API Controller to validate entered warehouse by user. Returns warehouseid if exists        
        /// </summary>
        /// <param name="warehouse">warehouse</param>  
        [HttpGet, Route("ValidateWarehouse")]
        public LstMessage GetValidWarehouse([FromQuery] string warehouse)
        {
            var validWhRes = _configService.ValidateWarehouse(warehouse);
            return validWhRes;
        }

        /// <summary>
        /// API Controller to validate entered username, returns 1 if exists otherwise 0 if not exists    
        /// </summary>
        /// <param name="userId">userId</param>  
        [HttpGet, Route("ValidateUserId")]
        public LstMessage GetValidateUserId([FromQuery] string userId)
        {
            var userIdRes = _configService.ValidateUserId(userId);
            return userIdRes;
        }
    }
}
