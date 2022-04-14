using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PMS.Models
{
    public class ExpenseCategotyViewModel
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Enter Expense Category name")]
        public string ExpenseCategory1 { get; set; }

        
        public int Parent { get; set; }

        public List<SelectListItem> ExpenseCategoryList { get; set; }
        public bool IsActive { get; set; }
        
    }
}