using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Database;
namespace PMS.Repository.Infrastructure
{
    public interface ILoanRepositor : IGenericRepository<loan>
    {
        List<SSP_Loan_Result> SearchLoans(string userId, Int32 branchid, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, DateTime FromDate, DateTime ToDate);

    }
}