using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Database;
namespace PMS.Repository.Infrastructure
{
    public interface ICompanyRepositor : IGenericRepository<company>
    {
        List<SSP_Company_Result> SearchCompany(Int32 StartIndex, Int32 PageSize,string SortBy, string OrderBy);
    }
}