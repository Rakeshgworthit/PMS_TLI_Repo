using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    //This Class is used only for to get client data {project and amount}
    public class CustomLoanDetailsViewModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
    }


    public class CustomSelectListItem
    {
       

        //
        // Summary:
        //     Gets or sets a value that indicates whether this System.Web.Mvc.SelectListItem
        //     is disabled.
        public bool Disabled { get; set; }
        
        //
        // Summary:
        //     Gets or sets a value that indicates whether 
        //     is selected.
        //
        // Returns:
        //     true if the item is selected; otherwise, false.
        public bool Selected { get; set; }
        //
        // Summary:
        //     Gets or sets the text of the selected item.
        //
        // Returns:
        //     The text.
        public string Text { get; set; }
        //
        // Summary:
        //     Gets or sets the value of the selected item.
        //
        // Returns:
        //     The value.
        public string Value { get; set; }
        /// <summary>
        /// this is related to cascading flag
        /// </summary>
        public string FlagId { get; set; }
    }
}