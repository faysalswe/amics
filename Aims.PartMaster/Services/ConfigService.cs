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
        List<LstUser> GetUsersList(string userId);
        List<LstUserAccess> GetUserAccess(string userId);
        List<LstAccessWarehouse> GetWarehouseAccess(string userId);
        LstMessage ValidateWarehouse(string warehouse);
        LstMessage ValidateUserId(string username);
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
                .FromSqlRaw($"exec amics_sp_api_get_list_company_options @optionid={OptionId},@screenname='{screenName}'")
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
                            .FromSqlRaw($"amics_sp_api_list_fieldproperties @labelnumber='{labelNum}'").ToList<LstFieldProperties>();

            return optResult;
        }

        /// <summary>
        /// API Service to get Message num and message text from db, returns all the data if parameter is null. 
        /// </summary>
        /// <param name="messagetext">Message Text</param>        
        public List<LstMessagetext> ListMessages(string messagetext)
        {
            var optMessageResult = _amicsDbContext.LstMessagetext
                            .FromSqlRaw($"amics_sp_api_list_messages @messagetext='{messagetext}'").ToList<LstMessagetext>();

            return optMessageResult;
        }

        /// <summary>
        /// API Service to get users list from sec_users table. Get all the users list if parameter is null
        /// </summary>
        /// <param name="userId">userId</param>        
        public List<LstUser> GetUsersList(string userId)
        {
            List<LstUser> lstUser = new List<LstUser>();
           
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_get_userlist";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlCommand.Parameters.Add(new SqlParameter("@userId", string.IsNullOrWhiteSpace(userId) ? string.Empty : userId));
                    var dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        LstUser UsersList = new LstUser();
                        UsersList.userId = dataReader["id"].ToString();
                        UsersList.userName = dataReader["userid"].ToString();
                        UsersList.firstName = dataReader["firstname"].ToString();
                        lstUser.Add(UsersList);
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    sqlCommand.Dispose();
                    conn.Close();
                }
            }
            return lstUser.ToList();
        }

        /// <summary>
        /// API Service to get user's access from sec_access and sec_users_access table. Get all the user's access list if parameter is null
        /// </summary>
        /// <param name="userId">userId</param>        
        public List<LstUserAccess> GetUserAccess(string userId)
        {           
            List<LstUserAccess> lstUserAccess = new List<LstUserAccess>();
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_get_secuseraccess";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlCommand.Parameters.Add(new SqlParameter("@userId", string.IsNullOrWhiteSpace(userId) ? string.Empty : userId));
                    var dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        LstUserAccess userAccess = new LstUserAccess();
                        userAccess.accessId = dataReader["accessid"].ToString();
                        userAccess.access = dataReader["access"].ToString();
                        
                        if (dataReader["readonly"].ToString() == "True")
                        {
                            userAccess.readOnly = 1;
                        }
                        else
                        {
                            userAccess.readOnly = 0;
                        }
                        
                        if (dataReader["OnTheFly"] != DBNull.Value)
                            userAccess.OnTheFly = Convert.ToInt16(dataReader["OnTheFly"]);
                        else
                            userAccess.OnTheFly = 0;
                        
                        lstUserAccess.Add(userAccess);
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    sqlCommand.Dispose();
                    conn.Close();
                }
            }
            return lstUserAccess.ToList();
        }

        /// <summary>
        /// API Service to get user's warehouse access from list_warehouse and sec_users_warehouses table. 
        /// Get all the warehouses if parameter is null 
        /// </summary>
        /// <param name="userId">userId</param>  
        public List<LstAccessWarehouse> GetWarehouseAccess(string userId)
        {
            
            List<LstAccessWarehouse> lstWarehouseAccess = new List<LstAccessWarehouse>();
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_get_secwhaccess";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlCommand.Parameters.Add(new SqlParameter("@userId", string.IsNullOrWhiteSpace(userId) ? string.Empty : userId));
                    var dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        LstAccessWarehouse whAccess = new LstAccessWarehouse();
                        whAccess.WarehouseId = dataReader["warehouses_id"].ToString();
                        whAccess.Warehouse = dataReader["warehouse"].ToString();
                        if (dataReader["transok"].ToString() == "True")
                        {
                            whAccess.readOnly = 1;
                        }
                        else
                        {
                            whAccess.readOnly = 0;
                        }
                        lstWarehouseAccess.Add(whAccess);
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    sqlCommand.Dispose();
                    conn.Close();
                }
            }
            return lstWarehouseAccess.ToList();
        }

        /// <summary>
        /// API Service to validate entered warehouse by user. Returns warehouseid if exists        
        /// </summary>
        /// <param name="warehouse">warehouse</param>  
        public LstMessage ValidateWarehouse(string warehouse)
        {
           
            string warehouseId = "";
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_validate_warehouse";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlCommand.Parameters.Add(new SqlParameter("@warehouse", string.IsNullOrWhiteSpace(warehouse) ? string.Empty : warehouse));
                    var dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.Read())
                    {
                        warehouseId = dataReader["id"].ToString();
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    sqlCommand.Dispose();
                    conn.Close();
                }
            }
            return new LstMessage { Message = warehouseId };
        }


        /// <summary>
        /// API Service to validate entered username, returns 1 if exists otherwise 0 if not exists    
        /// </summary>
        /// <param name="userId">userId</param>  
        public LstMessage ValidateUserId(string username)
        {           
            string isValidUser = "0";
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_validate_userId";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlCommand.Parameters.Add(new SqlParameter("@userId", string.IsNullOrWhiteSpace(username) ? string.Empty : username));
                    var dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.Read())
                    {
                        isValidUser = "1";
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    sqlCommand.Dispose();
                    conn.Close();
                }
            }
            return new LstMessage { Message = isValidUser };
        }
    }
}
