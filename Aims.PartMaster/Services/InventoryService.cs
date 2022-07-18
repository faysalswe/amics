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


        /// <summary>
        ///Interface for get details of trans action log, options parametes FromDate,ToDate and Reason.
        /// </summary>        
        /// <param name="FromDate">01/25/2020</param> 
        /// <param name="ToDate">01/25/2022</param> 
        /// <param name="Reason">MISC REC,MISC PICK</param> 

        List<LstTransLog> TransLog(string FromDate, string ToDate, string Reason);


        /// <summary>
        ///Interface for get List next numbers for receiving.
        /// </summary>      
        TransNextNum TransNumberRec();

        /// <summary>
        /// API Service for insert a temp table for increase the quantity of SERIAL or LOT number.
        /// </summary>   

        public LstMessage InsertInvSerLot(List<InvSerLot> InvSerLot);
      

        /// <summary>
        ///Interface for execute receipt stored procedure and increase the quantity.
        /// </summary> 
        public LstMessage UpdateInvReceipt(InvReceipts InvReceipts);
 
        ///<summary>
        /// Interface for validating the serial number and lot number pass FormName as optional parameter.
        /// </summary>        
        /// <param name="ValidateSerTag">InputValidateSerTag</param> 
        public OutValidateSerTag ValidateSerTag(InputValidateSerTag ValidateSerTag);
        /// <summary>
        ///Interface for Insert the decrease values into the inv_trans table
        /// </summary>   

        public LstMessage InsertInvTrans(List<InvTrans> InvTransData);


        /// <summary>
        ///Interface for execute the inv pick sp for decrease the inventory
        /// </summary> 
        public LstMessage ExecuteSpPick(SpPick Pick);

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
           // ItemsId = ItemsId.Length < 32 ? Guid.Empty.ToString() : ItemsId;

            var itemsGuId = string.IsNullOrEmpty(ItemsId) ? Guid.Empty : new Guid(ItemsId.ToString());
            var secUserGuId = string.IsNullOrEmpty(SecUserId) ? Guid.Empty : new Guid(SecUserId.ToString());
            var statusResult = _amicsDbContext.DbxInvStatus.FromSqlRaw($"select * from dbo.amics_fn_api_inv_status ('{itemsGuId}','{secUserGuId}')").AsEnumerable().FirstOrDefault();
            
            return statusResult;
        }


        /// <summary>
        /// API Service for get DefaultValues for the screen or the form, pass FormName as optional parameter.
        /// </summary>        
        /// <param name="FormName">string</param> 
        public List<LstDefaultsValues> DefaultValues(string FormName)
        {            
          
            var defaustResult = _amicsDbContext.ListDefaultsValues
                .FromSqlRaw($"exec amics_sp_api_get_list_defaults @formname='{FormName}'")
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
              .FromSqlRaw($"exec amics_sp_api_get_er_by_pn @itemsid='{itemsGuId}',@somain='{SoMain}'")
              .ToList();
            return erResult;
        }



        /// <summary>
        ///API Service get details of trans action log, options parametes FromDate,ToDate and Reason.
        /// </summary>        
        /// <param name="FromDate">01/25/2020</param> 
        /// <param name="ToDate">01/25/2022</param> 
        /// <param name="Reason">MISC REC,MISC PICK</param> 

        public List<LstTransLog> TransLog(string FromDate, string ToDate,string Reason)
        {                       
            var ViewTransLog = _amicsDbContext.ListTransLog.FromSqlRaw($"select newid() as id,* from [dbo].[amics_fn_api_translog_view] ('{FromDate}','{ToDate}','{Reason}')").ToList();
            return ViewTransLog;
        }


        /// <summary>
        /// API Service for get List next numbers for receiving.
        /// </summary>      
        public TransNextNum TransNumberRec()
        {            
            var transnumResult = _amicsDbContext.DbxTransNextNum.FromSqlRaw($"exec amics_sp_api_get_transnum").AsEnumerable().FirstOrDefault();
            return transnumResult;
        }


        /// <summary>
        /// API Service for insert a temp table for increase the quantity of SERIAL or LOT number.
        /// </summary>   

        public LstMessage InsertInvSerLot(List<InvSerLot> InvSerLot)
        {
                for (int i = 0; i < InvSerLot.Count; i++)
                {
                    var sql = $"exec amics_sp_api_insert_inv_serlot @transnum={InvSerLot[i].Transnum}";
                    sql += $",@serno='{InvSerLot[i].SerNo}'";
                    sql += $",@tagno='{InvSerLot[i].TagNo}'";
                    sql += $",@lotno='{InvSerLot[i].LotNo}'";
                    sql += $",@model='{InvSerLot[i].Model}'";
                    sql += $",@color='{InvSerLot[i].Color}'";
                    sql += $",@qty={InvSerLot[i].Qty}";
                    sql += $",@createdby='{InvSerLot[i].CreatedBy}'";
                  if(InvSerLot[i].ExpDate != null)
                    sql += $",@expdate='{InvSerLot[i].ExpDate}'";                              

                    var receiptResult = _amicsDbContext.LstMessage.FromSqlRaw(sql).AsEnumerable().FirstOrDefault();
                }
            
            return new LstMessage() { Message = "Successfully Saved" };

        }


        /// <summary>
        /// API Service for execute receipt stored procedure and increase the quantity.
        /// </summary>   

        public LstMessage UpdateInvReceipt(InvReceipts InvReceipts) {

            var sourcesRefId = InvReceipts.SourcesRefId == null ? Guid.Empty :  InvReceipts.SourcesRefId;
            var recExtedId = InvReceipts.ExtendedId == null ? Guid.Empty :  InvReceipts.ExtendedId;

            var sql = $"exec amics_sp_api_receipts @rec_sourcesrefid='{sourcesRefId}'";
            sql += $",@rec_extedid='{recExtedId}'";
            sql += $",@rec_source='{InvReceipts.Source}'";
            sql += $",@rec_warehouse='{InvReceipts.Warehouse}'";
            sql += $",@rec_location='{InvReceipts.Location}'";
            sql += $",@rec_item='{InvReceipts.ItemNumber}'";
            sql += $",@rec_rev='{InvReceipts.Rev}'";
            sql += $",@rec_cost={InvReceipts.Cost}";
            sql += $",@rec_quantity={InvReceipts.Quantity}";
            sql += $",@rec_misc_reason='{InvReceipts.MiscReason}'";
            sql += $",@rec_misc_ref='{InvReceipts.MiscRef}'";
            sql += $",@rec_misc_source='{InvReceipts.MiscSource}'";
            sql += $",@rec_notes='{InvReceipts.Notes}'";
            sql += $",@rec_transdate='{InvReceipts.TransDate}'";
            sql += $",@rec_user='{InvReceipts.User1}'";
            sql += $",@rec_transnum='{InvReceipts.TransNum}'";
            sql += $",@potype='{InvReceipts.PoType}'";
            sql += $",@rec_account='{InvReceipts.RecAccount}'";
            sql += $",@rec_packlist='{InvReceipts.RecPackList}'";
            sql += $",@lic_plate_flag={InvReceipts.LicPlatFlage}";
            sql += $",@rec_receiver={InvReceipts.ReceiverNum}";
            sql += $",@user2='{InvReceipts.User2}'";


            var receiptResult = _amicsDbContext.LstMessage.FromSqlRaw(sql).AsEnumerable().FirstOrDefault();
            return receiptResult;
        }


        /// <summary>
        /// API Service for validating the serial number and lot number pass FormName as optional parameter.
        /// </summary>        
        /// <param name="ValidateSerTag">InputValidateSerTag</param> 
        public OutValidateSerTag ValidateSerTag(InputValidateSerTag ValidateSerTag)
        {
            var sql = $"exec amics_sp_api_validate_sertag @itemsid='{ValidateSerTag.itemsid}'";
            sql += ValidateSerTag.option.ToUpper() == "SERIAL" ? $",@serno='{ValidateSerTag.sertag}'" : $",@tagno='{ValidateSerTag.sertag}'";

            var validateSerTag = _amicsDbContext.OutValidateSerTag
                .FromSqlRaw(sql)
                .AsEnumerable()
                .FirstOrDefault();

            return validateSerTag;
        }



        /// <summary>
        /// API Service for Insert into inv_trans table for decreasing inventory.
        /// </summary>   

        public LstMessage InsertInvTrans(List<InvTrans> InvTransData)
        {
            //Insert Into inv_trans(Source, invbasicid, itemsid, transqty, transnum) Values('MISC PICK', 'fd6346b6-50da-458f-b05c-ecfd32238941', '5fc61219-de9d-46bd-b3a0-398dec32153c', '3', '43782')

            for (int i = 0; i < InvTransData.Count; i++)
            {
                InvTrans invTrans = InvTransData[i];
                var sql = $"exec amics_sp_api_insert_inv_trans @TransNum={invTrans.TransNum}";

                if (invTrans.InvBasicId != null)
                    sql += $",@InvBasicId='{invTrans.InvBasicId}'";

                if (invTrans.InvSerialId != null)
                    sql += $",@InvSerialId='{invTrans.InvSerialId}'";

                if (invTrans.ItemsId != null)
                    sql += $",@ItemsId='{invTrans.ItemsId}'";

                if (invTrans.FromLocationId != null)
                    sql += $",@FromLocationId='{invTrans.FromLocationId}'";

                if (invTrans.ToLocationId != null)
                    sql += $",@ToLocationId='{invTrans.ToLocationId}'";

                //if (invTrans.TransQty != null)
                sql += $",@TransQty='{invTrans.TransQty}'";

                if (invTrans.BoxNum != null)
                    sql += $",@BoxNum='{invTrans.BoxNum}'";

                sql += $",@Createdby='{invTrans.CreatedBy}'";

                var receiptResult = _amicsDbContext.LstMessage.FromSqlRaw(sql).AsEnumerable().FirstOrDefault();
            }

            return new LstMessage() { Message = "Successfully Saved" };

        }

        /// <summary>
        /// API Service for execute receipt stored procedure and increase the quantity.
        /// </summary>   

        public LstPacklist ExecuteSpPick(SpPick spPick)
        {

            var sql = $"exec amics_sp_api_pick ";

            sql += $"@pick_transnum='{spPick.PickTransnum}'";
            if (spPick.PickTransdate != null)
                sql += $",@pick_transdate='{spPick.PickTransdate}'";

            if (spPick.PickSourcesrefId != null)
                sql += $",@pick_sourcesrefid='{spPick.PickSourcesrefId}'";

            sql += $",@pick_misc_reason='{spPick.PickMiscReason}'";
            sql += $",@pick_misc_ref='{spPick.PickMiscRef}'";
            sql += $",@pick_misc_source='{spPick.PickMiscSource}'";

            if (spPick.PickShipvia != null)
                sql += $",@Pick_Shipvia='{spPick.PickShipvia}'";
            sql += $",@pick_source='{spPick.PickSource}'";

            if (spPick.PickShipCharge != null)
                sql += $",@Pick_Shipcharge='{spPick.PickShipCharge}'";
            if (spPick.PickTrackingNum != null)
                sql += $",@Pick_Trackingnum='{spPick.PickTrackingNum}'";


            sql += $",@pick_notes='{spPick.PickNotes}'";

            if (spPick.PickPackNote != null)
                sql += $",@Pick_PackNote='{spPick.PickPackNote}'";

            if (spPick.PickInvoiceNote != null)
                sql += $",@Pick_InvoiceNote='{spPick.PickInvoiceNote}'";

            sql += $",@Pick_SalesTax='{spPick.PickSalesTax}'";
            sql += $",@Pick_Shipdate='{spPick.PickTransdate}'";
            sql += $",@pick_user='{spPick.PickUser}'";

            sql += $",@pick_setnum='{spPick.PickSetnum}'";

            if (spPick.PickWarehouse != null)
                sql += $",@pick_warehouse='{spPick.PickWarehouse}'";

            if (spPick.PickOriginalReceiptsid != null)
                sql += $",@original_receiptsid='{spPick.PickOriginalReceiptsid}'";

            var receiptResult = _amicsDbContext.LstPacklist.FromSqlRaw(sql).AsEnumerable().FirstOrDefault();

            return new LstPacklist() { Packlist = "Successfully Saved" };
        }


    }
}