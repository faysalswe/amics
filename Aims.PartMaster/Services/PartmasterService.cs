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

namespace Aims.Core.Services
{
    public interface IPartmasterService
    {
        LstItemDetails LoadPartmaster(string itemNumber, string rev);
        List<LstItemsBom> LoadItemsBom(string parentItemId);
        List<LstItemsPO> LoadItemsPO(string parentId);
        LstBomCount ItemsBomCount(string parentId);
        LstMessage ItemNumDelete(string itemNo, string rev);
        Task<LstMessage> ItemNumDetailsAddUpdateAsync(LstItemDetails item); 
        List<LstViewLocation> ViewLocationWarehouse(string itemsId, string secUsersId, string warehouse);
        Task<LstMessage> BomGridDetailsUpdation(List<LstBomGridItems> LstBomGridItems);
        List<LstInquiry> InquiryDetails(InquiryRequestDetails request);
        List<LstSerial> ViewSerial(string itemsId, string secUsersId, string warehouse, string serNo, string tagNo);
        List<LstNotes> ViewNotes(string parentId);
        LstMessage NotesUpdation(List<LstNotes> notesLst, string user);
    }

    public class PartmasterService: IPartmasterService
    {
        private readonly AmicsDbContext _amicsDbContext;
        public PartmasterService(AmicsDbContext aimsDbContext)
        {
            _amicsDbContext = aimsDbContext;
        }

        /// <summary>
        /// API Service to get Partmaster details
        /// </summary>
        /// <param name="itemnumber">itemnumber</param>  
        /// <param name="rev">rev</param>    
        public LstItemDetails LoadPartmaster(string itemNumber, string rev)
        {            
            var itemNum = string.IsNullOrEmpty(itemNumber) ? string.Empty : itemNumber;
            var revDef = string.IsNullOrEmpty(rev) ? "-" : rev;
            
            //The code above should resolve the error.
            var itemresult = _amicsDbContext.LstItemDetails
                .FromSqlRaw($"exec sp_webservice_load_partmaster5 @item ='{itemNum}',@rev = '{revDef}'").AsEnumerable().FirstOrDefault();

            return itemresult;
        }

        /// <summary>
        /// API Service to get BOM details
        /// </summary>
        /// <param name="parentId">Parent Item Id</param>          
        public List<LstItemsBom> LoadItemsBom(string parentId)
        {
            var itemsId = string.IsNullOrEmpty(parentId) ? Guid.Empty : new Guid(parentId.ToString());
            
            var bomItemresult = _amicsDbContext.LstItemsBom
                .FromSqlRaw($"exec sp_view_items_bom5 @parentid ='{itemsId}'").ToList();

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
                .FromSqlRaw($"exec sp_view_items_po5 @parentid ='{itemsId}'").ToList();

            return poItemresult;
        }

        /// <summary>
        /// API Service to check whether Item BOM is exist or not in database
        /// </summary>
        /// <param name="parentId">Parent Item Id</param>          
        public LstBomCount ItemsBomCount(string parentId)
        {
            var itemsId = string.IsNullOrEmpty(parentId) ? Guid.Empty : new Guid(parentId.ToString());

            var bomexist = _amicsDbContext.LstBomCount.FromSqlRaw($"exec amics_sp_itembom_exist @parentid ='{itemsId}'").AsEnumerable().FirstOrDefault();

            return bomexist;
        }

        /// <summary>
        /// API Service for deletion of Item Num details, column 'flag_delete' is update with 1 in the table 
        /// list items(item num deleted) if return message is null. Message will appear if item num is used in some other tables, so item num can't delete.
        /// </summary>
        /// <param name="itemNo">Item Number</param> 
        /// <param name="Rev">Rev</param> 
        public LstMessage ItemNumDelete(string itemNo, string rev)
        {
            var itemNum = string.IsNullOrEmpty(itemNo) ? string.Empty : itemNo;            
            var revDef = string.IsNullOrEmpty(rev) ? "-" : rev;

            var deleteMsg = _amicsDbContext.LstMessage.FromSqlRaw($"exec sp_delete_list_items5 @item='{itemNum}',@rev='{revDef}'").AsEnumerable().FirstOrDefault();

            return deleteMsg;
        }

