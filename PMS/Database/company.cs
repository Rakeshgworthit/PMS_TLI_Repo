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
    
    public partial class company
    {
        public long id { get; set; }
        public string company_name { get; set; }
        public string company_initial { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address4 { get; set; }
        public string zip_code { get; set; }
        public string email { get; set; }
        public string website { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string fax { get; set; }
        public string contact_person { get; set; }
        public string reg_no { get; set; }
        public string gst_reg_no { get; set; }
        public System.DateTime created_date { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public System.DateTime modified_date { get; set; }
        public Nullable<System.Guid> modified_by { get; set; }
        public bool isactive { get; set; }
    }
}