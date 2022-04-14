using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public partial class PrintDesignerReceiptViewModel
    {
        public List<Database.designer_receipt_detail> receipt_details { get; set; }
        public Database.SSP_DesignerReceiptById_Result ssp_receiptsById_result { get; set; }
        public List<Database.SSP_DesignerReceiptDetailByReceiptId_Result> ssp_receiptDetailByIReceiptid_result { get; set; }

        public List<Database.SSP_DesignerReceiptsDetailById_Result> ReceiptsDetail { get; set; }

    }
}