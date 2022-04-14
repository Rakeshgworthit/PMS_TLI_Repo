using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Database;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace PMS.Models
{
    public class CustomerViewModel
    {
        public long id { get; set; }
        [Required(ErrorMessage = "Enter name")]
        public string name1 { get; set; }
        public string name2 { get; set; }
        public string nric1 { get; set; }
        public string nric2 { get; set; }
        [Required(ErrorMessage = "Enter block number")]
        public string block_no { get; set; }
        [Required(ErrorMessage = "Enter job site")]
        public string job_site { get; set; }
        public string address { get; set; }
        public string contact_person { get; set; }
        //[Required(ErrorMessage = "Enter email address")]
        [EmailAddress(ErrorMessage = "Enter valid e-mail address")]
        public string email { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public Nullable<bool> gst_registered { get; set; }
        public string gst_no { get; set; }
        public System.DateTime created_date { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public System.DateTime modified_date { get; set; }
        public Nullable<System.Guid> modified_by { get; set; }
        public bool isactive { get; set; }

        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> GSTRegisterList { get; set; }
        public string customersearch { get; set; }

        
    }
}