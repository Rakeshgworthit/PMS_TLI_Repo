using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;


namespace PMS.Repository.DataService
{
    public class ExpenseService : GenericRepository<PMSEntities, expense>, IExpense
    {
        public List<SSP_Expense_Result> GetMyExpenses(string userId, Int32 BranchId, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, DateTime fromdate, DateTime todate, string searchText)
        {
            List<SSP_Expense_Result> objList = new List<SSP_Expense_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                if (searchText == null) searchText = "";
                objList = objDB.SSP_Expense(new Guid(userId), BranchId, StartIndex, PageSize, SortBy, OrderBy, fromdate, todate, searchText).ToList();
            }
            return objList;
        }
    }
}