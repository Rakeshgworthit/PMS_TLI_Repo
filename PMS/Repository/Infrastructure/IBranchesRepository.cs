using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Database;
namespace PMS.Repository.Infrastructure
{
    public interface IBranchesRepositor : IGenericRepository<branch>
    {
        List<SSP_Branches_Result> SearchBranches(Int32 StartIndex, Int32 PageSize,string SortBy, string OrderBy);
    }
}