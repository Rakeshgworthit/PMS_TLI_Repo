using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Database;
namespace PMS.Repository.Infrastructure
{
    public interface ICustomerRepositor : IGenericRepository<customer>
    {
        List<SSP_Customer_Result> SearchCustomer(Int32 StartIndex, Int32 PageSize,string SortBy, string OrderBy,string customersearch);
    }
}