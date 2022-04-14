using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class SalesmenService : GenericRepository<PMSEntities, salesman>, ISalesmenRepositor
    {
        public List<SSP_Salesmen_Result> SearchSalesmen(Int32 branchid,Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, string SalesmenSearch)
        {
            List<SSP_Salesmen_Result> objList = new List<SSP_Salesmen_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                objList = objDB.SSP_Salesmen(branchid, StartIndex, PageSize, SortBy, OrderBy, SalesmenSearch).ToList();
            }
            return objList;
        }

    }
}