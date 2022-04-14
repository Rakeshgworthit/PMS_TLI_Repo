using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class ExpenseCategoryService : GenericRepository<PMSEntities, ExpenseCategory> , IExpenseCategoryRepository
    {
        public List<SSP_EXCategory_Result> SearchExpenseCategory(Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy)
        {
            List<SSP_EXCategory_Result> objList = new List<SSP_EXCategory_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                objList = objDB.SSP_EXCategory(StartIndex, PageSize, SortBy, OrderBy).ToList();
            }
            return objList;
        }
    }
}