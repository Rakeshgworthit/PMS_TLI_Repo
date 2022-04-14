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

    public class DesignerReceiptsViewModel
    {
        public string UID { get; set; }
        public long id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Enter receipt date.")]
        public Nullable<System.DateTime> receipt_date { get; set; }
        [Range(minimum: 1, maximum: 9999999, ErrorMessage = "Please select mode of payment")]
        public Nullable<int> mode_of_payment_id { get; set; }
        //[Range(minimum: 1, maximum: 9999999, ErrorMessage = "Please select a bank")]
        public Nullable<int> bank_id { get; set; }

        public string cheque_details { get; set; }

        [Range(minimum: 0, maximum: 9999999, ErrorMessage = "Please select branch.")]
        public Nullable<long> branch_id { get; set; }

        public string designer_name { get; set; }

        public string remarks { get; set; }
        public System.DateTime created_date { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public System.DateTime modified_date { get; set; }
        public Nullable<System.Guid> modified_by { get; set; }
        public bool isactive { get; set; }

        public List<SelectListItem> bankList { get; set; }
        public List<SelectListItem> branchList { get; set; }
        public List<SelectListItem> invoiceList { get; set; }

        public List<SelectListItem> designerList { get; set; }
        public List<SelectListItem> mode_of_paymentList { get; set; }
        public List<SelectListItem> designer_list { get; set; }
        public List<SelectListItem> IsActiveList { get; set; }
        public List<designer_receipt_details> receipt_details { get; set; }

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
        public string SearchText { get; set; }
        public static DesignerReceiptsViewModel FromJson(string val)
        {
            JObject receiptobj = JObject.Parse(val);
            DesignerReceiptsViewModel obj = new DesignerReceiptsViewModel();
            obj.receipt_details = new List<designer_receipt_details>();

            obj.id = (int)receiptobj["id"];

            string receiptdate = Convert.ToString(receiptobj["receipt_date"]);
            obj.bank_id = (int)receiptobj["bank_id"];
            obj.receipt_date = Convert.ToDateTime(receiptdate);
            obj.mode_of_payment_id = (int)receiptobj["mode_of_payment_id"];
            obj.cheque_details = (string)receiptobj["cheque_details"];
            obj.isactive = (bool)receiptobj["isactive"];
            obj.remarks = (string)receiptobj["remarks"];
            obj.designer_name = (string)receiptobj["designer_name"];

        List<designer_receipt_details> receiptdetails = new List<designer_receipt_details>();
            foreach (JObject pd in receiptobj["receipt_details"])
            {
                designer_receipt_details d = new designer_receipt_details();
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

    public class designer_receipt_details
    {
        public decimal amount { get; set; }
        public decimal gst_percentage { get; set; }
        public decimal gst_amount { get; set; }
        public decimal total_amount { get; set; }
        public Int32 invoice_id { get; set; }
    }
}