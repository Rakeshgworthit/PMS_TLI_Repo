using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Routing;
namespace PMS.Common
{
    public class BranchFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {            
            if (String.IsNullOrEmpty(SessionManagement.SelectedBranchName) || Convert.ToInt32(SessionManagement.SelectedBranchID) == 0)
            {
                filterContext.Result = new RedirectResult("/Home/Index" + "?ReturnUrl=" + Convert.ToString(HttpContext.Current.Request.ServerVariables["HTTP_REFERER"]));
            }
        }
    }
}