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
    
    public partial class tbl_invoice_items
    {
        public int invoice_id { get; set; }
        public Nullable<int> project_id { get; set; }
        public string Contract_number { get; set; }
        public string item_description { get; set; }
        public decimal item_amount { get; set; }
    }
}
