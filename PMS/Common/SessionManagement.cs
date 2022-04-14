using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PMS.Common
{
    public static class SessionManagement
    {
        private static void SetSession<T>(string sessionId, T value)
        {
            HttpContext.Current.Session[sessionId] = value;
        }

        private static T GetSession<T>(string sessionId)
        {
            T val = default(T);
            var session = HttpContext.Current.Session;

            if (session[sessionId] != null)
                val = (T)session[sessionId];

            return val;
        }

        public static Guid UserID
        {
            get { return GetSession<Guid>("UserID"); }
            set { SetSession<Guid>("UserID", value); }
        }       

        public static String SelectedBranchName
        {
            get { return GetSession<String>("SelectedBranchName"); }
            set { SetSession<String>("SelectedBranchName", value); }
        }
        public static Int32 SelectedBranchID
        {
            get { return GetSession<Int32>("SelectedBranchID"); }
            set { SetSession<Int32>("SelectedBranchID", value); }
        }

        public static Int32 SelectedBranchBankId
        {
            get { return GetSession<Int32>("SelectedBranchBankId"); }
            set { SetSession<Int32>("SelectedBranchBankId", value); }
        }

        public static decimal BranchGST
        {
            get { return GetSession<decimal>("BranchGST"); }
            set { SetSession<decimal>("BranchGST", value); }
        }
    }
}
