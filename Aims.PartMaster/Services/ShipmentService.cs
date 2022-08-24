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
using Microsoft.AspNetCore.Mvc;

namespace Aims.Core.Services
{
    public interface IShipmentService
    {
        List<LstChangeLocSearch> ShipmentSearch(string projectId, string projectName, string er, string budget, string user);
        List<LstChangeLocSearch> ShipmentViewByProjER(string projectId, string somain);
        List<LstChangeLocSearch> ShipmentViewDetails(string soMain, string itemnumber, string userId, string soLineId, string invType);
        List<LstChangeLocSearch> ShipmentViewSelectedDetails(string itemsId, string username, string solinesid, string invType);
        LstMessage DeleteInvPickShip(string userName);
        LstMessage UpdateDelInvPickShip(List<LstChgLocTransItems> lstchgloc);
        LstMessage UpdateShipment(string userName, string mdatout);
    }

    public class ShipmentService : IShipmentService
    {
        private readonly AmicsDbContext _amicsDbContext;
        private ILogger<ShipmentService> _logger;
        Utility util = new Utility();
        public ShipmentService(AmicsDbContext aimsDbContext, ILogger<ShipmentService> logger)
        {
            _amicsDbContext = aimsDbContext;
            _logger = logger;
        }

        /// <summary>
        /// API Service to get search result from given Project Id/Project Name/ER/Budet/User
        /// </summary>
        /// <param name="projectId">Project Id</param>        
        /// <param name="projectName">Project Name</param>        
        /// <param name="er">ER</param>        
        /// <param name="budget">Budget Authority</param>        
        /// <param name="user">Username</param>        
        public List<LstChangeLocSearch> ShipmentSearch(string projectId, string projectName, string er, string budget, string user)
        {
            List<LstChangeLocSearch> lstChangeLocSearch = new List<LstChangeLocSearch>();

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_search_project_er";
                    conn.Open();

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@projectid", string.IsNullOrWhiteSpace(projectId) ? string.Empty : projectId));
                    sqlCommand.Parameters.Add(new SqlParameter("@projectname", string.IsNullOrWhiteSpace(projectName) ? string.Empty : projectName));
                    sqlCommand.Parameters.Add(new SqlParameter("@er", string.IsNullOrWhiteSpace(er) ? string.Empty : er));
                    sqlCommand.Parameters.Add(new SqlParameter("@budgetauthority", string.IsNullOrWhiteSpace(budget) ? string.Empty : budget));
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
        public List<LstChangeLocSearch> ShipmentViewByProjER(string projectId, string somain)
        {
            List<LstChangeLocSearch> lstChangeLoc = new List<LstChangeLocSearch>();
            var strInvType = string.Empty;
            string strQty = util.ReturnZeros(2);

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    if (projectId != null)
                    {
                        sqlCommand.CommandText = "amics_sp_api_view_proj_items";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@projectid", projectId.Trim()));
                    }
                    else
                    {
                        sqlCommand.CommandText = "amics_sp_api_view_er_items";
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
        /// API Service to change location pick quantity details for Basic/Serial
        /// </summary>
        /// <param name="soMain">SO Main</param>        
        /// <param name="itemnumber">Item Number</param>        
        /// <param name="userId">User Id</param>        
        /// <param name="soLineId">SO LineId</param> 
        public List<LstChangeLocSearch> ShipmentViewDetails(string soMain, string itemnumber, string userId, string soLineId, string invType)
        {
            List<LstChangeLocSearch> lstChangeLocSerial = new List<LstChangeLocSearch>();
            string strCurr = util.ReturnZeros(2);
            string strQty = util.ReturnZeros(2);

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {

                    sqlCommand.CommandText = "amics_sp_api_inv_somain_details_items_transfer_ship";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@so", soMain.Trim()));
                    sqlCommand.Parameters.Add(new SqlParameter("@item", itemnumber.Trim()));
                    sqlCommand.Parameters.Add(new SqlParameter("@user", userId.Trim()));
                    sqlCommand.Parameters.Add(new SqlParameter("@solinesid", soLineId.Trim()));
                    sqlCommand.Parameters.Add(new SqlParameter("@screen", "Shipment"));

                    conn.Open();
                    var dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        LstChangeLocSearch changeLoc = new LstChangeLocSearch();
                        if (invType == "SERIAL")
                        {
                            changeLoc.Warehouse = dataReader["wh"].ToString();
                            changeLoc.Location = dataReader["loc"].ToString();
                            changeLoc.SerNo = dataReader["serno"].ToString();
                            changeLoc.TagNo = dataReader["tagno"].ToString();
                            changeLoc.InvSerialId = dataReader["serid"].ToString();
                            changeLoc.Quantity = String.Format("{0:0." + strQty + "}", dataReader["quantity"]);
                            lstChangeLocSerial.Add(changeLoc);
                        }
                        else
                        {
                            changeLoc.Warehouse = dataReader["wh"].ToString();
                            changeLoc.Location = dataReader["loc"].ToString();
                            changeLoc.InvBasicId = dataReader["basid"].ToString();
                            changeLoc.Quantity = String.Format("{0:0." + strQty + "}", dataReader["quantity"]);
                            lstChangeLocSerial.Add(changeLoc);
                        }

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
            return lstChangeLocSerial.ToList();
        }

        /// <summary>
        /// API Service to select picked Basic/Serial items and populate it in the right table
        /// </summary>
        /// <param name="itemsId">itemsId</param>    
        /// <param name="username">UserName</param>    
        /// <param name="solinesId">solinesId</param>    
        /// <param name="invType">invType</param>
        public List<LstChangeLocSearch> ShipmentViewSelectedDetails(string itemsId, string username, string solinesid, string invType)
        {
            List<LstChangeLocSearch> lstChangeLocSelItems = new List<LstChangeLocSearch>();
            string strCurr = util.ReturnZeros(2);
            string strQty = util.ReturnZeros(2);

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    // sqlCommand.CommandText = "select * from inv_transfer_location where createdby='" + username + "'";
                    if (invType == "SERIAL")
                        sqlCommand.CommandText = "amics_sp_api_shipment_serial_selecteditems";
                    else
                        sqlCommand.CommandText = "amics_sp_api_shipment_basic_selecteditems";

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@itemsId", itemsId.Trim()));
                    sqlCommand.Parameters.Add(new SqlParameter("@userId", username.Trim()));
                    sqlCommand.Parameters.Add(new SqlParameter("@soLinesId", solinesid.Trim()));
                    conn.Open();
                    var dataReader = sqlCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        LstChangeLocSearch changeLoc = new LstChangeLocSearch();
                        if (invType == "SERIAL")
                        {
                            changeLoc.Warehouse = dataReader["warehouse"].ToString();
                            changeLoc.Location = dataReader["location"].ToString();
                            changeLoc.SerNo = dataReader["serno"].ToString();
                            changeLoc.TagNo = dataReader["tagno"].ToString();
                            changeLoc.InvSerialId = dataReader["invserialid"].ToString();
                            changeLoc.Quantity = String.Format("{0:0." + strQty + "}", dataReader["quantity"]);
                            lstChangeLocSelItems.Add(changeLoc);
                        }
                        else
                        {
                            changeLoc.Warehouse = dataReader["warehouse"].ToString();
                            changeLoc.Location = dataReader["location"].ToString();
                            changeLoc.InvBasicId = dataReader["invbasicid"].ToString();
                            changeLoc.Quantity = String.Format("{0:0." + strQty + "}", dataReader["quantity"]);
                            lstChangeLocSelItems.Add(changeLoc);
                        }
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
            return lstChangeLocSelItems.ToList();
        }

        /// <summary>
        /// API Service to clear the data in the table inv_pick_ship on page load.
        /// </summary>
        /// <param name="userName">UserName</param>                
        public LstMessage DeleteInvPickShip(string userName)
        {
            var conn = (SqlConnection)_amicsDbContext.Database.GetDbConnection();
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_shipment_invshippickdelete";
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


        /// <summary>
        /// API Service to insert the picked items data in the translog/translogsn/inv_soship/inv_pick tables and update qty 
        /// for corresponding invbasic/invserial tables
        /// This method InvPickShipCount() gets count from the table inv_pick_ship for username
        /// This SP amics_sp_api_shipment_ship used to ship the items.
        /// </summary>
        /// <param name="userName">UserName</param>   
        /// <param name="mdatout">To mdatout</param>         
        public LstMessage UpdateShipment(string userName, string mdatout)
        {
            int nCount = InvPickShipCount(userName);
            string packlstNum = "";

            if (nCount == 0)
                return new LstMessage() { Message = "No data" };

            List<LstChangeLocSearch> lstChangeLocSerial = new List<LstChangeLocSearch>();
            string strCurr = util.ReturnZeros(2);
            string strQty = util.ReturnZeros(2);

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    conn.Open();
                    sqlCommand.CommandText = "amics_sp_api_shipment_ship";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@user", userName.Trim()));
                    sqlCommand.Parameters.Add(new SqlParameter("@mdatout", mdatout.Trim()));
                    var dataReader = sqlCommand.ExecuteReader();

                    if (dataReader.Read())
                    {
                        packlstNum = dataReader[0].ToString();
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
            return new LstMessage() { Message = packlstNum };
        }

        /// This method InvPickShipCount() gets count from the table inv_pick_ship for passing parameter 'username'        
        private int InvPickShipCount(string userName)
        {
            int dataExistCnt = 0;

            var conn = (SqlConnection)_amicsDbContext.Database.GetDbConnection();
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    // sqlCommand.CommandText = "select COUNT(*) from inv_pick_ship where createdby='" + userName + "'";
                    sqlCommand.CommandText = "amics_sp_api_shipment_pickshipcntchk";
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

        /// <summary>
        /// API Route Controller to check pick items exist in inv_pick_ship table and also checks available quantity
        /// in the inv_serial/inv_basic table, if not exists, update invserialid/invbasicid, transqty, solinesid details into inv_pick_ship table
        /// </summary>
        /// <param name="LstChgLocTransItems">LstChgLocTransItems</param>                         
        public LstMessage UpdateDelInvPickShip(List<LstChgLocTransItems> lstchgloc)
        {
            var id = "";
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
                            sqlCommand.CommandText = "amics_sp_api_shipment_pickship_update";
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.Parameters.Add(new SqlParameter("@id", null));
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
                        var qtyRes = PickedDataCheckInvPickShip(lstchgloc[i].SoLinesId, lstchgloc[i].InvSerialId, lstchgloc[i].InvBasicId, lstchgloc[i].AvailQuantity);

                        var splitval = qtyRes.Split("&&");
                        var qtyReturn = splitval[0].ToString();
                        var userName = splitval[1].ToString();

                        if (qtyReturn == "0")  //quanity
                        {
                            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
                            {
                                sqlCommand.CommandText = "amics_sp_api_shipment_pickship_update";
                                sqlCommand.CommandType = CommandType.StoredProcedure;
                                sqlCommand.Parameters.Add(new SqlParameter("@id", null));
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
                            return new LstMessage() { Message = "This line is not available, it is being updated by user " + userName };
                        }
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
            }
            return new LstMessage() { Message = "Success" };
        }

        public string PickedDataCheckInvPickShip(string soLinesId, string invSerialId, string invBasicId, int availQty)
        {
            int quantity = 0;
            string createdBy = "";

            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_shipment_pickqty_toship";
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
    }
}
