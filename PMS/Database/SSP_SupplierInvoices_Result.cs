//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PMS.Database
{
    using System;
    
    public partial class SSP_SupplierInvoices_Result
    {
        public int Id { get; set; }
        public System.DateTime Invoicedate { get; set; }
        public string Invoice_date { get; set; }
        public string supplier_name { get; set; }
        public string CreatedUpdated { get; set; }
        public string company_name { get; set; }
        public string SupplierInvoiceItems { get; set; }
        public string SupplierInvoiceItemsSearch { get; set; }
        public Nullable<long> Rk { get; set; }
        public Nullable<int> TotalRecords { get; set; }
    }
}
