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
    
    public partial class SSP_DesignerReceipts_Result
    {
        public long id { get; set; }
        public string designer_name { get; set; }
        public Nullable<System.DateTime> receiptdate { get; set; }
        public string receipt_date { get; set; }
        public string receipt_detail { get; set; }
        public string mode_of_payment { get; set; }
        public string bank_name { get; set; }
        public string cheque_details { get; set; }
        public string remarks { get; set; }
        public string CreatedUpdated { get; set; }
        public Nullable<long> Rk { get; set; }
        public Nullable<long> TotalRecords { get; set; }
    }
}
