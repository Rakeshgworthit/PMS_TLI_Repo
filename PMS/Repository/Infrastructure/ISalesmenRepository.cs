using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Database;
namespace PMS.Repository.Infrastructure
{
    public interface ISalesmenRepositor : IGenericRepository<salesman>
    {
        List<SSP_Salesmen_Result> SearchSalesmen(Int32 branchid, Int32 StartIndex, Int32 PageSize,string SortBy, string OrderBy,string SalesmenSearch);

    }
}