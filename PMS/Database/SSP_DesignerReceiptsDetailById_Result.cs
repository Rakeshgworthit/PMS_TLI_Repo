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
    
    public partial class SSP_DesignerReceiptsDetailById_Result
    {
        public string Bank_Name { get; set; }
        public long id { get; set; }
        public Nullable<System.DateTime> receipt_date { get; set; }
        public string mode_of_payment { get; set; }
        public Nullable<int> bank_id { get; set; }
        public string cheque_details { get; set; }
        public string remarks { get; set; }
        public string branch_name { get; set; }
        public string branch_address { get; set; }
        public Nullable<decimal> amount { get; set; }
        public Nullable<decimal> gst_amount { get; set; }
        public Nullable<decimal> gst_percentage { get; set; }
        public Nullable<decimal> total_amount { get; set; }
        public string Invoice_number { get; set; }
        public Nullable<int> Payment_terms { get; set; }
        public Nullable<System.DateTime> Invoice_date { get; set; }
        public string payment_term { get; set; }
        public Nullable<int> payment_days { get; set; }
        public string designer_name { get; set; }
        public string contract_type { get; set; }
    }
}