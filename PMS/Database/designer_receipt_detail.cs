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
    using System.Collections.Generic;
    
    public partial class designer_receipt_detail
    {
        public long id { get; set; }
        public Nullable<long> receipt_id { get; set; }
        public Nullable<int> invoice_id { get; set; }
        public Nullable<decimal> amount { get; set; }
        public Nullable<decimal> gst_percentage { get; set; }
        public Nullable<decimal> gst_amount { get; set; }
        public Nullable<decimal> total_amount { get; set; }
    }
}