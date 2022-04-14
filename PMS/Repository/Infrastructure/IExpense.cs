using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Database;
namespace PMS.Repository.Infrastructure
{
    public interface IExpense : IGenericRepository<expense>
    {
        List<SSP_Expense_Result> GetMyExpenses(string userId, Int32 BranchId, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, DateTime fromdate, DateTime todate, string searchText);
    }

   
}