using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PMS.Models
{
    public class ExpenseViewModel
    {

        public List<SelectListItem> bankList { get; set; }
        public int bank_id { get; set; }
        public string cheque_number { get; set; }
        public string Pay_To { get; set; }
       
        public string UID { get; set; }

            public long id { get; set; }
            [Required(ErrorMessage = "Enter expense date")]
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
            public Nullable<System.DateTime> Expense_Date { get; set; }
            
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
            [DataType(DataType.Date)]
            public Nullable<System.DateTime> project_start_date { get; set; }

          
            [Required(ErrorMessage = "Please select branch")]
            public Nullable<long> branch_id { get; set; }

           
            [RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
            [Required(ErrorMessage = "Please enter amount")]
            public Nullable<decimal> Amount { get; set; }


        [Range(minimum: 1, maximum: 99999999, ErrorMessage = "Please select a company")]
        public int Company_Id { get; set; }
        [Range(minimum: 1, maximum: 99999999, ErrorMessage = "Please select expense item")]
        public int Expense_Item_Id { get; set; }
        [Range(minimum: 1, maximum: 99999999, ErrorMessage = "Please select expense category")]
        public int Expense_Category_Id { get; set; }
      
        public int status_id { get; set; }
            public string remarks { get; set; }
            public System.DateTime created_date { get; set; }
            public Nullable<System.Guid> created_by { get; set; }
            public System.DateTime modified_date { get; set; }
            public Nullable<System.Guid> modified_by { get; set; }

            public Int32 brId { get; set; }
          //  public List<SelectListItem> SalesmenList { get; set; }
               
           public List<SelectListItem> CompanyList { get; set; }
        public List<SelectListItem> ExpenseCategoryList { get; set; }
        public List<SelectListItem> ExpenseItemList { get; set; }
       // public List<SelectListItem> StatusList { get; set; }
           // public List<SelectListItem> ActiveInactiveList { get; set; }
      
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime? from_date { get; set; }

            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime? to_date { get; set; }
        
        }
    }