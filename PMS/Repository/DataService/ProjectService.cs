using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class ProjectService : GenericRepository<PMSEntities, project>, IProject
    {
        public List<SSP_Projects_Result> GetMyProjects(string userId, Int32 BranchId, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, DateTime fromdate, DateTime todate, Int32 projStatus, Int32 salesMenId, string searchText)
        {
            List<SSP_Projects_Result> objList = new List<SSP_Projects_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                if (searchText == null) searchText = "";
                objList = objDB.SSP_Projects(new Guid(userId), BranchId, StartIndex, PageSize, SortBy, OrderBy, fromdate, todate, projStatus, salesMenId, searchText).ToList();               
            }
            return objList;
        }
    }
}