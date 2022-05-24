using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PMS.Models
{
    public class LoanViewModel
    {

        public string UID { get; set; }

        public int Id { get; set; }
        public Nullable<long> branch_Id { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Enter loan date.")]
        public System.DateTime LoanDate { get; set; }

        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Please select type.")]
        public int rec_type { get; set; }

        [Required(ErrorMessage = "Please select person name.")]
        public string person_id { get; set; }
        public string person_type { get; set; }
        public string purpose { get; set; }

        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Please select payment mode.")]
        public int payment_mode { get; set; }
        public Nullable<int> bank_id { get; set; }
        public string cheque_number { get; set; }


        [RegularExpression("^-?[1-9]\\d*(\\.\\d+)?$", ErrorMessage = "Please enter valid amount.")]
        public decimal amount { get; set; }
        public System.Guid created_by { get; set; }
        public System.DateTime created_on { get; set; }
        public System.Guid updated_by { get; set; }
        public System.DateTime updated_on { get; set; }
        public bool isactive { get; set; }

        public int SalesmanId { get; set; }


        public List<SelectListItem> bankList { get; set; }
        public List<SelectListItem> branchList { get; set; }

        public List<SelectListItem> SalesmenAndUserList { get; set; }
        public List<SelectListItem> mode_of_paymentList { get; set; }
        public List<SelectListItem> IsActiveList { get; set; }
        public List<SelectListItem> RecordTypeList { get; set; }
        public Int32 brId { get; set; }

        [Required(ErrorMessage = "Enter from date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? SearchFrom { get; set; }
        [Required(ErrorMessage = "Enter to date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? SearchTo { get; set; }

        /// <summary>
        /// below are used for dynamic add project list as well as project amount
        /// </summary>
        public List<SelectListItem> projectList { get; set; }
        public Int32 searchProject { get; set; }
        public decimal projectAmount { get; set; }
        public List<CustomLoanDetailsViewModel> advanceLoanDetails { get; set; }
    }
}