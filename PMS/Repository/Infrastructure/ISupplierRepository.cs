using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Database;
namespace PMS.Repository.Infrastructure
{
    public interface ISupplierRepositor : IGenericRepository<supplier>
    {
        List<SSP_Supplier_Result> SearchSupplier(Int32 StartIndex, Int32 PageSize,string SortBy, string OrderBy);
    }
}