using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class UserRepository : GenericRepository<PMSEntities, user_detail>, IuserRepository
    {
        public List<user_detail> SearchUser()
        {
            List<user_detail> objList = new List<user_detail>();
            using (PMSEntities objDB = new PMSEntities())
            {
                objList = objDB.user_detail.ToList();
            }
            return objList;
        }
       
        //public List<SSP_Users_Result> SearchUser(int StartIndex, int PageSize)
        //{
        //    List<SSP_Users_Result> objList = new List<SSP_Users_Result>();
        //    using (SingDBEntities1 objDB = new SingDBEntities1())
        //    {
        //        objList = objDB.SSP_Users(StartIndex, PageSize).ToList();
        //    }
        //    return objList;
        //}
    }
}