using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class ProjectAdditionService : GenericRepository<PMSEntities, additions_omissions>, IProjectAdditionRepository
    {        
        public List<SSP_Project_additions_Result> GetMyProjects(string userId, int BranchId, int StartIndex, int PageSize, string SortBy, string OrderBy, DateTime FromDate, DateTime ToDate, int ProjectId, int ProjectSalesmenId, string searchText)
        {

            List<SSP_Project_additions_Result> objList = new List<SSP_Project_additions_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                if (searchText == null) searchText = "";
                objList = objDB.SSP_Project_additions(new Guid(userId), BranchId, FromDate, ToDate, ProjectId, StartIndex, PageSize, SortBy, OrderBy, ProjectSalesmenId, searchText).ToList();
            }
            return objList;

        }
    }
}