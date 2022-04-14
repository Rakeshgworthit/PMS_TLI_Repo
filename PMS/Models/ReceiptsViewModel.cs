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
    public class ReceiptsViewModel
    {
        public string UID { get; set; }
        public string paymentdet_project_id { get; set; }
        public long id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Enter receipt date.")]
        public Nullable<System.DateTime> receipt_date { get; set; }
        //[Range(minimum: 1, maximum: 9999999, ErrorMessage = "Please select project name")]
        //public Nullable<long> project_id { get; set; }
        [Range(minimum: 1, maximum: 9999999, ErrorMessage = "Please select mode of payment")]
        public Nullable<int> mode_of_payment_id { get; set; }
        //[Range(minimum: 1, maximum: 9999999, ErrorMessage = "Please select a bank")]
        public Nullable<int> bank_id { get; set; }

        public string cheque_details { get; set; }

        [Range(minimum: 1, maximum: 9999999, ErrorMessage = "Please select a customer")]
        public Nullable<int> customer_id { get; set; }

        //[Range(minimum: 1, maximum:int.MaxValue, ErrorMessage = "The amount must be greater than 0")]

        //[RegularExpression("^-?[1-9]\\d*(\\.\\d+)?$", ErrorMessage = "Please enter valid amount")]
        //public Nullable<decimal> amount { get; set; }

        //[RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid value")]
        //[Required(ErrorMessage = "Please enter gst percentage")]
        //public Nullable<decimal> gst_percentage { get; set; }

        //[RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
        //[RegularExpression("^-?[0-9]\\d*(\\.\\d+)?$", ErrorMessage = "Please enter valid amount")]
        //[Required(ErrorMessage = "Please enter gst amount")]
        //public Nullable<decimal> gst_amount { get; set; }

        //[RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
        //[RegularExpression("^-?[1-9]\\d*(\\.\\d+)?$", ErrorMessage = "Please enter valid amount")]
        //[Required(ErrorMessage = "Please enter total amount")]
        //public Nullable<decimal> total_amount { get; set; }


        [Range(minimum: 1, maximum: 9999999, ErrorMessage = "Please select salesman.")]
        public Nullable<long> sales_man_id { get; set; }

        [Range(minimum: 0, maximum: 9999999, ErrorMessage = "Please select branch.")]
        public Nullable<long> branch_id { get; set; }
        public string remarks { get; set; }
        public System.DateTime created_date { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public System.DateTime modified_date { get; set; }
        public Nullable<System.Guid> modified_by { get; set; }
        public bool isactive { get; set; }

        public List<SelectListItem> bankList { get; set; }
        public List<SelectListItem> branchList { get; set; }
        public List<SelectListItem> projectList { get; set; }
        public List<SelectListItem> invoiceList { get; set; }
        public List<CustomSelectListItem> invoiceListWithProjects { get; set; }

        public List<SelectListItem> salesmanList { get; set; }
        public List<SelectListItem> mode_of_paymentList { get; set; }
        public List<SelectListItem> IsActiveList { get; set; }
        public List<receipt_details> receipt_details { get; set; }
        public List<SelectListItem> customerList { get; set; }

        public Int32 brId { get; set; }
        public Int32 hdnInvoiceId { get; set; }

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
        public Int32 ProjectSalesmenId { get; set; }
        public string SearchText { get; set; }
        public static ReceiptsViewModel FromJson(string val)
        {
            JObject receiptobj = JObject.Parse(val);
            ReceiptsViewModel obj = new ReceiptsViewModel();
            obj.receipt_details = new List<receipt_details>();

            obj.id = (int)receiptobj["id"];

            string receiptdate = Convert.ToString(receiptobj["receipt_date"]);
            obj.bank_id = (int)receiptobj["bank_id"];
            obj.receipt_date = Convert.ToDateTime(receiptdate);
            obj.mode_of_payment_id = (int)receiptobj["mode_of_payment_id"];
            obj.cheque_details = (string)receiptobj["cheque_details"];
            obj.isactive = (bool)receiptobj["isactive"];
            obj.remarks = (string)receiptobj["remarks"];
            obj.customer_id = (int)receiptobj["customer_id"];
            obj.sales_man_id = (int)receiptobj["sales_man_id"];

            List<receipt_details> receiptdetails = new List<receipt_details>();
            foreach (JObject pd in receiptobj["receipt_details"])
            {
                //if (Convert.ToString(pd["supplier_inv_number"]) != "")
                //{
                receipt_details d = new receipt_details();
                d.project_id = Convert.ToInt32(pd["project_id"]);
                if (Convert.ToString(pd["amount"]) == "")
                {
                    d.amount = 0;
                }
                else
                {
                    d.amount = Convert.ToDecimal(pd["amount"]);
                }
                if (Convert.ToString(pd["gst_percentage"]) == "")
                {
                    d.gst_percentage = 0;
                }
                else
                {
                    d.gst_percentage = Convert.ToDecimal(pd["gst_percentage"]);
                }
                if (Convert.ToString(pd["gst_amount"]) == "")
                {
                    d.gst_amount = 0;
                }
                else
                {
                    d.gst_amount = Convert.ToDecimal(pd["gst_amount"]);
                }
                if (Convert.ToString(pd["total_amount"]) == "")
                {
                    d.total_amount = 0;
                }
                else
                {
                    d.total_amount = Convert.ToDecimal(pd["total_amount"]);
                }

                d.invoice_id = (int)(pd["invoice_id"]);

                receiptdetails.Add(d);
                //}
            }
            obj.receipt_details.AddRange(receiptdetails);

            return obj;
        }
    }

    public class receipt_details
    {
        // [Range(minimum: 1, maximum: 9999999, ErrorMessage = "Select select project.")]
        //[Required(ErrorMessage = "Please select project.")]
        public long project_id { get; set; }
        public decimal amount { get; set; }
        public decimal gst_percentage { get; set; }
        public decimal gst_amount { get; set; }
        public decimal total_amount { get; set; }
        public Int32 invoice_id { get; set; }
    }
}