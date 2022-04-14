using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Database;

namespace PMS.Models
{
    public partial class PrintPaymentViewModel
    {
        public PrintPaymentViewModel()
        {
            this.LoanDetails = new List<Database.SSP_GetLoanProjectsDetailsByLoanId_Result>();
        }
        public List<Database.payment_detail> payment_details { get; set; }
        public Database.SSP_PaymentById_Result ssp_paymentById_result { get; set; }
        public List<Database.tbl_invoice_items> Invoice_Items { get; set; }
        public List<Database.tbl_designerinvoice_items> Designer_Invoice_Items { get; set; }
        public Database.SSP_PrintInvoices_Result Invoice { get; set; }
        public Database.SSP_Print_DesignerInvoices_Result Designer_Invoice { get; set; }

        public Database.SSP_PrintLoan_Result LoanPrint { get; set; }

        /// <summary>
        /// For to set Meta Loan Details info
        /// </summary>
        public List<Database.SSP_GetLoanProjectsDetailsByLoanId_Result> LoanDetails { get; set; }
        public List<Database.SSP_GetPaymentDetails_Result> SSP_PaymentDetails { get; set; }
    }


}