        /// <summary>
        /// API Service for Add/Update of Item details
        /// Insert/Update the Item details from the Parent form into list_items table. If id is null, data will be added in the table. Data will be updated if id is not null. 
        /// </summary>
        /// <param name="LstItemDetails">Item Details</param>         
        public async Task<LstMessage> ItemNumDetailsAddUpdateAsync(LstItemDetails item)
        {
            int actionFlag = 0;
            if (item.Id == null || item.Id == Guid.Empty)
                actionFlag = 1;
            else
                actionFlag = 2;

            var itemsid = (item.Id == null || item.Id == Guid.Empty) ? Guid.Empty : item.Id;          
            var uomid = (item.Uomid == null || item.Uomid == Guid.Empty) ? Guid.Empty : item.Uomid;

            using(var conn = _amicsDbContext.Database.GetDbConnection())          
            using (var command = _amicsDbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "sp_maintain_partmaster25";
                conn.Open();

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@actionflag", actionFlag));
                command.Parameters.Add(new SqlParameter("@itemnumber", item.ItemNumber));
                command.Parameters.Add(new SqlParameter("@rev", item.Rev));
                command.Parameters.Add(new SqlParameter("@dwgno", item.DwgNo));
                command.Parameters.Add(new SqlParameter("@itemtypeidv", item.ItemType));
                command.Parameters.Add(new SqlParameter("@invtypeidv", item.InvType));
                command.Parameters.Add(new SqlParameter("@itemclassidv", item.ItemClass));
                command.Parameters.Add(new SqlParameter("@itemcodeidv", item.ItemCode));
                command.Parameters.Add(new SqlParameter("@description", item.Description));
                command.Parameters.Add(new SqlParameter("@SalesDescription", item.SalesDescription));
                command.Parameters.Add(new SqlParameter("@PurchaseDescription", item.PurchaseDescription));
                command.Parameters.Add(new SqlParameter("@cost", item.Cost));
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
               await  command.DisposeAsync();

                var command2 = _amicsDbContext.Database.GetDbConnection().CreateCommand();
                command2.CommandText = "select id from list_items where itemnumber='" + item.ItemNumber + "' and rev='" + item.Rev + "'";
                var dataReader = command2.ExecuteReader();
                var itemId ="";
                if (dataReader.Read())
                {
                    itemId = dataReader["id"].ToString();
                }

                var hasRows = dataReader.Read();
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
                        command.CommandText = "sp_maintain_item_bom5";
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
                    }

                    conn.Close();
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
            if (string.IsNullOrEmpty(warehouse)){
                var viewLocResult = _amicsDbContext.LstViewLocation.FromSqlRaw($"select * from fn_view_location_summary_essex('{itmId}','{usersId}')").ToList();
                return viewLocResult;
            }
            else {
                var viewLocWhResult = _amicsDbContext.LstViewLocationWh.FromSqlRaw($"select * from fn_view_location_summary_whs_essex('{itmId}','{usersId}','{warehouse}')").ToList();
                var viewLocResult = viewLocWhResult.Any()? viewLocWhResult.Select(r => new LstViewLocation() { Location = r.Location, Warehouse = "", Somain = r.Somain, Name = r.Name, Quantity = r.Quantity }).ToList(): new List<LstViewLocation>();
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
                    string strQty =util.ReturnZeros(2);

                    sqlCommand.CommandText = "sp_inquiry5";
                    conn.Open();
                     
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(new SqlParameter("@part", sp_inquiry.ItemNumber));
                    sqlCommand.Parameters.Add(new SqlParameter("@er", sp_inquiry.LotNo));
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
                }
            }
            return ItemList.ToList();

 
        }

        /// <summary>
        /// API Service to get warehouse,location, pomain, serial no, tag no, cost, quantity details using below sql function        
        /// </summary>
        /// <param name="itemsId">Items Id</param> 
        /// <param name="secUsersId">Sec_Users Id</param> 
        public List<LstSerial> ViewSerial(string itemsId, string secUsersId,string warehouse, string serNo, string tagNo)
        {
            var viewSerialResult = (dynamic)null;
            var itmId = string.IsNullOrEmpty(itemsId) ? string.Empty : itemsId;
            var usersId = string.IsNullOrEmpty(secUsersId) ? string.Empty : secUsersId;

            if ((warehouse == null || warehouse == "") && (serNo == null || serNo == "") && (tagNo == null || tagNo == ""))
                viewSerialResult = _amicsDbContext.LstSerial.FromSqlRaw($"select * from fn_view_location_detail('{itmId}','{usersId}') where quantity > 0").ToList();
            else if ((warehouse != null || warehouse != "") && (serNo == null || serNo == "") && (tagNo == null || tagNo == ""))
                viewSerialResult = _amicsDbContext.LstSerial.FromSqlRaw($"select * from fn_view_location_detail('{itmId}','{usersId}') where warehouse='{warehouse}'").ToList();
            else if ((warehouse == null || warehouse == "") && (serNo != null || serNo != "") && (tagNo == null || tagNo == ""))
                viewSerialResult = _amicsDbContext.LstSerial.FromSqlRaw($"select * from fn_view_location_detail('{itmId}','{usersId}') where serlot like '{serNo}%'").ToList();
            else if ((warehouse != null || warehouse != "") && (serNo != null || serNo != "") && (tagNo == null || tagNo == ""))
                viewSerialResult = _amicsDbContext.LstSerial.FromSqlRaw($"select * from fn_view_location_detail('{itmId}','{usersId}') where serlot like '{serNo}%' and warehouse='{warehouse}'").ToList();
            else if ((warehouse == null || warehouse == "") && (serNo == null || serNo == "") && (tagNo != null || tagNo != ""))
                viewSerialResult = _amicsDbContext.LstSerial.FromSqlRaw($"select * from fn_view_location_detail('{itmId}','{usersId}') where tagcol like '{tagNo}%'").ToList();
            else if ((warehouse != null || warehouse != "") && (serNo == null || serNo == "") && (tagNo != null || tagNo != ""))
                viewSerialResult = _amicsDbContext.LstSerial.FromSqlRaw($"select * from fn_view_location_detail('{itmId}','{usersId}') where tagcol like '{tagNo}%' and warehouse='{warehouse}'").ToList();

            return viewSerialResult;
        }

        /// <summary>
        /// API Service to load Notes details for item number
        /// </summary>
        /// <param name="parentId">Items Id</param>        
        public List<LstNotes> ViewNotes(string parentId)
        {
            var itemsId = string.IsNullOrEmpty(parentId) ? Guid.Empty : new Guid(parentId.ToString());

            var notesResult = _amicsDbContext.LstNotes.FromSqlRaw($"exec amics_sp_view_notes @itemsid='{itemsId}'").ToList();

            return notesResult;
        }


        /// <summary>
        /// API Service to add/update/delete notes for item number
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

                notesResult = _amicsDbContext.LstMessage.FromSqlRaw($"exec sp_maintain_notes_general5 @actionflag='{notes.ActionFlag}', @parentid='{notes.ParentId}',@linenum='{notes.LineNum}',@notesref='{notes.NotesRef}', @notes='{notes.Notes}',@createdby='{user}',@id='{notesId}'").AsEnumerable().FirstOrDefault();
            }
            return notesResult;
        }

    }
}
