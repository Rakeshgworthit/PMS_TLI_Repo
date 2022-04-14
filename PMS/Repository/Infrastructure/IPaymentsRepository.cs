using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Database;
namespace PMS.Repository.Infrastructure
{
    public interface IPaymentsRepositor : IGenericRepository<payment>
    {
        List<SSP_Payments_Result> SearchPayments(string userId,Int32 branchid, Int32 StartIndex, Int32 PageSize,string SortBy, string OrderBy, DateTime FromDate, DateTime ToDate, Int32 ProjectId, Int32 ProjectSalesmenId, string searchText);
        SSP_PaymentById_Result PaymentById(Int32 id);
    }
}