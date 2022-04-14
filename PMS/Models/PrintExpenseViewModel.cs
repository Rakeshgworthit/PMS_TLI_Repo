using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public class PrintExpenseViewModel
    {
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string BranchName { get; set; }
        public string ExpenseDate { get; set; }
        public string BankName { get; set; }
        public string ChequeNumber { get; set; }
        public decimal? Amount { get; set; }
        public string Remark { get; set; }
        public string Branch_Address { get; set; }
        public string Branch_Phone { get; set; }
        public string Branch_ZipCode { get; set; }
        public string Pay_To { get; set; }
    }
}