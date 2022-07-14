using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using Aims.Core.Models;
using Aims.PartMaster.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Aims.Core.Services;

namespace Aims.Core.Services
{  
    public interface IBulkTransferService {
        string ValidateLocation(string warehouse, string location);
        List<LstBulkTransfer> BulkTransferItemDetails(string warehouse, string location);
        string ExecuteBulkTransfer(LstBulkTransferUpdate lstBulkTransUpdate);
    }
    public class BulkTransferService: IBulkTransferService
    {
        private readonly AmicsDbContext _amicsDbContext;       
        Utility util = new Utility();       
       
        public BulkTransferService(AmicsDbContext amicsDbContext)
        {
            _amicsDbContext = amicsDbContext;            
        }

        /// <summary>
        /// API Service to check valid warehouse and location and returns locationid to get item details 
        /// </summary>
        /// <param name="warehouse">warehouse</param>          
        /// <param name="location">location</param>          
        public string ValidateLocation(string warehouse, string location)
        {
            string strLocationId = "";
           

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    string strQuery = "select dbo.fn_warehouseid('" + warehouse + "','" + location + "')";
                    sqlCommand.CommandText = strQuery;
                    sqlCommand.CommandType = CommandType.Text;
                    conn.Open();
                    var dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.Read())
                    {
                        strLocationId = dataReader[0].ToString();
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
            return strLocationId;
        }

        /// <summary>
        /// API Service to check valid warehouse and location and returns locationid to get item details 
        /// </summary>
        /// <param name="warehouse">warehouse</param>          
        /// <param name="location">location</param>                  
        public List<LstBulkTransfer> BulkTransferItemDetails(string warehouse, string location)
        {            
            List<LstBulkTransfer> lstBulkTransfer = new List<LstBulkTransfer>();
            int nQty = 0;
            string strQty = string.Empty;
            strQty = util.ReturnZeros(2);           

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {                    
                    sqlCommand.CommandText = "sp_bulk_transfer_view5";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlCommand.Parameters.Add(new SqlParameter("@warehouse", string.IsNullOrWhiteSpace(warehouse) ? string.Empty : warehouse));
                    sqlCommand.Parameters.Add(new SqlParameter("@location", string.IsNullOrWhiteSpace(location) ? string.Empty : location));
                    var dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        LstBulkTransfer bulkTransfer = new LstBulkTransfer();
                        bulkTransfer.ItemNumber = dataReader["itemnumber"].ToString();
                        bulkTransfer.Rev = dataReader["rev"].ToString();
                        bulkTransfer.Description = dataReader["description"].ToString();
                        //bulkTransfer.Quantity = dataReader["Quantity"].ToString();
                        bulkTransfer.Quantity = String.Format("{0:0." + strQty + "}", dataReader["Quantity"]);
                        lstBulkTransfer.Add(bulkTransfer);
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
            return lstBulkTransfer.ToList();
        }

        /// <summary>
        /// API Service to transfer item details from warehouse, location to warehouse location
        /// </summary>
        /// <param name="LstBulkTransferUpdate">LstBulkTransferUpdate</param>          
        public string ExecuteBulkTransfer(LstBulkTransferUpdate lstBulkTransUpdate)
        {
            string strResult = "";
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "sp_bulk_transfer5";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlCommand.Parameters.Add(new SqlParameter("@fromwh", string.IsNullOrWhiteSpace(lstBulkTransUpdate.WarehouseFrom) ? string.Empty : lstBulkTransUpdate.WarehouseFrom));
                    sqlCommand.Parameters.Add(new SqlParameter("@fromloc", string.IsNullOrWhiteSpace(lstBulkTransUpdate.LocationFrom) ? string.Empty : lstBulkTransUpdate.LocationFrom));
                    sqlCommand.Parameters.Add(new SqlParameter("@towh", string.IsNullOrWhiteSpace(lstBulkTransUpdate.WarehouseTo) ? string.Empty : lstBulkTransUpdate.WarehouseTo));
                    sqlCommand.Parameters.Add(new SqlParameter("@toloc", string.IsNullOrWhiteSpace(lstBulkTransUpdate.LocationTo) ? string.Empty : lstBulkTransUpdate.LocationTo));
                    sqlCommand.Parameters.Add(new SqlParameter("@bulk_xfr_user", string.IsNullOrWhiteSpace(lstBulkTransUpdate.UserName) ? string.Empty : lstBulkTransUpdate.UserName));
                    sqlCommand.ExecuteNonQuery();
                    strResult = "Successfully Transfered";

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
            return strResult;
        }
    }
}
