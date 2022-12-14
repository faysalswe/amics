using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Aims.Core.Models;
using Aims.PartMaster.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Aims.Core.Services
{
    public interface IMdatService {
        List<LstMdat> MdatOutSearch(string mdatNum, string er, string packlistNum, string status);
        LstMdat MdatOutViewDetails(string mdatNum);
        LstMessage MdatOutUpdateDetails(LstMdat lstMdatOut);
        List<LstErLookup> SomainLookup(string searchSomain, string somainId, string statusId);

        List<LstMdatStatusLookup> StatusLookup(string searchStatus, string statusId);
    }
    public class MdatService: IMdatService
    {
        private readonly AmicsDbContext _amicsDbContext;
        public MdatService(AmicsDbContext amicsDbContext)
        {
            _amicsDbContext = amicsDbContext;
        }

        /// <summary>
        /// API Service to get Mdat search result from parameter mdatNum/er/packlistNum/status
        /// </summary>
        /// <param name="mdatNum">mdatNum</param>  
        /// <param name="er">er</param> 
        /// <param name="packlistNum">packlistNum</param> 
        /// <param name="status">status</param>         
        public List<LstMdat> MdatOutSearch(string mdatNum, string er, string packlistNum, string status)
        {
            List<LstMdat> lstMdatOut = new List<LstMdat>();

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_search_mdat_out";
                    conn.Open();

                    sqlCommand.CommandType = CommandType.StoredProcedure;                  
                    sqlCommand.Parameters.Add(new SqlParameter("@mdatNum", mdatNum));                    
                    sqlCommand.Parameters.Add(new SqlParameter("@er", er));
                    sqlCommand.Parameters.Add(new SqlParameter("@packlistNum", packlistNum));
                    sqlCommand.Parameters.Add(new SqlParameter("@status", status));

                    var dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        LstMdat mdat = new LstMdat();

                        mdat.Id = new Guid(dataReader["id"].ToString());
                        mdat.MdatNum = dataReader["mdat_num"].ToString();
                        mdat.Status = dataReader["status"].ToString();  
                        mdat.Description = dataReader["description"].ToString();
                        mdat.Submitted_date = dataReader["submitted_date"].ToString();
                        mdat.Approved_date = dataReader["approved_date"].ToString();
                        mdat.Shipped_date = dataReader["shipped_date"].ToString();

                        lstMdatOut.Add(mdat);
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
            return lstMdatOut.ToList();
        }

        /// <summary>
        /// API Service to get Mdat view details from parameter mdatNum/er/packlistNum/status
        /// </summary>
        /// <param name="mdatNum">mdatNum</param>  
        /// <param name="er">er</param> 
        /// <param name="packlistNum">packlistNum</param> 
        /// <param name="status">status</param>         
        public LstMdat MdatOutViewDetails(string mdatNum)
        {
            LstMdat lstMdatOut = new LstMdat();

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_view_mdat_out";
                    conn.Open();

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@mdatNum", string.IsNullOrWhiteSpace(mdatNum) ? string.Empty : mdatNum));
                    
                    var dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.Read())
                    {
                        lstMdatOut.Id = new Guid(dataReader["id"].ToString());
                        lstMdatOut.MdatNum = dataReader["mdat_num"].ToString();
                        lstMdatOut.Somain = dataReader["somain"].ToString();
                        if (dataReader["packlistnum"].ToString() != "")
                            lstMdatOut.Packlistnum = Convert.ToInt32(dataReader["packlistnum"]);
                        lstMdatOut.Description = dataReader["description"].ToString();
                        lstMdatOut.Status = dataReader["status"].ToString();
                        lstMdatOut.Submitted_date = dataReader["submitted_date"].ToString();
                        lstMdatOut.Approved_date = dataReader["approved_date"].ToString();
                        lstMdatOut.Shipped_date = dataReader["shipped_date"].ToString();
                        lstMdatOut.Cancelled_date = dataReader["cancelled_date"].ToString();

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
            return lstMdatOut;
        }

        /// <summary>
        /// API Service to insert/update/delete Mdat details in the table inv_mdat_out
        /// </summary>
        /// <param name="LstMdat">LstMdat</param>          
        public LstMessage MdatOutUpdateDetails(LstMdat lstMdatOut)
        {
            string strMsg = "";
           
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_maintain_mdatout";
                    conn.Open();

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@actionFlag", lstMdatOut.ActionFlag));
                    sqlCommand.Parameters.Add(new SqlParameter("@id", lstMdatOut.Id));
                    sqlCommand.Parameters.Add(new SqlParameter("@mdat_num", string.IsNullOrWhiteSpace(lstMdatOut.MdatNum) ? string.Empty : lstMdatOut.MdatNum));
                    sqlCommand.Parameters.Add(new SqlParameter("@somain", string.IsNullOrWhiteSpace(lstMdatOut.Somain) ? string.Empty : lstMdatOut.Somain));
                    sqlCommand.Parameters.Add(new SqlParameter("@description", string.IsNullOrWhiteSpace(lstMdatOut.Description) ? string.Empty : lstMdatOut.Description));
                    sqlCommand.Parameters.Add(new SqlParameter("@packlistnum", lstMdatOut.Packlistnum));
                    sqlCommand.Parameters.Add(new SqlParameter("@status", string.IsNullOrWhiteSpace(lstMdatOut.Status) ? string.Empty : lstMdatOut.Status));
                    sqlCommand.Parameters.Add(new SqlParameter("@submitted_date", string.IsNullOrWhiteSpace(lstMdatOut.Submitted_date) ? string.Empty : lstMdatOut.Submitted_date));
                    sqlCommand.Parameters.Add(new SqlParameter("@approved_date", string.IsNullOrWhiteSpace(lstMdatOut.Approved_date) ? string.Empty : lstMdatOut.Approved_date));
                    sqlCommand.Parameters.Add(new SqlParameter("@shipped_date", string.IsNullOrWhiteSpace(lstMdatOut.Shipped_date) ? string.Empty : lstMdatOut.Shipped_date));
                    sqlCommand.Parameters.Add(new SqlParameter("@cancelled_date", string.IsNullOrWhiteSpace(lstMdatOut.Cancelled_date) ? string.Empty : lstMdatOut.Cancelled_date));
                    sqlCommand.Parameters.Add(new SqlParameter("@createdBy", string.IsNullOrWhiteSpace(lstMdatOut.Createdby) ? string.Empty : lstMdatOut.Createdby));
                    sqlCommand.Parameters.Add(new SqlParameter("@shippingId", lstMdatOut.ShippingId));
                    sqlCommand.ExecuteNonQuery();
                    
                    if (lstMdatOut.ActionFlag == 3)
                        strMsg= "Successfully deleted";
                    else
                        strMsg = "Successfully Saved";

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
            return new LstMessage() { Message = strMsg }; ;
        }

        public List<LstErLookup> SomainLookup(string searchSomain, string somainId, string statusId)
        {
            var soId = string.IsNullOrEmpty(somainId) ? Guid.Empty : new Guid(somainId.ToString());
            var status = string.IsNullOrEmpty(statusId) ? Guid.Empty : new Guid(statusId.ToString());
            var name = string.IsNullOrEmpty(searchSomain) ? string.Empty : searchSomain;

            var whresult = _amicsDbContext.ListErLookup
                .FromSqlRaw($"exec amics_sp_api_somain_lookup @somainid ='{soId}',@somain = '{name}', @statusid = '{status}'")
                .ToList();

            return whresult;
        }

        public List<LstMdatStatusLookup> StatusLookup(string searchStatus, string statusId)
        {
            var stId = string.IsNullOrEmpty(statusId) ? Guid.Empty : new Guid(statusId.ToString());
            var name = string.IsNullOrEmpty(searchStatus) ? string.Empty : searchStatus;


            List<LstMdatStatusLookup> lstMdatStatus = new List<LstMdatStatusLookup>();

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_mdat_list_status_lookup";
                    conn.Open();

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@statusid", stId));
                    sqlCommand.Parameters.Add(new SqlParameter("@status", name));

                    var dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        LstMdatStatusLookup status = new LstMdatStatusLookup();

                        status.Id = new Guid(dataReader["id"].ToString());
                        status.Status = dataReader["status"].ToString();

                        lstMdatStatus.Add(status);
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
            return lstMdatStatus;

            //var whresult = _amicsDbContext.ListErLookup
            //    .FromSqlRaw($"exec amics_sp_api_mdat_list_status_lookup @statusid ='{stId}',@status = '{name}'")
            //    .ToList();

            //return whresult;
        }
    }
}
