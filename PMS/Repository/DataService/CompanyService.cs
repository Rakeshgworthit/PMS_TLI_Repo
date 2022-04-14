using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class CompanyService : GenericRepository<PMSEntities, company>, ICompanyRepositor
    {
        public List<SSP_Company_Result> SearchCompany(Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy)
        {
            List<SSP_Company_Result> objList = new List<SSP_Company_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                objList = objDB.SSP_Company(StartIndex, PageSize, SortBy, OrderBy).ToList();
            }
            return objList;
        }

    }
}