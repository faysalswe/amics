using Aims.Core.Models;
using Aims.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Linq;

namespace Aims.PartMaster.Services
{
    public interface IPartMasterService
    {
        List<AimcsSpLookUp> CommonLookup(FieldNameSearch fieldName, string search_col1, string search_col2);
        List<LstWarehouse> WarehouseLookup(string searchWarehouse);
        List<LstLocation> LocationLookup(string searchLocation, string warehouseId);

    }

    public class PartMasterService:IPartMasterService
    {
        private readonly AmicsDbContext _amicsDbContext;
        public PartMasterService(AmicsDbContext aimsDbContext)
        {
            _amicsDbContext = aimsDbContext;
        }

        public List<AimcsSpLookUp> CommonLookup(FieldNameSearch fieldName, string search_col1, string search_col2 ) 
        { 
            var result = _amicsDbContext.AmicsSpLookups
                .FromSqlRaw($"exec amicsmvc_sp_lookup @fieldname='{fieldName.GetEnumDescription()}',@search_col1='{search_col1}',@search_col2='{search_col2 }'")
                .ToList< AimcsSpLookUp>();

            return result;
        }

        public List<LstWarehouse> WarehouseLookup(string searchWarehouse)
        {
            var result = _amicsDbContext.LstWarehouses
                .FromSqlInterpolated($"select id,warehouse from list_warehouses where trim(warehouse) like  {"%" + searchWarehouse + "%"} order by warehouse")
                .ToList();

            return result;
        }

        public List<LstLocation> LocationLookup(string searchLocation,string warehouseId)
        {
            var whouseId = new Guid(warehouseId.ToString());
            var result = _amicsDbContext.LstLocations
                .FromSqlInterpolated($"select id,location,,isnull(list_locations.invalid,0) as invalid,isnull(list_locations.sequenceno,'') as sequenceno,isnull(list_locations.route,'') as route from list_locations where warehousesid={whouseId} and location like  {"%" + searchLocation + "%"} and isnull(flag_delete,0)=0 order by location")
                .ToList();

            return result;
        }

    }

}
