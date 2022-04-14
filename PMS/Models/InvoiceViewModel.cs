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
    public class InvoiceViewModel
    {
        public string UID { get; set; }

        public List<SelectListItem> PaymentTermList { get; set; }
        public List<SelectListItem> ContractTypeList { get; set; }
        public List<SelectListItem> SalesPersonList { get; set; }
        public List<SelectListItem> SuppliersList { get; set; }
        public List<SelectListItem> BillToList { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<SelectListItem> IsTaxList { get; set; }
        public List<tbl_invoice_items> InvoiceItemList { get; set; }
        public List<tbl_supplierinvoice_items> SupplierInvoiceItemList { get; set; }
        public List<tbl_designerinvoice_items> DesignerInvoiceItemList { get; set; }
        //public List<SelectListItem> DesignerList { get; set; }
        public List<SelectListItem> hdnProjectList { get; set; }
        public int hdnProjectId { get; set; }

        public int Id { get; set; }

        [Range(minimum: 1, maximum: 99999999, ErrorMessage = "Please select a company")]
        public int Company_id { get; set; }

        [Range(minimum: 1, maximum: 99999999, ErrorMessage = "Please select a customer")]
        public int Bill_to { get; set; }

        
        public string Designer_name { get; set; }

        [Required(ErrorMessage = "Enter invoice date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime Invoice_date { get; set; }

        [Range(minimum: 1, maximum: 99999999, ErrorMessage = "Please select a sales person")]
        public int Sales_person { get; set; }

        public string Invoice_number { get; set; }
        public int Payment_terms { get; set; }
        //[Required(ErrorMessage = "Enter contract number")]
        //public string Contract_number { get; set; }
        public Nullable<int> Contract_type { get; set; }
        //[Required(ErrorMessage = "Please enter tax")]
        public Nullable<decimal> tax { get; set; }

        public decimal tax_amount { get; set; }
        public bool is_tax { get; set; }
        public System.DateTime created_date { get; set; }
        public System.Guid created_by { get; set; }
        public System.DateTime modification_date { get; set; }
        public System.Guid modification_by { get; set; }

        [Required(ErrorMessage = "Enter from date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? SearchFrom { get; set; }
        [Required(ErrorMessage = "Enter to date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? SearchTo { get; set; }
        public int SearchBill_to { get; set; }
        public int SearchSales_person { get; set; }
        public int SearchSupplier_id { get; set; }

        [Range(minimum: 1, maximum: 99999999, ErrorMessage = "Please select a supplier")]
        public Int32 supplier_id { get; set; }
        public DateTime creation_date { get; set; }
        public Guid modified_by { get; set; }
        public string SearchText { get; set; }




        public List<tbl_invoice_items> invoice_details { get; set; }

        public static InvoiceViewModel FromJson(string val)
        {
            JObject paymentobj = JObject.Parse(val);
            InvoiceViewModel obj = new InvoiceViewModel();
            obj.invoice_details = new List<tbl_invoice_items>();

            string invdate = Convert.ToString(paymentobj["Invoice_date"]);
            obj.Invoice_date = Convert.ToDateTime(invdate);
            obj.Id = (int)paymentobj["Id"];
            obj.Invoice_number = (string)paymentobj["Invoice_number"];
            obj.Bill_to = (int)paymentobj["Bill_to"];
            obj.Sales_person = (int)paymentobj["Sales_person"];
            obj.Contract_type = (int)(paymentobj["Contract_type"]);
            obj.Company_id = (int)(paymentobj["Company_id"]);

            if (!String.IsNullOrEmpty((string)paymentobj["tax"]))
            {
                obj.tax = (decimal)(paymentobj["tax"]);
            }
            if (!String.IsNullOrEmpty((string)paymentobj["tax_amount"]))
            {
                obj.tax_amount = (decimal)(paymentobj["tax_amount"]);
            }
            else
            {
                obj.tax_amount = 0;
            }

            obj.is_tax = (bool)(paymentobj["is_tax"]);
            obj.Payment_terms = (int)(paymentobj["Payment_terms"]);

            List<tbl_invoice_items> invoicedetails = new List<tbl_invoice_items>();
            foreach (JObject pd in paymentobj["invoice_details"])
            {
                tbl_invoice_items d = new tbl_invoice_items();
                d.invoice_id = Convert.ToInt32(pd["invoice_id"]);
                d.item_description = Convert.ToString(pd["item_description"]);
                d.Contract_number = Convert.ToString(pd["Contract_number"]);
                if (Convert.ToString(pd["item_amount"]) == "")
                {
                    d.item_amount = 0;
                }
                else
                {
                    d.item_amount = Convert.ToDecimal(pd["item_amount"]);
                }
                d.project_id = Convert.ToInt32(pd["project_id"]);
                invoicedetails.Add(d);
            }
            obj.invoice_details.AddRange(invoicedetails);

            return obj;
        }

        public static InvoiceViewModel DesignerInvoiceFromJson(string val)
        {
            JObject paymentobj = JObject.Parse(val);
            InvoiceViewModel obj = new InvoiceViewModel();
            obj.invoice_details = new List<tbl_invoice_items>();

            string invdate = Convert.ToString(paymentobj["Invoice_date"]);
            obj.Invoice_date = Convert.ToDateTime(invdate);
            obj.Id = (int)paymentobj["Id"];
            obj.Invoice_number = (string)paymentobj["Invoice_number"];
            obj.Designer_name = (string)paymentobj["Designer_name"];
            obj.Contract_type = (int)(paymentobj["Contract_type"]);
            obj.Company_id = (int)(paymentobj["Company_id"]);

            if (!String.IsNullOrEmpty((string)paymentobj["tax"]))
            {
                obj.tax = (decimal)(paymentobj["tax"]);
            }
            if (!String.IsNullOrEmpty((string)paymentobj["tax_amount"]))
            {
                obj.tax_amount = (decimal)(paymentobj["tax_amount"]);
            }
            else
            {
                obj.tax_amount = 0;
            }

            obj.is_tax = (bool)(paymentobj["is_tax"]);
            obj.Payment_terms = (int)(paymentobj["Payment_terms"]);

            List<tbl_invoice_items> invoicedetails = new List<tbl_invoice_items>();
            foreach (JObject pd in paymentobj["invoice_details"])
            {
                tbl_invoice_items d = new tbl_invoice_items();
                d.invoice_id = Convert.ToInt32(pd["invoice_id"]);
                d.item_description = Convert.ToString(pd["item_description"]);
                d.Contract_number = Convert.ToString(pd["Contract_number"]);
                if (Convert.ToString(pd["item_amount"]) == "")
                {
                    d.item_amount = 0;
                }
                else
                {
                    d.item_amount = Convert.ToDecimal(pd["item_amount"]);
                }
                invoicedetails.Add(d);
            }
            obj.invoice_details.AddRange(invoicedetails);

            return obj;
        }

    }


    public class  InvoicePaymentViewmodel
    {
        public string receipt_date { get; set; }
        public string cheque_details { get; set; }
        public string salesmen_name { get; set; }
        public string branch_name { get; set; }
        public string mobile { get; set; }
        public string phone { get; set; }
        public string name1 { get; set; }
        public Nullable<int> mode_of_payment_id { get; set; }
        public string mode_of_payment_name { get; set; }
        public Nullable<decimal> amount { get; set; }
        public string invoice_numbers { get; set; }
        public string invoice_amount_items { get; set; }
        public Nullable<bool> is_tax { get; set; }
        public decimal tax { get; set; }
        public Nullable<System.DateTime> ReceiptDate { get; set; }
        public decimal tax_amount { get; set; }
        public string invoicedate { get; set; }
        public bool IsPayment { get; set; }
    }
}