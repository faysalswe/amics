using Amics.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json; 

namespace Amics.web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    { 

        private readonly ILogger<ConfigController> _logger;
        private readonly JsonSerializerSettings _json;
        private readonly string _backEndApi;
        public ConfigController(IConfiguration configuration, ILogger<ConfigController> logger)
        {
            _backEndApi= configuration.GetValue<string>("PublicApiUrl");
            _logger = logger;
            _json = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                FloatParseHandling = FloatParseHandling.Decimal,
                NullValueHandling = NullValueHandling.Ignore
            };
        }
         
        [HttpGet]
        public AppConfig GetConfigJson()
        {
            AppConfig config = new AppConfig() { PublicApiUrl = _backEndApi };
            return config;
        }

        
    }
}
