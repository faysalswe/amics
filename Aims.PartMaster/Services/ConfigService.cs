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
        LstMessage AddSecUserDetails(LstUser userinfo);
        LstMessage AddSecUserAccessDetails(List<LstUserAccess> userAccess);
        LstMessage AddSecWhAccessDetails(List<LstAccessWarehouse> whAccess);
        LstMessage GetUserAccessByAccessNum(string userId, string accessNum);

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
                        UsersList.Id = dataReader["id"].ToString();
                        UsersList.UserId = dataReader["userid"].ToString();
                        UsersList.FirstName = dataReader["firstname"].ToString();
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
                        userAccess.AccessId = dataReader["accessid"].ToString();
                        userAccess.Access = dataReader["access"].ToString();
                        
                        if (dataReader["readonly"].ToString() == "True")
                        {
                            userAccess.ReadOnly = 1;
                        }
                        else
                        {
                            userAccess.ReadOnly = 0;
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
                            whAccess.ReadOnly = 1;
                        }
                        else
                        {
                            whAccess.ReadOnly = 0;
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
        /// API Service to validate entered username while adding new user, returns 1 if exists otherwise 0 if not exists    
        /// </summary>
        /// <param name="username">username</param>  
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

        /// <summary>
        /// API Service to add/update/delete user in sec_users/login_cred table
        /// </summary>
        /// <param name="LstUser">User Info</param>        
        public LstMessage AddSecUserDetails(LstUser userinfo)
        {
            string strResult = ""; string dbName = ""; var id = "";
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    var userSql = "Select dbname from login_cred where username = '" + userinfo.Createdby + "'";
                    sqlCommand.CommandText = userSql;
                    sqlCommand.CommandType = CommandType.Text;
                    conn.Open();
                    var dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.Read())
                    {
                        dbName = dataReader[0].ToString();
                    }
                    dataReader.Close();
                    conn.Close();

                    sqlCommand.CommandText = "amics_sp_api_maintain_secuser";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();                   

                    sqlCommand.Parameters.Add(new SqlParameter("@actionflag", userinfo.ActionFlag));
                    sqlCommand.Parameters.Add(new SqlParameter("@id", userinfo.Id));
                    sqlCommand.Parameters.Add(new SqlParameter("@userid", string.IsNullOrWhiteSpace(userinfo.UserId) ? string.Empty : userinfo.UserId));
                    sqlCommand.Parameters.Add(new SqlParameter("@firstname", string.IsNullOrWhiteSpace(userinfo.FirstName) ? string.Empty : userinfo.FirstName));
                    sqlCommand.Parameters.Add(new SqlParameter("@lastname", string.IsNullOrWhiteSpace(userinfo.LastName) ? string.Empty : userinfo.LastName));                    
                    sqlCommand.Parameters.Add(new SqlParameter("@password", string.IsNullOrWhiteSpace(userinfo.Password) ? string.Empty : userinfo.Password));
                    sqlCommand.Parameters.Add(new SqlParameter("@warehouse", string.IsNullOrWhiteSpace(userinfo.Warehouse) ? string.Empty : userinfo.Warehouse));
                    sqlCommand.Parameters.Add(new SqlParameter("@email", string.IsNullOrWhiteSpace(userinfo.Email) ? string.Empty : userinfo.Email));
                    sqlCommand.Parameters.Add(new SqlParameter("@buyer", userinfo.Buyer));
                    sqlCommand.Parameters.Add(new SqlParameter("@salesperson", userinfo.SalesPerson));
                    sqlCommand.Parameters.Add(new SqlParameter("@webaccess", userinfo.WebAccess));
                    sqlCommand.Parameters.Add(new SqlParameter("@signature", string.IsNullOrWhiteSpace(userinfo.Signature) ? string.Empty : userinfo.Signature));
                    sqlCommand.Parameters.Add(new SqlParameter("@employee", userinfo.EmpList));
                    sqlCommand.Parameters.Add(new SqlParameter("@user",userinfo.AmicsUser));
                    sqlCommand.Parameters.Add(new SqlParameter("@forgotpwdans", string.IsNullOrWhiteSpace(userinfo.Forgotpwdans) ? string.Empty : userinfo.Forgotpwdans));
                    sqlCommand.Parameters.Add(new SqlParameter("@dbName", string.IsNullOrWhiteSpace(dbName) ? string.Empty : dbName));
                    sqlCommand.ExecuteNonQuery();

                    if (userinfo.ActionFlag == 1 || userinfo.ActionFlag == 2)
                        strResult = "Successfully Saved";
                    else
                        strResult = "Successfully deleted";
                    
                    sqlCommand.Dispose();

                    var command = _amicsDbContext.Database.GetDbConnection().CreateCommand();
                    command.CommandText = "select id from sec_users where userid='" + userinfo.UserId.Trim() + "'";
                    var dreader = command.ExecuteReader();
                   
                    if (dreader.Read())
                    {
                        id = dataReader["id"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    strResult = ex.Message;
                }
                finally
                {
                    sqlCommand.Dispose();
                    conn.Close();
                }
            }
            return new LstMessage { Message = id };
        }

        /// <summary>
        /// API Service to deletes userid's existing user access then add module access in the sec_users_access table
        /// </summary>
        /// <param name="lstUseraccess">User Access</param>           
        public LstMessage AddSecUserAccessDetails(List<LstUserAccess> userAccess)
        {
            string strResult = ""; 
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var command = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    var userSql = "delete from sec_users_access where users_id='" + userAccess[0].UserId + "'";
                    command.CommandText = userSql;
                    command.CommandType = CommandType.Text;
                    conn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();


                    for (int i = 0; i < userAccess.Count; i++)
                    {
                        using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
                        {
                            sqlCommand.CommandText = "amics_sp_api_maintain_secuseraccess";
                            sqlCommand.CommandType = CommandType.StoredProcedure;

                            sqlCommand.Parameters.Add(new SqlParameter("@actionflag", userAccess[i].ActionFlag));
                            sqlCommand.Parameters.Add(new SqlParameter("@id", userAccess[i].Id));
                            sqlCommand.Parameters.Add(new SqlParameter("@userid", userAccess[i].UserId));
                            sqlCommand.Parameters.Add(new SqlParameter("@accessid", userAccess[i].AccessId));
                            sqlCommand.Parameters.Add(new SqlParameter("@readonly", userAccess[i].ReadOnly));
                            sqlCommand.Parameters.Add(new SqlParameter("@onthefly", userAccess[i].OnTheFly));
                            sqlCommand.Parameters.Add(new SqlParameter("@createdby", userAccess[i].Createdby));
                            sqlCommand.ExecuteNonQuery();
                            sqlCommand.Dispose();
                        }
                    }          
                    strResult = "Sucessfully Saved";
                    
                }
                catch (Exception ex)
                {
                    strResult = ex.Message;
                }
                finally
                {                  
                    conn.Close();
                }
            }
            return new LstMessage { Message = strResult };
        }

        /// <summary>
        /// API Service to deletes userid's existing warehouse access then add access in the 
        /// sec_users_warehouses table
        /// </summary>
        /// <param name="LstAccessWarehouse">User Warehouse Access</param>          
        public LstMessage AddSecWhAccessDetails(List<LstAccessWarehouse> whAccess)
        {
            string strResult = ""; var id = "";
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var command = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    var userSql = "delete from sec_users_warehouses where users_id='" + whAccess[0].UserId + "'";
                    command.CommandText = userSql;
                    command.CommandType = CommandType.Text;
                    conn.Open();
                    command.ExecuteNonQuery();
                    command.Dispose();

                    for (int i = 0; i < whAccess.Count; i++)
                    {
                        using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
                        {
                            sqlCommand.CommandText = "amics_sp_api_maintain_secwarehouseaccess";
                            sqlCommand.CommandType = CommandType.StoredProcedure;

                            sqlCommand.Parameters.Add(new SqlParameter("@actionflag", whAccess[i].ActionFlag));
                            sqlCommand.Parameters.Add(new SqlParameter("@id", whAccess[i].Id));
                            sqlCommand.Parameters.Add(new SqlParameter("@userid", whAccess[i].UserId));
                            sqlCommand.Parameters.Add(new SqlParameter("@warehouseid", whAccess[i].WarehouseId));
                            sqlCommand.Parameters.Add(new SqlParameter("@readonly", whAccess[i].ReadOnly));
                            sqlCommand.Parameters.Add(new SqlParameter("@createdby", whAccess[i].Createdby));
                            sqlCommand.ExecuteNonQuery();
                            sqlCommand.Dispose();
                        }
                    }
                    strResult = "Sucessfully Saved";
                }
                catch (Exception ex)
                {
                    strResult = ex.Message;
                }
                finally
                {                    
                    conn.Close();
                }
            }
            return new LstMessage { Message = strResult };
        }

        /// <summary>
        /// API Service to get sec_users_access.readonly access for passing userid and accessnumber. If readonly access is 0, then user cannot access the screen/page
        /// </summary>
        /// <param name="userId">userId</param> 
        /// <param name="accessNum">access Number</param> 
        public LstMessage GetUserAccessByAccessNum(string userId, string accessNum)
        {
            List<LstUserAccess> lstUserAccess = new List<LstUserAccess>();
            var secUserAccess = "";
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_get_secuseraccessnum";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlCommand.Parameters.Add(new SqlParameter("@userId", string.IsNullOrWhiteSpace(userId) ? string.Empty : userId));
                    sqlCommand.Parameters.Add(new SqlParameter("@accessNum", string.IsNullOrWhiteSpace(accessNum) ? string.Empty : accessNum));
                    var dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.Read())
                    {
                        secUserAccess = dataReader["readonly"].ToString();
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
            return new LstMessage { Message = secUserAccess };
        }
    }
}
