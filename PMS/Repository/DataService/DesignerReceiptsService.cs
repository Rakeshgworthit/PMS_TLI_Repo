using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;


namespace PMS.Repository.DataService
{
    public class DesignerReceiptsService: GenericRepository<PMSEntities, designer_receipts>, IDesignerReceiptsRepository
    {
        public List<SSP_DesignerReceipts_Result> SearchReceipts(string userId, Int32 branchid, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, DateTime FromDate, DateTime ToDate, string searchText)
        {
            List<SSP_DesignerReceipts_Result> objList = new List<SSP_DesignerReceipts_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                if (searchText == null) searchText = "";
                objList = objDB.SSP_DesignerReceipts(new Guid(userId), branchid, FromDate, ToDate, StartIndex, PageSize, SortBy, OrderBy, searchText).ToList();
            }
            return objList;
        }
        public SSP_DesignerReceiptById_Result ReceiptById(int id)
        {
            SSP_DesignerReceiptById_Result objList = new SSP_DesignerReceiptById_Result();
            using (PMSEntities objDB = new PMSEntities())
            {
                objList = objDB.SSP_DesignerReceiptById(id).SingleOrDefault();
            }
            return objList;
        }
    }
}