using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PMS.Common;
using PMS.Models;
using static PMS.Common.CommonFunction;

namespace PMS.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    ApplicationUser user = UserManager.FindByName(model.UserName);
                    Guid userid = new Guid(user.Id);
                    Boolean isActive = true;
                    try
                    {
                        using (Database.PMSEntities objDB = new Database.PMSEntities())
                        {
                            isActive = Convert.ToBoolean(objDB.user_detail.Where(o => o.user_id == userid).Select(o => o.is_active).SingleOrDefault());
                        }
                        if (isActive)
                        {
                            return RedirectToLocal("/home/index");
                        }
                        else
                        {
                            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                            ModelState.AddModelError("", "Account in-active.");
                            return View(model);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                   
                    

                //return RedirectToLocal("/home/index");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //// POST: /Account/Login
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(LoginViewModel model, string returnUrl)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // This doesn't count login failures towards account lockout
        //    // To enable password failures to trigger account lockout, change to shouldLockout: true
        //    var result = SignInManager.PasswordSignIn(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal("/home/index");
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
        //        case SignInStatus.Failure:
        //        default:
        //            ModelState.AddModelError("", "Invalid login attempt.");
        //            return View(model);
        //    }
        //}

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        //[AllowAnonymous]
        //public ActionResult Register()
        //{
        //    return View();
        //}

        //
        // POST: /Account/Register
        [HttpPost]        
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            string uid = User.Identity.GetUserId();
            RegisterViewModel ObjView = new RegisterViewModel();
            //ObjView.SalesmanList = CommonFunction.Active_Salesmen("");
            //if (ModelState.IsValid)
            //{
            if (String.IsNullOrEmpty(model.Id))
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    using (Database.PMSEntities obj = new Database.PMSEntities())
                    {
                        Database.user_detail ud = new Database.user_detail()
                        {
                            
                            user_id = new Guid(user.Id),
                            Name = model.Name,
                            mobile = model.mobile,
                            phone = model.phone,
                            address1 = model.address1,
                            address2 = model.address2,
                            address3 = model.address3,
                            address4 = model.address4,
                            country_id = model.country_id,
                            website = model.website,
                            zip_code = model.zip_code,
                            is_active = model.is_active,
                            created_by = new Guid(uid),
                            created_date = DateTime.Now,
                            modified_by = new Guid(uid),
                            modified_date = DateTime.Now,
                            company_id = model.company_id
                        };
                        obj.user_detail.Add(ud);
                        obj.SaveChanges();
                    }

                    UserManager.AddToRole(user.Id, "User");

                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Edit", "Account", new { Id = user.Id, msg = "1" });
                }
                string str = "";
                foreach (var err in result.Errors)
                {
                    str += err + " ";
                }
                return RedirectToAction("Edit", "Account", new { msg = str });
                //AddErrors(result);
                //return RedirectToAction("Index", "Home");
            }
            else
            {
                /* Edit User */
                using (Database.PMSEntities obj = new Database.PMSEntities())
                {
                    Database.user_detail ud = obj.user_detail.Where(o => o.user_id == new Guid(model.Id)).SingleOrDefault();
                    if (ud != null)
                    {
                        model.created_by = ud.created_by;
                        model.created_date = Convert.ToDateTime(ud.created_date);
                        Common.CommonFunction.MergeObjects(ud, model, true);
                        ud.user_id = new Guid(model.Id);
                        ud.modified_by = new Guid(uid);
                        ud.modified_date = DateTime.Now;
                        obj.SaveChanges();
                    }

                    Int32 numEmail = obj.AspNetUsers.Where(o => o.Email == model.Email && o.Id != model.Id).Count();

                    if (numEmail == 0)
                    {
                        Database.AspNetUser au = obj.AspNetUsers.Where(o => o.Id == model.Id).SingleOrDefault();
                        if (au != null)
                        {
                            au.Email = model.Email;
                            //au.PhoneNumber = model.PhoneNumber;
                            obj.SaveChanges();
                        }
                    }
                    else
                    {
                        return RedirectToAction("Edit", "Account", new { Id = model.Id, msg = "3" });                        
                    }
                }

                return RedirectToAction("Edit", "Account", new { Id = model.Id, msg = "2" });
            }
            //}

            // If we got this far, something failed, redisplay form
            //return RedirectToAction("Edit", "Account", new { Id = model.Id });
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Users()
        {
            return View();
        }
       
        public ActionResult Edit(string Id, string msg)
        {
            RegisterViewModel objView = new RegisterViewModel();
            objView.StatusList = Common.CommonFunction.StatusList();
            objView.CountryList = Common.CommonFunction.CountryList();
            objView.RoleList = Common.CommonFunction.UserRoleList(Id);
            objView.CompanyList = Common.CommonFunction.BranchList(true);
            objView.SalesmanList = Common.CommonFunction.Active_Salesmen("");
            if (!String.IsNullOrEmpty(Id))
            {
                using (Database.PMSEntities obj = new Database.PMSEntities())
                {
                    Database.user_detail ud = obj.user_detail.Where(o => o.user_id == new Guid(Id)).SingleOrDefault();
                    if (ud != null)
                    {
                        Common.CommonFunction.MergeObjects(objView, ud, true);
                        objView.SalesmanId = CommonFunction.GetSalesmanIdByUser(Id);
                    }

                    Database.AspNetUser au = obj.AspNetUsers.Where(o => o.Id == Id).SingleOrDefault();
                    if (au != null)
                    {
                        objView.Id = Id;
                        objView.UserName = au.UserName;
                        objView.Email = au.Email;
                        // objView.PhoneNumber = au.PhoneNumber;                       

                        
                    }
                    System.Collections.Generic.List<Database.SSP_UsersRolesById_Result> userrolelst = new System.Collections.Generic.List<Database.SSP_UsersRolesById_Result>();
                    userrolelst = obj.SSP_UsersRolesById(Id).ToList();
                    if (userrolelst.Count > 0)
                    {
                        objView.UserRoleName = Convert.ToString(userrolelst[0].Name);
                    }
                    else {
                        objView.UserRoleName = "";
                    }

                }
            }
            else
            {
                objView.is_active = true;
            }

            if (msg != "")
            {
                if (msg == "1")
                {
                    ViewBag.MSG = "User record added successfully.";
                }
                else if (msg == "2")
                {
                    ViewBag.MSG = "User record updated successfully.";
                }
                else if (msg == "3")
                {
                    ViewBag.MSG = "User with this email address already exist.";
                }
                else
                {
                    ViewBag.MSG = msg;
                }
            }

            return View(objView);
        }
        

        public JsonResult UpdateRole(string Id,string roleId,string SalesmanId)
        {
            string BranchId = "";
            SuccessMessage successMessage = new SuccessMessage();
            UserManager.RemoveFromRole(Id, "SuperAdmin");
            UserManager.RemoveFromRole(Id, "User");
            UserManager.AddToRole(Id, roleId);

            //if (roleValue)
            //{               
            //    UserManager.AddToRole(Id, roleId);
            //} else
            //{
            //    UserManager.RemoveFromRole(Id, roleId);
            //}            
            if (!string.IsNullOrEmpty(SalesmanId) && !string.IsNullOrEmpty(Id))
            {
                BranchId = Convert.ToString(Common.SessionManagement.SelectedBranchID);
                successMessage = CommonFunction.UpdateSalemanUser(Id, SalesmanId, BranchId);
            }
            if (successMessage.Result == "1")
            {
                return Json(new { msg = successMessage.Errormessage, cls = "success" });
            }
            else if (successMessage.Result == "-1")
            {
                return Json(new { msg = successMessage.Errormessage, cls = "warning" });
            }
            else
            {
                return Json(new { msg = "The Salesman mapping is failed.", cls = "error" });
            }

        }

        public ActionResult UserBranches(string userId)
        {
            System.Collections.Generic.List<Database.SSP_UserBranches_Result> branchList = new System.Collections.Generic.List<Database.SSP_UserBranches_Result>();
            using (Database.PMSEntities objDB = new Database.PMSEntities())
            {
                branchList = objDB.SSP_UserBranches(new Guid(userId)).ToList();
            }

            return View(branchList);
        }
        
        public JsonResult AssignBranch(string userId, Int32 Id)
        {
            string uid = User.Identity.GetUserId();
            using (Database.PMSEntities objDB = new Database.PMSEntities())
            {
                Int32 totalBranch = objDB.user_access_rights.Where(o => o.branch_id == Id && o.user_id == new Guid(userId)).Count();
                if(totalBranch == 0)
                {
                    Database.user_access_rights objRec = new Database.user_access_rights
                    {
                        user_id = new Guid(userId),
                        branch_id = Id,
                        created_by = new Guid(uid),
                        created_date = DateTime.Now,
                        modified_by = new Guid(uid),
                        modified_date = DateTime.Now,
                        isactive = true
                    };
                    objDB.user_access_rights.Add(objRec);
                    objDB.SaveChanges();
                    return Json(new { msg = "Branch assign successfully.", cls = "success" });
                }
                else
                {
                    return Json(new { msg = "Branch already assigned.", cls = "error" });
                }
            }           
        }
        
        public JsonResult RemoveBranch(Int32 Id)
        {
            using (Database.PMSEntities objDB = new Database.PMSEntities())
            {
                Database.user_access_rights objRec = objDB.user_access_rights.Where(o => o.id == Id).SingleOrDefault();
                if(objRec != null)
                {
                    objDB.user_access_rights.Remove(objRec);
                    objDB.SaveChanges();
                }
            }

            return Json(new { msg = "Branch removed successfully.", cls = "success" });
        }
       
        public JsonResult ChangeStatus(Int32 Id)
        {
            using (Database.PMSEntities objDB = new Database.PMSEntities())
            {
                Database.user_access_rights objRec = objDB.user_access_rights.Where(o => o.id == Id).SingleOrDefault();
                if (objRec != null)
                {
                    objRec.isactive = objRec.isactive ? false : true;
                    objDB.SaveChanges();
                }
            }

            return Json(new { msg = "Status updated successfully.", cls = "success" });
        }

        public SuccessMessage UpdateSalesmenRole(string Id, string supplierId)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();

            using (Database.PMSEntities objDB = new Database.PMSEntities())
            {
                //objDB.(Convert.ToInt32(Id));

            }
            return _SuccessMessage;
        }
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]        
        public JsonResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindByName(model.UserEmail);
                //if (user == null || !(UserManager.IsEmailConfirmed(user.Id)))
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return Json(new { msg = "User not found.", cls = "error" });
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                 string code = UserManager.GeneratePasswordResetToken(user.Id);
                 var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                string msg = "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>";
                //Common.MailObject smtp = new Common.MailObject("admin@pms.com", model.Email, "Reset Password Pms.com", msg);
                //smtp.SendMail();

               // UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
               //  return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return Json(new { msg = "Please check your email to reset your password.", cls = "success" });
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}