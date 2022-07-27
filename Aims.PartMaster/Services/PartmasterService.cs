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

namespace Aims.Core.Services
{
    public interface IPartmasterService
    {
        LstItemDetails LoadPartmaster(string itemNumber, string rev);
        List<LstItemsBom> LoadItemsBom(string parentItemId);
        List<LstItemsPO> LoadItemsPO(string parentId);
        LstBomCount ItemsBomCount(string parentId);
        Task<List<string>> ItemNumDelete(string itemNo, string rev);
        Task<LstMessage> ItemNumDetailsAddUpdateAsync(LstItemDetails item);
        List<LstViewLocation> ViewLocationWarehouse(string itemsId, string secUsersId, string warehouse);
        Task<LstMessage> BomGridDetailsUpdation(List<LstBomGridItems> LstBomGridItems);
        List<LstInquiry> InquiryDetails(InquiryRequestDetails request);
        List<LstSerial> ViewSerial(string itemsId, string secUsersId, string warehouse, string serNo, string tagNo);
        List<LstNotes> ViewNotes(string parentId);
        LstMessage NotesUpdation(List<LstNotes> notesLst, string user);
        string ChangeSerialTag(LstChangeSerial lstChSerial);
    }

    public class PartmasterService : IPartmasterService
    {
        private readonly AmicsDbContext _amicsDbContext;
        private ILogger<PartmasterService> _logger;
        public PartmasterService(AmicsDbContext aimsDbContext, ILogger<PartmasterService> logger)
        {
            _amicsDbContext = aimsDbContext;
            _logger = logger;
        }

