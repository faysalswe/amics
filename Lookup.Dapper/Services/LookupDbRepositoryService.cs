using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Amics.Lookup.Models;

namespace Lookup.Dapper
{
    public interface ILookupDbRepositoryService
    {
        public (bool,string) ValidateUser(string userName, string encryptedPassword);
        public Task<string> GetDbName(string userName);
        public Task<ApplicationUser> GetUsersInfo(string userName);

    }
    public class LookupDbRepositoryService :ILookupDbRepositoryService
    {
        private readonly string _connectionString;
        private readonly ILogger<LookupDbRepositoryService> _logger;

        public LookupDbRepositoryService(IConfiguration configuration, ILogger<LookupDbRepositoryService> logger)
        {
            _connectionString = configuration.GetValue<string>("ConnectionStrings:LookUpDB"); 
            _logger = logger;
        }

        public (bool,string) ValidateUser(string userName, string encryptedPassword)
        {
            using IDbConnection conn = new SqlConnection(_connectionString);

            var userDbSql = $"Select dbname from login_cred where username = '{userName}' and password = (select dbo.amics_fn_encrypt('{encryptedPassword}'))";
            try
            {
                var result = conn.Query<string>(userDbSql);
                return result.ToList().Count >0 ? (true, result.First().ToString()): (false,string.Empty) ;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return (false, string.Empty);
            }
        }

        public async Task<string> GetDbName(string userName)
        {
            using IDbConnection conn = new SqlConnection(_connectionString);

            var userDbSql = $"Select dbname from login_cred where username = {userName})";
            try
            {
                var result = await conn.ExecuteScalarAsync<string>(userDbSql);
                return result.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return string.Empty;
            }
        }

        public async Task<ApplicationUser> GetUsersInfo(string userName)
        {

            ApplicationUser user = new ApplicationUser();

            try
            {
                using IDbConnection conn = new SqlConnection(_connectionString);

                string selectQuery = "select sec_users.*,list_warehouses.warehouse from sec_users left outer join list_warehouses on list_warehouses.id = sec_users.defaultwh where sec_users.userid='" + userName + "'";

                var result =  await conn.QueryFirstOrDefaultAsync<ApplicationUser>(selectQuery);
                return result;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return await Task.FromResult(user);
        }
    }
}
