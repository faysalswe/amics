using Aims.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Aims.PartMaster.Models;

namespace Aims.Core.Services
{
    public interface ISalesOrderService {
        List<LstSoMain> SalesOrderSearchOnOpen();
        List<LstSoMain> SalesOrderSearch(string soMain, string status, string requestor, string projectId, string projectName, string mdatIn);
        LstSoMain SalesOrderMainView(string soMainId);
        List<LstSoLines> SoLinesView(string soMain);
        string ValidateSO(string Project, string Status, string Shipvia);
        LstMessage UpdateSoMain(LstSoMain somain);
        LstMessage UpdateSoLines(List<LstSoLines> solines);
        List<LstSoLines> LoadProjTransferView(string soMain);
        List<LstChangeLocSearch> ProjTransferInvTypeView(string soMain, string itemNumber, string userId);
        LstMessage ProjTransferUpdate(List<LstProjectTransData> ProjectTransferData);       
        List<LstInvSerLot> ViewFromProjTransfer(string soMain);
        List<LstInvSerLot> ViewToProjTransfer(string soMain);
        List<LstInvSerLot> ViewProjTransferRowClicked(string itemsid, string somainid, string tosomainid, string invtype, string itemno, string transoption);
        List<LstShipments> GetSoShipments(string soMainId);
        string ValidatePackListNumber(string packListNumber);
        List<LstPoOnSo> GetSoPurchaseOrder(string SoMainId);
        List<LstSoLines> CreatePOfmSO(string SoMain, string level);
        List<LstDocumentFields> GetDocuments(string SoMainId);
    }

    public class SalesOrderService: ISalesOrderService
    {
        private readonly AmicsDbContext _amicsDbContext;     
        Utility util = new Utility();
        public SalesOrderService(AmicsDbContext aimsDbContext)
        {
            _amicsDbContext = aimsDbContext;       
        }

        /// <summary>
        /// API Service to search results of Sales Order on page load
        /// </summary>            
        public List<LstSoMain> SalesOrderSearchOnOpen()
        {
            List<LstSoMain> lstSoMain = new List<LstSoMain>();

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_search_SoOnOpen";
                    conn.Open();

                    sqlCommand.CommandType = CommandType.StoredProcedure;                    
                    
                    var dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        LstSoMain soMain = new LstSoMain();
                        soMain.Id = dataReader["id"].ToString();
                        soMain.SoNumber = dataReader["somain"].ToString();
                        soMain.SoDate = dataReader["sodate"].ToString();
                        soMain.CustomerName = dataReader["customername"].ToString();
                        lstSoMain.Add(soMain);                        
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
            return lstSoMain.ToList();
        }

        /// <summary>
        /// API Service to get Somain, Sodate, Id & Customername details based on the search 
        /// <param name="soMain">soMain</param> 
        /// <param name="status">status</param> 
        /// <param name="requestor">requestor</param> 
        /// <param name="projectId">projectId</param> 
        /// <param name="projectName">projectName</param> 
        /// <param name="mdatIn">mdatIn</param> 
        /// </summary>            
        public List<LstSoMain> SalesOrderSearch(string soMain, string status, string requestor, string projectId, string projectName, string mdatIn)
        {
            List<LstSoMain> lstSoMain = new List<LstSoMain>();
            
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_search_so";
                    conn.Open();

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@somain", string.IsNullOrWhiteSpace(soMain) ? string.Empty : soMain));
                    sqlCommand.Parameters.Add(new SqlParameter("@user3", string.IsNullOrWhiteSpace(requestor) ? string.Empty : requestor));
                    sqlCommand.Parameters.Add(new SqlParameter("@status", string.IsNullOrWhiteSpace(status) ? string.Empty : status));
                    sqlCommand.Parameters.Add(new SqlParameter("@projectid", string.IsNullOrWhiteSpace(projectId) ? string.Empty : projectId));
                    sqlCommand.Parameters.Add(new SqlParameter("@projectname", string.IsNullOrWhiteSpace(projectName) ? string.Empty : projectName));
                    sqlCommand.Parameters.Add(new SqlParameter("@mdatIn", string.IsNullOrWhiteSpace(mdatIn) ? string.Empty : mdatIn)); 
                    var dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        LstSoMain somain = new LstSoMain();
                        somain.Id = dataReader["id"].ToString();
                        somain.SoNumber = dataReader["somain"].ToString();
                        somain.SoDate = dataReader["sodate"].ToString();
                        somain.CustomerName = dataReader["customername"].ToString();
                        lstSoMain.Add(somain);
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
            return lstSoMain.ToList();
        }

