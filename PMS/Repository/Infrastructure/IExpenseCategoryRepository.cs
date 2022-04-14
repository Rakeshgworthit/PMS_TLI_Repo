using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Database;
namespace PMS.Repository.Infrastructure
{
    
    public interface IExpenseCategoryRepository : IGenericRepository<ExpenseCategory>
    {
        List<SSP_EXCategory_Result> SearchExpenseCategory(Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy);
    }
}
