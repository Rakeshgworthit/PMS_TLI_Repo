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
    
    public partial class SSP_Invoices_Result
    {
        public int Id { get; set; }
        public string Invoice_number { get; set; }
        public decimal tax_amount { get; set; }
        public System.DateTime Invoicedate { get; set; }
        public string Invoice_date { get; set; }
        public string salesmen_name { get; set; }
        public string customer_name { get; set; }
        public string CreatedUpdated { get; set; }
        public string company_name { get; set; }
        public decimal tax { get; set; }
        public Nullable<bool> is_tax { get; set; }
        public Nullable<long> Rk { get; set; }
        public Nullable<long> TotalRecords { get; set; }
        public string address { get; set; }
        public string Total { get; set; }
        public string Items { get; set; }
    }
}
