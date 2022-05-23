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
            var result = _amicsDbContext.amicsSpLookups
                .FromSqlInterpolated($"exec amicsmvc_sp_lookup @fieldname='{fieldName.GetEnumDescription()}',@search_col1='{search_col1}',@search_col2='{search_col2 }'")
                .ToList< AimcsSpLookUp>();

            return result;
        }

    }

}
