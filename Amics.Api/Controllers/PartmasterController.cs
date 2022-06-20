﻿using Aims.PartMaster.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aims.PartMaster.Services;
using Aims.Core.Services;
using Aims.Core.Models;
using Amics.Api.Model;

namespace Amics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PartmasterController : ControllerBase
    {
        private readonly IPartmasterService _partMastService;
        private readonly ILogger<PartmasterController> _logger;
        public PartmasterController(IPartmasterService partMastService, ILogger<PartmasterController> logger)
        {
            _partMastService = partMastService;
            _logger = logger;
        }

        /// <summary>
        /// API Route Controller to get Partmaster details for parent form, pass Item number and rev as parameter.
        /// </summary>
        /// <param name="itemnumber">itemnumber</param>  
        /// <param name="rev">rev</param>      
        [HttpGet, Route("")]
        public LstItemDetails GetPartmasterDetails([FromQuery] string itemnumber, [FromQuery] string rev)
        {
            var resultPMInfo = _partMastService.LoadPartmaster(itemnumber, rev);

            return resultPMInfo;
        }

        /// <summary>
        /// API Route Controller to get Partmaster BOM details, pass itemsId as parameter.
        /// </summary>
        /// <param name="itemsId">Items Id</param>          
        [HttpGet, Route("BomDetails")]
        public IList<LstItemsBom> GetItemsBom([FromQuery] string itemsId)
        {
            var resultBomInfo = _partMastService.LoadItemsBom(itemsId);

            return resultBomInfo;
        }

        /// <summary>
        /// API Route Controller to get Partmaster PO details for grid, pass itemsId as parameter.
        /// </summary>
        /// <param name="itemsId">Items Id</param>          
        [HttpGet, Route("PODetails")]
        public IList<LstItemsPO> GetItemsPO([FromQuery] string itemsId)
        {
            var resultPOInfo = _partMastService.LoadItemsPO(itemsId);

            return resultPOInfo;
        }

        /// <summary>
        /// API Route Controller to get Partmaster PO details for grid, pass itemsId as parameter.
        /// </summary>
        /// <param name="itemsId">Items Id</param>          
        [HttpGet, Route("BOMCount")]
        public LstBomCount GetItemsBomCount([FromQuery] string itemsId)
        {
            var dataExist = _partMastService.ItemsBomCount(itemsId);

            return dataExist;
        }

        /// <summary>
        /// API Route Controller for deletion of Item Num details, column 'flag_delete' is update with 1 in the table 
        /// list items(item num deleted) if return message is null. Message will appear if item num is used in some other tables, so item num can't delete.
        /// </summary>
        /// <param name="itemnum">Item Number</param>          
        /// /// <param name="rev">Rev</param>          
        [HttpDelete, Route("")]
        public LstMessage ItemDetailsDelete([FromQuery] string itemnum, [FromQuery] string rev)
        {
            var dataExist = _partMastService.ItemNumDelete(itemnum,rev);

            return dataExist;
        }

        /// <summary>
        /// API Route Controller to get Partmaster PO details for grid, pass itemsId as parameter.
        /// </summary>
        /// <param name="itemsId">Items Id</param>          
        [HttpPost, Route("")]
        public async Task<string> ItemDetailsAddUpdate([FromBody] LstItemDetails  pmItem)
        { 
            var itemUpdate = await _partMastService.ItemNumDetailsAddUpdateAsync(pmItem);
            return itemUpdate;
        }

        /// <summary>
        /// API Route Controller for Insert/Update/Delete Bom Item details in the items_bom table
        /// </summary>        
        [HttpPost, Route("BomDetails")]
        public LstMessage BomGridItemDetailsUpdation([FromBody] List<LstBomGridItems> LstBomGridItems)
        {
            var BomGridUpdate = _partMastService.BomGridDetailsUpdation(LstBomGridItems);

            return BomGridUpdate;
        }

        /// <summary>
        /// API Route Controller to get warehouse,location, somain, quantity & name details 
        /// </summary>
        /// <param name="itemsId">Items Id</param>          
        /// /// <param name="secUsersId">SecUsers Id</param>          
        [HttpGet, Route("ViewLocation")]
        public IList<LstViewLocation> ViewLocation([FromQuery] string itemsId, [FromQuery] string secUsersId)
        {
            var getLocResult = _partMastService.ViewLocation(itemsId, secUsersId);

            return getLocResult;
        }

        /// <summary>
        /// API Route Controller to get location, somain, quantity & name details for specified warehouse 
        /// </summary>
        /// <param name="itemsId">Items Id</param>          
        /// <param name="secUsersId">SecUsers Id</param>      
        /// <param name="warehouse">Warehouse</param>      
        [HttpGet, Route("ViewLocationWarehouse")]
        public IList<LstViewLocationWh> ViewLocationWarehouse([FromQuery] string itemsId, [FromQuery] string secUsersId, [FromQuery] string warehouse)
        {
            var getLocResult = _partMastService.ViewLocationWarehouse(itemsId, secUsersId,warehouse);

            return getLocResult;
        }

        // <summary>
        /// API Route Controller to get list of records from Item Number/Descrption/SerialNo/TagNo/Location/ER/MdatIn search
        /// </summary>
        /// <param name="itemnum">Item Number</param>                  
        [HttpPost, Route("Inquiry")]
        public IList<LstInquiry> GetInquiryDetails([FromBody] InquiryRequest request)
        {
            var details = getInquiryRequestDetails(request);
            details.User = request.User.ToString();
            var getLocResult = _partMastService.InquiryDetails(details);

            return getLocResult;
        }
        private InquiryRequestDetails getInquiryRequestDetails(InquiryRequest req)
        {
            var details = new InquiryRequestDetails();
            details.Action = req.Action;
            switch (req.Action)
            {
                case InquiryActionType.ER:
                    details.ER = req.SearchText;
                    break;
                case InquiryActionType.Location:
                    details.Location = req.SearchText;
                    break;
                case InquiryActionType.Description:
                    details.Description = req.SearchText;
                    break;
                case InquiryActionType.Serial:
                    details.Serial = req.SearchText;
                    break;
                case InquiryActionType.Tag:
                    details.Tag = req.SearchText;
                    break;
                case InquiryActionType.MdatIn:
                    details.MDATIn = req.SearchText;
                    break;
                case InquiryActionType.PartMaster:
                default:
                    details.ItemNumber = req.SearchText;                   
                    break;
            }

            return details;
        }

        // <summary>
        /// API Route Controllerto get warehouse, location, pomain,serial no, tag no, cost, quantity details   
        /// </summary>
        /// <param name="itemsId">Items Id</param>          
        /// <param name="secUsersId">SecUsers Id</param>              
        [HttpGet, Route("ViewSerial")]
        public IList<LstSerial> ViewSerial([FromQuery] string ItemsId, [FromQuery] string secUsersId)
        {
            var vwSerialResult = _partMastService.ViewSerial(ItemsId, secUsersId);

            return vwSerialResult;
        }

        // <summary>
        /// API Route Controller to get location, pomain,serial no, tag no, cost, quantity details for selected warehouse
        /// </summary>
        /// <param name="itemsId">Items Id</param>          
        /// <param name="secUsersId">SecUsers Id</param>      
        /// <param name="warehouse">Warehouse</param>      
        [HttpGet, Route("ViewSerialWarehouse")]
        public IList<LstSerial> ViewSerialWh([FromQuery] string ItemsId, [FromQuery] string secUsersId, [FromQuery] string warehouse)
        {
            var vwSerialWhResult = _partMastService.ViewSerialWarehouse(ItemsId, secUsersId, warehouse);

            return vwSerialWhResult;
        }

        // <summary>
        /// API Route Controller to get location, pomain,serial no, tag no, cost, quantity details for selected warehouse and serial no 
        /// </summary>
        /// <param name="itemsId">Items Id</param>          
        /// <param name="secUsersId">SecUsers Id</param>      
        /// <param name="warehouse">Warehouse</param>  
        /// <param name="serialNo">Serial No</param>   
        [HttpGet, Route("ViewSerialSerNo")]
        public IList<LstSerial> ViewSerialSerNo([FromQuery] string ItemsId, [FromQuery] string secUsersId, [FromQuery] string warehouse, [FromQuery] string serNo)
        {
            var vwSerNoResult = _partMastService.ViewSerialSerNo(ItemsId, secUsersId, warehouse, serNo);

            return vwSerNoResult;
        }


        // <summary>
        /// API Route Controller to get location, pomain,serial no, tag no, cost, quantity details for selected warehouse and tag no
        /// </summary>
        /// <param name="itemsId">Items Id</param>          
        /// <param name="secUsersId">SecUsers Id</param>      
        /// <param name="warehouse">Warehouse</param>   
        /// <param name="tagNo">Tag No</param>   
        [HttpGet, Route("ViewSerialTagNo")]
        public IList<LstSerial> ViewSerialTagNo([FromQuery] string ItemsId, [FromQuery] string secUsersId, [FromQuery] string warehouse, [FromQuery] string tagNo)
        {
            var vwTagNoResult = _partMastService.ViewSerialTagNo(ItemsId, secUsersId, warehouse, tagNo);

            return vwTagNoResult;
        }
    }
}
