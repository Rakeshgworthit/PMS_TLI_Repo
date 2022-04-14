using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Database;
namespace PMS.Repository.Infrastructure
{
    public interface IReceiptsRepositor : IGenericRepository<receipt>
    {
        List<SSP_Receipts_Result> SearchReceipts(string userId, Int32 branchid, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, DateTime FromDate, DateTime ToDate, Int32 ProjectId, Int32 ProjectSalesmenId, string searchText);

        SSP_ReceiptsById_Result ReceiptById(Int32 id);
    }
}