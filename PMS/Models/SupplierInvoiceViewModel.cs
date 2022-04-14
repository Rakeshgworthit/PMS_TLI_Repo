using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PMS.Database;
using Newtonsoft.Json.Linq;

namespace PMS.Models
{
    public class SupplierInvoiceEditList
    {
        public tbl_supplierinvoice_items SupplierInvoice { get; set; }
        public List<SelectListItem> ProjectList { get; set; }
    }
    public class SupplierInvoiceViewModel
    {
        public string UID { get; set; }

        public List<SelectListItem> SuppliersList { get; set; }
        public List<SelectListItem> SalesPersonList { get; set; }
        public List<SupplierInvoiceEditList> SupplierInvoiceItemList { get; set; }
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter invoice date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Invoice_date { get; set; }

        [Range(minimum: 1, maximum: 99999999, ErrorMessage = "Please select a supplier")]
        public Int32 supplier_id { get; set; }
        public DateTime creation_date { get; set; }
        public Guid modified_by { get; set; }
        public Guid created_by { get; set; }

        //public int salesperson_id { get; set; }
        //public int project_id { get; set; }
        //public string invoice_number { get; set; }
        //public Nullable<decimal> gst { get; set; }
        //public Nullable<decimal> invoice_amt_without_gst { get; set; }
        //public Nullable<decimal> invoice_amt_with_gst { get; set; }
        //public Nullable<decimal> agreed_amt { get; set; }


        public List<tbl_supplierinvoice_items> invoice_details { get; set; }

        public static SupplierInvoiceViewModel FromJson(string val)
        {
            JObject paymentobj = JObject.Parse(val);
            SupplierInvoiceViewModel obj = new SupplierInvoiceViewModel();
            obj.invoice_details = new List<tbl_supplierinvoice_items>();

            string invdate = Convert.ToString(paymentobj["Invoice_date"]);
            obj.Invoice_date = Convert.ToDateTime(invdate);
            obj.Id = (int)paymentobj["Id"];
            obj.supplier_id = (int)paymentobj["supplier_id"];

            List<tbl_supplierinvoice_items> invoicedetails = new List<tbl_supplierinvoice_items>();
            foreach (JObject pd in paymentobj["invoice_details"])
            {
                tbl_supplierinvoice_items d = new tbl_supplierinvoice_items();
                d.Id = Convert.ToInt32(pd["Id"]);
                d.invoice_id = Convert.ToInt32(pd["invoice_id"]);
                d.salesperson_id = Convert.ToInt32(pd["item_salesperson"]);
                d.project_id = Convert.ToInt32(pd["item_project"]);
                d.invoice_number = (string)pd["item_invoicenum"];
                d.gst = (decimal)(pd["item_gst"]);
                d.invoice_amt_without_gst = (decimal)(pd["item_InvoiceAmtwithoutgst"]);
                d.invoice_amt_with_gst = (decimal)(pd["item_InvoiceAmtwithgst"]);
                d.agreed_amt_without_gst = Convert.ToDecimal((pd["item_AgreedAmtwithoutgst"]));
                d.agreed_amt = (decimal)(pd["item_AgreedAmt"]);

                invoicedetails.Add(d);
            }
            obj.invoice_details.AddRange(invoicedetails);

            return obj;
        }
    }
}