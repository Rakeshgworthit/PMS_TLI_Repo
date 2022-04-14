using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Database;
namespace PMS.Repository.Infrastructure
{
    public interface IProject : IGenericRepository<project>
    {
        List<SSP_Projects_Result> GetMyProjects(string userId, Int32 BranchId, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, DateTime fromdate, DateTime todate, Int32 projStatus, Int32 salesMenId, string searchText);
    }
}
