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
    public interface IChangeLocService
    {
        List<LstChangeLocSearch> ChangeLocSearch(string projectId, string projectName, string er, string budget, string user);
        List<LstChangeLocSearch> ChangeLocationView(string projectId, string somain);
        List<LstChangeLocSearch> ChangeLocationSerialView(string soMain, string itemnumber, string userId, string soLineId);
        List<LstChangeLocSearch> ChangeLocationBasicView(string soMain, string itemnumber, string userId, string soLineId);
        LstMessage UpdateChangeLocation(string userName, string toWarehouse, string toLocation);      
        string UpdateInvTransLocation(List<LstChgLocTransItems> lstchgloc);
        LstMessage DeleteInvTransferLoc(string userName);
    }
    public class ChangeLocationService : IChangeLocService
    {
        private readonly AmicsDbContext _amicsDbContext;
        Utility util = new Utility();       
        public ChangeLocationService(AmicsDbContext aimsDbContext)
        {
            _amicsDbContext = aimsDbContext;
        }
        /// <summary>
        /// API Service to get search result from given Project Id/Project Name/ER/Budet/User
        /// </summary>
        /// <param name="projectId">Project Id</param>        
        /// <param name="projectName">Project Name</param>        
        /// <param name="er">ER</param>        
        /// <param name="budget">Budget Authority</param>        
        /// <param name="user">Username</param>        
        public List<LstChangeLocSearch> ChangeLocSearch(string projectId, string projectName, string er, string budget, string user)
        {           
            List<LstChangeLocSearch> lstChangeLocSearch = new List<LstChangeLocSearch>();
            
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "sp_search_project_er";
                    conn.Open();

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@projectid", projectId));
                    sqlCommand.Parameters.Add(new SqlParameter("@projectname", projectName));
                    sqlCommand.Parameters.Add(new SqlParameter("@er", er));
                    sqlCommand.Parameters.Add(new SqlParameter("@budgetauthority", budget));
                    sqlCommand.Parameters.Add(new SqlParameter("@user2", user));

                    var dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        LstChangeLocSearch changeLoc = new LstChangeLocSearch();

                        changeLoc.Project = dataReader["project"].ToString();
                        changeLoc.Name = dataReader["name"].ToString();
                        changeLoc.SoMain = dataReader["somain"].ToString();

                        lstChangeLocSearch.Add(changeLoc);
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
            return lstChangeLocSearch.ToList();
        }

        /// <summary>
        /// API Service to get SO with itemnum details for given parameter ProjectId or SOMain
        /// </summary>
        /// <param name="projectId">Project Id</param>        
        /// <param name="somain">SO Main</param>                
        public List<LstChangeLocSearch> ChangeLocationView(string projectId, string somain)
        {
            List<LstChangeLocSearch> lstChangeLoc = new List<LstChangeLocSearch>();
            var strInvType = string.Empty;

            string strCurr = util.ReturnZeros(2);
            string strQty = util.ReturnZeros(2);

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {

                    if (projectId != null)
                    {
                        sqlCommand.CommandText = "sp_view_proj_items";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@projectid", projectId.Trim()));
                    }
                    else
                    {
                        sqlCommand.CommandText = "sp_view_er_items";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@somain", somain.Trim()));
                    }
                    conn.Open();
                    var dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        LstChangeLocSearch changeLoc = new LstChangeLocSearch();

                        changeLoc.SoMain = dataReader["somain"].ToString();
                        changeLoc.LineNum = Convert.ToInt32(dataReader["linenum"]);
                        changeLoc.SoLinesId = dataReader["solinesid"].ToString();
                        changeLoc.Itemnumber = dataReader["itemnumber"].ToString();
                        changeLoc.Description = dataReader["description"].ToString();
                        changeLoc.ItemType = dataReader["itemtype"].ToString();
                        changeLoc.Quantity = String.Format("{0:0." + strQty + "}", dataReader["quantity"]);
                        changeLoc.ItemsId = dataReader["itemsid"].ToString();
                        changeLoc.InvType = dataReader["invtype"].ToString();
                        strInvType = dataReader["invtype"].ToString();
                        changeLoc.User = dataReader["user2"].ToString();

                        lstChangeLoc.Add(changeLoc);
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
            return lstChangeLoc.ToList();
        }
        /// <summary>
        /// API Service to get change location details for Serial
        /// </summary>
        /// <param name="soMain">SO Main</param>        
        /// <param name="itemnumber">Item Number</param>        
        /// <param name="userId">User Id</param>        
        /// <param name="soLineId">SO LineId</param> 
        public List<LstChangeLocSearch> ChangeLocationSerialView(string soMain, string itemnumber, string userId, string soLineId)
        {
            List<LstChangeLocSearch> lstChangeLocSerial = new List<LstChangeLocSearch>();
            string strCurr = util.ReturnZeros(2);
            string strQty = util.ReturnZeros(2);

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {

                    sqlCommand.CommandText = "sp_inv_somain_details_items_transfer_ship";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@so", soMain.Trim()));
                    sqlCommand.Parameters.Add(new SqlParameter("@item", itemnumber.Trim()));
                    sqlCommand.Parameters.Add(new SqlParameter("@user", userId.Trim()));
                    sqlCommand.Parameters.Add(new SqlParameter("@solinesid", soLineId.Trim()));

                    conn.Open();
                    var dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        LstChangeLocSearch changeLoc = new LstChangeLocSearch();
                        changeLoc.Warehouse = dataReader["wh"].ToString();
                        changeLoc.Location = dataReader["loc"].ToString();
                        changeLoc.SerNo = dataReader["serno"].ToString();
                        changeLoc.TagNo = dataReader["tagno"].ToString();                        
                        changeLoc.InvSerialId = dataReader["serid"].ToString();                        
                        changeLoc.Quantity = String.Format("{0:0." + strQty + "}", dataReader["quantity"]);
                        lstChangeLocSerial.Add(changeLoc);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return lstChangeLocSerial.ToList();
        }
        /// <summary>
        /// API Service to get change location details for Basic
        /// </summary>
        /// <param name="soMain">SO Main</param>        
        /// <param name="itemnumber">Item Number</param>        
        /// <param name="userId">User Id</param>        
        /// <param name="soLineId">SO LineId</param>                
        public List<LstChangeLocSearch> ChangeLocationBasicView(string soMain, string itemnumber, string userId, string soLineId)
        {
            List<LstChangeLocSearch> lstChangeLocSerial = new List<LstChangeLocSearch>();
            string strCurr = util.ReturnZeros(2);
            string strQty = util.ReturnZeros(2);

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "sp_inv_somain_details_items_transfer_ship";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@so", soMain.Trim()));
                    sqlCommand.Parameters.Add(new SqlParameter("@item", itemnumber.Trim()));
                    sqlCommand.Parameters.Add(new SqlParameter("@user", userId.Trim()));
                    sqlCommand.Parameters.Add(new SqlParameter("@solinesid", soLineId.Trim()));

                    conn.Open();
                    var dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        LstChangeLocSearch changeLoc = new LstChangeLocSearch();
                        changeLoc.Warehouse = dataReader["wh"].ToString();
                        changeLoc.Location = dataReader["loc"].ToString();
                        changeLoc.InvBasicId = dataReader["basid"].ToString();
                        changeLoc.Quantity = String.Format("{0:0." + strQty + "}", dataReader["quantity"]);
                        lstChangeLocSerial.Add(changeLoc);
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {

                }
                finally {                   
                    sqlCommand.Dispose();
                    conn.Close();
                }
            }
            return lstChangeLocSerial.ToList();
        }

        /// <summary>
        /// API Service to get change location details for Basic

        /// </summary>
        /// <param name="userName">SO Main</param>        
        /// <param name="toWarehouse">To Warehouse</param>                
        /// <param name="toLocation">To Location</param>        
        public string UpdateInvTransLocation(List<LstChgLocTransItems> lstchgloc)
        {            
            try
            {
                var con = (SqlConnection)_amicsDbContext.Database.GetDbConnection();
                con.Open();
                for (int i = 0; i < lstchgloc.Count; i++)
                {
                    if (lstchgloc[i].Action == 2)
                    {
                        using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
                        {
                            sqlCommand.CommandText = "amics_sp_chgloc_translocupdate";
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.Parameters.Add(new SqlParameter("@id", lstchgloc[i].Id));
                            sqlCommand.ExecuteNonQuery();
                        }                        
                    }
                    else
                    {
                        var qtyRes = PickDataCheckTransLocation(lstchgloc[i].SoLinesId, lstchgloc[i].InvSerialId, lstchgloc[i].InvBasicId, lstchgloc[i].AvailQuantity);

                        var splitval = qtyRes.Split("&&");
                        var qtyReturn = splitval[0].ToString();
                        var userName = splitval[1].ToString();
                        
                        if (qtyReturn == "0")  //quanity
                        {   
                            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
                            {                               
                                sqlCommand.CommandText = "amics_sp_chgloc_translocupdate";
                                sqlCommand.CommandType = CommandType.StoredProcedure;
                                sqlCommand.Parameters.Add(new SqlParameter("@id", lstchgloc[i].Id));
                                sqlCommand.Parameters.Add(new SqlParameter("@action", lstchgloc[i].Action));
                                sqlCommand.Parameters.Add(new SqlParameter("@solinesid", lstchgloc[i].SoLinesId));
                                sqlCommand.Parameters.Add(new SqlParameter("@quantity", lstchgloc[i].TransQuantity));
                                sqlCommand.Parameters.Add(new SqlParameter("@invserialid", lstchgloc[i].InvSerialId));
                                sqlCommand.Parameters.Add(new SqlParameter("@invbasicid", lstchgloc[i].InvBasicId));
                                sqlCommand.Parameters.Add(new SqlParameter("@createdby", lstchgloc[i].CreatedBy));
                                sqlCommand.ExecuteNonQuery();
                            }                            
                        }
                        else
                        {
                            return "This line is not available, it is being updated by user " + userName;
                        }
                    }
              }
              con.Close();                
            }
            catch (Exception ex) {
                return ex.Message;
            }            
            return "Success";
        }

        public string PickDataCheckTransLocation(string soLinesId,string invSerialId, string invBasicId, int availQty)
        {
            int quantity = 0;
            string createdBy = "";
           
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_chgloc_pickqty_transloc";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                         
                    sqlCommand.Parameters.Add(new SqlParameter("@solinesid", soLinesId));

                    if (invSerialId != "" && invSerialId != null)
                        sqlCommand.Parameters.Add(new SqlParameter("@invserialid", invSerialId));

                    if (invBasicId != "" && invBasicId != null)
                        sqlCommand.Parameters.Add(new SqlParameter("@invbasicid", invBasicId));

                    var dataReader = sqlCommand.ExecuteReader();

                    if (dataReader.Read())
                    {
                        createdBy = dataReader["createdby"].ToString();
                        quantity += Convert.ToInt32(dataReader[0]);
                        // break;
                    }

                    dataReader.NextResult();

                    if (dataReader.Read())
                    {
                        if (availQty != Convert.ToInt32(dataReader["quantity"]))
                            quantity += 1;

                        createdBy = dataReader["createdby"].ToString();                                
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {

                }
            }                
            return quantity + "&&" + createdBy;
        }

        /// <summary>
        /// API Service to transfer the pick items in translog and corresponding basic/serial tables
        /// This method ChangeLocationTransCount() gets count from the table inv_transfer_location for passing parameter 'username'
        /// This method GetTransLogNum() updates translognum+1 for translognum field in the table list_next_number and 
        /// fetch the translognum to insert into translog and update inv_transfer_location table
        /// Finally, calling this SP sp_essex_transfer5 to transfer the item, it updates the translog,invBasic/invSerial tables.
        /// This method GetTransDatefmTransNum() gets transaction date using translog number
        /// </summary>
        /// <param name="userName">SO Main</param>        
        /// <param name="toWarehouse">To Warehouse</param>                
        /// <param name="toLocation">To Location</param>        
        public LstMessage UpdateChangeLocation(string userName, string toWarehouse, string toLocation)
        {
            int nCount = ChangeLocationTransCount(userName);

            if (nCount == 0)
                return new LstMessage() { Message = "There is no data"};

            int transNum = GetTransLogNum();
            string transdate = "";

            List<LstChangeLocSearch> lstChangeLocSerial = new List<LstChangeLocSearch>();
            string strCurr = util.ReturnZeros(2);
            string strQty = util.ReturnZeros(2);
            int loopend = 0;

                using (var conn = _amicsDbContext.Database.GetDbConnection())
                using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
                {
                    try
                    {
                        conn.Open();
                        for (int i = 1; i <= nCount; i += 10)
                        {
                            var start = i;
                            var end = (i - 1) + 10;

                            loopend = end;

                            sqlCommand.CommandText = "sp_essex_transfer5";
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.Parameters.Add(new SqlParameter("@xfr_createdby", userName.Trim()));
                            sqlCommand.Parameters.Add(new SqlParameter("@toloc", toLocation.Trim()));
                            sqlCommand.Parameters.Add(new SqlParameter("@towh", toWarehouse.Trim()));
                            sqlCommand.Parameters.Add(new SqlParameter("@xfr_transnum", transNum));
                            sqlCommand.Parameters.Add(new SqlParameter("@counter_solines", start));
                            sqlCommand.Parameters.Add(new SqlParameter("@total_solines", end));
                            sqlCommand.ExecuteNonQuery();
                        }

                        if (loopend < nCount)
                        {
                            sqlCommand.CommandText = "sp_essex_transfer5";
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.Parameters.Add(new SqlParameter("@xfr_createdby", userName.Trim()));
                            sqlCommand.Parameters.Add(new SqlParameter("@toloc", toLocation.Trim()));
                            sqlCommand.Parameters.Add(new SqlParameter("@towh", toWarehouse.Trim()));
                            sqlCommand.Parameters.Add(new SqlParameter("@xfr_transnum", transNum));
                            sqlCommand.Parameters.Add(new SqlParameter("@counter_solines", loopend + 1));
                            sqlCommand.Parameters.Add(new SqlParameter("@total_solines", nCount));
                            sqlCommand.ExecuteNonQuery();
                        }

                        transdate = GetTransDatefmTransNum(transNum);

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
            return new LstMessage() { Message = "Success" + "&&" + transNum + "&&" + transdate };
        }
        
        /// This method ChangeLocationTransCount() gets count from the table inv_transfer_location for passing parameter 'username'        
        public int ChangeLocationTransCount(string userName)
        {
            int dataExistCnt = 0;

            var conn = (SqlConnection)_amicsDbContext.Database.GetDbConnection();          
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    // sqlCommand.CommandText = "select COUNT(*) from inv_transfer_location where createdby='" + userName + "'";
                    sqlCommand.CommandText = "amics_sp_chgloc_transcntchk";
                    sqlCommand.Parameters.Add(new SqlParameter("@createdby", userName.Trim()));
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    var dataReader = sqlCommand.ExecuteReader();

                    if (dataReader.Read())
                    {
                        dataExistCnt = Convert.ToInt32(dataReader[0].ToString());                 
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
            return dataExistCnt;
        }

        /// This method GetTransLogNum() updates translognum+1 for translognum field in the table list_next_number and
        /// fetch the translognum to insert into translog and update inv_transfer_location table
        public int GetTransLogNum()
        {
            int transNumber = 0;
                        
            var conn = (SqlConnection)_amicsDbContext.Database.GetDbConnection();
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    conn.Open();                    
                    sqlCommand.CommandText = "amics_sp_chgloc_listnextnumupd";                    
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    var sqlReader = sqlCommand.ExecuteReader();
                    if (sqlReader.Read())
                    {
                        transNumber = Convert.ToInt32(sqlReader[0].ToString());
                    }
                    sqlReader.Close();
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
            return transNumber;
        }

        public string GetTransDatefmTransNum(int transNumber)
        {
            string transDate = "";

           // var conn = (SqlConnection)_amicsDbContext.Database.GetDbConnection();
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {                    
                    sqlCommand.CommandText = "amics_sp_chgloc_transnumdate";
                    sqlCommand.Parameters.Add(new SqlParameter("@transnum", transNumber));
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    //conn.Open();
                    var dataReader = sqlCommand.ExecuteReader();

                    if (dataReader.Read())
                    {
                        transDate = dataReader[0].ToString();
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {

                }
                finally {
                    sqlCommand.Dispose();
                   // conn.Close();
                }
            }
            return transDate;
        }

        /// <summary>
        /// API Service to clear the data in the table inv_transfer_location         
        /// </summary>
        /// <param name="userName">SO Main</param>                 
        public LstMessage DeleteInvTransferLoc(string userName)
        {
            var conn = (SqlConnection)_amicsDbContext.Database.GetDbConnection();
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {                    
                    sqlCommand.CommandText = "amics_sp_chgloc_invtranlocdelete";
                    sqlCommand.Parameters.Add(new SqlParameter("@createdby", userName));
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlCommand.ExecuteNonQuery();
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
            return new LstMessage() { Message = "Successfully deleted" };
        }
    }
}
