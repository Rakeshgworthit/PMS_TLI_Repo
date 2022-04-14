using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class InvoiceService : GenericRepository<PMSEntities, tbl_invoice>, IInvoice
    {
        public List<SSP_Invoices_Result> SearchInvoice(string userId, Int32 branchid, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, DateTime FromDate, DateTime ToDate, Int32 BilltoId, Int32 SalesmenId, bool IsTax, string searchText)
        {
            List<SSP_Invoices_Result> objList = new List<SSP_Invoices_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                if (searchText == null) searchText = "";
                objList = objDB.SSP_Invoices(new Guid(userId), branchid, FromDate, ToDate, BilltoId, SalesmenId, StartIndex, PageSize, SortBy, OrderBy, IsTax, searchText).ToList();
            }
            return objList;
        }
    }
}