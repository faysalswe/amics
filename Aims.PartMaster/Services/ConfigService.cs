using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using Aims.Core.Models;
using Aims.PartMaster.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Aims.Core.Services
{
    public interface IConfigService
    {
        /// <summary>
        /// Interface  for get Company Options. Use this options for show or hide the fields or set default request globally 
        /// </summary>
        /// <param name="OptionId">Integer value as a parameter.</param>
        /// <param name="ScreenName">Get options by screen name if it is global, parameter value should be 'GENERAL' </param>
        List<LstCompanyOptions> CompanyOptions(decimal OptionId, string ScreenName);
        List<LstFieldProperties> LoadFieldProperties(string labelNum);
        List<LstMessagetext> ListMessages(string messagetext);
    }

    public class ConfigService: IConfigService
    {
        private readonly AmicsDbContext _amicsDbContext;
        public ConfigService(AmicsDbContext aimsDbContext)
        {
            _amicsDbContext = aimsDbContext;
        }

        /// <summary>
        /// API Service for get Company Options. Use this options for show or hide the fields or set default request globally 
        /// </summary>
        /// <param name="OptionId">Integer value as a parameter.</param>
        /// <param name="ScreenName">Get options by screen name if it is global, parameter value should be 'GENERAL' </param>

        public List<LstCompanyOptions> CompanyOptions(decimal OptionId, string ScreenName)
        {

            var screenName = string.IsNullOrEmpty(ScreenName) ? string.Empty : ScreenName;

            var searchResult = _amicsDbContext.ListCompanyOptions
                .FromSqlRaw($"exec sp_webapi_get_list_company_options @optionid={OptionId},@screenname='{screenName}'")
                .ToList<LstCompanyOptions>();

            return searchResult;
        }

        /// <summary>
        /// API Service to get My Label info from db, returns all the data if parameter is null. Label no can pass single or multiple number with comma separated.
        /// </summary>
        /// <param name="labelNum">Label Number</param>        
        public List<LstFieldProperties> LoadFieldProperties(string labelNum)
        {
            var optResult = _amicsDbContext.LstFieldProperties
                            .FromSqlRaw($"amics_sp_list_fieldproperties @labelnumber='{labelNum}'").ToList<LstFieldProperties>();

            return optResult;
        }

        /// <summary>
        /// API Service to get Message num and message text from db, returns all the data if parameter is null. 
        /// </summary>
        /// <param name="messagetext">Message Text</param>        
        public List<LstMessagetext> ListMessages(string messagetext)
        {
            var optMessageResult = _amicsDbContext.LstMessagetext
                            .FromSqlRaw($"amics_sp_list_messages @messagetext='{messagetext}'").ToList<LstMessagetext>();

            return optMessageResult;
        }

    }
}
