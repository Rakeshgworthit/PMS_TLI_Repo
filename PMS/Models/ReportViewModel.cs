using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;



namespace PMS.Models
{
    public class ReportViewModel
    {
        public string Uid { get; set; }
        public List<SelectListItem> MonthList { get; set; }
        public List<SelectListItem> YearList { get; set; }
        public Int32 BranchId { get; set; }
        public List<SelectListItem> BranchList { get; set; }
        public Int32 SalesmenId { get; set; }
        public List<SelectListItem> SalenmenList { get; set; }
        public List<SelectListItem> DesignerList { get; set; }
        public Int32 BankId { get; set; }
        public List<SelectListItem> BankList { get; set; }
        public Int32 ProjectId { get; set; }
        public List<SelectListItem> ProjectList { get; set; }
        public Int32 SupplierId { get; set; }
        public List<SelectListItem> SupplierList { get; set; }

        public Int32 ReportMonth { get; set; }
        public Int32 ReportYear { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        //[DataType(DataType.Date)]
        public DateTime? from_date { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        //[DataType(DataType.Date)]
        public DateTime? to_date { get; set; }
        public string GridData { get; set; }

        [RegularExpression("([0-9]+)", ErrorMessage = "Enter number")]
        public Int32? CheckNoFrom { get; set; }
        [RegularExpression("([0-9]+)", ErrorMessage = "Enter number")]
        public Int32? CheckNoTo { get; set; }

        public List<SelectListItem> AlphabetList { get; set; }
        public string Alphabet { get; set; }

        public List<SelectListItem> SalesmenAndUserList { get; set; }
        public string SalesmenOrUserId { get; set; }

        public string DesignerName { get; set; }
        public List<SelectListItem> SalesmenStatusList { get; set; }
        public string SearchSalesmenStatus { get; set; }
    }
}