        /// <summary>
        /// API Service to load Partmaster details
        /// </summary>
        /// <param name="itemnumber">itemnumber</param>  
        /// <param name="rev">rev</param>    
        public LstItemDetails LoadPartmaster(string itemNumber, string rev)
        {
            var itemNum = string.IsNullOrEmpty(itemNumber) ? string.Empty : itemNumber;
            var revDef = string.IsNullOrEmpty(rev) ? "-" : rev;


            LstItemDetails itemDetails = new LstItemDetails();
            Utility util = new Utility();
            string strCurr = string.Empty;
            string strQty = string.Empty;

            strCurr = util.ReturnZeros(2);
            strQty = util.ReturnZeros(2);
          
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_load_partmaster";

                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    sqlCommand.Parameters.Add(new SqlParameter("@item", itemNum));
                    sqlCommand.Parameters.Add(new SqlParameter("@rev", revDef));

                    conn.Open();

                    var dataReader = sqlCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        //LstObjListItems ItemListings = new LstObjListItems();
                        itemDetails.Id = dataReader["id"].ToString().Trim();
                        itemDetails.ItemNumber = dataReader["itemnumber"].ToString().Trim();
                        itemDetails.Rev = dataReader["rev"].ToString().Trim();
                        itemDetails.DwgNo = dataReader["dwgno"].ToString().Trim();

                        itemDetails.Description = dataReader["description"].ToString().Trim();
                        itemDetails.SalesDescription = dataReader["SalesDescription"].ToString().Trim();
                        itemDetails.PurchaseDescription = dataReader["PurchaseDescription"].ToString().Trim();

                        itemDetails.ItemType = dataReader["itemtype"].ToString().Trim();
                        itemDetails.ItemClass = dataReader["itemclass"].ToString().Trim();
                        itemDetails.ItemCode = dataReader["itemcode"].ToString().Trim();

                        if ((dataReader["cost"] != DBNull.Value) || (dataReader["cost"].ToString() != ""))
                        {
                            itemDetails.Cost = Convert.ToDecimal(String.Format("{0:0." + strCurr + "}", dataReader["cost"]));
                        }
                        if ((dataReader["markup"] != DBNull.Value) || (dataReader["markup"].ToString() != ""))
                        {
                            itemDetails.Markup = Convert.ToDecimal(dataReader["markup"].ToString());
                        }
                        if ((dataReader["price"] != DBNull.Value) || (dataReader["price"].ToString() != ""))
                        {
                            itemDetails.Price = Convert.ToDecimal(String.Format("{0:0." + strCurr + "}", dataReader["price"]));
                        }
                        if ((dataReader["price2"] != DBNull.Value) || (dataReader["price2"].ToString() != ""))
                        {
                            itemDetails.Price2 = Convert.ToDecimal(dataReader["price2"].ToString());
                        }
                        if ((dataReader["price3"] != DBNull.Value) || (dataReader["price3"].ToString() != ""))
                        {
                            itemDetails.Price3 = Convert.ToDecimal(dataReader["price3"].ToString());
                        }
                        if ((dataReader["weight"] != DBNull.Value) || (dataReader["weight"].ToString() != ""))
                        {
                            itemDetails.Weight = Convert.ToDouble(dataReader["weight"].ToString());
                        }


                        itemDetails.Uomref = dataReader["uomref"].ToString().Trim();
                        itemDetails.Purchasing_Uom = dataReader["Purchasing_UOM"].ToString().Trim();

                        if ((dataReader["conversion"] != DBNull.Value) || (dataReader["conversion"].ToString() != ""))
                        {
                            itemDetails.Conversion = Convert.ToDecimal(dataReader["conversion"].ToString());
                        }

                        if ((dataReader["leadtime"] != DBNull.Value) || (dataReader["leadtime"].ToString() != ""))
                        {
                            itemDetails.LeadTime = Convert.ToDouble(dataReader["leadtime"].ToString());
                        }

                        if ((dataReader["minimum"] != DBNull.Value) || (dataReader["minimum"].ToString() != ""))
                        {
                            itemDetails.Minimum = Convert.ToDouble(dataReader["minimum"].ToString());
                        }

                        if ((dataReader["maximum"] != DBNull.Value) || (dataReader["maximum"].ToString() != ""))
                        {

                            itemDetails.Maximum = Convert.ToDouble(dataReader["maximum"].ToString());
                        }

                        itemDetails.GLSales = dataReader["glsales"].ToString().Trim();
                        itemDetails.GLInv = dataReader["glinv"].ToString().Trim();
                        itemDetails.GLCOGS = dataReader["glcogs"].ToString().Trim();

                        itemDetails.Warehouse = dataReader["warehouse"].ToString().Trim();
                        itemDetails.Location = dataReader["location"].ToString().Trim();
                        itemDetails.InvType = dataReader["InvType"].ToString().Trim();

                        if (dataReader["userbit"] != DBNull.Value)
                            itemDetails.UserBit = Convert.ToBoolean(dataReader["userbit"]);

                        if (dataReader["buyitem"] != DBNull.Value)
                            itemDetails.BuyItem = Convert.ToBoolean(dataReader["buyitem"]);

                        if (dataReader["obsolete"] != DBNull.Value)
                            itemDetails.Obsolete = Convert.ToBoolean(dataReader["obsolete"]);


                        //taa
                        if (dataReader["userbit"] != DBNull.Value)
                            itemDetails.UserBit = Convert.ToBoolean(dataReader["userbit"]);

                        //createpo
                        if (dataReader["userbit2"] != DBNull.Value)
                            itemDetails.UserBit2 = Convert.ToBoolean(dataReader["userbit2"]);


                        itemDetails.Quantity = Convert.ToDouble(String.Format("{0:0." + strQty + "}", dataReader["quantity"]));

                        itemDetails.Notes = dataReader["notes"].ToString().Trim();

                        itemDetails.User1 = dataReader["user1"].ToString().Trim();
                        itemDetails.User2 = dataReader["user2"].ToString().Trim();
                        itemDetails.User3 = Convert.ToDouble(dataReader["user3"].ToString().Trim());
                        itemDetails.User4 = dataReader["user4"].ToString().Trim();
                        itemDetails.User5 = dataReader["user5"].ToString().Trim();
                        itemDetails.User6 = dataReader["user6"].ToString().Trim();
                        itemDetails.User7 = dataReader["user7"].ToString().Trim();
                        itemDetails.User8 = dataReader["user8"].ToString().Trim();
                        itemDetails.User9 = dataReader["user9"].ToString().Trim();
                        itemDetails.User10 = dataReader["user10"].ToString().Trim();
                        itemDetails.User11 = dataReader["user11"].ToString().Trim();
                        itemDetails.User12 = dataReader["user12"].ToString().Trim();
                        itemDetails.User13 = dataReader["user13"].ToString().Trim();
                        itemDetails.User14 = dataReader["user14"].ToString().Trim();
                        itemDetails.User15 = dataReader["user15"].ToString().Trim();

                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"LoadPartmaster - {ex.Message} - {ex.StackTrace}");
                    //Log.ErrorLog(ex.Message, "Search : LoadPartmaster");
                }
                finally
                {
                    conn.Close();
                }
            }

            return itemDetails;
        }

        /// <summary>
        /// API Service to get BOM details
        /// </summary>
        /// <param name="parentId">Parent Item Id</param>          
        public List<LstItemsBom> LoadItemsBom(string parentId)
        {
            var itemsId = string.IsNullOrEmpty(parentId) ? Guid.Empty : new Guid(parentId.ToString());

            var bomItemresult = _amicsDbContext.LstItemsBom
                .FromSqlRaw($"exec amics_sp_api_view_items_bom @parentid ='{itemsId}'").ToList();

            return bomItemresult;
        }

        /// <summary>
        /// API Service to get PO details
        /// </summary>
        /// <param name="parentId">Parent Item Id</param>          
        public List<LstItemsPO> LoadItemsPO(string parentId)
        {
            var itemsId = string.IsNullOrEmpty(parentId) ? Guid.Empty : new Guid(parentId.ToString());

            var poItemresult = _amicsDbContext.LstItemsPO
                .FromSqlRaw($"exec amics_sp_api_view_items_po @parentid ='{itemsId}'").ToList();

            return poItemresult;
        }

        /// <summary>
        /// API Service to check whether Item BOM is exist or not in the Items Bom table for Copy to New functionality
        /// </summary>
        /// <param name="parentId">Parent Item Id</param>          
        public LstBomCount ItemsBomCount(string parentId)
        {
            var itemsId = string.IsNullOrEmpty(parentId) ? Guid.Empty : new Guid(parentId.ToString());

            var bomexist = _amicsDbContext.LstBomCount.FromSqlRaw($"exec amics_sp_api_itembom_exist @parentid ='{itemsId}'").AsEnumerable().FirstOrDefault();

            return bomexist;
        }

        /// <summary>
        /// API Route Controller to update column 'flag_delete' with 1 in the table list items for item number, can not access
        /// that item number. Item numnber cannot delete if it is used in some other tables.
        /// </summary>
        /// <param name="itemNo">Item Number</param> 
        /// <param name="Rev">Rev</param> 
        public async Task<List<string>> ItemNumDelete(string itemNo, string rev)
        {
            var itemNum = string.IsNullOrEmpty(itemNo) ? string.Empty : itemNo;
            var revDef = string.IsNullOrEmpty(rev) ? "-" : rev;
            List<string> messages = new List<string>();

            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_delete_list_items";
                    conn.Open();

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@item", itemNum));
                    sqlCommand.Parameters.Add(new SqlParameter("@rev", revDef));
                    var dataReader = await sqlCommand.ExecuteReaderAsync();

                    while (await dataReader.ReadAsync())
                    {
                        messages.Add(dataReader["message"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    //Log.ErrorLog(ex.Message, "Lookup : GetDeleteMessages");
                }
                finally
                {
                    conn.Close();
                }
            }
            return messages;
        }

        /// <summary>
        /// API Service for Add/Update of Item details
        /// Insert/Update the Item details from the Parent form into list_items table. If id is null, data will be added in the table. 
        /// Data will be updated if id is not null. 
        /// </summary>
        /// <param name="LstItemDetails">Item Details</param>         
        public async Task<LstMessage> ItemNumDetailsAddUpdateAsync(LstItemDetails item)
        {
            int actionFlag = 0;
            if (string.IsNullOrEmpty(item.Id) || item.Id== "00000000-0000-0000-0000-000000000000")
                actionFlag = 1;
            else
                actionFlag = 2;

            var itemsid = (string.IsNullOrEmpty(item.Id)) ? Guid.Empty.ToString() : item.Id;
            var uomid = (item.Uomid == null || item.Uomid == Guid.Empty) ? Guid.Empty : item.Uomid;
            var revDef = string.IsNullOrEmpty(item.Rev) ? "-" : item.Rev;
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var command = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "amics_sp_api_maintain_partmaster";
                conn.Open();

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@actionflag", actionFlag));
                command.Parameters.Add(new SqlParameter("@itemnumber", item.ItemNumber));
                command.Parameters.Add(new SqlParameter("@rev", revDef));
                command.Parameters.Add(new SqlParameter("@dwgno", item.DwgNo));
                command.Parameters.Add(new SqlParameter("@itemtypeidv", item.ItemType));
                command.Parameters.Add(new SqlParameter("@invtypeidv", item.InvType));
                command.Parameters.Add(new SqlParameter("@itemclassidv", item.ItemClass));
                command.Parameters.Add(new SqlParameter("@itemcodeidv", item.ItemCode));
                command.Parameters.Add(new SqlParameter("@description", item.Description));
                command.Parameters.Add(new SqlParameter("@SalesDescription", item.SalesDescription));
                command.Parameters.Add(new SqlParameter("@PurchaseDescription", item.PurchaseDescription));
                command.Parameters.Add(new SqlParameter("@cost", Convert.ToDouble(item.Cost)));
                command.Parameters.Add(new SqlParameter("@markup", item.Markup));
                command.Parameters.Add(new SqlParameter("@price", item.Price));
                command.Parameters.Add(new SqlParameter("@price2", item.Price2));
                command.Parameters.Add(new SqlParameter("@price3", item.Price3));
                command.Parameters.Add(new SqlParameter("@leadtime", item.LeadTime));
                command.Parameters.Add(new SqlParameter("@weight", item.Weight));
                command.Parameters.Add(new SqlParameter("@uomid", uomid));
                command.Parameters.Add(new SqlParameter("@glsales", item.GLSales));
                command.Parameters.Add(new SqlParameter("@glinv", item.GLInv));
                command.Parameters.Add(new SqlParameter("@glcogs", item.GLCOGS));
                command.Parameters.Add(new SqlParameter("@buyitem", item.BuyItem));
                command.Parameters.Add(new SqlParameter("@obsolete", item.Obsolete));
                command.Parameters.Add(new SqlParameter("@userbit", item.UserBit));
                command.Parameters.Add(new SqlParameter("@userbit2", item.UserBit2));


                command.Parameters.Add(new SqlParameter("@warehouseidv", item.Warehouse.Trim()));
                command.Parameters.Add(new SqlParameter("@locationsidv", item.Location.Trim()));
                command.Parameters.Add(new SqlParameter("@notes", item.Notes));

                command.Parameters.Add(new SqlParameter("@minimum", item.Minimum));
                command.Parameters.Add(new SqlParameter("@maximum", item.Maximum));

                command.Parameters.Add(new SqlParameter("@user1", item.User1));
                command.Parameters.Add(new SqlParameter("@user2", item.User2));
                command.Parameters.Add(new SqlParameter("@user3", item.User3));
                command.Parameters.Add(new SqlParameter("@user4", item.User4));
                command.Parameters.Add(new SqlParameter("@user5", item.User5));
                command.Parameters.Add(new SqlParameter("@user6", item.User6));
                command.Parameters.Add(new SqlParameter("@user7", item.User7));
                command.Parameters.Add(new SqlParameter("@user8", item.User8));

                SqlParameter returnValue = new SqlParameter("@retmsg", System.Data.SqlDbType.Text);
                returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
                command.Parameters.Add(returnValue);
                await command.ExecuteNonQueryAsync();
                await command.DisposeAsync();

                var command2 = _amicsDbContext.Database.GetDbConnection().CreateCommand();
                command2.CommandText = "select id from list_items where itemnumber='" + item.ItemNumber + "' and rev='" + item.Rev + "'";
                var dataReader = command2.ExecuteReader();
                var itemId = "";
                if (dataReader.Read())
                {
                    itemId = dataReader["id"].ToString();
                }

                var hasRows = dataReader.Read();
                conn.Close();
                return new LstMessage() { Message = itemId };
            }

        }

        /// <summary>        
        /// API Service for Insert/Update/Delete Bom Item details in the items_bom table
        /// </summary>
        /// <param name="LstBomGridItems">Bom Item details</param>         
        public async Task<LstMessage> BomGridDetailsUpdation(List<LstBomGridItems> itemsBOM)
        {
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            {
                for (int i = 0; i < itemsBOM.Count; i++)
                {
                    using (var command = _amicsDbContext.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = "amics_sp_api_maintain_item_bom";
                        command.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        try
                        {
                            command.Parameters.Add(new SqlParameter("@actionflag", itemsBOM[i].ActionFlag));
                            command.Parameters.Add(new SqlParameter("@itemsid_parent", itemsBOM[i].Parent_ItemsId));
                            command.Parameters.Add(new SqlParameter("@itemsid_child", itemsBOM[i].Child_ItemsId));
                            command.Parameters.Add(new SqlParameter("@linenum", itemsBOM[i].LineNum));
                            command.Parameters.Add(new SqlParameter("@quantity", itemsBOM[i].Quantity));
                            command.Parameters.Add(new SqlParameter("@ref", itemsBOM[i].Ref));
                            command.Parameters.Add(new SqlParameter("@comments", itemsBOM[i].Comments));
                            command.Parameters.Add(new SqlParameter("@createdby", itemsBOM[i].Createdby));
                            var itemsId = string.IsNullOrEmpty(itemsBOM[i].Id) ? Guid.Empty : new Guid(itemsBOM[i].Id);
                            if (itemsId != Guid.Empty)
                                command.Parameters.Add(new SqlParameter("@id", itemsBOM[i].Id));
                            await command.ExecuteNonQueryAsync();
                            command.Dispose();
                        }
                        catch (Exception ex)
                        {
                            //Log.ErrorLog(ex.Message, "Maintain : MaintainItemsBom");
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


        /// <summary>
        /// API Service to get location, somain, quantity & name details for given Warehouse
        /// </summary>
        /// <param name="itemsId">Items Id</param> 
        /// <param name="secUsersId">Sec_Users Id</param> 
        /// <param name="warehouse">Warehouse</param> 
        public List<LstViewLocation> ViewLocationWarehouse(string itemsId, string secUsersId, string warehouse)
        {
            var itmId = string.IsNullOrEmpty(itemsId) ? string.Empty : itemsId;
            var usersId = string.IsNullOrEmpty(secUsersId) ? string.Empty : secUsersId;
            if (string.IsNullOrEmpty(warehouse))
            {
                var viewLocResult = _amicsDbContext.LstViewLocation.FromSqlRaw($"select * from amics_fn_api_view_location_summary('{itmId}','{usersId}')").ToList();
                return viewLocResult;
            }
            else
            {
                var viewLocWhResult = _amicsDbContext.LstViewLocationWh.FromSqlRaw($"select * from amics_fn_api_view_location_summary_whs('{itmId}','{usersId}','{warehouse}')").ToList();
                var viewLocResult = viewLocWhResult.Any() ? viewLocWhResult.Select(r => new LstViewLocation() { Location = r.Location, Warehouse = "", Somain = r.Somain, Name = r.Name, Quantity = r.Quantity }).ToList() : new List<LstViewLocation>();
                return viewLocResult;
            }
        }

        /// <summary>
        /// API Service to get Inquiry details based on the search
        /// </summary>
        /// <param name="itemnum">Items Id</param> 
        /// <param name="desc">Description</param> 
        /// <param name="lotno">Lot No</param> 
        /// <param name="serial">Serial No</param> 
        /// <param name="tag">Tag No</param> 
        /// <param name="location">Location</param> 
        /// <param name="action">Action</param> 
        /// <param name="er">ER</param> 
        public List<LstInquiry> InquiryDetails(InquiryRequestDetails sp_inquiry)
        {
            //var inquiryResult = _amicsDbContext.LstInquiry.FromSqlRaw($"exec sp_inquiry5 @part='{request.ItemNumber}',@desc = '{request.Description}',@lotno='{request.LotNo}',@location ='{request.Location}',@action = '{request.Action}', @serial='{request.Serial}',@tag ='{request.Tag}',@user = '{request.User}', @er = '{request.ER}', @mdatIn='{request.MDATIn}'").ToList();
            List<LstInquiry> ItemList = new List<LstInquiry>();
            var actionFlag = sp_inquiry.Action.ToString("d");
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    Utility util = new Utility();
                    string strCurr = util.ReturnZeros(2);
                    string strQty = util.ReturnZeros(2);

                    sqlCommand.CommandText = "amics_sp_api_Inquiry";
                    conn.Open();

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@part", sp_inquiry.ItemNumber));
                    sqlCommand.Parameters.Add(new SqlParameter("@er", sp_inquiry.ER));
                    sqlCommand.Parameters.Add(new SqlParameter("@lotno", sp_inquiry.LotNo));
                    sqlCommand.Parameters.Add(new SqlParameter("@serial", sp_inquiry.Serial));
                    sqlCommand.Parameters.Add(new SqlParameter("@tag", sp_inquiry.Tag));
                    sqlCommand.Parameters.Add(new SqlParameter("@location", sp_inquiry.Location));
                    sqlCommand.Parameters.Add(new SqlParameter("@desc", sp_inquiry.Description));
                    sqlCommand.Parameters.Add(new SqlParameter("@user", sp_inquiry.User));
                    sqlCommand.Parameters.Add(new SqlParameter("@action", actionFlag));
                    sqlCommand.Parameters.Add(new SqlParameter("@mdatIn", sp_inquiry.MDATIn));


                    var dataReader = sqlCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        string serno = "", tagno = "", lotno = "";
                        double qty = 0;

                        LstInquiry InquiryDetails = new LstInquiry();

                        InquiryDetails.Itemnumber = dataReader["itemnumber"].ToString();
                        InquiryDetails.Description = dataReader["description"].ToString();
                        InquiryDetails.Location = dataReader["location"].ToString();

                        InquiryDetails.Cost = String.Format("{0:0." + strCurr + "}", dataReader["cost"]);

                        InquiryDetails.Itemtype = dataReader["itemtype"].ToString();
                        InquiryDetails.ER = dataReader["er"].ToString();

                        if (sp_inquiry.Action == InquiryActionType.Serial || sp_inquiry.Action == InquiryActionType.Tag)
                        {
                            InquiryDetails.Quantity = dataReader["quantity"].ToString();
                            InquiryDetails.Serial = dataReader["serial"].ToString();
                            InquiryDetails.Lotno = dataReader["lotno"].ToString();
                        }
                        else if (sp_inquiry.Action == InquiryActionType.ER || sp_inquiry.Action == InquiryActionType.MdatIn)
                        {
                            if (dataReader["serial"].ToString() != "")
                            {
                                InquiryDetails.Quantity = String.Format("{0:0." + strQty + "}", dataReader["quantity"]);
                                InquiryDetails.Serial = dataReader["serial"].ToString();
                                InquiryDetails.Lotno = dataReader["lotno"].ToString();
                            }
                            else if (dataReader["lotno"].ToString() != "")
                            {
                                InquiryDetails.Quantity = dataReader["lotno"].ToString();
                                InquiryDetails.Lotno = dataReader["color"].ToString();
                                //InquiryDetails.Mdat_In = dataReader["mdat_in"].ToString();
                            }
                            else
                            {
                                InquiryDetails.Quantity = String.Format("{0:0." + strQty + "}", dataReader["quantity"]);
                                InquiryDetails.Serial = "";
                                InquiryDetails.Lotno = "";
                            }
                        }
                        else
                        {
                            InquiryDetails.Quantity = String.Format("{0:0." + strQty + "}", dataReader["quantity"]);
                            InquiryDetails.Serial = dataReader["serial"].ToString();
                            InquiryDetails.Lotno = dataReader["lotno"].ToString();
                        }

                        InquiryDetails.Color = "";
                        if (dataReader["mdatin"].ToString() != "")
                        {
                            InquiryDetails.Mdatin = dataReader["mdatin"].ToString();
                        }
                        else
                        {
                            InquiryDetails.Mdatin = "";
                        }

                        ItemList.Add(InquiryDetails);
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    conn.Close();
                }
            }
            return ItemList.ToList();


        }

        /// <summary>
        /// API Service to get warehouse,location, pomain, serial no, tag no, cost, quantity details using below sql function        
        /// </summary>
        /// <param name="itemsId">Items Id</param> 
        /// <param name="secUsersId">Sec_Users Id</param> 
        public List<LstSerial> ViewSerial(string itemsId, string secUsersId, string warehouse, string serNo, string tagNo)
        {
            var viewSerialResult = (dynamic)null;
            var itmId = string.IsNullOrEmpty(itemsId) ? string.Empty : itemsId;
            var usersId = string.IsNullOrEmpty(secUsersId) ? string.Empty : secUsersId;

            if ((warehouse == null || warehouse == "") && (serNo == null || serNo == "") && (tagNo == null || tagNo == ""))
                viewSerialResult = _amicsDbContext.LstSerial.FromSqlRaw($"select * from amics_fn_api_view_location_detail('{itmId}','{usersId}') where quantity > 0").ToList();
            else if ((warehouse != null || warehouse != "") && (serNo == null || serNo == "") && (tagNo == null || tagNo == ""))
                viewSerialResult = _amicsDbContext.LstSerial.FromSqlRaw($"select * from amics_fn_api_view_location_detail('{itmId}','{usersId}') where warehouse='{warehouse}'").ToList();
            else if ((warehouse == null || warehouse == "") && (serNo != null || serNo != "") && (tagNo == null || tagNo == ""))
                viewSerialResult = _amicsDbContext.LstSerial.FromSqlRaw($"select * from amics_fn_api_view_location_detail('{itmId}','{usersId}') where serlot like '{serNo}%'").ToList();
            else if ((warehouse != null || warehouse != "") && (serNo != null || serNo != "") && (tagNo == null || tagNo == ""))
                viewSerialResult = _amicsDbContext.LstSerial.FromSqlRaw($"select * from amics_fn_api_view_location_detail('{itmId}','{usersId}') where serlot like '{serNo}%' and warehouse='{warehouse}'").ToList();
            else if ((warehouse == null || warehouse == "") && (serNo == null || serNo == "") && (tagNo != null || tagNo != ""))
                viewSerialResult = _amicsDbContext.LstSerial.FromSqlRaw($"select * from amics_fn_api_view_location_detail('{itmId}','{usersId}') where tagcol like '{tagNo}%'").ToList();
            else if ((warehouse != null || warehouse != "") && (serNo == null || serNo == "") && (tagNo != null || tagNo != ""))
                viewSerialResult = _amicsDbContext.LstSerial.FromSqlRaw($"select * from amics_fn_api_view_location_detail('{itmId}','{usersId}') where tagcol like '{tagNo}%' and warehouse='{warehouse}'").ToList();

            return viewSerialResult;
        }

        /// <summary>
        /// API Service to load Notes details for item number
        /// </summary>
        /// <param name="parentId">Items Id</param>        
        public List<LstNotes> ViewNotes(string parentId)
        {
            var itemsId = string.IsNullOrEmpty(parentId) ? Guid.Empty : new Guid(parentId.ToString());

            var notesResult = _amicsDbContext.LstNotes.FromSqlRaw($"exec amics_sp_api_view_notes @itemsid='{itemsId}'").ToList();

            return notesResult;
        }


        /// <summary>
        /// API Service to add/update/delete notes for parent item number
        /// </summary>
        /// <param name="LstNotes">Notes details</param>        
        /// <param name="user">user</param>        
        public LstMessage NotesUpdation(List<LstNotes> notesLst, string user)
        {
            var notesResult = (dynamic)null;
            foreach (var notes in notesLst)
            {
                var notesId = (notes.Id == null || notes.Id == Guid.Empty) ? Guid.Empty : notes.Id;

                if ((notesId == null || notesId == Guid.Empty) && (notes.ActionFlag == 0))
                    notes.ActionFlag = 1;

                notesResult = _amicsDbContext.LstMessage.FromSqlRaw($"exec amics_sp_api_maintain_notes_general @actionflag='{notes.ActionFlag}', @parentid='{notes.ParentId}',@linenum='{notes.LineNum}',@notesref='{notes.NotesRef}', @notes='{notes.Notes}',@createdby='{user}',@id='{notesId}'").AsEnumerable().FirstOrDefault();
            }
            return notesResult;
        }


        /// <summary>
        /// API Service to update From Serial No/Tag No/Model No/Cost to Serial No/Tag No/Model No/Cost
        /// </summary>
        /// <param name="LstChangeSerial">LstChangeSerial</param>                   
        public string ChangeSerialTag(LstChangeSerial lstChSerial)
        {
            using (var conn = _amicsDbContext.Database.GetDbConnection())
            using (var sqlCommand = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    sqlCommand.CommandText = "amics_sp_api_process_change_sertag";
                    conn.Open();

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@serialid", lstChSerial.SerialId));
                    sqlCommand.Parameters.Add(new SqlParameter("@serno_prior", lstChSerial.SerNoFm));
                    sqlCommand.Parameters.Add(new SqlParameter("@serno_current", string.IsNullOrWhiteSpace(lstChSerial.SerNoTo) ? string.Empty : lstChSerial.SerNoTo));
                    sqlCommand.Parameters.Add(new SqlParameter("@tagno_prior", string.IsNullOrWhiteSpace(lstChSerial.TagNoFm) ? string.Empty : lstChSerial.TagNoFm));
                    sqlCommand.Parameters.Add(new SqlParameter("@tagno_current", string.IsNullOrWhiteSpace(lstChSerial.TagNoTo) ? string.Empty : lstChSerial.TagNoTo));
                    sqlCommand.Parameters.Add(new SqlParameter("@memo", lstChSerial.Notes));
                    sqlCommand.Parameters.Add(new SqlParameter("@createdby", string.IsNullOrWhiteSpace(lstChSerial.User) ? string.Empty : lstChSerial.User));
                    sqlCommand.Parameters.Add(new SqlParameter("@model_prior", string.IsNullOrWhiteSpace(lstChSerial.ModelFm) ? string.Empty : lstChSerial.ModelFm));
                    sqlCommand.Parameters.Add(new SqlParameter("@model_current", string.IsNullOrWhiteSpace(lstChSerial.ModelTo) ? string.Empty : lstChSerial.ModelTo));
                    sqlCommand.Parameters.Add(new SqlParameter("@cost_prior", string.IsNullOrWhiteSpace(lstChSerial.CostFm) ? string.Empty : lstChSerial.CostFm));
                    sqlCommand.Parameters.Add(new SqlParameter("@cost_current", string.IsNullOrWhiteSpace(lstChSerial.CostTo) ? string.Empty : lstChSerial.CostTo));                    
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
            return "Successfully Saved";

        }
    }
}
