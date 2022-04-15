using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace PMS.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        //[Required(ErrorMessage = "Please enter e-mail")]
        //[Display(Name = "Email")]
        //[EmailAddress(ErrorMessage = "Please enter valid e-mail")]
        //public string Email { get; set; }

        [Required(ErrorMessage = "Please enter user name")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter e-mail")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,})", ErrorMessage = "Password must contains 6 or more characters. Passwords must have at least one number, one lowercase ('a'-'z'), one uppercase ('A'-'Z') and one special character(@#$%).")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage ="Please enter user name")]
       // [StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [RegularExpression("^[a-zA-Z0-9_.@!]{6,15}$", ErrorMessage = "Enter alphanumeric user name between 6-15 characters in length")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter address 1")]
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address4 { get; set; }
        public string zip_code { get; set; }
        [Required(ErrorMessage = "Enter e-mail address")]
        [EmailAddress(ErrorMessage = "Enter valid e-mail address")]
       
        public string website { get; set; }
        public string PhoneNumber { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public Int32  country_id { get; set; }        
        public System.DateTime created_date { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public System.DateTime modified_date { get; set; }
        public Nullable<System.Guid> modified_by { get; set; }
        public bool is_active { get; set; }
        public string Id { get; set; }       
        public string msg { get; set; }
        public List<System.Web.Mvc.SelectListItem> StatusList { get; set; }
        public List<System.Web.Mvc.SelectListItem> CountryList { get; set; }

        public List<System.Web.Mvc.SelectListItem> SalesmanList { get; set; }
        public List<Database.SSP_UsersRoles_Result> RoleList { get; set; }

        public string  UserRoleName { get; set; }

        [Required(ErrorMessage = "Please select company")]
        public Int32 company_id { get; set; }

        public Int32 SalesmanId { get; set; }

        public List<System.Web.Mvc.SelectListItem> CompanyList { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter e-mail")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Please enter password")]
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,})", ErrorMessage = "Password must contains 6 or more characters. Passwords must have at least one number, one lowercase ('a'-'z'), one uppercase ('A'-'Z') and one special character(@#$%).")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage ="Enter e-mail address")]
        [EmailAddress(ErrorMessage = "Enter valid e-mail address")]
        [Display(Name = "Email")]
        public string UserEmail { get; set; }
    }

    public class HomeViewModel
    {
        public List<System.Web.Mvc.SelectListItem> BranchList { get; set; }
        public Int32 BranchID { get; set; }
        public string ReturnUrl { get; set; }
    }
}