        /// <summary>
        /// API Service to get data for So Main(Parent form)
        /// <param name="soMainId">soMainId</param>       
        /// </summary>            
        public LstSoMain SalesOrderMainView(string soMainId)
        {
            LstSoMain somain = new LstSoMain();
            string strCurr = util.ReturnZeros(2);
            string strQty = util.ReturnZeros(2);

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_view_somain";
                    conn.Open();

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@soMainId", soMainId));
                    var dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.Read())
                    {
                        somain.Id = dataReader["id"].ToString();
                        somain.CustomerId = dataReader["customerid"].ToString();
                        somain.SoNumber = dataReader["somain"].ToString();
                        somain.CustomerSmallName = dataReader["smallname"].ToString();
                        somain.SoDate = dataReader["sodate"].ToString();
                        somain.SalesPerson = dataReader["salesperson"].ToString();
                        somain.ShipVia = dataReader["shipvia"].ToString();
                        somain.FOB = dataReader["fob"].ToString();
                        somain.CustPo = dataReader["custpo"].ToString();
                        somain.CustomerName = dataReader["customername"].ToString();
                        somain.OrderType = dataReader["ordertype"].ToString();
                        somain.Status = dataReader["status"].ToString();
                        somain.Terms = dataReader["terms"].ToString();
                        somain.User1 = dataReader["user1"].ToString();
                        somain.User2 = dataReader["user2"].ToString();
                        somain.User3 = dataReader["user3"].ToString();
                        somain.User4 = dataReader["user4"].ToString();
                        somain.User5 = dataReader["user5"].ToString();
                        somain.User6 = dataReader["user6"].ToString();
                        somain.User9 = dataReader["user7"].ToString();
                        somain.User10 = dataReader["user8"].ToString();

                        somain.BillToName = dataReader["billtoname"].ToString();
                        somain.BillToAddress1 = dataReader["billtoaddress1"].ToString();
                        somain.BillToAddress2 = dataReader["billtoaddress2"].ToString();
                        somain.BillToAddress3 = dataReader["billtoaddress3"].ToString();
                        somain.BillToAddress4 = dataReader["billtoaddress4"].ToString();
                        somain.BillToAddress5 = dataReader["billtoaddress5"].ToString();
                        somain.BillToAddress6 = dataReader["billtoaddress6"].ToString();

                        somain.ShipToName = dataReader["shiptoname"].ToString();
                        somain.ShipToAddress1 = dataReader["shiptoaddress1"].ToString();
                        somain.ShipToAddress2 = dataReader["shiptoaddress2"].ToString();
                        somain.ShipToAddress3 = dataReader["shiptoaddress3"].ToString();
                        somain.ShipToAddress4 = dataReader["shiptoaddress4"].ToString();
                        somain.ShipToAddress5 = dataReader["shiptoaddress5"].ToString();
                        somain.ShipToAddress6 = dataReader["shiptoaddress6"].ToString();

                        somain.SoNotes = dataReader["sonotes"].ToString();
                        somain.ShipNotes = dataReader["shipnotes"].ToString();
                        somain.InvoiceNotes = dataReader["invoicenotes"].ToString();

                        somain.AccMsg = dataReader["acctmsg"].ToString();

                        somain.ContactsId = dataReader["contactsid"].ToString();

                        somain.ContactName = dataReader["contactName"].ToString();
                        somain.ContactEmail = dataReader["contactEmail"].ToString();
                        somain.ContactPhone = dataReader["contactPhone"].ToString();
                        somain.ContactFax = dataReader["contactFax"].ToString();

                        somain.SalesTax = dataReader["salestax"].ToString();
                        somain.Commission = dataReader["commission"].ToString();
                        somain.Est = dataReader["est"].ToString();
                        somain.Reqdate = dataReader["reqdate"].ToString();
                        somain.Prcomp = dataReader["prcomp"].ToString();
                        somain.Funded = dataReader["funded"].ToString();
                        somain.ProjectId = dataReader["projectid"].ToString();
                        somain.ProjectName = dataReader["projectName"].ToString();
                        somain.ProjectUId = dataReader["projectUid"].ToString();
                        somain.contractNum = dataReader["contractnum"].ToString();

                        somain.Markup1 = dataReader["markup1"].ToString();
                        somain.Markup2 = dataReader["markup2"].ToString();

                        somain.Markup3 = String.Format("{0:0." + "0000" + "}", Convert.ToDecimal(somain.Markup1) * Convert.ToDecimal(somain.Markup2));

                        somain.Budget = String.Format("{0:0." + strCurr + "}", dataReader["budget"]);
                        somain.MdatIn = dataReader["mdat_in"].ToString();
                        somain.Org = dataReader["org"].ToString();

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
            return somain;
        }

        /// <summary>
        /// API Service to get So line items for Sales Order(Child form)
        /// <param name="soMain">soMain</param>        
        /// </summary>    
        public List<LstSoLines> SoLinesView(string soMain)
        {
            List<LstSoLines> lstSoLines = new List<LstSoLines>();
            string strCurr = util.ReturnZeros(2);
            string strQty = util.ReturnZeros(2);

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_view_solines";
                    conn.Open();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@somain", string.IsNullOrWhiteSpace(soMain) ? string.Empty : soMain));
                    var dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        LstSoLines solines = new LstSoLines();
                        solines.SoLineId = dataReader["id"].ToString();
                        solines.Line = Convert.ToInt16(dataReader["linenum"]);
                        solines.ItemsId = dataReader["itemsid"].ToString();
                        solines.ItemNumber = dataReader["itemnumber"].ToString();
                        solines.Rev = dataReader["revision"].ToString();
                        solines.Description = dataReader["description"].ToString();

                        solines.Qty = dataReader["quantity"].ToString();
                        solines.ShippedQty = dataReader["pickedQty"].ToString();
                        solines.Uom = dataReader["uom"].ToString();

                        solines.Cost = Convert.ToDouble(dataReader["COST_U"]);
                        solines.ExtCost = Convert.ToDouble(dataReader["extCost"]);

                        solines.CostStr = String.Format("{0:0." + strCurr + "}", dataReader["POCOST_U"]);
                        solines.CostStr1 = String.Format("{0:0." + strCurr + "}", dataReader["COST_U"]);
                        solines.CostStr2 = String.Format("{0:0." + strCurr + "}", dataReader["COST_L"]);
                        solines.CostStr4 = String.Format("{0:0." + strCurr + "}", dataReader["POCOST_L"]);

                        solines.ExtCostStr = String.Format("{0:0." + strCurr + "}", dataReader["extCost"]);
                        solines.QtyStr = String.Format("{0:0." + strQty + "}", dataReader["quantity"]);
                        solines.ShippedQtyStr = String.Format("{0:0." + strQty + "}", dataReader["pickedQty"]);

                        solines.MarkupStr = String.Format("{0:0." + strQty + "}", dataReader["markup"]);

                        solines.ItemType = dataReader["itemtype"].ToString();

                        solines.User1 = dataReader["user1"].ToString();
                        solines.EstShipDate = dataReader["estshipdate"].ToString();
                        solines.ShipDate = dataReader["shipdate"].ToString();
                        solines.WarrantyYears = dataReader["warranty_years"].ToString();
                        solines.WarrantyDays = dataReader["warranty_days"].ToString();
                        solines.Markup = Convert.ToDouble(dataReader["markup"]);
                        solines.SoLinesNotes = dataReader["solinesnotes"].ToString();
                        solines.CustomersItemsId = dataReader["customerItem"].ToString();
                        solines.Obsolete = dataReader["obsolete"].ToString();
                        lstSoLines.Add(solines);
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
            return lstSoLines.ToList();
        }

        /// This method ValidateSO() calls on click buttone Save, validating Project Id, Statuss and Ship via
        public string ValidateSO(string Project, string Status, string Shipvia)
        {
            string strMsgResult = string.Empty;

            var conn = (SqlConnection)_amicsDbContext.Database.GetDbConnection();
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    // sqlCommand.CommandText = "select COUNT(*) from inv_transfer_location where createdby='" + userName + "'";
                    sqlCommand.CommandText = "amics_sp_api_validate_so";
                    sqlCommand.Parameters.Add(new SqlParameter("@Project", Project.Trim()));
                    sqlCommand.Parameters.Add(new SqlParameter("@Status", Status.Trim()));
                    sqlCommand.Parameters.Add(new SqlParameter("@Shipvia", Shipvia.Trim()));
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    var dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        strMsgResult += dataReader["results"].ToString() + ",";

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
            return strMsgResult;
        }

