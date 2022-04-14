using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Database;

namespace PMS.Repository.Infrastructure
{
    public interface ILoanDetailRepository : IGenericRepository<loansDetail>
    {
        /// <summary>
        /// Remove_Loan Projects Details By LoanId
        /// </summary>
        /// <param name="loanId">foreign key {Relation Key}</param>
        /// <returns></returns>
        bool RemoveLoanDetailById(int loanId);
        /// <summary>
        /// Get data related to loan id which are exist in loan detail table as referred with loanId
        /// </summary>
        /// <param name="loanId">@sp_parameter</param>
        /// <returns></returns>
        List<SSP_GetLoanProjectsDetailsByLoanId_Result> GetLoanProjectsDetailsByLoanId(int loanId);
    }
}
