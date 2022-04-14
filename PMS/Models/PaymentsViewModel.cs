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
    
    public class PaymentChkExistingViewModel
    {
        public long id { get; set; }     
      //  public long project_id { get; set; }
        public long supplier_id { get; set; }        
        public List<payment_details_chk_Existing> payment_details_chk_Existing { get; set; }
        public static PaymentChkExistingViewModel FromJson(string val)
        {
            JObject paymentobj = JObject.Parse(val);
            PaymentChkExistingViewModel obj = new PaymentChkExistingViewModel();
            obj.payment_details_chk_Existing = new List<payment_details_chk_Existing>();
            obj.id = (int)paymentobj["id"];
            obj.supplier_id = (int)paymentobj["supplier_id"];
          //  obj.project_id = (int)paymentobj["project_id"];
            List<payment_details_chk_Existing> paymentdetails = new List<payment_details_chk_Existing>();
            foreach (JObject pd in paymentobj["payment_details_chk_Existing"])
            {              
                payment_details_chk_Existing d = new payment_details_chk_Existing();
                d.project_id = Convert.ToInt32(pd["project_id"]);
                d.supplier_inv_number = Convert.ToString(pd["supplier_inv_number"]);
                paymentdetails.Add(d);                
            }
            obj.payment_details_chk_Existing.AddRange(paymentdetails);
            return obj;
        }
    }

    public class PaymentsViewModel
    {
        public List<Database.SSP_GetInvoiceList_Result> InvoiceList { get; set; }
        public string UID { get; set; }
        public string paymentdet_project_id { get; set; }
        public long id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Enter payment date.")]
        public Nullable<System.DateTime> payment_date { get; set; }

        public Nullable<System.DateTime> collection_date { get; set; }

        [Range(minimum: 1, maximum: 9999999, ErrorMessage = "Select select supplier.")]
        [Required(ErrorMessage = "Please select supplier.")]
        public long supplier_id { get; set; }


        public string supplier_inv_number { get; set; }
        [RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
        public Nullable<decimal> invoice_amount { get; set; }
        [RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
        public Nullable<decimal> gst_amount { get; set; }
        [RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
        public Nullable<decimal> payment_amount { get; set; }

        //[Range(minimum: 1, maximum: 9999999, ErrorMessage = "Select select bank.")]
        //[Required(ErrorMessage = "Please select bank.")]
        public int bank_id { get; set; }

        //[Required(ErrorMessage = "Please enter cheque number.")]
        public string cheque_number { get; set; }

        [RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
        public Nullable<decimal> rebate_amount { get; set; }
        [Required(ErrorMessage = "Enter Actual Amount Received.")]
        [RegularExpression("((\\-?\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
        public decimal paid_amount { get; set; }
        public decimal loan_return { get; set; }

        public string remarks { get; set; }
        public System.DateTime created_date { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public System.DateTime modified_date { get; set; }
        public Nullable<System.Guid> modified_by { get; set; }
        public bool isactive { get; set; }
        public List<SelectListItem> projectList { get; set; }
        public List<SelectListItem> supplierList { get; set; }
        public List<SelectListItem> bankList { get; set; }
        public List<SelectListItem> IsActiveList { get; set; }
        public List<SelectListItem> mode_of_paymentList { get; set; }
        public List<payment_details> payment_details { get; set; }
        public Int32 payment_mode { get; set; }

        public List<SelectListItem> SalesmenList { get; set; }
        public Int32 ProjectSalesmenId { get; set; }

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
        public string SearchText { get; set; }
        public static PaymentsViewModel FromJson(string val)
        {
            JObject paymentobj = JObject.Parse(val);
            PaymentsViewModel obj = new PaymentsViewModel();
            obj.payment_details = new List<payment_details>();

            obj.id = (int)paymentobj["id"];

            string paymentdate = Convert.ToString(paymentobj["payment_date"]);

            obj.payment_date = Convert.ToDateTime(paymentdate);
            string collectiondate = Convert.ToString(paymentobj["collection_date"]);
            if (!String.IsNullOrEmpty(collectiondate))
            {
                obj.collection_date = Convert.ToDateTime(collectiondate);
            }
            else
            {
                obj.collection_date = null;
            }
            obj.supplier_id = (int)paymentobj["supplier_id"];
            obj.cheque_number = (string)paymentobj["cheque_number"];
            obj.isactive = (bool)paymentobj["isactive"];
           // obj.project_id = (int)paymentobj["project_id"];
            obj.bank_id = (int)paymentobj["bank_id"];
            obj.rebate_amount = Convert.ToDecimal(paymentobj["rebate_amount"]);
            obj.paid_amount = Convert.ToDecimal(paymentobj["paid_amount"]);
            obj.loan_return = Convert.ToDecimal(paymentobj["loan_return"]);
            obj.remarks = (string)paymentobj["remarks"];
            obj.payment_mode = (int)paymentobj["payment_mode"];
            List<payment_details> paymentdetails = new List<payment_details>();
            foreach (JObject pd in paymentobj["payment_details"])
            {
                //if (Convert.ToString(pd["supplier_inv_number"]) != "")
                //{
                    payment_details d = new payment_details();
                //d.project_id = Convert.ToInt32(pd["project_id"]);
                //d.supplier_inv_number = Convert.ToString(pd["supplier_inv_number"]);
                //    if (Convert.ToString(pd["invoice_amount"]) == "")
                //    {
                //        d.invoice_amount = 0;
                //    }
                //    else
                //    {
                //        d.invoice_amount = Convert.ToDecimal(pd["invoice_amount"]);
                //    }
                //    if (Convert.ToString(pd["gst_percentage"]) == "")
                //    {
                //        d.gst_percentage = 0;
                //    }
                //    else {
                //        d.gst_percentage = Convert.ToDecimal(pd["gst_percentage"]);
                //    }
                //    if (Convert.ToString(pd["gst_amount"]) == "")
                //    {
                //        d.gst_amount = 0;
                //    }
                //    else {
                //        d.gst_amount = Convert.ToDecimal(pd["gst_amount"]);
                //    }

                d.invoice_id = Convert.ToInt32(pd["invoice_id"]);
                //d.projet_id = Convert.ToInt32(pd["project_id"]);

                if (Convert.ToString(pd["payment_amount"]) == "")
                    {
                        d.payment_amount = 0;
                    }
                    else {
                        d.payment_amount = Convert.ToDecimal(pd["payment_amount"]);
                    }

                if (Convert.ToString(pd["agreed_amount"]) == "")
                {
                    d.agreed_amount = 0;
                }
                else
                {
                    d.agreed_amount = Convert.ToDecimal(pd["agreed_amount"]);
                }
                if (Convert.ToString(pd["loan_amount"]) == "")
                {
                    d.loan_amount = 0;
                }
                else
                {
                    d.loan_amount = Convert.ToDecimal(pd["loan_amount"]);
                }
                paymentdetails.Add(d);
                //}
            }
            obj.payment_details.AddRange(paymentdetails);

            return obj;
        }
    }

    public class payment_details
    {
        // [Range(minimum: 1, maximum: 9999999, ErrorMessage = "Select select project.")]
        //[Required(ErrorMessage = "Please select project.")]        
        public int payment_id { get; set; }
        public Nullable<int> invoice_id { get; set; }
        public Nullable<decimal> agreed_amount { get; set; }
        public Nullable<decimal> payment_amount { get; set; }
        public Nullable<decimal> loan_amount { get; set; }
    }
    public class payment_details_chk_Existing
    {
        public string supplier_inv_number { get; set; }
        public long project_id { get; set; }
    }


}