        /// <summary>
        /// API Service for Add/Update/Delete of SoMain details
        /// Insert/Update the So Main details in the so_main table. If id is null, data will be added in the table.         
        /// </summary>
        /// <param name="LstSoMain">SoMain Details</param>     
        /// /// <param name="userName">userName </param>     
        public LstMessage UpdateSoMain(LstSoMain somain)
        {
            //string validateMsg= ValidateSO(somain.ProjectId, somain.Status, somain.ShipVia);

            int actionFlag = 0;
            if (string.IsNullOrEmpty(somain.Id) || somain.Id == "00000000-0000-0000-0000-000000000000")
                actionFlag = 1;
            else
                actionFlag = 2;

            var soMainId = (string.IsNullOrEmpty(somain.Id)) ? Guid.Empty.ToString() : somain.Id;
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                sqlCommand.CommandText = "amics_sp_api_maintain_so";
                conn.Open();

                sqlCommand.CommandType = CommandType.StoredProcedure;

                if (string.IsNullOrEmpty(somain.Id) || somain.Id == "00000000-0000-0000-0000-000000000000")
                {
                    actionFlag = 1;
                    sqlCommand.Parameters.Add(new SqlParameter("@actionflag", actionFlag));
                    sqlCommand.Parameters.Add(new SqlParameter("@id", null));
                }
                else
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@actionflag", somain.ActionFlag));
                    sqlCommand.Parameters.Add(new SqlParameter("@id", somain.Id));
                }

                sqlCommand.Parameters.Add(new SqlParameter("@somain", somain.SoNumber));
                sqlCommand.Parameters.Add(new SqlParameter("@sodate", somain.SoDate));
                sqlCommand.Parameters.Add(new SqlParameter("@custpo", somain.CustPo));
                sqlCommand.Parameters.Add(new SqlParameter("@salesperson", somain.SalesPerson));

                if (somain.SalesTax != "")
                    sqlCommand.Parameters.Add(new SqlParameter("@salestax", somain.SalesTax));

                if (somain.Commission != "")
                    sqlCommand.Parameters.Add(new SqlParameter("@commission", somain.Commission));

                sqlCommand.Parameters.Add(new SqlParameter("@customername", somain.CustomerName));
                sqlCommand.Parameters.Add(new SqlParameter("@status", somain.Status));
                sqlCommand.Parameters.Add(new SqlParameter("@terms", somain.Terms));
                sqlCommand.Parameters.Add(new SqlParameter("@fob", somain.FOB));
                sqlCommand.Parameters.Add(new SqlParameter("@shipvia", somain.ShipVia));
                sqlCommand.Parameters.Add(new SqlParameter("@ordertype", somain.OrderType));
                sqlCommand.Parameters.Add(new SqlParameter("@billtoname", somain.BillToName));
                sqlCommand.Parameters.Add(new SqlParameter("@billtoaddress1", somain.BillToAddress1));
                sqlCommand.Parameters.Add(new SqlParameter("@billtoaddress2", somain.BillToAddress2));
                sqlCommand.Parameters.Add(new SqlParameter("@billtoaddress3", somain.BillToAddress3));
                sqlCommand.Parameters.Add(new SqlParameter("@billtoaddress4", somain.BillToAddress4));
                sqlCommand.Parameters.Add(new SqlParameter("@billtoaddress5", somain.BillToAddress5));
                sqlCommand.Parameters.Add(new SqlParameter("@billtoaddress6", somain.BillToAddress6));
                sqlCommand.Parameters.Add(new SqlParameter("@shiptoname", somain.ShipToName));
                sqlCommand.Parameters.Add(new SqlParameter("@shiptoaddress1", somain.ShipToAddress1));
                sqlCommand.Parameters.Add(new SqlParameter("@shiptoaddress2", somain.ShipToAddress2));
                sqlCommand.Parameters.Add(new SqlParameter("@shiptoaddress3", somain.ShipToAddress3));
                sqlCommand.Parameters.Add(new SqlParameter("@shiptoaddress4", somain.ShipToAddress4));
                sqlCommand.Parameters.Add(new SqlParameter("@shiptoaddress5", somain.ShipToAddress5));
                sqlCommand.Parameters.Add(new SqlParameter("@shiptoaddress6", somain.ShipToAddress6));

                if (somain.ContactsId != "")
                    sqlCommand.Parameters.Add(new SqlParameter("@Contactsid", somain.ContactsId));

                sqlCommand.Parameters.Add(new SqlParameter("@sonotes", somain.SoNotes));
                sqlCommand.Parameters.Add(new SqlParameter("@shipnotes", somain.ShipNotes));
                sqlCommand.Parameters.Add(new SqlParameter("@salespersonnotes", somain.SalesPersonNotes));
                sqlCommand.Parameters.Add(new SqlParameter("@packnotes", somain.PackNotes));
                sqlCommand.Parameters.Add(new SqlParameter("@invoicenotes", somain.InvoiceNotes));

                sqlCommand.Parameters.Add(new SqlParameter("@user1", somain.User1));
                sqlCommand.Parameters.Add(new SqlParameter("@user2", somain.User2));
                sqlCommand.Parameters.Add(new SqlParameter("@user3", somain.User3));
                sqlCommand.Parameters.Add(new SqlParameter("@user4", somain.User4));
                sqlCommand.Parameters.Add(new SqlParameter("@user5", somain.User5));
                sqlCommand.Parameters.Add(new SqlParameter("@user6", somain.User6));
                sqlCommand.Parameters.Add(new SqlParameter("@user7", somain.User9));
                sqlCommand.Parameters.Add(new SqlParameter("@user8", somain.User10));
                sqlCommand.Parameters.Add(new SqlParameter("@est", somain.Est));
                sqlCommand.Parameters.Add(new SqlParameter("@reqdate", somain.Reqdate));
                sqlCommand.Parameters.Add(new SqlParameter("@prcomp", somain.Prcomp));
                sqlCommand.Parameters.Add(new SqlParameter("@funded", somain.Funded));
                sqlCommand.Parameters.Add(new SqlParameter("@projectid", somain.ProjectUId));
                sqlCommand.Parameters.Add(new SqlParameter("@budget", somain.Budget));
                sqlCommand.Parameters.Add(new SqlParameter("@createdby", "admin"));
                sqlCommand.Parameters.Add(new SqlParameter("@mdatIn", somain.MdatIn));
                sqlCommand.Parameters.Add(new SqlParameter("@org", somain.Org));              
                var dataReader = sqlCommand.ExecuteReader();
                if (dataReader.Read())
                {
                    soMainId = dataReader["id"].ToString();
                }

