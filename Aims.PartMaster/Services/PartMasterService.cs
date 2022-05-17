using Aims.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aims.PartMaster.Services
{
    public interface IPartMasterService
    {
         Task<LstInventoryStatus> GetInventoryStatus(string itemsId, string userUid);

    }

}
