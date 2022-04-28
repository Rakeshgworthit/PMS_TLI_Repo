using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PMS.Models
{
    public class ProjectViewModel
    {
        public string UID { get; set; }

        public long id { get; set; }
        [Required(ErrorMessage = "Enter contract date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> contract_date { get; set; }
        [Required(ErrorMessage = "Enter project number")]
        public string project_number { get; set; }

          [Required(ErrorMessage = "Enter project name")]
        public string project_name { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> project_start_date { get; set; }
       
        public Nullable<long> salesmen_id { get; set; }
        public Nullable<decimal> saleman_commission { get; set; }
        [Required(ErrorMessage = "Please select branch")]
        public Nullable<long> branch_id { get; set; }

        [Range(minimum: 1, maximum: 99999999, ErrorMessage = "Please select a customer")]
        public long customer_id { get; set; }

        [RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
        [Required(ErrorMessage = "Please enter contract amount")]
        public Nullable<decimal> contract_amount { get; set; }

        
        [RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid value")]
        [Required(ErrorMessage = "Please enter gst percentage")]
        public Nullable<decimal> gst_percentage { get; set; }

        [RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
        [Required(ErrorMessage = "Please enter gst amount")]
        public Nullable<decimal> gst_amount { get; set; }

        [RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
        [Required(ErrorMessage = "Please enter total amount")]
        public Nullable<decimal> total_amount { get; set; }


        public int bank_id { get; set; }
        public int status_id { get; set; }
        public string remarks { get; set; }
        public System.DateTime created_date { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public System.DateTime modified_date { get; set; }
        public Nullable<System.Guid> modified_by { get; set; }
        public bool isactive { get; set; }

        public Int32 brId { get; set; }
        public List<SelectListItem> SalesmenList { get; set; }
        public List<SelectListItem> BranchList { get; set; }
        public List<SelectListItem> CustomerList { get; set; }
        public List<SelectListItem> BankList { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> ActiveInactiveList { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? from_date { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? to_date { get; set; }

        public int? ProjectStatusId { get; set; }
        public Int32 ProjectSalesmenId { get; set; }

        public string SearchText { get; set; }
    }

    public class ProjectAdditionsViewModel
    {
        public string UID { get; set; }

        public long id { get; set; }
        [Required(ErrorMessage = "Enter date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public System.DateTime date { get; set; }

        [Required(ErrorMessage = "Please select a project")]
        [Range(minimum:1,maximum:99999999,ErrorMessage = "Please select a project")]
        public long project_id { get; set; }
        public string addition_omissioni_description { get; set; }

        [RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
        [Required(ErrorMessage = "Please enter amount")]
        public Nullable<decimal> amount { get; set; }

        [RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid value")]
        [Required(ErrorMessage = "Please enter gst percentage")]
        public Nullable<decimal> gst_percentage { get; set; }

        [RegularExpression("^-?[0-9]\\d*(\\.\\d+)?$", ErrorMessage = "Please enter valid amount")]
        [Required(ErrorMessage = "Please enter gst amount")]
        public Nullable<decimal> gst_amount { get; set; }

        [RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
        [Required(ErrorMessage = "Please enter total amount")]
        public Nullable<decimal> total_amount { get; set; }

        public Int32 record_type { get; set; }

        public string remarks { get; set; }
        public System.DateTime created_date { get; set; }
        public System.DateTime modified_date { get; set; }
        public System.Guid created_by { get; set; }
        public System.Guid modified_by { get; set; }
        public bool isactive { get; set; }
        public List<SelectListItem> ActiveInactiveList { get; set; }
        public List<SelectListItem> ProjectList { get; set; }
        public List<SelectListItem> AdditionTypeList { get; set; }
        public Int32 brId { get; set; }

        [Required(ErrorMessage = "Enter from date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? SearchFrom { get; set; }
        [Required(ErrorMessage = "Enter to date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? SearchTo { get; set; }
        public Int32 SearchProject { get; set; }

        public bool isProjectClosed { get; set; }
        public List<SelectListItem> SalesmenList { get; set; }
        public Int32 ProjectSalesmenId { get; set; }
        public string SearchText { get; set; }
    }

    public class ProjectDocumentViewModel
    {
        public long project_id { get; set; }
        public List<project_document_list> project_document_list { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(project_id)}={project_id.ToString()}, {nameof(project_document_list)}={project_document_list}}}";
        }
    }

    public class project_document_list
    {
        public long document_id { get; set; }
        public Int16  Id { get; set; }
        public long project_id { get; set; }
        public Guid uploaded_by { get; set; }
        public System.DateTime uploaded_on { get; set; }
        public Int32 Uploaded_By_Name { get; set; }
        public string file_name { get; set; }
        public string file_desc { get; set; }

        public string document_path { get; set; }
       
        public string project_number { get; set; }


        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id.ToString()}, {nameof(project_id)}={project_id.ToString()}, {nameof(uploaded_by)}={uploaded_by.ToString()}, {nameof(uploaded_on)}={uploaded_on.ToString()}, {nameof(Uploaded_By_Name)}={Uploaded_By_Name}, {nameof(file_name)}={file_name}, {nameof(file_desc)}={file_desc}}}";
        }
    }


}