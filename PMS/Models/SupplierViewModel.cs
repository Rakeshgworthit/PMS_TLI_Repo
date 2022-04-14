using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PMS.Models
{
    public class SupplierViewModel
    {
        public long id { get; set; }
        [Required(ErrorMessage = "Please enter supplier name.")]
        public string supplier_name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address4 { get; set; }
        public string zip_code { get; set; }
        //[Required(ErrorMessage = "Please enter email.")]
        [EmailAddress(ErrorMessage = "Enter valid e-mail address")]
        public string email { get; set; }
        public string website { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string contact_person { get; set; }
        public string nric_no { get; set; }
        public Nullable<bool> gst_registered { get; set; }
        public string gst_no { get; set; }
        public System.DateTime created_date { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public System.DateTime modified_date { get; set; }
        public Nullable<System.Guid> modified_by { get; set; }
        public bool isactive { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> GSTRegisteredList { get; set; }
        [RegularExpression("^-?[0-9]\\d*(\\.\\d+)?$", ErrorMessage = "Please enter valid number.")]
        public decimal rebate_per { get; set; }
    }
}
