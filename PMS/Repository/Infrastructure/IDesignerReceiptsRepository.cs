using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Database;

namespace PMS.Repository.Infrastructure
{
   public interface IDesignerReceiptsRepository : IGenericRepository<designer_receipts>
    {
        List<SSP_DesignerReceipts_Result> SearchReceipts(string userId, Int32 branchid, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, DateTime FromDate, DateTime ToDate, string searchText);

        SSP_DesignerReceiptById_Result ReceiptById(int id);
    }
}