                conn.Close();
            }
            return new LstMessage() { Message = soMainId };
        }

        /// <summary>
        /// API Service for Add/Update of Item details
        /// Insert/Update the Item details from the Parent form into list_items table. If id is null, data will be added in the table. 
        /// Data will be updated if id is not null. 
        /// </summary>
        /// <param name="LstSoLines">Item Details</param>         
        // public LstMessage UpdateSoLines(List<LstSoLines> solines, string userName)
        public LstMessage UpdateSoLines(List<LstSoLines> solines)
        {

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            {
                for (int i = 0; i < solines.Count; i++)
                {
                    using (var command = _amicsDbContext.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = "amics_sp_api_maintain_solines";
                        command.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        try
                        {
                            command.Parameters.Add(new SqlParameter("@actionflag", solines[i].ActionFlag));
                            command.Parameters.Add(new SqlParameter("@somain", solines[i].SoNumber));
                            if (solines[i].CustomersItemsId != null && solines[i].CustomersItemsId != "")
                                command.Parameters.Add(new SqlParameter("@customersItem", solines[i].CustomersItemsId));

                            command.Parameters.Add(new SqlParameter("@line", solines[i].Line));
                            command.Parameters.Add(new SqlParameter("@itemnumber", solines[i].ItemNumber));
                            command.Parameters.Add(new SqlParameter("@rev", solines[i].Rev));
                            command.Parameters.Add(new SqlParameter("@description", solines[i].Description));
                            command.Parameters.Add(new SqlParameter("@qty", solines[i].Qty));
                            command.Parameters.Add(new SqlParameter("@uom", solines[i].Uom));
                            command.Parameters.Add(new SqlParameter("@unitcost", solines[i].UnitCost));
                            command.Parameters.Add(new SqlParameter("@markup", solines[i].Markup));
                            command.Parameters.Add(new SqlParameter("@extcost", solines[i].ExtCost));

                            if (solines[i].WarrantyYears != null)
                                if (solines[i].WarrantyYears.Trim() != "")
                                    command.Parameters.Add(new SqlParameter("@estshipdate", solines[i].EstShipDate));
                                else
                                    command.Parameters.Add(new SqlParameter("@estshipdate", DBNull.Value));

                            if (solines[i].WarrantyDays != null)
                                if (solines[i].WarrantyDays.Trim() != "")
                                    command.Parameters.Add(new SqlParameter("@shipdate", solines[i].ShipDate));
                                else
                                    command.Parameters.Add(new SqlParameter("@shipdate", DBNull.Value));

                            command.Parameters.Add(new SqlParameter("@warrantyyear", solines[i].WarrantyYears));
                            command.Parameters.Add(new SqlParameter("@warrantydays", solines[i].WarrantyDays));
                            command.Parameters.Add(new SqlParameter("@solineid", solines[i].SoLineId));
                            command.Parameters.Add(new SqlParameter("@solinesnotes", solines[i].SoLinesNotes));
                            command.Parameters.Add(new SqlParameter("@user1", solines[i].User1));
                            command.Parameters.Add(new SqlParameter("@createdby", "admin"));
                            command.ExecuteNonQuery();
                            command.Dispose();
                        }
                        catch (Exception ex)
                        {                           
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
            return new LstMessage() { Message = "Successfully Saved" };
        }

        //---------------------------------- Create Project Transfer ------------------------------------------
        /// <summary>
        /// API Service to populate project transfer view for top grid of Sales Order - Create Project Transfer
        /// <param name="soMain">soMain</param>       
        /// </summary>            
        public List<LstSoLines> LoadProjTransferView(string soMain)
        {
            List<LstSoLines> lstSoLines = new List<LstSoLines>();
            string strCurr = util.ReturnZeros(2);
            string strQty = util.ReturnZeros(2);

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_so_lines_copy";
                    conn.Open();

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@somain", string.IsNullOrWhiteSpace(soMain) ? string.Empty : soMain));                    
                    var dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        LstSoLines soLines = new LstSoLines();
                        soLines.SoLineId = dataReader["id"].ToString();
                        soLines.Line = Convert.ToInt16(dataReader["linenum"]);
                        soLines.ItemsId = dataReader["itemsid"].ToString();
                        soLines.ItemNumber = dataReader["itemnumber"].ToString();
                        soLines.Rev = dataReader["revision"].ToString();
                        soLines.Description = dataReader["description"].ToString();

                        soLines.Qty = dataReader["quantity"].ToString();
                        soLines.ShippedQty = dataReader["pickedQty"].ToString();
                        soLines.Uom = dataReader["uom"].ToString();

                        soLines.Cost = Convert.ToDouble(dataReader["unitcost"]);
                        soLines.ExtCost = Convert.ToDouble(dataReader["extCost"]);

                        soLines.CostStr = String.Format("{0:0." + strCurr + "}", dataReader["unitcost"]);

                        soLines.CostStr1 = String.Format("{0:0." + strCurr + "}", dataReader["cost"]);
                        soLines.CostStr2 = String.Format("{0:0." + strCurr + "}", dataReader["pricemarkup"]);
                        soLines.CostStr4 = String.Format("{0:0." + strCurr + "}", dataReader["pmcostmarkup"]);

                        soLines.ExtCostStr = String.Format("{0:0." + strCurr + "}", dataReader["extCost"]);
                        soLines.QtyStr = String.Format("{0:0." + strQty + "}", dataReader["quantity"]);
                        soLines.ShippedQtyStr = String.Format("{0:0." + strQty + "}", dataReader["pickedQty"]);

                        soLines.AvailQty = String.Format("{0:0." + strQty + "}", dataReader["availqty"]);

                        soLines.MarkupStr = String.Format("{0:0." + strQty + "}", dataReader["markup"]);

                        soLines.ItemType = dataReader["itemtype"].ToString();

                        soLines.User1 = dataReader["user1"].ToString();
                        soLines.EstShipDate = dataReader["estshipdate"].ToString();
                        soLines.ShipDate = dataReader["shipdate"].ToString();
                        soLines.WarrantyYears = dataReader["warranty_years"].ToString();
                        soLines.WarrantyDays = dataReader["warranty_days"].ToString();
                        soLines.Markup = Convert.ToDouble(dataReader["markup"]);
                        soLines.SoLinesNotes = dataReader["solinesnotes"].ToString();
                        soLines.CustomersItemsId = dataReader["customerItem"].ToString();
                        soLines.Obsolete = dataReader["obsolete"].ToString();
                        lstSoLines.Add(soLines);
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
            return lstSoLines.ToList();
        }

        /// <summary>
        /// API Service to populate project transfer view for bottom grid of Sales Order - Create Project Transfer
        /// shows data based on the invtype Serial/Basic
        /// <param name="soMain">soMain</param>       
        /// </summary>            
        public List<LstChangeLocSearch> ProjTransferInvTypeView(string soMain, string itemNumber, string userId)
        {
            List<LstChangeLocSearch> lstInvSerLot = new List<LstChangeLocSearch>();
            string strCurr = util.ReturnZeros(2);
            string strQty = util.ReturnZeros(2);

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_inv_somain_details_items_combined";
                    conn.Open();

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@somain", string.IsNullOrWhiteSpace(soMain) ? string.Empty : soMain));
                    sqlCommand.Parameters.Add(new SqlParameter("@itemnumber", string.IsNullOrWhiteSpace(itemNumber) ? string.Empty : itemNumber));                   
                    var sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        LstChangeLocSearch invSerLot = new LstChangeLocSearch();
                        invSerLot.Warehouse = sqlDataReader["warehouse"].ToString();
                        invSerLot.Location = sqlDataReader["location"].ToString();
                        invSerLot.SerNo = sqlDataReader["serno"].ToString();
                        invSerLot.TagNo = sqlDataReader["tagno"].ToString();
                        invSerLot.InvSerialId = sqlDataReader["serialid"].ToString();

                        invSerLot.ItemType = sqlDataReader["itemtype"].ToString();
                        invSerLot.Itemnumber = sqlDataReader["itemnumber"].ToString();
                        //invSerLot.Quantity = Convert.ToDouble(sqlDataReader["Available"].ToString());
                        invSerLot.Quantity = sqlDataReader["Available"].ToString();
                        //  invSerLot.InvType = sqlDataReader["invtype"].ToString();

                        invSerLot.InvBasicId = sqlDataReader["basicid"].ToString();

                        invSerLot.SoMain = sqlDataReader["somain"].ToString();
                        invSerLot.SoLinesId = sqlDataReader["solinesid"].ToString();

                        invSerLot.LineNum = Convert.ToInt32(sqlDataReader["linenum"].ToString());
                   
                        if (sqlDataReader["sncost"] != DBNull.Value)
                            invSerLot.Cost = Convert.ToDouble(sqlDataReader["sncost"].ToString());

          
                        if (sqlDataReader["cost"] != DBNull.Value)
                            invSerLot.Cost = Convert.ToDouble(sqlDataReader["cost"].ToString());

                        lstInvSerLot.Add(invSerLot);

                    }
                    sqlDataReader.Close();
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
            return lstInvSerLot.ToList();
        }

        /// <summary>
        /// API Service to transfer items from Wareouse/location to warehouse/location
        /// This method is called on clicking Transfer button 
        /// Get picked items SoMain, From warehouse, To warehouse, From location, To location, invType, itemsId, transQty, etc. from bottom grid of Create Project Transfer
        /// In ItemsChangeLocationProjTrans, get location id, transNumner and insert data into inv_trans in InvtransUpdate method.
        /// Transfer project items using sp amics_sp_api_proj_transfer in ExecuteSpTransferProjTrans
        /// <param name="ProjectTransferData">ProjectTransferData</param>       
        /// </summary>            
        public LstMessage ProjTransferUpdate(List<LstProjectTransData> ProjectTransferData)
        {
            List<LstChangeLocView> lstchangeLocationView = new List<LstChangeLocView>();
            string strCurr = util.ReturnZeros(2);
            string strQty = util.ReturnZeros(2);
            
            try
            {
                foreach (LstProjectTransData element in ProjectTransferData)
                {                       
                    LstChangeLocView changeLocationView = new LstChangeLocView();

                    changeLocationView.ItemsId = element.ItemsId;
                    changeLocationView.Warehouse = element.Warehouse;
                    changeLocationView.Location = element.Location;
                    changeLocationView.SerialId = element.SerialId;
                    changeLocationView.BasicId = element.BasicId;
                    changeLocationView.TransactionQuantity = element.TransactionQuantity;
                    changeLocationView.InvType = element.InvType;
                    changeLocationView.Notes = "";
                    lstchangeLocationView.Add(changeLocationView);

                    ItemsChangeLocationProjTrans(element.ToWarehouse, element.ToLocation, lstchangeLocationView, element.userName, element.soMain, element.toSoMain, element.fromSoLine, element.toSoLine, element.toSolinesId);

                }                                   
            }
            catch (Exception ex)
            {
            } 
            return new LstMessage() { Message = "Successfully Saved" };         
        }


        public string ItemsChangeLocationProjTrans(string toWarehouse, string toLocation, List<LstChangeLocView> changeLocationView, string userName, string soMain, string toSoMain, string fromSoLine, string toSoLine, string toSolinesId)
        {
            TransNextNum trans = new TransNextNum();
            LstMessage msgModel = new LstMessage();
            string locId = "";
            int TransNum = 0;

            string fromLocationId = "", toLocationId = "", sameWarehouse = "", sameLocation = "", previousItemsID = "";
            var canSpExecute = 0; var spExecuted = 0;            
            toLocationId = Convert.ToString(GetLocationID(toWarehouse, toLocation));            
            ////trans.sp_rec = Convert.ToInt32(_inventoryService.TransNumberRec());
            TransNum = GetTransLogNum();

            for (int i = 0; i < changeLocationView.Count; i++)
            {

                if (previousItemsID != changeLocationView[i].ItemsId)
                {
                    if (previousItemsID != "")
                        canSpExecute = 1;

                    previousItemsID = changeLocationView[i].ItemsId;
                }

                if (canSpExecute == 1)
                {
                    ExecuteSpTransferProjTrans(TransNum, userName, soMain, toSoMain, fromSoLine, toSoLine, toSolinesId);
                    TransNum = GetTransLogNum();
                    canSpExecute = 0;
                    spExecuted = 1;
                }

                if (sameWarehouse != changeLocationView[i].Warehouse && sameLocation != changeLocationView[i].Location)
                    fromLocationId = GetLocationID(changeLocationView[i].Warehouse, changeLocationView[i].Location);

                if (changeLocationView[i].InvType.ToUpper() == "LOT")
                    InvTransUpdate(changeLocationView[i], TransNum, fromLocationId, toLocationId, userName, changeLocationView[i].InvType);
                else if (changeLocationView[i].InvType.ToUpper() == "SERIAL")
                    InvTransUpdate(changeLocationView[i], TransNum, fromLocationId, toLocationId, userName, changeLocationView[i].InvType);
                else
                    InvTransUpdate(changeLocationView[i], TransNum, fromLocationId, toLocationId, userName, changeLocationView[i].InvType);


                sameWarehouse = changeLocationView[i].Warehouse;
                sameLocation = changeLocationView[i].Location;

            }
            ExecuteSpTransferProjTrans(TransNum, userName, soMain, toSoMain, fromSoLine, toSoLine, toSolinesId);

            return "Successfully Saved";
        }

        public string GetLocationID(string warehouse, string location)
        {
            string locationID = ""; 

            var conn = (SqlConnection)_amicsDbContext.Database.GetDbConnection();
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "select dbo.amics_fn_api_warehouseid('" + warehouse + "','" + location + "')";
                    conn.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    var dataReader = sqlCommand.ExecuteReader();

                    if (dataReader.Read())
                    {
                        locationID = dataReader[0].ToString();
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
            return locationID;
        }

        private int GetTransLogNum()
        {
            int transNumber = 0;

            var conn = (SqlConnection)_amicsDbContext.Database.GetDbConnection();
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    conn.Open();
                    sqlCommand.CommandText = "amics_sp_api_get_transnum";
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


        private string GetItemsIdFromInvBasic(string invBasicId)
        {
            string itemsId = string.Empty;
            var conn = (SqlConnection)_amicsDbContext.Database.GetDbConnection();
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "select itemsid from inv_basic where id='" + invBasicId + "'";
                    conn.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    var sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.Read())
                    {
                        itemsId = sqlDataReader[0].ToString();
                    }
                    sqlCommand.Dispose();
                    sqlDataReader.Close();
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
            return itemsId;
        }
               
        public void InvTransUpdate(LstChangeLocView lstChangeLocationView, int transNumber, string fromLocationId, string toLocationId, string userName, string invType)
        {
            string itemsId = "";
            
            if (fromLocationId == null || fromLocationId == "" || fromLocationId == string.Empty)
                fromLocationId = lstChangeLocationView.LocationsId;

            if (invType.ToUpper() == "LOT")
                itemsId = GetItemsIdFromInvBasic(lstChangeLocationView.BasicId);


            var conn = (SqlConnection)_amicsDbContext.Database.GetDbConnection();
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {                   
                    if (invType.ToUpper() == "SERIAL")
                        sqlCommand.CommandText = "insert into inv_trans(source,invserialid,fromlocid,tolocid,transqty,createdby,transnum,notes) values ('LocXfer','" + lstChangeLocationView.SerialId + "','" + fromLocationId + "','" + toLocationId + "','" + lstChangeLocationView.TransactionQuantity + "','" + userName + "','" + transNumber + "','" + lstChangeLocationView.Notes + "')";
                    else if (invType.ToUpper() == "BASIC")
                        sqlCommand.CommandText = "insert into inv_trans(source,invbasicid,fromlocid,tolocid,transqty,createdby,transnum,notes) values ('LocXfer','" + lstChangeLocationView.BasicId + "','" + fromLocationId + "','" + toLocationId + "','" + lstChangeLocationView.TransactionQuantity + "','" + userName + "','" + transNumber + "','" + lstChangeLocationView.Notes + "')";
                    else
                        sqlCommand.CommandText = "insert into inv_trans(source,itemsid,invbasicid,fromlocid,tolocid,transqty,createdby,transnum,notes) values ('LocXfer','" + itemsId + "','" + lstChangeLocationView.BasicId + "','" + fromLocationId + "','" + toLocationId + "','" + lstChangeLocationView.TransactionQuantity + "','" + userName + "','" + transNumber + "','" + lstChangeLocationView.Notes + "')";

                    conn.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Dispose();
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
        }

        public void ExecuteSpTransferProjTrans(int transNumber, string userName, string soMain, string toSoMain, string fromSoLine, string toSoLine, string toSolinesId)
        {
            var conn = (SqlConnection)_amicsDbContext.Database.GetDbConnection();
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_proj_transfer"; //sp_proj_transfer5
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlCommand.Parameters.Add(new SqlParameter("@xfr_transnum", transNumber));
                    sqlCommand.Parameters.Add(new SqlParameter("@xfr_user", userName));
                    sqlCommand.Parameters.Add(new SqlParameter("@xfr_trans", "PROJ XFR"));
                    sqlCommand.Parameters.Add(new SqlParameter("@xfr_somain", soMain));
                    sqlCommand.Parameters.Add(new SqlParameter("@xfr_tosomain", toSoMain));
                    sqlCommand.Parameters.Add(new SqlParameter("@xfr_fromline", fromSoLine));
                    sqlCommand.Parameters.Add(new SqlParameter("@xfr_toline", toSoLine));
                    sqlCommand.Parameters.Add(new SqlParameter("@xfr_tosolinesid", toSolinesId));
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Dispose();
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
        }

        //---------------------------------- Create Project Transfer End ------------------------------------------

        //---------------------------------- View Project Transfer ------------------------------------------------


        /// <summary>
        /// API Service to view 'From project' data using parameters SoMain and Actionflag is 0
        /// To populate records for Top of the grid of View Project Transfer
        /// <param name="soMain">soMain</param>       
        /// </summary>            
        public List<LstInvSerLot> ViewFromProjTransfer(string soMain)
        {
            List<LstInvSerLot> lstInvSerLot = new List<LstInvSerLot>();

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_view_project_transfer"; //sp_proj_transfer5
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlCommand.Parameters.Add(new SqlParameter("@somain", soMain));
                    sqlCommand.Parameters.Add(new SqlParameter("@actionflag", 0));
                    var sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        LstInvSerLot invSerLot = new LstInvSerLot();

                        invSerLot.ItemNumber = sqlDataReader["Part Number"].ToString();
                        invSerLot.Description = sqlDataReader["description"].ToString();
                        invSerLot.SoNumber = sqlDataReader["To ER"].ToString();
                        invSerLot.ItemType = sqlDataReader["Mfr"].ToString();
                        invSerLot.Qtystring = sqlDataReader["Task Qty"].ToString();
                        invSerLot.CreateDate = sqlDataReader["Trans Date"].ToString();
                        invSerLot.FromER = sqlDataReader["From ER"].ToString();
                        invSerLot.ItemsId = sqlDataReader["itemsid"].ToString();
                        invSerLot.SomainId = sqlDataReader["somainid"].ToString();
                        invSerLot.ToSoMainId = sqlDataReader["tosomainid"].ToString();
                        invSerLot.InvType = sqlDataReader["invtype"].ToString();

                        lstInvSerLot.Add(invSerLot);
                    }
                    sqlCommand.Dispose();
                    sqlDataReader.Close();

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
            return lstInvSerLot.ToList();
        }

        /// <summary>
        /// API Service to view 'To project' data using parameters SoMain and Actionflag is 1
        /// To populate records for bottom of the grid of View Project Transfer
        /// <param name="soMain">soMain</param>       
        /// </summary>            
        public List<LstInvSerLot> ViewToProjTransfer(string soMain)
        {
            List<LstInvSerLot> lstInvSerLot = new List<LstInvSerLot>();

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_view_project_transfer"; //sp_proj_transfer5
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlCommand.Parameters.Add(new SqlParameter("@somain", soMain));
                    sqlCommand.Parameters.Add(new SqlParameter("@actionflag", 1));
                    var sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        LstInvSerLot invSerLot = new LstInvSerLot();

                        invSerLot.ItemNumber = sqlDataReader["Part Number"].ToString();
                        invSerLot.Description = sqlDataReader["description"].ToString();
                        invSerLot.SoNumber = sqlDataReader["To ER"].ToString();
                        invSerLot.ItemType = sqlDataReader["Mfr"].ToString();
                        invSerLot.Qtystring = sqlDataReader["Task Qty"].ToString();
                        invSerLot.CreateDate = sqlDataReader["Trans Date"].ToString();
                        invSerLot.FromER = sqlDataReader["From ER"].ToString();
                        invSerLot.ItemsId = sqlDataReader["itemsid"].ToString();
                        invSerLot.SomainId = sqlDataReader["somainid"].ToString();
                        invSerLot.ToSoMainId = sqlDataReader["tosomainid"].ToString();
                        invSerLot.InvType = sqlDataReader["invtype"].ToString();

                        lstInvSerLot.Add(invSerLot);
                    }
                    sqlCommand.Dispose();
                    sqlDataReader.Close();

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
            return lstInvSerLot.ToList();
        }
        /// <summary>
        /// API Service to populate records in the popup window on clicking the row 
        /// Click row from Top of the grid, transoption "ERIN"
        /// Click row from Top of the grid, transoption "EROUT"
        /// <param name="itemsid">itemsid</param>       
        /// <param name="somainid">somainid</param>       
        /// <param name="tosomainid">tosomainid</param>       
        /// <param name="invtype">invtype</param>       
        /// <param name="itemno">itemno</param>       
        /// <param name="transoption">transoption</param>       
        ///</summary>            
        public List<LstInvSerLot> ViewProjTransferRowClicked(string itemsid, string somainid, string tosomainid, string invtype, string itemno, string transoption)
        {
            List<LstInvSerLot> lstInvSerLot = new List<LstInvSerLot>();
            //LoadFromProject = "ERIN"
            //LoadToProject = "EROUT"
            string strCurr = util.ReturnZeros(2);
            string strQty = util.ReturnZeros(2);

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_view_project_transfer_details"; //sp_proj_transfer5
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlCommand.Parameters.Add(new SqlParameter("@itemsid", itemsid));                    ;
                    sqlCommand.Parameters.Add(new SqlParameter("@fromsomainid", somainid));
                    sqlCommand.Parameters.Add(new SqlParameter("@tosomainid", tosomainid));
                    sqlCommand.Parameters.Add(new SqlParameter("@tranoption", transoption));
                    sqlCommand.Parameters.Add(new SqlParameter("@itemtype", invtype));

                    var sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        LstInvSerLot invSerLot = new LstInvSerLot();
                        if (invtype == "SERIAL")
                        {
                            invSerLot.SerNo = sqlDataReader["serno"].ToString();
                            invSerLot.TagNo = sqlDataReader["tagno"].ToString();

                            invSerLot.Coststring = String.Format("{0:0." + strCurr + "}", sqlDataReader["cost"]);
                        }
                        else
                        {
                            invSerLot.Location = sqlDataReader["location"].ToString();
                            invSerLot.Qtystring = sqlDataReader["transqty"].ToString();


                            invSerLot.Coststring = String.Format("{0:0." + strCurr + "}", sqlDataReader["cost"]);
                        }
                        lstInvSerLot.Add(invSerLot);                        
                    }
                    sqlCommand.Dispose();
                    sqlDataReader.Close();

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
            return lstInvSerLot.ToList();
        }


        //---------------------------------- Sales Order - Option Shipments ------------------------------------------------------
        /// <summary>
        /// API Service to populate data in the SO Shipments grid      
        /// <param name="soMainId">soMainId</param>       
        /// </summary>            
        public List<LstShipments> GetSoShipments(string soMainId)
        {
            List<LstShipments> lstShipments = new List<LstShipments>();
            string strCurr = util.ReturnZeros(2);
            string strQty = util.ReturnZeros(2);

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_so_getshipments";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlCommand.Parameters.Add(new SqlParameter("@somain", soMainId));
                    var dataReader = sqlCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        LstShipments shipment = new LstShipments();

                        shipment.ItemNumber = dataReader["itemnumber"].ToString();
                        shipment.Rev = dataReader["revision"].ToString();
                        shipment.Description = dataReader["description"].ToString();
                        shipment.ShipDate = dataReader["shipdate"].ToString();
                        shipment.PackList = dataReader["packlist"].ToString();
                        shipment.CustPo = dataReader["custpo"].ToString();

                        if ((dataReader["shippedqty"] != DBNull.Value) || (dataReader["shippedqty"].ToString() != ""))
                        {
                            shipment.ShippedQuantity = String.Format("{0:0." + strQty + "}", dataReader["shippedqty"]);
                        }

                        shipment.EstDate = dataReader["estshipdate"].ToString();
                        shipment.ShippedBy = dataReader["createdby"].ToString();
                        shipment.MdatOut = dataReader["mdatout"].ToString();
                        lstShipments.Add(shipment);
                    }
                    sqlCommand.Dispose();
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
            return lstShipments.ToList();
        }


        /// <summary>
        /// API Service to validate packlist no on clicking VoidShip button, 
        /// it redirects to report page if packlist number exists        
        /// <param name="soMain">soMain</param>       
        /// </summary>            
        public string ValidatePackListNumber(string packListNumber)
        {
            string isValid = "0";

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_soshipment_validpacklstnum";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlCommand.Parameters.Add(new SqlParameter("@packlstnum", packListNumber));
                    var dataReader = sqlCommand.ExecuteReader();

                    if (dataReader.Read())
                    {
                        isValid = "1";
                    }
                    sqlCommand.Dispose();
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
            return isValid;
        }
        //---------------------------------- Sales Order - Option Purchase Order  ------------------------------------------------------

        /// <summary>
        /// API Service to populate purchase order details in SO         
        /// <param name="SoMainId">SoMainId</param>       
        /// </summary>            
        public List<LstPoOnSo> GetSoPurchaseOrder(string SoMainId)
        {
            string strCurr = util.ReturnZeros(2);
            string strQty = util.ReturnZeros(2);
            List<LstPoOnSo> lstPoOnSo = new List<LstPoOnSo>();

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_view_sopo";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlCommand.Parameters.Add(new SqlParameter("@somainid", SoMainId));
                    var dataReader = sqlCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        LstPoOnSo objPoOnSo = new LstPoOnSo();
                        objPoOnSo.dummy1 = dataReader["dummy1"].ToString();
                        objPoOnSo.dummy2 = dataReader["dummy2"].ToString();
                        objPoOnSo.ItemType = dataReader["itemtype"].ToString();
                        objPoOnSo.ItemNumber = dataReader["itemnumber"].ToString();
                        objPoOnSo.Desc = dataReader["description"].ToString();
                        objPoOnSo.PoMain = dataReader["pomain"].ToString();

                        if ((dataReader["Ordered"] != DBNull.Value) || (dataReader["Ordered"].ToString() != ""))
                        {
                            objPoOnSo.OrderdQty = String.Format("{0:0." + strQty + "}", dataReader["Ordered"]);
                        }
                        if ((dataReader["Received"] != DBNull.Value) || (dataReader["Received"].ToString() != ""))
                        {
                            objPoOnSo.ReceivedQty = String.Format("{0:0." + strQty + "}", dataReader["Received"]);
                        }

                        if ((dataReader["unitcost"] != DBNull.Value) || (dataReader["unitcost"].ToString() != ""))
                        {
                            objPoOnSo.UnitCost = String.Format("{0:0." + strCurr + "}", dataReader["unitcost"]);
                        }

                        if ((dataReader["extcost"] != DBNull.Value) || (dataReader["extcost"].ToString() != ""))
                        {
                            objPoOnSo.ExtCost = String.Format("{0:0." + strCurr + "}", dataReader["extcost"]);
                        }

                        if (dataReader["deliverydate"] != DBNull.Value)
                        {
                            objPoOnSo.DeliveryDate = String.Format("{0:MM/dd/yy}", Convert.ToDateTime((dataReader["deliverydate"])));
                        }
                        else
                            objPoOnSo.DeliveryDate = "";


                        objPoOnSo.InvType = dataReader["invtype"].ToString();

                        objPoOnSo.PoMainId = dataReader["pomainid"].ToString();
                        objPoOnSo.POLinesId = dataReader["polinesid"].ToString();
                        lstPoOnSo.Add(objPoOnSo);
                    }
                    sqlCommand.Dispose();
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
            return lstPoOnSo.ToList();
        }

        /// <summary>
        /// API Service to Create Requistion/PO, popup opens with level "use current level/lower level" on clicking "Create PO" button
        /// To populate data based on the selection of level 
        /// <param name="SoMainId">SoMainId</param>       
        /// </summary>            
        public List<LstSoLines> CreatePOfmSO(string SoMain, string level)
        {
            string strCurr = util.ReturnZeros(2);
            string strQty = util.ReturnZeros(2);
            List<LstSoLines> lstSoLines = new List<LstSoLines>();

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    if (level == "Current Level")
                    {
                        sqlCommand.CommandText = "amics_sp_api_view_sopocreate";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        sqlCommand.Parameters.Add(new SqlParameter("@somain", SoMain));
                        var dataReader = sqlCommand.ExecuteReader();

                        while (dataReader.Read())
                        {
                            LstSoLines solines = new LstSoLines();
                            solines.SoLineId = dataReader["id"].ToString();
                            solines.Line = Convert.ToInt16(dataReader["linenum"]);
                            solines.ItemsId = dataReader["itemsid"].ToString();
                            solines.ItemNumber = dataReader["itemnumber"].ToString();
                            solines.Rev = dataReader["revision"].ToString();
                            solines.Description = dataReader["description"].ToString();

                            solines.Qty = dataReader["quantity"].ToString();
                            solines.ShippedQty = dataReader["pickedQty"].ToString();
                            solines.Uom = dataReader["uom"].ToString();

                            solines.Cost = Convert.ToDouble(dataReader["unitcost"]);
                            solines.ExtCost = Convert.ToDouble(dataReader["extCost"]);

                            solines.CostStr = String.Format("{0:0." + strCurr + "}", dataReader["unitcost"]);

                            solines.CostStr1 = String.Format("{0:0." + strCurr + "}", dataReader["cost"]);
                            solines.CostStr2 = String.Format("{0:0." + strCurr + "}", dataReader["pricemarkup"]);
                            solines.CostStr4 = String.Format("{0:0." + strCurr + "}", dataReader["pmcostmarkup"]);

                            solines.ExtCostStr = String.Format("{0:0." + strCurr + "}", dataReader["extCost"]);
                            solines.QtyStr = String.Format("{0:0." + strQty + "}", dataReader["sopoqty"]);
                            solines.ShippedQtyStr = String.Format("{0:0." + strQty + "}", dataReader["pickedQty"]);

                            solines.MarkupStr = String.Format("{0:0." + strQty + "}", dataReader["markup"]);

                            solines.ItemType = dataReader["itemtype"].ToString();

                            solines.User1 = dataReader["user1"].ToString();
                            solines.EstShipDate = dataReader["estshipdate"].ToString();
                            solines.ShipDate = dataReader["shipdate"].ToString();
                            solines.WarrantyYears = dataReader["warranty_years"].ToString();
                            solines.WarrantyDays = dataReader["warranty_days"].ToString();
                            solines.Markup = Convert.ToDouble(dataReader["markup"]);
                            solines.SoLinesNotes = dataReader["solinesnotes"].ToString();
                            solines.CustomersItemsId = dataReader["customerItem"].ToString();
                            solines.Obsolete = dataReader["obsolete"].ToString();
                            lstSoLines.Add(solines);
                        }
                        dataReader.Close();
                    }
                    else
                    {
                        //Lower level
                        sqlCommand.CommandText = "amics_sp_api_summarized_bom_solines_po";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        sqlCommand.Parameters.Add(new SqlParameter("@somain", SoMain));
                        var dataReader = sqlCommand.ExecuteReader();

                        while (dataReader.Read())
                        {
                            LstSoLines solines = new LstSoLines();

                            solines.SoLineId = dataReader["childid"].ToString();
                            solines.ItemNumber = dataReader["itemnumber"].ToString();
                            solines.Rev = dataReader["rev"].ToString();
                            solines.Description = dataReader["description"].ToString();
                            solines.Qty = dataReader["totalqty"].ToString();
                            solines.Uom = dataReader["uom"].ToString();
                            solines.Cost = Convert.ToDouble(dataReader["unitcost"]);
                            solines.CostStr = String.Format("{0:0." + strCurr + "}", dataReader["unitcost"]);
                            solines.QtyStr = String.Format("{0:0." + strQty + "}", dataReader["totalqty"]);
                        }
                        dataReader.Close();
                    }
                    sqlCommand.Dispose();
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
            return lstSoLines.ToList();
        }

        //On clicking button CreatePO, it redirects to Maintain Purchase Order
        //---------------------------------- Sales Order - Option Documents ------------------------------------------------------
        /// <summary>
        /// API Service to get documents details
        /// <param name="SoMainId">SoMainId</param>       
        /// </summary>            
        public List<LstDocumentFields> GetDocuments(string SoMainId)
        {
            List<LstDocumentFields> lstDocumentField = new List<LstDocumentFields>();
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_list_documents";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlCommand.Parameters.Add(new SqlParameter("@somainid", SoMainId));
                    var dataReader = sqlCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        LstDocumentFields docField = new LstDocumentFields();

                        docField.Id = dataReader["id"].ToString();
                        docField.LineNumber = dataReader["linenum"].ToString();
                        docField.docName = dataReader["filename"].ToString();
                        docField.ParentId = dataReader["parentid"].ToString();

                        lstDocumentField.Add(docField);
                    }
                    sqlCommand.Dispose();
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
            return lstDocumentField.ToList();
        }

    }
}