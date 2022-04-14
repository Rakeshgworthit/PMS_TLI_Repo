using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class DesignerInvoiceService : GenericRepository<PMSEntities, tbl_designer_invoice>, IDesignerInvoice
    {
        public List<SSP_DesignerInvoices_Result> SearchInvoice(string userId, int branchid, int StartIndex, int PageSize, string SortBy, string OrderBy, DateTime FromDate, DateTime ToDate, bool IsTax, string searchText)
        {
            List<SSP_DesignerInvoices_Result> objList = new List<SSP_DesignerInvoices_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                if (searchText == null) searchText = "";
                objList = objDB.SSP_DesignerInvoices(new Guid(userId), branchid, FromDate, ToDate, StartIndex, PageSize, SortBy, OrderBy, IsTax, searchText).ToList();
            }
            return objList;
        }
    }
}