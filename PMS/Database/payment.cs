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
    
    public partial class payment
    {
        public long id { get; set; }
        public Nullable<System.DateTime> payment_date { get; set; }
        public long supplier_id { get; set; }
        public int bank_id { get; set; }
        public string cheque_number { get; set; }
        public Nullable<decimal> rebate_amount { get; set; }
        public Nullable<decimal> paid_amount { get; set; }
        public string remarks { get; set; }
        public System.DateTime created_date { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public System.DateTime modified_date { get; set; }
        public Nullable<System.Guid> modified_by { get; set; }
        public Nullable<int> payment_mode { get; set; }
        public bool isactive { get; set; }
        public Nullable<System.DateTime> collection_date { get; set; }
    }
}
