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
    
    public partial class tbl_invoice
    {
        public int Id { get; set; }
        public Nullable<int> Company_id { get; set; }
        public int Bill_to { get; set; }
        public System.DateTime Invoice_date { get; set; }
        public string Invoice_number { get; set; }
        public int Sales_person { get; set; }
        public int Payment_terms { get; set; }
        public Nullable<int> Contract_type { get; set; }
        public Nullable<bool> is_tax { get; set; }
        public Nullable<decimal> tax { get; set; }
        public System.DateTime created_date { get; set; }
        public System.Guid created_by { get; set; }
        public System.DateTime modification_date { get; set; }
        public System.Guid modification_by { get; set; }
        public Nullable<decimal> tax_amount { get; set; }
    }
}
