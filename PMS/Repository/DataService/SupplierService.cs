using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class SupplierService : GenericRepository<PMSEntities, supplier>, ISupplierRepositor
    {
        public List<SSP_Supplier_Result> SearchSupplier(Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy)
        {
            List<SSP_Supplier_Result> objList = new List<SSP_Supplier_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                objList = objDB.SSP_Supplier(StartIndex, PageSize, SortBy, OrderBy).ToList();
            }
            return objList;
        }

    }
}