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
    
    public partial class user_access_rights
    {
        public long id { get; set; }
        public System.Guid user_id { get; set; }
        public long branch_id { get; set; }
        public System.DateTime created_date { get; set; }
        public System.Guid created_by { get; set; }
        public System.DateTime modified_date { get; set; }
        public System.Guid modified_by { get; set; }
        public bool isactive { get; set; }
    }
}