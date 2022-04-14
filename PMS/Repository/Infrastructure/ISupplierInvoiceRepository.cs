using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Database;

namespace PMS.Repository.Infrastructure
{
    public interface ISupplierInvoiceRepository : IGenericRepository<tbl_supplier_invoice>
    {
        List<SSP_SupplierInvoices_Result> SearchSupplierInvoice(string userId, Int32 branchid, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, DateTime FromDate, DateTime ToDate, Int32 SupplierId, string searchText);
    }
}