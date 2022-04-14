using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Database;
namespace PMS.Repository.Infrastructure
{
    public interface IProjectAdditionRepository : IGenericRepository<additions_omissions>
    {
        List<SSP_Project_additions_Result> GetMyProjects(string userId, Int32 BranchId, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, DateTime FromDate, DateTime ToDate, Int32 ProjectId, Int32 ProjectSalesmenId,string searchText);
    }
}
