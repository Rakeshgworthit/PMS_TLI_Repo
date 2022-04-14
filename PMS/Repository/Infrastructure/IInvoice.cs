using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Database;

namespace PMS.Repository.Infrastructure
{
    public interface IInvoice : IGenericRepository<tbl_invoice>
    {
        List<SSP_Invoices_Result> SearchInvoice(string userId, Int32 branchid, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, DateTime FromDate, DateTime ToDate, Int32 BilltoId, Int32 SalesmenId, bool IsTax, string searchText);
    }
}