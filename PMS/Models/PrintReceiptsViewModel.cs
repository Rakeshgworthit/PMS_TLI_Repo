using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public partial  class PrintReceiptViewModel
    {

        public List<Database.receipt_detail> receipt_details { get; set; }
        public Database.SSP_ReceiptsById_Result ssp_receiptsById_result { get; set; }
        public List<Database.SSP_ReceiptDetailByReceiptId_Result> ssp_receiptDetailByIReceiptid_result { get; set; }

        public List<Database.SSP_ReceiptsDetailById_Result> ReceiptsDetail { get; set; }
          



    }

    
}