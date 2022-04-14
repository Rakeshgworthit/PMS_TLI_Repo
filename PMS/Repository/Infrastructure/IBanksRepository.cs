using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Database;
namespace PMS.Repository.Infrastructure
{
    public interface IBanksRepositor : IGenericRepository<bank>
    {
        List<SSP_Banks_Result> SearchBanks(Int32 StartIndex, Int32 PageSize,string SortBy, string OrderBy);

    }
}