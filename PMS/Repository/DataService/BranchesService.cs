using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class BranchesService : GenericRepository<PMSEntities, branch>, IBranchesRepositor
    {
        public List<SSP_Branches_Result> SearchBranches(Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy)
        {
            List<SSP_Branches_Result> objList = new List<SSP_Branches_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                objList = objDB.SSP_Branches(StartIndex, PageSize, SortBy, OrderBy).ToList();
            }
            return objList;
        }

    }
}