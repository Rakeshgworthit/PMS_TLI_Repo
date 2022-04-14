using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class SupplierInvoiceService : GenericRepository<PMSEntities, tbl_supplier_invoice>, ISupplierInvoiceRepository
    {
        public List<SSP_SupplierInvoices_Result> SearchSupplierInvoice(string userId, Int32 branchid, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, DateTime FromDate, DateTime ToDate, Int32 SupplierId, string searchText)
        {
            List<SSP_SupplierInvoices_Result> objList = new List<SSP_SupplierInvoices_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                if (searchText == null) searchText = "";
                objList = objDB.SSP_SupplierInvoices(new Guid(userId), branchid, FromDate, ToDate, SupplierId, StartIndex, PageSize, SortBy, OrderBy, false, searchText).ToList();
            }
            return objList;
        }
    }
}