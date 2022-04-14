using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class LoanDetailService : GenericRepository<PMSEntities, loansDetail>, ILoanDetailRepository
    {
        /// <summary>
        /// Get data related to loan id which are exist in loan detail table as referred with loanId
        /// </summary>
        /// <param name="loanId">@sp_parameter</param>
        /// <returns></returns>
        public List<SSP_GetLoanProjectsDetailsByLoanId_Result> GetLoanProjectsDetailsByLoanId(int loanId)
        {
            List<SSP_GetLoanProjectsDetailsByLoanId_Result> result = new List<SSP_GetLoanProjectsDetailsByLoanId_Result>();
            try
            {
                using (PMSEntities obj = new PMSEntities())
                {
                    result = obj.SSP_GetLoanProjectsDetailsByLoanId(loanId).ToList();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Something went wrong! {0}", ex.Message));
                //Need to throw exception if not deleted
            }
            return result;
        }

        /// <summary>
        /// Remove_Loan Projects Details By LoanId
        /// </summary>
        /// <param name="loanId">foreign key {Relation Key}</param>
        /// <returns></returns>
        public bool RemoveLoanDetailById(int loanId)
        {
            bool isRemoved = false;
            try
            {
                using (PMSEntities obj = new PMSEntities())
                {
                    obj.SSP_Remove_LoanProjectsDetailsByLoanId(loanId);
                    isRemoved = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Something went wrong! {0}", ex.Message));
                //Need to throw exception if not deleted
            }
            return isRemoved;
        }
    }
}