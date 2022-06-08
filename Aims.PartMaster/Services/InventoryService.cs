using Aims.Core.Models;
using Aims.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Linq;
using Amics.Core.utils;
using Aims.PartMaster.Models;

namespace Aims.PartMaster.Services
{
    public interface IInventoryService
    {

        /// <summary>
        /// Interface for get item's status, Must pass ItemsGUID and UsersGUID as parameter
        /// </summary>
        /// <param name="ItemsId">The ItemsId of the data.</param>
        /// <param name="SecUserId">The SecUserId for the access of associated warehouse.</param>

        InvStatus InventoryStatus(string ItemsId, string SecUserId);


        /// <summary>
        /// Interface for get DefaultValues for the screen or the form, pass FormName as optional parameter.
        /// </summary>        
        /// <param name="FormName">string</param> 
        List<LstDefaultsValues> DefaultValues(string FormName);

        /// <summary>
        ///Interface for get sales order or ER number an item, must pass ItemsGUID, optional somain as parameters.
        /// </summary>        
        /// <param name="ItemsId">items guid</param> 
        /// <param name="SoMain">perfix so main</param> 

        List<LstErLookup> ErLookup(string ItemsId, string SoMain);


    }
    public class InventoryService : IInventoryService
    {
        private readonly AmicsDbContext _amicsDbContext;
        public InventoryService(AmicsDbContext aimsDbContext)
        {
            _amicsDbContext = aimsDbContext;
        }


        /// <summary>
        /// API Service for get item's status, Must pass ItemsGUID and UsersGUID as parameter
        /// </summary>
        /// <param name="ItemsId">The ItemsId of the data.</param>
        /// <param name="SecUserId">The SecUserId for the access of associated warehouse.</param>

        public InvStatus InventoryStatus(string ItemsId, string SecUserId)
        {
            var itemsGuId = string.IsNullOrEmpty(ItemsId) ? Guid.Empty : new Guid(ItemsId.ToString());
            var secUserGuId = string.IsNullOrEmpty(SecUserId) ? Guid.Empty : new Guid(SecUserId.ToString());
            var statusResult = _amicsDbContext.dbxInvStatus.FromSqlRaw($"select * from dbo.webapi_fn_inv_status ('{itemsGuId}','{secUserGuId}')").AsEnumerable().FirstOrDefault();
            
            return statusResult;
        }


        /// <summary>
        /// API Service for get DefaultValues for the screen or the form, pass FormName as optional parameter.
        /// </summary>        
        /// <param name="FormName">string</param> 
        public List<LstDefaultsValues> DefaultValues(string FormName)
        {            
          
            var defaustResult = _amicsDbContext.ListDefaultsValues
                .FromSqlRaw($"exec sp_webapi_get_list_defaults @formname='{FormName}'")
                .ToList();

            return defaustResult;
        }

        /// <summary>
        /// API Service for get sales order or ER number an item, must pass ItemsGUID, optional somain as parameters.
        /// </summary>        
        /// <param name="ItemsId">items guid</param> 
        /// <param name="SoMain">perfix so main</param> 

        public List<LstErLookup> ErLookup(string ItemsId, string SoMain)
        {
            var itemsGuId = string.IsNullOrEmpty(ItemsId) ? Guid.Empty : new Guid(ItemsId.ToString());
            var erResult = _amicsDbContext.ListErLookup
              .FromSqlRaw($"exec sp_webapi_get_er_by_pn @itemsid='{itemsGuId}',@somain='{SoMain}'")
              .ToList();
            return erResult;
        }

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public string GetSoLinesId(string itemNumber, string Rev, string soMain)
        //{
        //    string soLinesId = "";

        //    sqlConnection.Open();
        //    sqlCommand = new SqlCommand("select id from so_lines where somainid = (select id from so_main where somain=@soMain) and itemsid = (select id from list_items where itemnumber=@itemNumber and rev = @rev)", sqlConnection);
        //    sqlCommand.Parameters.Add("@itemNumber", itemNumber);
        //    sqlCommand.Parameters.Add("@rev", Rev);
        //    sqlCommand.Parameters.Add("@soMain", soMain);

        //    dataReader = sqlCommand.ExecuteReader();
        //    if (dataReader.Read())
        //    {
        //        soLinesId = dataReader["id"].ToString();
        //    }
        //    sqlConnection.Close();

        //    return soLinesId;
        //}

    }
}