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


    }
    public class InventoryService : IInventoryService
    {
        private readonly AmicsDbContext _amicsDbContext;
        public InventoryService(AmicsDbContext aimsDbContext)
        {
            _amicsDbContext = aimsDbContext;
        }

        public InvStatus InventoryStatus(string ItemsId, string SecUserId)
        {

            //var itemsGuId = string.IsNullOrEmpty(ItemsId) ? Guid.Empty : new Guid(ItemsId.ToString());
            //var secUserGuId = string.IsNullOrEmpty(SecUserId) ? Guid.Empty : new Guid(SecUserId.ToString());

            //var searchResult = _amicsDbContext.InvStatus.Allocated($"select * from dbo.webapi_fn_inv_status ('{itemsGuId}','{secUserGuId}')").AsEnumerable().FirstOrDefault<InvStatus>();
            var searchResult = new InvStatus();

            //InvStatus

            return searchResult;
        }
    }
}