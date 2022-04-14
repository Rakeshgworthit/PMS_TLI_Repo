using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class BanksService : GenericRepository<PMSEntities, bank>, IBanksRepositor
    {
        public List<SSP_Banks_Result> SearchBanks(Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy)
        {

            List<SSP_Banks_Result> objList = new List<SSP_Banks_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                objList = objDB.SSP_Banks(StartIndex, PageSize, SortBy, OrderBy).ToList();
            }
            return objList;
        }

    }
}