using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class CustomerService : GenericRepository<PMSEntities, customer>, ICustomerRepositor
    {
        public List<SSP_Customer_Result> SearchCustomer(Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy,string customersearch)
        {
            List<SSP_Customer_Result> objList = new List<SSP_Customer_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                objList = objDB.SSP_Customer(StartIndex, PageSize, SortBy, OrderBy, customersearch).ToList();
            }
            return objList;
        }

    }
}