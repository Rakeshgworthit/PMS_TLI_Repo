using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMS.Models;
using Microsoft.AspNet.Identity;
using System.Text;
using PMS.Common;
namespace PMS.Controllers
{
    [Authorize]
    [BranchFilter]
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReceiptReport(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            List<SelectListItem> branchList = new List<SelectListItem>();
            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
                // branchList = Common.CommonFunction.BranchList();
                objView.ProjectList = CommonFunction.UserProjectList("00000000-0000-0000-0000-000000000000");
            }
            else
            {
                // branchList = Common.CommonFunction.UserBranchList(uid);
                objView.ProjectList = CommonFunction.UserProjectList(uid);
            }
            objView.Uid = uid;

            //branchList.Insert(0, new SelectListItem { Text = "Select Branch", Value = "0" });
            //objView.BranchList = branchList;


            DateTime now = DateTime.Now;
            //var startDate = new DateTime(now.Year, now.Month, 1);
            //var endDate = startDate.AddMonths(1).AddDays(-1);
            // var startDate = new DateTime(now.Year, (now.Month - 5), 1);
            var CurrentstartDate = new DateTime(now.Year, now.Month, 1);
            var endDate = CurrentstartDate.AddMonths(1).AddDays(-1);

            if (objView.from_date == null)
            {
                objView.from_date = CurrentstartDate.AddMonths(-5);
            }
            else
            {
                objView.from_date = objView.from_date;
            }
            if (objView.to_date == null)
            {
                objView.to_date = endDate;
            }
            else
            {
                objView.to_date = objView.to_date;
            }

            objView.BankList = CommonFunction.BankList();
            objView.SalesmenAndUserList = CommonFunction.SalesmenListStatusWise(objView.Uid);
            objView.SalesmenStatusList = CommonFunction.SalesmenStatusList();


            ExportReceiptReport(objView, true);

            return View(objView);
        }

        #region Backup
        //public void ExportReceiptReport(ReportViewModel objView, bool isGridData = false)
        //{
        //    string uid = User.Identity.GetUserId();
        //    if (User.IsInRole("SuperAdmin"))
        //    {
        //        uid = "";
        //    }

        //    List<Database.SSP_ReceiptReport_Result> reportData = new List<Database.SSP_ReceiptReport_Result>();

        //    using (Database.PMSEntities objDB = new Database.PMSEntities())
        //    {
        //        reportData = objDB.SSP_ReceiptReport(uid, Common.SessionManagement.SelectedBranchID, objView.from_date, objView.to_date, objView.BankId, objView.ProjectId, objView.SalesmenId).ToList();
        //    }

        //    string projectName = "";
        //    string salesmanname = "";
        //    string bankname = "";


        //    StringBuilder str = new StringBuilder("");

        //    str.Append("<table width='100%' border='0' class='table table-striped'>");

        //    if (isGridData == false)
        //    {
        //        if (objView.ProjectId > 0)
        //        {
        //            projectName = CommonFunction.UserProjectList(Convert.ToString("00000000-0000-0000-0000-000000000000")).Where(o => o.Value == objView.ProjectId.ToString()).Select(o => o.Text).SingleOrDefault();
        //        }
        //        if (objView.SalesmenId > 0)
        //        {
        //            salesmanname = CommonFunction.SalesmenList().Where(o => o.Value == objView.SalesmenId.ToString()).Select(o => o.Text).SingleOrDefault();
        //        }
        //        if (objView.BankId > 0)
        //        {
        //            bankname = CommonFunction.BankList().Where(o => o.Value == objView.BankId.ToString()).Select(o => o.Text).SingleOrDefault();
        //        }
        //        str.Append("<tr>");
        //        str.Append("<th style='text-align:left;' colspan='7'><b>Customer Payment Report</b></th>");
        //        str.Append("</tr>");

        //        str.Append("<tr>");
        //        str.Append("<th  style='text-align:left;' colspan='7'><b>Branch: </b> " + SessionManagement.SelectedBranchName + "</th>");
        //        str.Append("</tr>");

        //        str.Append("<tr>");
        //        str.Append("<td style='text-align:left;' colspan='2'><b>From:</b> " + Convert.ToDateTime(objView.from_date).ToString("dd/MM/yyyy") + "</td>");
        //        str.Append("<td style='text-align:left;' colspan='5'><b>To:</b> " + Convert.ToDateTime(objView.to_date).ToString("dd/MM/yyyy") + "</td>");
        //        str.Append("</tr>");

        //        if (Convert.ToString(projectName) != "" || Convert.ToString(salesmanname) != "" || Convert.ToString(bankname) != "")
        //        {
        //            str.Append("<tr>");
        //        }

        //        if (Convert.ToString(bankname) != "")
        //        {
        //            str.Append("<td colspan='2' style='text-align:left;'><b>Bank: </b> " + bankname + "</td>");
        //        }
        //        if (Convert.ToString(projectName) != "")
        //        {
        //            str.Append("<td colspan='2' style='text-align:left;'><b>Project: </b> " + projectName + "</td>");
        //        }
        //        if (Convert.ToString(salesmanname) != "")
        //        {
        //            str.Append("<td colspan='2' style='text-align:left;'><b>Salesmen: </b> " + salesmanname + "</td>");
        //        }


        //        if (Convert.ToString(projectName) != "" || Convert.ToString(salesmanname) != "" || Convert.ToString(bankname) != "")
        //        {
        //            str.Append("<td></td>");
        //            str.Append("</tr>");
        //            str.Append("<tr><td colspan='7'></td></tr>");
        //        }
        //    }

        //    str.Append("<tr>");
        //    str.Append("<th>Date</th><th>Cheque No.</th><th style='text-align:right;'>Amount</th><th>Job Site Location</th><th>Tel</th><th>Client</th><th>Salesmen</th>");
        //    str.Append("</tr>");

        //    string tele = "";
        //    foreach (var items in reportData)
        //    {
        //        if (items.phone != null && items.mobile != null)
        //        {
        //            tele = items.phone + " / " + items.mobile;
        //        }
        //        else if (items.phone != null)
        //        {
        //            tele = items.phone;
        //        }
        //        else
        //        {
        //            tele = items.mobile;
        //        }

        //        str.Append("<tr>");
        //        str.Append("<td>" + items.receipt_date + "</td><td>" + items.cheque_details + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.amount)) + "</td><td>" + items.branch_name + "</td>");
        //        str.Append("<td>" + tele + "</td><td>" + items.name1 + "</td><td>" + items.salesmen_name + "</td>");
        //        str.Append("</tr>");
        //    }

        //    str.Append("<tr>");
        //    str.Append("<td colspan='2'><strong>Cash</strong></td><td  align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Where(o => o.mode_of_payment_id == 2).Select(o => o.amount).Sum())) + "</strong></td><td></td><td></td><td></td><td></td>");
        //    str.Append("</tr>");

        //    str.Append("<tr>");
        //    str.Append("<td colspan='2'><strong>Cheque</strong></td><td  align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Where(o => o.mode_of_payment_id == 4).Select(o => o.amount).Sum())) + "</strong></td><td></td><td></td><td></td><td></td>");
        //    str.Append("</tr>");

        //    str.Append("<tr>");
        //    str.Append("<td colspan='2'><strong>Bank TFR</strong></td><td  align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Where(o => o.mode_of_payment_id == 3).Select(o => o.amount).Sum())) + "</strong></td><td></td><td></td><td></td><td></td>");
        //    str.Append("</tr>");

        //    double nets = Convert.ToDouble(reportData.Where(o => o.mode_of_payment_id == 1).Select(o => o.amount).Sum());
        //    //double netfee = (nets * 0.008);
        //    double gst = (nets * 0.07);

        //    str.Append("<tr>");
        //    //str.Append("<td colspan='2'><strong>NETS</strong></td><td><strong>$" + nets + "</strong></td><td><strong>NETS FEES</strong></td><td>$" + netfee + "</td><td><strong>GST</strong></td><td>$" + gst + "</td>");
        //    str.Append("<td colspan='2'><strong>NETS</strong></td><td align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(nets)) + "</strong></td><td></td><td></td><td><strong>GST</strong></td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(gst)) + "</td>");
        //    str.Append("</tr>");

        //    str.Append("<tr>");
        //    str.Append("<td colspan='2'><strong>Total</strong></td><td align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.amount).Sum())) + "</strong></td><td></td><td></td><td></td><td></td>");
        //    str.Append("</tr>");

        //    str.Append("</table>");
        //    if (isGridData == true)
        //    {
        //        objView.GridData = str.ToString();
        //    }
        //    else
        //    {
        //        objView.GridData = "";
        //        GenerateReport("ReceiptReport_" + objView.ReportMonth.ToString() + "_" + objView.ReportYear.ToString() + ".xls", str.ToString());

        //    }

        //}
        #endregion

        public void ExportReceiptReport(ReportViewModel objView, bool isGridData = false)
        {
            string uid = User.Identity.GetUserId();
            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
            }

            List<Database.SSP_ReceiptReport_Result> reportData = new List<Database.SSP_ReceiptReport_Result>();


            List<Database.SSP_InvoiceWithAndWithoutPayment_Result> allInvoiceList = new List<Database.SSP_InvoiceWithAndWithoutPayment_Result>();

            using (Database.PMSEntities objDB = new Database.PMSEntities())
            {
                reportData = objDB.SSP_ReceiptReport(uid, Common.SessionManagement.SelectedBranchID, objView.from_date, objView.to_date, objView.BankId, objView.ProjectId, objView.SalesmenId).ToList();
                allInvoiceList = objDB.SSP_InvoiceWithAndWithoutPayment(uid, Common.SessionManagement.SelectedBranchID, objView.from_date, objView.to_date, objView.BankId, objView.ProjectId, objView.SalesmenId).ToList();
            }

            string projectName = "";
            string salesmanname = "";
            string bankname = "";

            reportData.AddRange(allInvoiceList.Where(x => !reportData.Select(z => z.invoiceId).Contains(x.invoiceId)).Select(x => new Database.SSP_ReceiptReport_Result
            {
                amount = 0,
                branch_name = x.branch_name,
                cheque_details = "",
                invoice_amount_items = x.invoice_amount_items,
                invoice_numbers = x.invoice_numbers,
                is_tax = x.is_tax,
                mobile = x.mobile,
                mode_of_payment_id = 0,
                mode_of_payment_name = "",
                name1 = x.name1,
                phone = x.phone,
                receipt_date = x.Invoice_date,
                salesmen_name = x.salesmen_name,
                tax = x.tax,
                tax_amount = x.tax_amount,
                invoiceId = x.invoiceId
            }).ToList());

            reportData.OrderBy(x => x.receipt_date);

            reportData = reportData.GroupBy(d => d.invoiceId)
    .Select(
        g => new Database.SSP_ReceiptReport_Result
        {
            amount = g.Sum(s => s.amount),
            branch_name = g.FirstOrDefault().branch_name,
            cheque_details = g.FirstOrDefault().cheque_details,
            invoiceId = g.FirstOrDefault().invoiceId,
            receipt_date = g.FirstOrDefault().receipt_date,
            invoice_amount_items = g.FirstOrDefault().invoice_amount_items,
            invoice_numbers = g.FirstOrDefault().invoice_numbers,
            is_tax = g.FirstOrDefault().is_tax,
            mobile = g.FirstOrDefault().mobile,
            mode_of_payment_id = g.FirstOrDefault().mode_of_payment_id,
            mode_of_payment_name = g.FirstOrDefault().mode_of_payment_name,
            name1 = g.FirstOrDefault().name1,
            phone = g.FirstOrDefault().phone,
            ReceiptDate = g.FirstOrDefault().ReceiptDate,
            salesmen_name = g.FirstOrDefault().salesmen_name,
            tax = g.FirstOrDefault().tax,
            tax_amount = g.FirstOrDefault().tax_amount
        }).ToList();

            StringBuilder str = new StringBuilder("");

            str.Append("<table width='100%' border='0' class='table table-striped dataTable' id='tableid' >");

            if (isGridData == false)
            {
                if (objView.ProjectId > 0)
                {
                    projectName = CommonFunction.UserProjectList(Convert.ToString("00000000-0000-0000-0000-000000000000")).Where(o => o.Value == objView.ProjectId.ToString()).Select(o => o.Text).SingleOrDefault();
                }
                if (objView.SalesmenId > 0)
                {
                    salesmanname = CommonFunction.AllSalesmen().Where(o => o.Value == objView.SalesmenId.ToString()).Select(o => o.Text).SingleOrDefault();
                }
                if (objView.BankId > 0)
                {
                    bankname = CommonFunction.BankList().Where(o => o.Value == objView.BankId.ToString()).Select(o => o.Text).SingleOrDefault();
                }
                str.Append("<tr>");
                str.Append("<th style='text-align:left;' colspan='8'><b>Customer Payment Report</b></th>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th  style='text-align:left;' colspan='8'><b>Branch: </b> " + SessionManagement.SelectedBranchName + "</th>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<td style='text-align:left;' colspan='2'><b>From:</b> " + Convert.ToDateTime(objView.from_date).ToString("dd/MM/yyyy") + "</td>");
                str.Append("<td style='text-align:left;' colspan='6'><b>To:</b> " + Convert.ToDateTime(objView.to_date).ToString("dd/MM/yyyy") + "</td>");
                str.Append("</tr>");

                if (Convert.ToString(projectName) != "" || Convert.ToString(salesmanname) != "" || Convert.ToString(bankname) != "")
                {
                    str.Append("<tr>");
                }

                if (Convert.ToString(bankname) != "")
                {
                    str.Append("<td colspan='2' style='text-align:left;'><b>Bank: </b> " + bankname + "</td>");
                }
                if (Convert.ToString(projectName) != "")
                {
                    str.Append("<td colspan='2' style='text-align:left;'><b>Project: </b> " + projectName + "</td>");
                }
                if (Convert.ToString(salesmanname) != "")
                {
                    str.Append("<td colspan='2' style='text-align:left;'><b>Salesmen: </b> " + salesmanname + "</td>");
                }


                if (Convert.ToString(projectName) != "" || Convert.ToString(salesmanname) != "" || Convert.ToString(bankname) != "")
                {
                    str.Append("<td></td>");
                    str.Append("</tr>");
                    str.Append("<tr><td colspan='8'></td></tr>");
                }
            }

            str.Append("<thead>");
            str.Append("<tr>");
            str.Append("<th>Date</th><th>Payment Mode</th><th>Invoice Numbers</th><th style='text-align:right;'>Invoice Amount</th><th style='text-align:right;'>Payment Amount</th><th style='text-align:right;'>Balance</th><th>Job Site Location</th><th>Tel</th><th>Client</th><th>Salesmen</th>");
            str.Append("</tr>");
            str.Append("</thead>");

            str.Append("<tbody>");
            string tele = "";
            foreach (var items in reportData)
            {
                if (items.phone != null && items.mobile != null)
                {
                    tele = items.phone + " / " + items.mobile;
                }
                else if (items.phone != null)
                {
                    tele = items.phone;
                }
                else
                {
                    tele = items.mobile;
                }
                decimal invoiceitemsAmountSum = Convert.ToDecimal(CommonHelper.CalcualteInvoiceAmount(items.invoice_amount_items, items.tax, items.is_tax, items.tax_amount, true, false));
                str.Append("<tr>");
                str.Append("<td>" + items.receipt_date + "</td>");
                if (items.mode_of_payment_id == 4) // in case  of cheque
                {
                    str.Append("<td>" + items.mode_of_payment_name + " / " + items.cheque_details + "</td>");
                }
                else
                {
                    str.Append("<td>" + items.mode_of_payment_name + "</td>");
                }

                DateTime R_Date = DateTime.Parse(items.receipt_date);
                //List<decimal?> totalPayment = reportData.Where(x => x.invoiceId == items.invoiceId && DateTime.Parse(x.receipt_date) <= R_Date).Select(x => x.amount).ToList();
                str.Append("<td>" + items.invoice_numbers + "</td>");
                str.Append("<td align='right'>$" + invoiceitemsAmountSum.ToString("#,##0.00") + "</td>");
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.amount)) + "</td>");//CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.amount))
                //if (totalPayment != null && totalPayment.Count > 1)
                //{
                //    decimal? amt = totalPayment.Sum();
                //    str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(invoiceitemsAmountSum - amt)) + "</td>");
                //}
                //else
                //{
                //    str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(invoiceitemsAmountSum - items.amount)) + "</td>");
                //}
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(invoiceitemsAmountSum - items.amount)) + "</td>");
                str.Append("<td>" + items.branch_name + "</td>");
                str.Append("<td>" + tele + "</td><td>" + items.name1 + "</td><td>" + items.salesmen_name + "</td>");
                str.Append("</tr>");
            }
            str.Append("</tbody>");

            str.Append("<tr>");
            str.Append("<td colspan='4'><strong>Cash</strong></td><td  align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Where(o => o.mode_of_payment_id == 2).Select(o => o.amount).Sum())) + "</strong></td><td></td><td></td><td></td><td></td><td></td>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<td colspan='4'><strong>Cheque</strong></td><td  align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Where(o => o.mode_of_payment_id == 4).Select(o => o.amount).Sum())) + "</strong></td><td></td><td></td><td></td><td></td></td><td>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<td colspan='4'><strong>Bank TFR</strong></td><td  align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Where(o => o.mode_of_payment_id == 3).Select(o => o.amount).Sum())) + "</strong></td><td></td><td></td><td></td><td></td></td><td>");
            str.Append("</tr>");

            double nets = Convert.ToDouble(reportData.Where(o => o.mode_of_payment_id == 1).Select(o => o.amount).Sum());
            //double netfee = (nets * 0.008);
            double gst = (nets * 0.07);

            str.Append("<tr>");
            //str.Append("<td colspan='2'><strong>NETS</strong></td><td><strong>$" + nets + "</strong></td><td><strong>NETS FEES</strong></td><td>$" + netfee + "</td><td><strong>GST</strong></td><td>$" + gst + "</td>");
            str.Append("<td colspan='4'><strong>NETS</strong></td><td align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(nets)) + "</strong></td><td></td><td></td></td><td><td><strong>GST</strong></td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(gst)) + "</td>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<td colspan='4'><strong>Total</strong></td><td align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.amount).Sum())) + "</strong></td><td></td><td></td><td></td><td></td></td><td>");
            str.Append("</tr>");
            str.Append("</table>");
            if (isGridData == true)
            {
                objView.GridData = str.ToString();
            }
            else
            {
                objView.GridData = "";
                GenerateReport("ReceiptReport_" + objView.ReportMonth.ToString() + "_" + objView.ReportYear.ToString() + ".xls", str.ToString());

            }

        }

        public ActionResult SaleSummaryReport(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            // List<SelectListItem> branchList = new List<SelectListItem>();
            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
                //   branchList = Common.CommonFunction.BranchList();
            }
            else
            {
                //  branchList = Common.CommonFunction.UserBranchList(uid);
            }
            objView.Uid = uid;

            //branchList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
            //objView.BranchList = branchList;

            objView.YearList = Common.CommonFunction.YearList();

            if (objView.ReportYear == 0)
            {
                objView.ReportYear = DateTime.Now.Year;
            }
            ExportSaleSummaryReport(objView, true);
            return View(objView);
        }

        public void ExportSaleSummaryReport(ReportViewModel objView, bool isGridData = false)
        {
            string uid = User.Identity.GetUserId();
            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
            }
            objView.Uid = uid;

            objView.YearList = Common.CommonFunction.YearList();

            if (objView.ReportYear == 0)
            {
                objView.ReportYear = DateTime.Now.Year;
            }

            List<Database.SSP_SaleSummeryReport_Result> reportData = new List<Database.SSP_SaleSummeryReport_Result>();

            using (Database.PMSEntities objDB = new Database.PMSEntities())
            {
                reportData = objDB.SSP_SaleSummeryReport(uid, SessionManagement.SelectedBranchID, objView.ReportYear).ToList();
            }

            StringBuilder str = new StringBuilder("");
            str.Append("<table width='100%' class='table table-striped'>");

            if (isGridData == false)
            {
                str.Append("<tr>");
                str.Append("<th  style='text-align:left;' colspan='14'><b>Sales Summary Report</b></th>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th  style='text-align:left;' colspan='14'><b>Branch: </b> " + SessionManagement.SelectedBranchName + "</th>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th  style='text-align:left;' colspan='14'><b>Year: </b> " + objView.ReportYear.ToString() + "</th>");
                str.Append("</tr>");
            }

            str.Append("<tr>");
            str.Append("<th>Sales " + objView.ReportYear.ToString() + "</th><th style='text-align:right;'>Jan</th><th style='text-align:right;'>Feb</th><th style='text-align:right;'>Mar</th><th style='text-align:right;'>April</th><th style='text-align:right;'>May</th><th style='text-align:right;'>June</th><th style='text-align:right;'>July</th><th style='text-align:right;'>Aug</th><th style='text-align:right;'>Sep</th><th style='text-align:right;'>Oct</th><th style='text-align:right;'>Nov</th><th style='text-align:right;'>Dec</th><th style='text-align:right;'>Total</th>");
            str.Append("</tr>");

            double total = 0;
            double grandTotal = 0;
            foreach (var items in reportData)
            {
                total = Convert.ToDouble(items.jan) + Convert.ToDouble(items.feb) + Convert.ToDouble(items.mar) + Convert.ToDouble(items.apr) + Convert.ToDouble(items.may) + Convert.ToDouble(items.jun) + Convert.ToDouble(items.jul) + Convert.ToDouble(items.aug) + Convert.ToDouble(items.sep) + Convert.ToDouble(items.oct) + Convert.ToDouble(items.nov) + Convert.ToDouble(items.dec);
                grandTotal = grandTotal + total;

                str.Append("<tr>");
                str.Append("<td>" + items.salesmen_name + "</td><td align='right'>$" + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.jan)) + "</td><td align='right'>$ " + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.feb)) + "</td><td align='right'>$" + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.mar)) + "</td>");
                str.Append("<td align='right'>$" + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.apr)) + "</td><td align='right'>$ " + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.may)) + "</td><td align='right'>$" + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.jun)) + "</td>");
                str.Append("<td align='right'>$" + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.jul)) + "</td><td align='right'>$ " + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.aug)) + "</td><td align='right'>$" + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.sep)) + "</td>");
                str.Append("<td align='right'>$" + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.oct)) + "</td><td align='right'>$ " + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.nov)) + "</td><td align='right'>$" + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.dec)) + "</td>");
                str.Append("<td align='right'>$" + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(total)) + "</td>");
                str.Append("</tr>");
            }

            str.Append("<tr>");
            str.Append("<td>Total</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.jan).Sum())) + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.feb).Sum())) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.mar).Sum())) + "</td>");
            str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.apr).Sum())) + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.may).Sum())) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.jun).Sum())) + "</td>");
            str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.jul).Sum())) + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.aug).Sum())) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.sep).Sum())) + "</td>");
            str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.oct).Sum())) + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.nov).Sum())) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.dec).Sum())) + "</td>");
            str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(grandTotal)) + "</td>");
            str.Append("</tr>");

            if (isGridData == true)
            {
                objView.GridData = str.ToString();

            }
            else
            {
                objView.GridData = "";
                GenerateReport("Sales_Summary_" + objView.ReportYear.ToString() + ".xls", str.ToString());

            }


        }

        public ActionResult PaymentReport(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
                //  objView.ProjectList = CommonFunction.UserProjectList("00000000-0000-0000-0000-000000000000");
            }
            else
            {
                // objView.ProjectList = CommonFunction.UserProjectList(uid);
            }
            objView.Uid = uid;


            DateTime now = DateTime.Now;
            //var startDate = new DateTime(now.Year, now.Month, 1);
            //var endDate = startDate.AddMonths(1).AddDays(-1);

            //var startDate = new DateTime(now.Year, (now.Month - 5), 1);
            var CurrentstartDate = new DateTime(now.Year, now.Month, 1);
            var endDate = CurrentstartDate.AddMonths(1).AddDays(-1);

            if (objView.from_date == null)
            {
                objView.from_date = CurrentstartDate.AddMonths(-5);
            }
            else
            {
                objView.from_date = objView.from_date;
            }
            if (objView.to_date == null)
            {
                objView.to_date = endDate;
            }
            else
            {
                objView.to_date = objView.to_date;
            }
            objView.BankList = CommonFunction.BankList();
            //objView.SalenmenList = CommonFunction.AllSalesmen();
            objView.SalesmenAndUserList = CommonFunction.SalesmenListStatusWise(objView.Uid);
            objView.SalesmenStatusList = CommonFunction.SalesmenStatusList();

            //List<SelectListItem> branchList = new List<SelectListItem>();
            //if (uid == "")
            //{
            //    branchList = Common.CommonFunction.BranchList();
            //}
            //else
            //{
            //    branchList = Common.CommonFunction.UserBranchList(uid);
            //}
            //branchList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
            //objView.BranchList = branchList;


            //objView.BranchId = 0;


            ExportPaymentReport(objView, true);


            return View(objView);
        }

        public void ExportPaymentReport(ReportViewModel objView, bool isGridData = false)
        {
            string uid = User.Identity.GetUserId();
            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
            }
            string projectName = "";
            string salesmanname = "";
            string bankname = "";
            double paid_amount = 0;
            string payment_date = "";

            string supplier_name = "";
            double rebate_amount = 0;
            string bank = "";
            string cheque_number = "";
            double payment_amount = 0;
            double total_paid_amount = 0;
            List<Database.SSP_PaymentReport_Result> reportData = new List<Database.SSP_PaymentReport_Result>();


            using (Database.PMSEntities objDB = new Database.PMSEntities())
            {
                reportData = objDB.SSP_PaymentReport(uid, objView.from_date, objView.to_date, SessionManagement.SelectedBranchID, objView.BankId, objView.ProjectId, objView.SalesmenId).ToList();
            }

            StringBuilder str = new StringBuilder("");
            str.Append("<table width='100%' class='table table-striped'>");

            if (isGridData == false)
            {

                if (objView.ProjectId > 0)
                {
                    projectName = CommonFunction.UserProjectList("00000000-0000-0000-0000-000000000000").Where(o => o.Value == objView.ProjectId.ToString()).Select(o => o.Text).SingleOrDefault();
                }
                if (objView.SalesmenId > 0)
                {
                    salesmanname = CommonFunction.AllSalesmen().Where(o => o.Value == objView.SalesmenId.ToString()).Select(o => o.Text).SingleOrDefault();
                }
                if (objView.BankId > 0)
                {
                    bankname = CommonFunction.BankList().Where(o => o.Value == objView.BankId.ToString()).Select(o => o.Text).SingleOrDefault();
                }

                str.Append("<tr>");
                str.Append("<th  style='text-align:left;' colspan='7'><b>Supplier Payment Report</b></th>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th  style='text-align:left;'  colspan='7'><b>Branch: </b> " + SessionManagement.SelectedBranchName + "</th>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<td style='text-align:left;' colspan='2'><b>From:</b> " + Convert.ToDateTime(objView.from_date).ToString("dd/MM/yyyy") + "</td>");
                str.Append("<td style='text-align:left;' colspan='5'><b>To:</b> " + Convert.ToDateTime(objView.to_date).ToString("dd/MM/yyyy") + "</td>");
                str.Append("</tr>");

                if (Convert.ToString(projectName) != "" || Convert.ToString(salesmanname) != "" || Convert.ToString(bankname) != "")
                {
                    str.Append("<tr>");
                }

                if (Convert.ToString(bankname) != "")
                {
                    str.Append("<td colspan='2' style='text-align:left;'><b>Bank: </b> " + bankname + "</td>");
                }
                if (Convert.ToString(projectName) != "")
                {
                    str.Append("<td colspan='2' style='text-align:left;'><b>Project: </b> " + projectName + "</td>");
                }
                if (Convert.ToString(salesmanname) != "")
                {
                    str.Append("<td colspan='2' style='text-align:left;'><b>Salesmen: </b> " + salesmanname + "</td>");
                }

                if (Convert.ToString(projectName) != "" || Convert.ToString(salesmanname) != "" || Convert.ToString(bankname) != "")
                {
                    str.Append("<td></td>");
                    str.Append("</tr>");
                    str.Append("<tr><td></td></tr>");
                }
            }
            str.Append("<tr>");
            str.Append("<th>Payment Date</th><th >Supplier</th><th style='text-align:right;'>Capital</td><th style='text-align:right;'>Rebate</th><th>Bank</th><th>Cheque#</th><th style='text-align:right;'>Amount Paid</th><th style='text-align:right;'>Actual Amount Paid</th>");
            str.Append("</tr>");

            foreach (var items in reportData)
            {
                 payment_date = items.payment_date;

                 supplier_name = Convert.ToString(items.supplier_name);
                //double invoice_amount = Convert.ToDouble(items.invoice_amount);
                 rebate_amount = Convert.ToDouble(items.rebate_amount);
                 bank = Convert.ToString(items.bank);
                 cheque_number = Convert.ToString(items.cheque_number);
                 payment_amount = Convert.ToDouble(items.payment_amount);
                paid_amount = Convert.ToDouble(items.paid_amount);
                


                str.Append("<tr>");
                str.Append("<td>" + payment_date + "</td><td>" + supplier_name + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(payment_amount)) + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(rebate_amount)) + "</td><td>" + bank + "</td>");
                str.Append("<td>" + cheque_number + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(payment_amount) - Convert.ToDecimal(rebate_amount)) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(paid_amount)) + "</td>");
                str.Append("</tr>");
            }

            double capital_amount = Convert.ToDouble(reportData.Select(o => o.payment_amount).Sum());
            double dis_received = Convert.ToDouble(reportData.Select(o => o.rebate_amount).Sum());
            double amount_paid = capital_amount - dis_received;

            double total_amount_report = (amount_paid + dis_received);

            total_paid_amount = Convert.ToDouble(reportData.Select(o => o.paid_amount).Sum());

            str.Append("<tr>");
            str.Append("<td></td><td></td><td align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(capital_amount)) + "</strong></td><td align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(dis_received)) + "</strong></td><td></td><td></td> <td align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(amount_paid)) + "</strong></td><td align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(total_paid_amount)) + "</strong></td>");
            str.Append("</tr>");





            str.Append("</table>");



            if (isGridData == true)
            {
                objView.GridData = str.ToString();

            }
            else
            {


                objView.GridData = "";
                GenerateReport("PaymentReport_" + Convert.ToDateTime(objView.from_date).ToString("ddMMyyyy") + "_" + Convert.ToDateTime(objView.to_date).ToString("ddMMyyyy") + ".xls", str.ToString());


            }
        }


        public ActionResult IndividualSaleReport(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
           // List<SelectListItem> salesmenList = new List<SelectListItem>();
            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
                //salesmenList = Common.CommonFunction.SalesmenList();
            }
            else
            {
                // salesmenList = Common.CommonFunction.UserSalesmenList(uid);
            }
           // salesmenList = Common.CommonFunction.AllSalesmen();
            objView.Uid = uid;
            // objView.SalenmenList = salesmenList;
            objView.SalesmenAndUserList = CommonFunction.SalesmenListStatusWise("");
            objView.SalesmenStatusList = CommonFunction.SalesmenStatusList();
            objView.YearList = Common.CommonFunction.YearList();
            if (objView.SearchSalesmenStatus == null)
            {
                objView.SearchSalesmenStatus = "1";
                objView.SalesmenAndUserList = CommonFunction.SalesmenListStatusWise("1");
            }

            if (objView.ReportYear == 0)
            {
                objView.ReportYear = DateTime.Now.Year;
            }

            ExportIndividualSaleReport(objView, true);


            return View(objView);

        }

        public void ExportIndividualSaleReport(ReportViewModel objView, bool isGridData = false)
        {
            string uid = User.Identity.GetUserId();
            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
            }
            List<Database.SSP_SaleIndividualReport_Result> report_items = new List<Database.SSP_SaleIndividualReport_Result>();
            using (Database.PMSEntities objDB = new Database.PMSEntities())
            {
                report_items = objDB.SSP_SaleIndividualReport(uid, objView.SalesmenId, objView.ReportYear).ToList();
            }

            string prevMonth = "";
            string salesmen = "";
            int count = 0;
            Int32 totalRec = 0;
            StringBuilder str = new StringBuilder("");
            str.Append("<table width='100%' class='table table-striped'>");

            if (isGridData == false)
            {
                str.Append("<tr>");
                str.Append("<th style='text-align:left;' colspan='5'><b>Individual Sale Report</b></th>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<th style='text-align:left;'  colspan='5'><b>Branch: </b>" + SessionManagement.SelectedBranchName + "</th>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<th style='text-align:left;'  colspan='5'><b>Year: </b>" + objView.ReportYear.ToString() + "</th>");
                str.Append("</tr>");
            }

            if (report_items != null)
            {
                if (report_items.Count > 0)
                {
                    salesmen = report_items[0].salesmen_name;
                    str.Append("<tr>");
                    str.Append("<td colspan='5'><b>ID: </b>" + salesmen + "</td>");
                    str.Append("</tr>");
                }
            }

            foreach (var items in report_items)
            {
                count = count + 1;
                if (prevMonth != items.receipt_date)
                {
                    str.Append("<tr>");
                    str.Append("<td colspan='5'></td>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td style='background-color:#C0C0C0;'>No</td><td style='background-color:#C0C0C0;'>" + items.receipt_date + "</td><td style='background-color:#C0C0C0;'>Date of Contract</td><td style='background-color:#C0C0C0;'>Contract No</td><td style='background-color:#C0C0C0; text-align:right;'>Sales Amt</td></td>");
                    str.Append("</tr>");
                    prevMonth = items.receipt_date;
                    count = 1;
                    totalRec = report_items.Where(o => o.receipt_date == prevMonth).Select(o => o.id).Count();
                }

                str.Append("<tr>");
                str.Append("<td>" + count + "</td><td>" + items.project_name + "</td>");
                str.Append("<td>" + items.contract_date + "</td><td>" + items.project_number + "</td>");// items.total_amount
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.total_amount)) + "</td>");
                str.Append("</tr>");

                if (count == totalRec)
                {
                    str.Append("<tr>");
                    str.Append("<td></td><td></td>");
                    str.Append("<td></td><td>Total</td>");
                    str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(report_items.Where(o => o.receipt_date == prevMonth).Select(o => o.total_amount).Sum())) + "</td>");
                    str.Append("</tr>");
                }
            }

            if (report_items != null && report_items.Count > 0)
            {
                str.Append("<tr>");
                str.Append("<td></td><td></td>");
                str.Append("<td></td><td>Grand Total</td>");
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(report_items.Select(o => o.total_amount).Sum())) + "</td>");
                str.Append("</tr>");
            }

            str.Append("</table>");

            if (isGridData == true)
            {
                objView.GridData = str.ToString();

            }
            else
            {
                objView.GridData = "";
                GenerateReport("SaleReport_" + objView.ReportYear.ToString() + "_" + salesmen.Replace(" ", "") + ".xls", str.ToString());
            }

        }

        public ActionResult ProjectListingReport(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            //List<SelectListItem> branchList = new List<SelectListItem>();
            List<SelectListItem> salesmenList = new List<SelectListItem>();

            DateTime now = DateTime.Now;
            //var startDate = new DateTime(now.Year, now.Month, 1);
            //var endDate = startDate.AddMonths(1).AddDays(-1);

            //var startDate = new DateTime(now.Year, (now.Month - 5), 1);
            var CurrentstartDate = new DateTime(now.Year, now.Month, 1);
            var endDate = CurrentstartDate.AddMonths(1).AddDays(-1);
            if (objView.from_date == null)
            {
                objView.from_date = CurrentstartDate.AddMonths(-5);
            }
            if (objView.to_date == null)
            {
                objView.to_date = endDate;
            }

            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
            }

           // salesmenList = CommonFunction.AllSalesmen();
            objView.Uid = uid;
            // branchList.Insert(0, new SelectListItem { Text = "Select Branch", Value = "0" });

            // objView.BranchList = branchList;
            // objView.SalenmenList = salesmenList;
            objView.SalesmenAndUserList = CommonFunction.SalesmenListStatusWise("");
            objView.SalesmenStatusList = CommonFunction.SalesmenStatusList();
            //objView.YearList = Common.CommonFunction.YearList();

            //if (objView.ReportYear == 0)
            //{
            //    objView.ReportYear = DateTime.Now.Year;
            //}
            ExportProjectListingReport(objView, true);
            return View(objView);
        }

        public void ExportProjectListingReport(ReportViewModel objView, bool isGridData = false)
        {
            string uid = User.Identity.GetUserId();
            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
            }

            List<Database.SSP_ProjectsBySalesman_Result> report_items = new List<Database.SSP_ProjectsBySalesman_Result>();
            using (Database.PMSEntities objDB = new Database.PMSEntities())
            {
                report_items = objDB.SSP_ProjectsBySalesman(objView.from_date, objView.to_date, uid, objView.SalesmenId, SessionManagement.SelectedBranchID).ToList();
            }

            StringBuilder str = new StringBuilder("");
            str.Append("<table width='100%' class='table table-striped'>");

            if (isGridData == false)
            {
                string salesmanname = "";
                if (objView.SalesmenId > 0)
                {
                    salesmanname = CommonFunction.AllSalesmen().Where(o => o.Value == objView.SalesmenId.ToString()).Select(o => o.Text).SingleOrDefault();
                }

                str.Append("<tr>");
                str.Append("<th style='text-align:left;' colspan='5'><b>Project Listing Report</b></th>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<th style='text-align:left;'  colspan='5'><b>Branch: </b>" + SessionManagement.SelectedBranchName + "</th>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<td style='text-align:left;' colspan='1'><b>From:</b> " + Convert.ToDateTime(objView.from_date).ToString("dd/MM/yyyy") + "</td>");
                str.Append("<td style='text-align:left;' colspan='4'><b>To:</b> " + Convert.ToDateTime(objView.to_date).ToString("dd/MM/yyyy") + "</td>");
                str.Append("</tr>");
                if (salesmanname != "")
                {
                    str.Append("<tr>");
                    str.Append("<th style='text-align:left;'  colspan='5'><b>Salesmen: </b>" + salesmanname + "</th>");
                    str.Append("</tr>");
                }
            }

            str.Append("<tr>");
            str.Append("<th>Project Name</th><th>Date of Contract</th>");
            str.Append("<th>Date of Project Closed</th><th>Salesmen</th>");
            str.Append("<th>Branch</th>");
            str.Append("</tr>");

            foreach (var items in report_items)
            {
                str.Append("<tr>");
                str.Append("<td>" + items.project_name + "</td><td>" + items.contract_date + "</td>");
                str.Append("<td>" + items.closing_date + "</td><td>" + items.salesmen_name + "</td>");
                str.Append("<td>" + items.branch_name + "</td>");
                str.Append("</tr>");
            }

            str.Append("</table>");

            if (isGridData == true)
            {
                objView.GridData = str.ToString();

            }
            else
            {
                objView.GridData = "";
                GenerateReport("ProjectListingReport_" + objView.ReportYear.ToString() + ".xls", str.ToString());
            }


        }

        public ActionResult ProjectSummaryReport(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            //  List<SelectListItem> branchList = new List<SelectListItem>();
            List<SelectListItem> salesmenList = new List<SelectListItem>();

            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
            }
           // salesmenList = CommonFunction.AllSalesmen();
            objView.Uid = uid;
            // objView.SalenmenList = salesmenList;
            objView.SalesmenAndUserList = CommonFunction.SalesmenListStatusWise("");
            objView.SalesmenStatusList = CommonFunction.SalesmenStatusList();
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            if (objView.from_date == null)
            {
                objView.from_date = startDate;
            }

            if (objView.to_date == null)
            {
                objView.to_date = endDate;
            }

            ExportProjectSummeryReport(objView, true);

            return View(objView);
        }

        public void ExportProjectSummeryReport(ReportViewModel objView, bool isGridData = false)
        {
            string uid = User.Identity.GetUserId();
            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
            }

            List<Database.SSP_ProjectSummeryReport_Result> reportData = new List<Database.SSP_ProjectSummeryReport_Result>();

            using (Database.PMSEntities objDB = new Database.PMSEntities())
            {
                reportData = objDB.SSP_ProjectSummeryReport(uid, objView.from_date, objView.to_date, SessionManagement.SelectedBranchID, objView.SalesmenId).ToList();
                reportData = reportData.OrderBy(o => o.contract_date).ThenBy(o => o.contract_date).ToList();
            }

            StringBuilder str = new StringBuilder("");

            string salesmanname = "";

            str.Append("<table width='100%' class='table table-striped'>");

            if (isGridData == false)
            {
                if (objView.SalesmenId > 0)
                {
                    salesmanname = CommonFunction.AllSalesmen().Where(o => o.Value == objView.SalesmenId.ToString()).Select(o => o.Text).SingleOrDefault();
                }

                str.Append("<tr>");
                str.Append("<th style='text-align:left;' colspan='7'><b>Weekly Project report</b></th>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th  style='text-align:left;'  colspan='7'><b>Branch: </b> " + SessionManagement.SelectedBranchName + "</th>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<td style='text-align:left;' colspan='2'><b>From:</b> " + Convert.ToDateTime(objView.from_date).ToString("dd/MM/yyyy") + "</td>");
                str.Append("<td style='text-align:left;' colspan='5'><b>To:</b> " + Convert.ToDateTime(objView.to_date).ToString("dd/MM/yyyy") + "</td>");
                str.Append("</tr>");

                if (Convert.ToString(salesmanname) != "")
                {
                    str.Append("<tr>");
                    str.Append("<td style='text-align:left;' colspan='7'><b>Salesmen: </b> " + salesmanname + "</td>");
                    str.Append("</tr>");
                }
            }


            int i = 0;
            int yer = 0;
            StringBuilder innerStr = new StringBuilder("");

            foreach (var groups in reportData.GroupBy(o => o.contract_date))
            {
                foreach (var items in groups)
                {
                    if (yer != items.contract_date)
                    {
                        yer = items.contract_date ?? 0;
                        i = 0;
                        str.Append("<tr>");
                        str.Append("<td style='text-align:left;' colspan='7'></td>");
                        str.Append("</tr>");

                        str.Append("<tr><th>No.</th><th>Job Site " + items.contract_date + "</th><th>Salesmen</th><th>Date of Collect Payment</th><th style='text-align:right;'>Contract Value</th><th style='text-align:right;'>Progress Claim</th><th style='text-align:right;'>Balance Amt</th><th style='text-align:right;'>Costing Amt</th><th style='text-align:right;'>PC - CA</th>");
                        str.Append("<th>Remarks</th>");
                        /*if (isGridData == false)
                        {
                            str.Append("<th>Remarks</th>");
                        }*/
                        str.Append("<th> Profit Margin (%)</th>");
                        str.Append("</tr>");
                    }

                    i = i + 1;

                    str.Append("<tr>");
                    str.Append("<td>" + i + "</td><td>" + items.project_name + "</td><td>" + items.salesmen_name + "</td><td>" + items.receipt_date + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(items.contract_value) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(items.progress_claim) + "</td>");
                    str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal((items.contract_value - items.progress_claim)) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.costing_amt)) + "</td>"+ "</td>");
                    var pcbcdif = CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.progress_claim) - Convert.ToDecimal(items.costing_amt));
                    if (Convert.ToDecimal(pcbcdif)>0)
                    {
                        str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.progress_claim) - Convert.ToDecimal(items.costing_amt)) + "</td>");
                    }
                    else
                    {
                        str.Append("<td align='right' style='color: red'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.progress_claim) - Convert.ToDecimal(items.costing_amt)) + "</td>");
                    }
                    str.Append("<td align='right'>" + items.remarks + "</td>");
                    str.Append("<td align='right'>" + Math.Round(items.ProfitMargin, 2) + "%</td>");
                    /*if (isGridData == false)
                    {
                        str.Append("<td>Remarks</td>");
                    }*/

                    str.Append("</tr>");


                    //str.Append("<tr>");
                    //str.Append("<td></td><td></td><td></td><td>$" + CommonFunction.ConvertAmountoDecimal(reportData.Sum(o => o.contract_value).ToString()) + "</td><td>$" + CommonFunction.ConvertAmountoDecimal(reportData.Sum(o => o.progress_claim).ToString()) + "</td>");
                    //str.Append("<td>$" + CommonFunction.ConvertAmountoDecimal((reportData.Sum(o => o.contract_value) - reportData.Sum(o => o.progress_claim)).ToString()) + "</td><td>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Sum(o => o.costing_amt)).ToString()) + "</td>");
                    //str.Append("</tr>");

                }

                str.Append("<tr>");
                str.Append("<td></td><td></td><td></td><td></td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(groups.Sum(o => o.contract_value)) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(groups.Sum(o => o.progress_claim)) + "</td>");
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal((groups.Sum(o => o.contract_value) - groups.Sum(o => o.progress_claim))) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(groups.Sum(o => o.costing_amt))) + "</td>");
                if (isGridData == false)
                {
                    str.Append("<td></td>");
                }
                str.Append("</tr>");
            }



            str.Append("</table>");

            if (isGridData == true)
            {
                objView.GridData = str.ToString();
            }
            else
            {
                objView.GridData = "";
                GenerateReport("ProjectSummaryReport_" + objView.ReportYear.ToString() + ".xls", str.ToString());
            }
        }

        [ActionName("ProjectbyAddress")]
        public ActionResult ProjectStatusbyBranchReport(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
            }
            objView.Uid = uid;

            objView.YearList = Common.CommonFunction.YearList();
            if (objView.ReportYear == 0)
            {
                objView.ReportYear = DateTime.Now.Year;
            }
            //List<SelectListItem> branchList = new List<SelectListItem>();
            //if (uid == "")
            //{
            //    branchList = Common.CommonFunction.BranchList();
            //}
            //else
            //{
            //    branchList = Common.CommonFunction.UserBranchList(uid);
            //}
            //branchList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
            //objView.BranchList = branchList;
            //objView.BranchId = 0;
            ExportProjectStatusbyBranchReport(objView, true);
            return View("ProjectStatusbyBranchReport", objView);

        }

        public void ExportProjectStatusbyBranchReport(ReportViewModel objView, bool isGridData = false)
        {
            string uid = User.Identity.GetUserId();
            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
            }

            List<Database.SSP_ProjectListingStatusByBranch_Result> reportData = new List<Database.SSP_ProjectListingStatusByBranch_Result>();

            using (Database.PMSEntities objDB = new Database.PMSEntities())
            {
                reportData = objDB.SSP_ProjectListingStatusByBranch(uid, objView.ReportYear, SessionManagement.SelectedBranchID).ToList();
            }

            StringBuilder str = new StringBuilder("");
            str.Append("<table width='100%' class='table table-striped'>");
            if (isGridData == false)
            {
                str.Append("<tr>");
                str.Append("<th style='text-align:left;' colspan='8'><b>Project By Address</b></th>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<th style='text-align:left;' colspan='8'><b>Branch: </b>" + SessionManagement.SelectedBranchName + "</th>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<th style='text-align:left;' colspan='8'><b>Year: </b>" + objView.ReportYear.ToString() + "</th>");
                str.Append("</tr>");
            }
            str.Append("<tr><td><b>Updated: " + Convert.ToDateTime(DateTime.Now).ToString("MM/dd/yyyy").ToString() + "</b></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
            str.Append("<tr>");
            //string headerstyle = "style='background-color:#bfbfbf'";
            string FirstCharStyle = "style='background-color:#f2dcdb'";

            str.Append("<th>Job Site " + objView.ReportYear.ToString() + "</th><th>Contact No.</th><th>Customer</th><th>ID In-Charge</th><th>Date of Contract</th><th>Tiles</th><th>M. Bldg Products</th><th>Project Closed</th>");
            str.Append("</tr>");

            string first_char = "";
            foreach (var items in reportData)
            {
                string job_sites = Convert.ToString(items.job_sites);

                string contact_no = Convert.ToString(items.contact_no);
                string customer = Convert.ToString(items.customer);
                string id_in_charge = Convert.ToString(items.id_in_charge);
                string contract_date = Convert.ToString(items.contract_date);
                string tiles = Convert.ToString(items.tiles);
                string m_bldg_products = Convert.ToString(items.m_bldg_products);
                string project_closed = Convert.ToString(items.project_closed);
                if (job_sites.ToString().Length > 0)
                {
                    if (job_sites.Substring(0, 1).ToString().ToUpper() != first_char.ToString())
                    {
                        first_char = job_sites.Substring(0, 1).ToString().ToUpper();
                        str.Append("<tr><td " + FirstCharStyle + "><b>" + job_sites.Substring(0, 1).ToString().ToUpper() + "</b></td><td " + FirstCharStyle + "></td><td " + FirstCharStyle + "></td><td " + FirstCharStyle + "></td><td " + FirstCharStyle + "></td><td " + FirstCharStyle + "></td><td " + FirstCharStyle + "></td><td " + FirstCharStyle + "></td></tr>");
                    }
                }

                str.Append("<tr>");
                str.Append("<td>" + items.project_name + "</td><td>" + contact_no + "</td><td>" + customer + "</td><td>" + id_in_charge + "</td><td>" + contract_date + "</td>");
                str.Append("<td>" + tiles + "</td><td>" + m_bldg_products + "</td><td>" + project_closed + "</td>");
                str.Append("</tr>");
                if (job_sites.ToString().Length > 0)
                {
                    if (job_sites.Substring(0, 1).ToString().ToUpper() != first_char.ToString())
                    {
                        str.Append("<tr><td></td></tr><tr><td></td></tr><tr><td></td></tr><tr><td></td></tr>");
                    }
                }
            }
            str.Append("</table>");

            if (isGridData == true)
            {
                objView.GridData = str.ToString();

            }
            else
            {
                objView.GridData = "";
                GenerateReport("ProjectStatusbyBranchReport_" + objView.ReportYear.ToString() + ".xls", str.ToString());
            }

        }


        public ActionResult ProjectCostingReport(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            List<SelectListItem> branchList = new List<SelectListItem>();
            if (User.IsInRole("SuperAdmin"))
            {
                uid = "00000000-0000-0000-0000-000000000000";
            }

            objView.Uid = uid;
            objView.YearList = CommonFunction.YearList();
            if (objView.ReportYear == 0)
            {
                objView.ReportYear = DateTime.Now.Year;
            }
            //objView.SalenmenList = CommonFunction.AllSalesmen();
            objView.SalesmenAndUserList = CommonFunction.SalesmenListStatusWise("");
            objView.SalesmenStatusList = CommonFunction.SalesmenStatusList(); 
            //objView.ProjectList = CommonFunction.UserProjectList(Convert.ToString(SessionManagement.UserID));
            objView.ProjectList = CommonFunction.UserProjectListByYear(objView.Uid, objView.ReportYear, objView.SalesmenId);

            if (objView.ProjectId > 0)
            {
                ExportProjectCostingReport(objView, true);
            }

            return View(objView);
        }

        public class PaymentCls
        {
            //public DateTime? Date { get; set; }
            //public string Description { get; set; }
            public string SupplierName { get; set; }
            public string InvNo { get; set; }
            public decimal? NonGst { get; set; }
            public decimal? Gst { get; set; }
            public decimal AgreedAmountWithoutGst { get; set; }
            public decimal AgreedAmount { get; set; }
            public string PaymentDate { get; set; }
            public string Invoicedate { get; set; }
            public int invoice_id { get; set; }
        }

        public class ReceiptCls
        {
            public DateTime? ReceiptDate { get; set; }
            public string ChequeDetails { get; set; }
            public string Remarks { get; set; }
            public decimal? Amt { get; set; }
            public decimal? TotalAmt { get; set; }
            public string invoice_number { get; set; }
        }

        #region Backup
        //public void ExportProjectCostingReport(ReportViewModel objView, bool isGridData = false)
        //{
        //    StringBuilder str = new StringBuilder("");

        //    Database.SSP_ProjectCostingReport_Result costingResult = new Database.SSP_ProjectCostingReport_Result();
        //    // List<Database.receipt> receiptResult = new List<Database.receipt>();
        //    List<ReceiptCls> receiptResult = new List<ReceiptCls>();
        //    List<PaymentCls> paymentResult = new List<PaymentCls>();
        //    List<Database.additions_omissions> additionResult = new List<Database.additions_omissions>();
        //    List<Database.SSP_ProjectsBySalesmanReport_Result> SalesManprojects = new List<Database.SSP_ProjectsBySalesmanReport_Result>();

        //    using (Database.PMSEntities objDB = new Database.PMSEntities())
        //    {
        //        costingResult = objDB.SSP_ProjectCostingReport(objView.ProjectId).SingleOrDefault();
        //        paymentResult = (from p in objDB.payments
        //                         join c in objDB.payment_detail on p.id equals c.payment_id
        //                         join s in objDB.suppliers on p.supplier_id equals s.id
        //                         join si in objDB.tbl_supplierinvoice_items on c.supplierinvoice_item_id equals si.Id
        //                         // where c.project_id == objView.ProjectId
        //                         where si.project_id == objView.ProjectId
        //                         orderby p.payment_date
        //                         select new PaymentCls
        //                         {
        //                             Date = p.payment_date,
        //                             // Description = p.remarks,
        //                             SupplierName = s.supplier_name,
        //                             InvNo = si.invoice_number,
        //                             //NonGst = c.invoice_amount,
        //                             Gst = c.payment_amount,
        //                             AgreedAmount = c.agreed_amount.HasValue ? c.agreed_amount.Value : 0
        //                         }
        //                     ).ToList();

        //        receiptResult = (from r in objDB.receipts
        //                         join c in objDB.receipt_detail on r.id equals c.receipt_id
        //                         where c.project_id == objView.ProjectId
        //                         orderby r.receipt_date
        //                         select new ReceiptCls
        //                         {
        //                             ReceiptDate = r.receipt_date,
        //                             ChequeDetails = r.cheque_details,
        //                             Remarks = r.remarks,
        //                             Amt = c.amount,
        //                             TotalAmt = c.total_amount
        //                         }
        //                     ).ToList();


        //        // receiptResult = objDB.receipt_detail.Where(o => o.project_id == objView.ProjectId).ToList();
        //        additionResult = objDB.additions_omissions.Where(o => o.project_id == objView.ProjectId).ToList();

        //        SalesManprojects = objDB.SSP_ProjectsBySalesmanReport(objView.SalesmenId, objView.ProjectId).ToList();
        //    }

        //    str.Append("<table class='table'>");
        //    str.Append("<tr>");
        //    str.Append("<td colspan='11' style='text-align:center;'><strong>Project Costing</strong></td>");
        //    str.Append("</tr>");
        //    str.Append("<tr>");
        //    str.Append("<th style='text-align:left;' colspan='11'><b>Branch: </b>" + SessionManagement.SelectedBranchName + "</th>");
        //    str.Append("</tr>");
        //    str.Append("<tr>");
        //    str.Append("<td><strong>Date:</strong></td>");
        //    str.Append("<td colspan='4'>" + costingResult.contract_date + "</td>");
        //    str.Append("<td></td>");
        //    str.Append("<td><strong>Project No:</strong></td>");
        //    str.Append("<td colspan='4'>" + costingResult.project_number + "</td>");
        //    str.Append("</tr>");
        //    str.Append("<tr>");
        //    str.Append("<td><strong>Sales Person:</strong></td>");
        //    str.Append("<td colspan='4'>" + costingResult.salesmen_name + "</td>");
        //    str.Append("<td></td>");
        //    str.Append("<td><strong>Email:</></td>");
        //    str.Append("<td colspan='4'>" + costingResult.email + "</td>");
        //    str.Append("</tr>");

        //    str.Append("<tr>");
        //    str.Append("<td><strong>Customer:</strong></td>");
        //    str.Append("<td colspan='4'>" + costingResult.customer_name + "</td>");
        //    str.Append("<td></td>");
        //    str.Append("<td><strong>NRIC No:</strong></td>");
        //    str.Append("<td colspan='4'>" + costingResult.nric1 + "</td>");
        //    str.Append("</tr>");
        //    str.Append("<tr>");

        //    if (costingResult.Name2 != "")
        //    {
        //        str.Append("<tr>");
        //        str.Append("<td><strong>Customer:</strong></td>");
        //        str.Append("<td colspan='4'>" + costingResult.Name2 + "</td>");
        //        str.Append("<td></td>");
        //        str.Append("<td><strong>NRIC No:</strong></td>");
        //        str.Append("<td colspan='4'>" + costingResult.Nric2 + "</td>");
        //        str.Append("</tr>");
        //        str.Append("<tr>");
        //    }

        //    str.Append("<td><strong>Address:</strong></td>");
        //    str.Append("<td colspan='4'>" + costingResult.address + "</td>");
        //    str.Append("<td></td>");
        //    str.Append("<td><strong>Contact No:</strong></td>");
        //    str.Append("<td colspan='4'>" + costingResult.contact_no + "</td>");
        //    str.Append("</tr>");

        //    str.Append("<tr>");
        //    str.Append("<td colspan='2'><strong>Start Of  Work:</strong></td>");
        //    str.Append("<td colspan='3'><strong>HDB Permit Date:</strong></td>");
        //    str.Append("<td></td>");
        //    str.Append("<td colspan='5'><strong>Completion:</strong></td>");
        //    str.Append("</tr>");

        //    str.Append("<tr>");
        //    str.Append("<td colspan='5'>");
        //    str.Append("<table class='table'>");
        //    str.Append("<tr style='background-color:#d2d2d2;'>");
        //    str.Append("<th>Date</th>");
        //    str.Append("<th>Supplier Name</th>");
        //    str.Append("<th>INV No</th>");
        //    str.Append("<th style='text-align:right;'>Agreed Amount</th>");
        //    str.Append("<th style='text-align:right;'>Payment Amount</th>");
        //    //str.Append("<th style='text-align:right;'>GST Supplier</th>");
        //    str.Append("</tr>");

        //    foreach (var payments in paymentResult)
        //    {
        //        str.Append("<tr>");
        //        str.Append("<td>" + Convert.ToDateTime(payments.Date).ToString("dd/MM/yyyy") + "</td>");
        //        str.Append("<td>" + Convert.ToString(payments.SupplierName) + "</td>");
        //        str.Append("<td>" + payments.InvNo + "</td>");
        //        //str.Append("<td align='right'>$" + payments.NonGst + "</td>");
        //        str.Append("<td align='right'>$" + payments.AgreedAmount + "</td>");
        //        str.Append("<td align='right'>$" + payments.Gst + "</td>");
        //        str.Append("</tr>");
        //    }

        //    str.Append("<tr>");
        //    str.Append("<td colspan='3' style='background-color:#efefef;'><strong>Total Costing</strong></td>");
        //    //str.Append("<td style='background-color:#efefef;' align='right'><strong>$" + paymentResult.Sum(o => o.NonGst) + "</strong></td>");
        //    str.Append("<td style='background-color:#efefef;' align='right'><strong>$" + paymentResult.Sum(o => o.AgreedAmount) + "</strong></td>");
        //    str.Append("<td style='background-color:#efefef;' align='right'><strong>$" + paymentResult.Sum(o => o.Gst) + "</strong></td>");
        //    //str.Append("<td style='background-color:#efefef;' align='right'><strong>$" + paymentResult.Sum(o => o.AgreedAmount) + "</strong></td>");
        //    str.Append("</tr>");

        //    str.Append("</table>");

        //    #region New Section 
        //    str.Append("<table class='table'>");
        //    str.Append("<tr style='background-color:#d2d2d2;'>");
        //    str.Append("<th>Date</th>");
        //    str.Append("<th>Rec Type</th>");
        //    str.Append("<th>Payment Mode</th>");
        //    str.Append("<th style='text-align:right;'>Amount</th>");
        //    str.Append("</tr>");
        //    if (SalesManprojects.Count() > 0)
        //    {
        //        foreach (var project in SalesManprojects)
        //        {
        //            str.Append(string.Format("</tr><td>{0}</td><td>{1}</td><td>{2}</td><td style='text-align:right;'>${3}</td></tr>", Convert.ToDateTime(project.LoanDate).ToString("dd/MM/yyyy"), CommonFunction.GetRecordTypeById(project.rec_type.HasValue ? project.rec_type.Value : 0), project.mode_of_payment, project.amount));
        //        }
        //    }
        //    else
        //    {

        //        str.Append("</tr><td colspan='3'>no records found</td></tr>");
        //    }

        //    str.Append("</table>");
        //    #endregion

        //    str.Append("</td>");

        //    str.Append("<td></td>");

        //    str.Append("<td colspan='5'>");
        //    str.Append("<table class='table'>");
        //    str.Append("<tr style='background-color:#d2d2d2;'>");
        //    str.Append("<th>Cheque Number</th>");
        //    str.Append("<th style='text-align:right;'>Payment Collected</th>");
        //    str.Append("<th style='text-align:right;'>Non GST Payment</th>");
        //    str.Append("<th style='text-align:right;'>GST Payment</th>");
        //    str.Append("<th>Date Payment</th>");
        //    str.Append("</tr>");

        //    foreach (var recepts in receiptResult)
        //    {
        //        str.Append("<td>" + recepts.ChequeDetails + "</td>");
        //        str.Append("<td>" + recepts.Remarks + "</td>");
        //        if (receiptResult.Sum(o => o.Amt) < 0)
        //        {
        //            str.Append("<td align='right' style='color:red;'>$" + receiptResult.Sum(o => o.Amt) + "</td>");
        //        }
        //        else
        //        {
        //            str.Append("<td align='right'>$" + receiptResult.Sum(o => o.Amt) + "</td>");
        //        }

        //        if (receiptResult.Sum(o => o.TotalAmt) < 0)
        //        {
        //            str.Append("<td align='right' style='color:red;'>$" + receiptResult.Sum(o => o.TotalAmt) + "</td>");
        //        }
        //        else
        //        {
        //            str.Append("<td align='right'>$" + receiptResult.Sum(o => o.TotalAmt) + "</td>");
        //        }

        //        str.Append("<td>" + Convert.ToDateTime(recepts.ReceiptDate).ToString("dd/MM/yyyy") + "</td>");
        //        str.Append("</tr>");
        //    }

        //    str.Append("<tr style='background-color:#D9EDF7;'>");
        //    str.Append("<td colspan='2' class='info'>Total Amount Collected</td>");
        //    str.Append("<td class='info' align='right'>$" + receiptResult.Sum(o => o.Amt) + "</td>");
        //    str.Append("<td class='info' align='right'>$" + receiptResult.Sum(o => o.Amt) + "</td>");
        //    str.Append("<td class='info'></td>");
        //    str.Append("</tr>");

        //    str.Append("<tr><td colspan='5'></td></tr>");

        //    str.Append("<tr style='background-color:#d2d2d2;'>");
        //    str.Append("<th colspan='2'>Contract</th>");
        //    str.Append("<th style='text-align:right;'>Non GST</th>");
        //    str.Append("<th style='text-align:right;'>w/ GST</th>");
        //    str.Append("<th style='text-align:right;'>GST</th>");
        //    str.Append("</tr>");

        //    str.Append("<tr>");
        //    str.Append("<td colspan='2'>1st Contract Amount (Project)</td>");
        //    str.Append("<td align='right'>$" + costingResult.contract_amount + "</td>");
        //    str.Append("<td align='right'>$" + costingResult.total_amount + "</td>");
        //    str.Append("<td align='right'>$" + costingResult.gst_amount + "</td>");
        //    str.Append("</tr>");
        //    decimal totalNonGSTAddition = Convert.ToDecimal(costingResult.contract_amount);
        //    decimal totalGSTAddition = Convert.ToDecimal(costingResult.total_amount);
        //    decimal totalGSTPer = Convert.ToDecimal(costingResult.gst_amount);

        //    foreach (var additions in additionResult)
        //    {
        //        str.Append("<tr>");
        //        //if (additions.record_type == 1)
        //        if (additions.record_type != Convert.ToInt32(CommonHelper.RecordType.LoanReturn))
        //        {
        //            totalNonGSTAddition = totalNonGSTAddition + Convert.ToDecimal(additions.amount);
        //            totalGSTAddition = totalGSTAddition + Convert.ToDecimal(additions.total_amount);
        //            totalGSTPer = totalGSTPer + Convert.ToDecimal(additions.gst_amount);

        //            str.Append("<td colspan='2'>");
        //            str.Append("Additional");
        //        }
        //        else
        //        {
        //            totalNonGSTAddition = totalNonGSTAddition - Convert.ToDecimal(additions.amount);
        //            totalGSTAddition = totalGSTAddition - Convert.ToDecimal(additions.total_amount);
        //            totalGSTPer = totalGSTPer - Convert.ToDecimal(additions.gst_amount);
        //            str.Append("<td colspan='2' style='color:red;'>");
        //            str.Append("Omission");
        //        }

        //        str.Append("</td>");
        //        str.Append("<td align='right'>$" + additions.amount + "</td>");
        //        str.Append("<td align='right'>$" + additions.total_amount + "</td>");
        //        str.Append("<td align='right'>$" + additions.gst_amount + "</td>");
        //        str.Append("</tr>");
        //    }

        //    str.Append("<tr style='background-color:#D9EDF7;'>");
        //    str.Append("<td colspan='2' class='info'>Total Contract Amount</td>");
        //    str.Append("<td class='info' align='right'>$" + totalNonGSTAddition + "</td>");
        //    str.Append("<td class='info' align='right'>$" + totalGSTAddition + "</td>");
        //    str.Append("<td class='info' align='right'>$" + totalGSTPer + "</td>");
        //    str.Append("</tr>");

        //    str.Append("<tr><td colspan='5'></td></tr>");

        //    str.Append("<tr style='background-color:#D9EDF7;'><td colspan='2'>Balance Amount Due</td><td align='right' style='color:red;'>$" + (totalNonGSTAddition - Convert.ToDecimal(receiptResult.Sum(o => o.Amt))) + "</td><td align='right' style='color:red;'>$" + (totalGSTAddition - Convert.ToDecimal(receiptResult.Sum(o => o.TotalAmt))) + "</td><td align='right'></td></tr>");

        //    str.Append("<tr><td colspan='5'></td></tr>");

        //    str.Append("<tr>");
        //    str.Append("<td colspan='2' style='background-color:#efefef;'><strong>Profit & Loss</strong></td>");
        //    str.Append("<td style='background-color:#efefef; text-align:right;'><strong>Non GST</strong></td>");
        //    str.Append("<td style='background-color:#efefef; text-align:right;'><strong>w/ GST</strong></td>");
        //    str.Append("<td></td>");
        //    str.Append("</tr>");

        //    str.Append("<tr>");
        //    str.Append("<td colspan='2'>Final Amount Collected</td>");
        //    str.Append("<td align='right'>$" + receiptResult.Sum(o => o.Amt) + "</td>");
        //    str.Append("<td align='right'>$" + receiptResult.Sum(o => o.TotalAmt) + "</td>");
        //    str.Append("<td></td>");
        //    str.Append("</tr>");

        //    str.Append("<tr>");
        //    str.Append("<td colspan='2'>Total Costing</td>");
        //    str.Append("<td align='right'>$" + paymentResult.Sum(o => o.NonGst) + "</td>");
        //    str.Append("<td align='right'>$" + paymentResult.Sum(o => o.Gst) + "</td>");
        //    str.Append("<td></td>");
        //    str.Append("</tr>");

        //    decimal costingNonGST = Convert.ToDecimal(paymentResult.Sum(o => o.NonGst));
        //    decimal costingWithGST = Convert.ToDecimal(paymentResult.Sum(o => o.Gst));

        //    decimal totalFinalAmtNonGST = Convert.ToDecimal(receiptResult.Sum(o => o.Amt));
        //    decimal totalFinalAmtWithGST = Convert.ToDecimal(receiptResult.Sum(o => o.TotalAmt));

        //    str.Append("<tr>");
        //    str.Append("<td colspan='2'>Total Profit</td>");
        //    str.Append("<td align='right'>$" + (totalFinalAmtNonGST - costingNonGST) + "</td>");
        //    str.Append("<td align='right'>$" + (totalFinalAmtWithGST - costingWithGST) + "</td>");
        //    str.Append("<td></td>");
        //    str.Append("</tr>");

        //    decimal salesmenComm = 0;
        //    if (costingResult.saleman_commission > 0)
        //    {
        //        salesmenComm = Math.Round((Convert.ToDecimal((totalFinalAmtNonGST - costingNonGST) * costingResult.saleman_commission) / 100), 2);
        //    }
        //    str.Append("<tr>");
        //    str.Append("<td colspan='2'>Sales Commission (%)</td>");
        //    str.Append("<td align='right'>$" + salesmenComm + "</td>");
        //    str.Append("<td></td>");
        //    str.Append("<td></td>");
        //    str.Append("</tr>");
        //    decimal profit = 100;
        //    if (totalFinalAmtNonGST > 0)
        //    {
        //        profit = Math.Round(((totalFinalAmtNonGST - costingNonGST) / totalFinalAmtNonGST) * 100, 2);
        //    }

        //    str.Append("<tr>");
        //    str.Append("<td colspan='2'>Profit Margin (%)</td>");
        //    str.Append("<td align='right'>" + profit + "%</td>");
        //    str.Append("<td></td>");
        //    str.Append("<td></td>");
        //    str.Append("</tr>");

        //    str.Append("</table>");
        //    str.Append("</td>");
        //    str.Append("</tr>");
        //    str.Append("</table>");

        //    if (isGridData == true)
        //    {
        //        objView.GridData = str.ToString();
        //    }
        //    else
        //    {
        //        objView.GridData = "";
        //        GenerateReport("ProjectCostingReport_" + objView.ProjectId + ".xls", str.ToString());
        //    }
        //}
        #endregion


        public void ExportProjectCostingReport(ReportViewModel objView, bool isGridData = false)
        {
            StringBuilder str = new StringBuilder("");

            Database.SSP_ProjectCostingReport_Result costingResult = new Database.SSP_ProjectCostingReport_Result();
            // List<Database.receipt> receiptResult = new List<Database.receipt>();
            List<ReceiptCls> receiptResult = new List<ReceiptCls>();
            List<PaymentCls> paymentResult = new List<PaymentCls>();
            List<Database.additions_omissions> additionResult = new List<Database.additions_omissions>();
            List<Database.SSP_ProjectsBySalesmanReport_Result> SalesManprojects = new List<Database.SSP_ProjectsBySalesmanReport_Result>();

            using (Database.PMSEntities objDB = new Database.PMSEntities())
            {
                costingResult = objDB.SSP_ProjectCostingReport(objView.ProjectId).SingleOrDefault();

                paymentResult = objDB.SSP_SupplierPaymentAndAssignedInvoices(objView.ProjectId)
                    .Select(d => new PaymentCls()
                    {
                        AgreedAmountWithoutGst = d.AgreedAmountWithoutGst,
                        AgreedAmount = d.AgreedAmount,
                        Gst = d.Gst,
                        InvNo = d.InvNo,
                        invoice_id=d.invoice_id,
                        SupplierName = d.SupplierName,
                        Invoicedate = d.invoice_date,
                        PaymentDate = d.payment_date,
                    }).ToList();
                //if (paymentResult != null && paymentResult.Count > 0)
                //{
                //    paymentResult = paymentResult.GroupBy(i => i.invoice_id).Select(i => new PaymentCls()
                //    {
                //        AgreedAmountWithoutGst = i.FirstOrDefault().AgreedAmountWithoutGst,
                //        AgreedAmount = i.FirstOrDefault().AgreedAmount,
                //        Gst = i.Sum(item => item.Gst),
                //        InvNo = i.FirstOrDefault().InvNo,
                //        invoice_id = i.FirstOrDefault().invoice_id,
                //        SupplierName = i.FirstOrDefault().SupplierName,
                //        Invoicedate = i.FirstOrDefault().Invoicedate,
                //        PaymentDate = i.OrderByDescending(x => x.PaymentDate).FirstOrDefault().PaymentDate,
                //    }).ToList();
                //}
                //paymentResult = (from p in objDB.payments
                //                 join c in objDB.payment_detail on p.id equals c.payment_id
                //                 join s in objDB.suppliers on p.supplier_id equals s.id
                //                 join si in objDB.tbl_supplierinvoice_items on c.supplierinvoice_item_id equals si.Id
                //                 // where c.project_id == objView.ProjectId
                //                 where si.project_id == objView.ProjectId
                //                 orderby p.payment_date
                //                 select new PaymentCls
                //                 {
                //                     Date = p.payment_date,
                //                     // Description = p.remarks,
                //                     SupplierName = s.supplier_name,
                //                     InvNo = si.invoice_number,
                //                     //NonGst = c.invoice_amount,
                //                     Gst = c.payment_amount,
                //                     AgreedAmount = c.agreed_amount.HasValue ? c.agreed_amount.Value : 0
                //                 }
                //             ).ToList();
                #region Backup 10052019
                //var receiptResult2 = (from r in objDB.receipts
                //                      join c in objDB.receipt_detail on r.id equals c.receipt_id
                //                      where c.project_id == objView.ProjectId
                //                      orderby r.receipt_date
                //                      select new ReceiptCls
                //                      {
                //                          ReceiptDate = r.receipt_date,
                //                          ChequeDetails = r.cheque_details,
                //                          Remarks = r.remarks,
                //                          Amt = c.amount,
                //                          TotalAmt = c.total_amount
                //                      }
                //                ).ToList();
                #endregion




                receiptResult = (from r in objDB.receipts
                                 join c in objDB.receipt_detail on r.id equals c.receipt_id
                                 join inv in objDB.tbl_invoice on c.invoice_id equals inv.Id
                                 where c.project_id == objView.ProjectId
                                 orderby r.receipt_date
                                 select new ReceiptCls
                                 {
                                     ReceiptDate = r.receipt_date,
                                     ChequeDetails = r.cheque_details,
                                     Remarks = r.remarks,
                                     Amt = c.amount,
                                     TotalAmt = c.total_amount,
                                     invoice_number = inv.Invoice_number
                                 }
                             ).ToList();



                // receiptResult = objDB.receipt_detail.Where(o => o.project_id == objView.ProjectId).ToList();
                additionResult = objDB.additions_omissions.Where(o => o.project_id == objView.ProjectId).ToList();

                SalesManprojects = objDB.SSP_ProjectsBySalesmanReport(objView.SalesmenId, objView.ProjectId).ToList();
            }

            str.Append("<table class='table'>");
            str.Append("<tr>");
            str.Append("<td colspan='11' style='text-align:center;'><strong>Project Costing</strong></td>");
            str.Append("</tr>");
            str.Append("<tr>");
            str.Append("<th style='text-align:left;' colspan='11'><b>Branch: </b>" + SessionManagement.SelectedBranchName + "</th>");
            str.Append("</tr>");
            str.Append("<tr>");
            str.Append("<td><strong>Date:</strong></td>");
            str.Append("<td colspan='4'>" + costingResult.contract_date + "</td>");
            str.Append("<td></td>");
            str.Append("<td><strong>Project No:</strong></td>");
            str.Append("<td colspan='4'>" + costingResult.project_number + "</td>");
            str.Append("</tr>");
            str.Append("<tr>");
            str.Append("<td><strong>Sales Person:</strong></td>");
            str.Append("<td colspan='4'>" + costingResult.salesmen_name + "</td>");
            str.Append("<td></td>");
            str.Append("<td><strong>Email:</></td>");
            str.Append("<td colspan='4'>" + costingResult.email + "</td>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<td><strong>Customer:</strong></td>");
            str.Append("<td colspan='4'>" + costingResult.customer_name + "</td>");
            str.Append("<td></td>");
            str.Append("<td><strong>NRIC No:</strong></td>");
            str.Append("<td colspan='4'>" + costingResult.nric1 + "</td>");
            str.Append("</tr>");
            str.Append("<tr>");

            if (costingResult.Name2 != "")
            {
                str.Append("<tr>");
                str.Append("<td><strong>Customer:</strong></td>");
                str.Append("<td colspan='4'>" + costingResult.Name2 + "</td>");
                str.Append("<td></td>");
                str.Append("<td><strong>NRIC No:</strong></td>");
                str.Append("<td colspan='4'>" + costingResult.Nric2 + "</td>");
                str.Append("</tr>");
                str.Append("<tr>");
            }

            str.Append("<td><strong>Address:</strong></td>");
            str.Append("<td colspan='4'>" + costingResult.address + "</td>");
            str.Append("<td></td>");
            str.Append("<td><strong>Contact No:</strong></td>");
            str.Append("<td colspan='4'>" + costingResult.contact_no + "</td>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<td colspan='2'><strong>Start Of  Work:</strong></td>");
            str.Append("<td colspan='3'><strong>HDB Permit Date:</strong></td>");
            str.Append("<td></td>");
            str.Append("<td colspan='5'><strong>Completion:</strong></td>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<td colspan='5'>");
            str.Append("<table class='table'>");
            str.Append("<tr style='background-color:#d2d2d2;'>");
            str.Append("<th>Date</th>");
            str.Append("<th>Supplier Name</th>");
            str.Append("<th>INV No</th>");
            str.Append("<th style='text-align:right;'>Agreed Amount Without Gst</th>");
            str.Append("<th style='text-align:right;'>Agreed Amount With Gst</th>");
            str.Append("<th style='text-align:right;'>Payment Amount</th>");
            //str.Append("<th style='text-align:right;'>GST Supplier</th>");
            str.Append("</tr>");

            foreach (var payments in paymentResult)
            {
                str.Append("<tr>");
                str.Append(string.Format("<td>{0}</td>", !string.IsNullOrEmpty(payments.PaymentDate) ? payments.PaymentDate : payments.Invoicedate));
                //str.Append("<td>" + payments.PaymentDate != null ? Convert.ToDateTime(payments.PaymentDate).ToString("dd/MM/yyyy") : string.Empty + "</td>");
                //str.Append("<td>" + payments.Invoicedate != null ? Convert.ToDateTime(payments.Invoicedate).ToString("dd/MM/yyyy") : string.Empty + "</td>");
                str.Append("<td>" + Convert.ToString(payments.SupplierName) + "</td>");
                str.Append("<td>" + payments.InvNo + "</td>");
                //str.Append("<td align='right'>$" + payments.NonGst + "</td>");
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(payments.AgreedAmountWithoutGst)) + "</td>");//CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(nets)
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(payments.AgreedAmount)) + "</td>");
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(payments.Gst)) + "</td>");
                str.Append("</tr>");
            }

            str.Append("<tr>");
            str.Append("<td colspan='3' style='background-color:#efefef;'><strong>Total Costing</strong></td>");
            //str.Append("<td style='background-color:#efefef;' align='right'><strong>$" + paymentResult.Sum(o => o.NonGst) + "</strong></td>");
            str.Append("<td style='background-color:#efefef;' align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(paymentResult.Sum(o => o.AgreedAmountWithoutGst)))  + "</strong></td>");
            str.Append("<td style='background-color:#efefef;' align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(paymentResult.Sum(o => o.AgreedAmount))) + "</strong></td>");
            str.Append("<td style='background-color:#efefef;' align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(paymentResult.Sum(o => o.Gst))) + "</strong></td>");
            //str.Append("<td style='background-color:#efefef;' align='right'><strong>$" + paymentResult.Sum(o => o.AgreedAmount) + "</strong></td>");
            str.Append("</tr>");

            str.Append("</table>");

            #region New Section 
            str.Append("<table class='table'>");
            str.Append("<tr style='background-color:#d2d2d2;'>");
            str.Append("<th>Date</th>");
            str.Append("<th>Rec Type</th>");
            str.Append("<th>Payment Mode</th>");
            str.Append("<th>Remarks</th>");
            str.Append("<th style='text-align:right;'>Amount</th>");
            str.Append("</tr>");
            if (SalesManprojects.Count() > 0)
            {
                foreach (var project in SalesManprojects)
                {
                    str.Append(string.Format("</tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td style='text-align:right;'>${4}</td></tr>", Convert.ToDateTime(project.LoanDate).ToString("dd/MM/yyyy"), CommonFunction.GetRecordTypeById(project.rec_type.HasValue ? project.rec_type.Value : 0), project.mode_of_payment, project.purpose, CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(project.amount))));
                }
            }
            else
            {

                str.Append("</tr><td colspan='3'>no records found</td></tr>");
            }

            str.Append("</table>");
            #endregion

            str.Append("</td>");

            str.Append("<td></td>");

            str.Append("<td colspan='5'>");
            str.Append("<table class='table'>");
            str.Append("<tr style='background-color:#d2d2d2;'>");
            str.Append("<th>Cheque Number</th>");
            str.Append("<th>Invoice Number</th>");
            str.Append("<th style='text-align:right;'>Payment Collected</th>");
            str.Append("<th style='text-align:right;'>Non GST Payment</th>");
            str.Append("<th style='text-align:right;'>GST Payment</th>");
            str.Append("<th>Date Payment</th>");
            str.Append("</tr>");
            foreach (var recepts in receiptResult)
            {
                str.Append("<td>" + recepts.ChequeDetails + "</td>");
                str.Append("<td>" + recepts.invoice_number + "</td>");
                str.Append("<td>" + recepts.Remarks + "</td>");
                if (recepts.Amt < 0)
                {
                    str.Append("<td align='right' style='color:red;'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(recepts.Amt)) + "</td>");
                }
                else
                {
                    str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(recepts.Amt)) + "</td>");
                }

                if (recepts.TotalAmt < 0)
                {
                    str.Append("<td align='right' style='color:red;'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(recepts.TotalAmt)) + "</td>");
                }
                else
                {
                    str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(recepts.TotalAmt)) + "</td>");
                }
                str.Append("<td>" + Convert.ToDateTime(recepts.ReceiptDate).ToString("dd/MM/yyyy") + "</td>");
                str.Append("</tr>");
            }
            #region Backup
            //foreach (var recepts in receiptResult)
            //{
            //    str.Append("<td>" + recepts.ChequeDetails + "</td>");
            //    str.Append("<td>" + recepts.invoice_number + "</td>");
            //    str.Append("<td>" + recepts.Remarks + "</td>");
            //    if (receiptResult.Sum(o => o.Amt) < 0)
            //    {
            //        str.Append("<td align='right' style='color:red;'>$" + receiptResult.Sum(o => o.Amt) + "</td>");
            //    }
            //    else
            //    {
            //        str.Append("<td align='right'>$" + receiptResult.Sum(o => o.Amt) + "</td>");
            //    }

            //    if (receiptResult.Sum(o => o.TotalAmt) < 0)
            //    {
            //        str.Append("<td align='right' style='color:red;'>$" + receiptResult.Sum(o => o.TotalAmt) + "</td>");
            //    }
            //    else
            //    {
            //        str.Append("<td align='right'>$" + receiptResult.Sum(o => o.TotalAmt) + "</td>");
            //    }

            //    str.Append("<td>" + Convert.ToDateTime(recepts.ReceiptDate).ToString("dd/MM/yyyy") + "</td>");
            //    str.Append("</tr>");
            //}
            #endregion

            str.Append("<tr style='background-color:#D9EDF7;'>");
            str.Append("<td colspan='3' class='info'>Total Amount Collected</td>");
            str.Append("<td class='info' align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(receiptResult.Sum(o => o.Amt))) + "</td>");
            str.Append("<td class='info' align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(receiptResult.Sum(o => o.TotalAmt))) + "</td>");
            str.Append("<td class='info' style='border-bottom: 1px solid #ddd;'></td>");
            str.Append("</tr>");

            str.Append("<tr><td colspan='5'></td></tr>");

            str.Append("<tr style='background-color:#d2d2d2;'>");
            str.Append("<th colspan='3'>Contract</th>");
            str.Append("<th style='text-align:right;'>Non GST</th>");
            str.Append("<th style='text-align:right;'>w/ GST</th>");
            str.Append("<th style='text-align:right;'>GST</th>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<td colspan='3'>1st Contract Amount (Project)</td>");
            str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(costingResult.contract_amount)) + "</td>");
            str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(costingResult.total_amount)) + "</td>");
            str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(costingResult.gst_amount)) + "</td><td></td>");
            str.Append("</tr>");
            decimal totalNonGSTAddition = Convert.ToDecimal(costingResult.contract_amount);
            decimal totalGSTAddition = Convert.ToDecimal(costingResult.total_amount);
            decimal totalGSTPer = Convert.ToDecimal(costingResult.gst_amount);

            foreach (var additions in additionResult)
            {
                str.Append("<tr>");
                //if (additions.record_type == 1)
                if (additions.record_type != Convert.ToInt32(CommonHelper.RecordType.LoanReturn))
                {
                    totalNonGSTAddition = totalNonGSTAddition + Convert.ToDecimal(additions.amount);
                    totalGSTAddition = totalGSTAddition + Convert.ToDecimal(additions.total_amount);
                    totalGSTPer = totalGSTPer + Convert.ToDecimal(additions.gst_amount);

                    str.Append("<td colspan='3'>");
                    str.Append("Additional" + (!String.IsNullOrEmpty(additions.addition_omissioni_description) ? (" (" + additions.addition_omissioni_description.ToLower() + ")") : ("")));
                }
                else
                {
                    totalNonGSTAddition = totalNonGSTAddition - Convert.ToDecimal(additions.amount);
                    totalGSTAddition = totalGSTAddition - Convert.ToDecimal(additions.total_amount);
                    totalGSTPer = totalGSTPer - Convert.ToDecimal(additions.gst_amount);
                    str.Append("<td colspan='3' style='color:red;'>");
                    str.Append("Omission" + (!String.IsNullOrEmpty(additions.addition_omissioni_description) ? (" (" + additions.addition_omissioni_description.ToLower() + ")") : ("")));
                }

                str.Append("</td>");
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(additions.amount)) + "</td>");
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(additions.total_amount)) + "</td>");
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(additions.gst_amount)) + "</td><td></td>");
                str.Append("</tr>");
            }

            str.Append("<tr style='background-color:#D9EDF7;'>");
            str.Append("<td colspan='3' class='info'>Total Contract Amount</td>");
            str.Append("<td class='info' align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(totalNonGSTAddition)) + "</td>");
            str.Append("<td class='info' align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(totalGSTAddition)) + "</td>");
            str.Append("<td class='info' align='right' style='border-bottom: 1px solid #ddd;'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(totalGSTPer)) + "</td>");
            str.Append("</tr>");

            str.Append("<tr><td colspan='5'></td></tr>");

            str.Append("<tr style='background-color:#D9EDF7;'><td colspan='3'>Balance Amount Due</td><td align='right' style='color:red;'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(totalNonGSTAddition - Convert.ToDecimal(receiptResult.Sum(o => o.Amt)))) + "</td><td align='right' style='color:red;'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(totalGSTAddition - Convert.ToDecimal(receiptResult.Sum(o => o.TotalAmt)))) + "</td><td align='right' style='border-bottom: 1px solid #ddd;'></td></tr>");

            str.Append("<tr><td colspan='5'></td></tr>");

            str.Append("<tr>");
            str.Append("<td colspan='3' style='background-color:#efefef;'><strong>Profit & Loss</strong></td>");
            str.Append("<td style='background-color:#efefef; text-align:right;'><strong>Non GST</strong></td>");
            str.Append("<td style='background-color:#efefef; text-align:right;'><strong>w/ GST</strong></td>");
            str.Append("<td style='background-color:#efefef; text-align:right;'></td>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<td colspan='3'>Final Amount Collected</td>");
            str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(receiptResult.Sum(o => o.Amt))) + "</td>");
            str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(receiptResult.Sum(o => o.TotalAmt))) + "</td>");
            str.Append("<td></td>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<td colspan='3'>Total Costing</td>");
            str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(paymentResult.Sum(o => o.AgreedAmountWithoutGst))) + "</td>");
            str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(paymentResult.Sum(o => o.AgreedAmount))) + "</td>");
            //str.Append("<td align='right'>$" + paymentResult.Sum(o => o.Gst) + "</td>");
            str.Append("<td></td>");
            str.Append("</tr>");

            decimal costingNonGST = Convert.ToDecimal(paymentResult.Sum(o => o.AgreedAmountWithoutGst));
            //decimal costingWithGST = Convert.ToDecimal(paymentResult.Sum(o => o.Gst));
            decimal costingWithGST = Convert.ToDecimal(paymentResult.Sum(o => o.AgreedAmount));

            decimal totalFinalAmtNonGST = Convert.ToDecimal(receiptResult.Sum(o => o.Amt));
            decimal totalFinalAmtWithGST = Convert.ToDecimal(receiptResult.Sum(o => o.TotalAmt));

            str.Append("<tr>");
            str.Append("<td colspan='3'>Total Profit</td>");
            str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal((totalFinalAmtNonGST - costingNonGST))) + "</td>");
            str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal((totalFinalAmtWithGST - costingWithGST))) + "</td>");
            str.Append("<td></td>");
            str.Append("</tr>");

            decimal salesmenComm = 0;
            if (costingResult.saleman_commission > 0)
            {
                salesmenComm = Math.Round((Convert.ToDecimal((totalFinalAmtNonGST - costingNonGST) * costingResult.saleman_commission) / 100), 2);
            }
            str.Append("<tr>");
            str.Append("<td colspan='3'>Sales Commission (%)</td>");
            str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(salesmenComm)) + "</td>");
            str.Append("<td></td>");
            str.Append("<td></td>");
            str.Append("</tr>");
            decimal profit = 100;
            if (totalFinalAmtNonGST > 0)
            {
                profit = Math.Round(((totalFinalAmtNonGST - costingNonGST) / totalFinalAmtNonGST) * 100, 2);
            }

            str.Append("<tr>");
            str.Append("<td colspan='3'>Profit Margin (%)</td>");
            str.Append("<td align='right'>" + profit + "%</td>");
            str.Append("<td></td>");
            str.Append("<td></td>");
            str.Append("</tr>");

            str.Append("</table>");
            str.Append("</td>");
            str.Append("</tr>");
            str.Append("</table>");

            if (isGridData == true)
            {
                objView.GridData = str.ToString();
            }
            else
            {
                objView.GridData = "";
                GenerateReport("ProjectCostingReport_" + objView.ProjectId + ".xls", str.ToString());
            }
        }

        public ActionResult BankChequeDetails(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            //  List<SelectListItem> branchList = new List<SelectListItem>();
            List<SelectListItem> salesmenList = new List<SelectListItem>();

            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
            }
            salesmenList = CommonFunction.AllSalesmen();
            objView.Uid = uid;
            objView.SalenmenList = salesmenList;
            objView.BankList = CommonFunction.BankList();

            if (objView.from_date == null)
            {
                objView.from_date = DateTime.Now;
            }

            if (objView.to_date == null)
            {
                objView.to_date = DateTime.Now;
            }

            ExportBankChequeDetails(objView, true);

            return View(objView);
        }

        public void ExportBankChequeDetails(ReportViewModel objView, bool isGridData = false)
        {
            string uid = User.Identity.GetUserId();
            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
            }

            List<Database.SSP_BankChequeReport_Result> report_items = new List<Database.SSP_BankChequeReport_Result>();
            using (Database.PMSEntities objDB = new Database.PMSEntities())
            {
                report_items = objDB.SSP_BankChequeReport(objView.from_date, objView.to_date, SessionManagement.SelectedBranchID, objView.BankId, Convert.ToInt32(objView.CheckNoFrom), Convert.ToInt32(objView.CheckNoTo)).ToList();
            }

            StringBuilder str = new StringBuilder("");
            str.Append("<table width='100%' class='table table-striped'>");

            if (isGridData == false)
            {
                string bankname = "";
                if (objView.BankId > 0)
                {
                    bankname = CommonFunction.BankList().Where(o => o.Value == objView.BankId.ToString()).Select(o => o.Text).SingleOrDefault();
                }

                str.Append("<tr>");
                str.Append("<th style='text-align:left;' colspan='4'><b>Cheque Running Number</b></th>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<th style='text-align:left;'  colspan='4'><b>Branch: </b>" + SessionManagement.SelectedBranchName + "</th>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<td style='text-align:left;' colspan='2'><b>From:</b> " + Convert.ToDateTime(objView.from_date).ToString("dd/MM/yyyy") + "</td>");
                str.Append("<td style='text-align:left;' colspan='2'><b>To:</b> " + Convert.ToDateTime(objView.to_date).ToString("dd/MM/yyyy") + "</td>");
                str.Append("</tr>");
                if (bankname != "")
                {
                    str.Append("<tr>");
                    str.Append("<th style='text-align:left;'  colspan='4'><b>Bank: </b>" + bankname + "</th>");
                    str.Append("</tr>");
                }

                if (objView.CheckNoFrom != null)
                {
                    str.Append("<tr>");
                    str.Append("<th style='text-align:left;'  colspan='2'><b>Cheque No From: </b>" + objView.CheckNoFrom + "</th>");
                    str.Append("<th style='text-align:left;'  colspan='2'><b>To: </b>" + objView.CheckNoTo + "</th>");
                    str.Append("</tr>");
                }
            }

            str.Append("<tr>");
            str.Append("<th>Date</th><th>Chq Number</th>");
            str.Append("<th>Suppliers</th><th style='text-align:right;'>Amt Payable</th><th style='text-align:right;'>Actual Amount Paid</th>");
            str.Append("</tr>");

            foreach (var items in report_items)
            {
                str.Append("<tr>");
                str.Append("<td>" + items.payment_date + "</td><td>" + items.cheque_number + "</td>");
                str.Append("<td>" + items.supplier_name + "</td><td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(items.PaymentAmount - items.rebate_amount) + "</td><td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(items.paid_amount) + "</td>");
                str.Append("</tr>");
            }

            str.Append("<tr>");
            str.Append("<td style='text-align:right;'  colspan='4'><b>$" + CommonFunction.ConvertAmountoDecimal(report_items.Sum(o => o.PaymentAmount) - report_items.Sum(o => o.rebate_amount)) + "</b></td>");
            str.Append("<td style='text-align:right;'  colspan='4'><b>$" + CommonFunction.ConvertAmountoDecimal(report_items.Sum(o => o.paid_amount)) + "</b></td>");
            str.Append("</tr>");

            str.Append("</table>");

            if (isGridData == true)
            {
                objView.GridData = str.ToString();
            }
            else
            {
                objView.GridData = "";
                GenerateReport("BankChequeDetails.xls", str.ToString());
            }
        }


        public ActionResult SupplierReport(ReportViewModel objView)
        {
            List<SelectListItem> supplierList = new List<SelectListItem>();
            string uid = User.Identity.GetUserId();
            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
            }
            objView.Uid = uid;

            objView.YearList = CommonFunction.YearList();
            objView.AlphabetList = CommonFunction.AlphabetList();

            supplierList = CommonFunction.UserSupplierList(uid);

            if (objView.Alphabet != "0" && objView.Alphabet != null)
            {
                if (objView.Alphabet == "1")
                {
                    supplierList = supplierList.Where(o => (o.Text.StartsWith("0") || o.Text.StartsWith("1") || o.Text.StartsWith("2")
                    || o.Text.StartsWith("3") || o.Text.StartsWith("4") || o.Text.StartsWith("5") || o.Text.StartsWith("6") || o.Text.StartsWith("7")
                    || o.Text.StartsWith("8") || o.Text.StartsWith("9"))).ToList();


                }
                else
                {
                    supplierList = supplierList.Where(o => (o.Text.StartsWith(objView.Alphabet))).ToList();
                }
                supplierList.Insert(0, new SelectListItem { Text = "Select Supplier", Value = "0" });
            }

            objView.SupplierList = supplierList;

            if (objView.ReportYear == 0)
            {
                objView.ReportYear = DateTime.Now.Year;
                objView.Alphabet = "0";
            }

            if (objView.SupplierId == 0 && objView.Alphabet == "0")
            {

            }
            else
            {
                ExportSupplierReport(objView, true);
            }

            return View(objView);
        }

        public void ExportSupplierReport(ReportViewModel objView, bool isGridData = false)
        {
            string uid = User.Identity.GetUserId();
            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
            }
            List<Database.SSP_SupplierReport_Result> report_items = new List<Database.SSP_SupplierReport_Result>();
            using (Database.PMSEntities objDB = new Database.PMSEntities())
            {
                report_items = objDB.SSP_SupplierReport(uid, objView.ReportYear, SessionManagement.SelectedBranchID, objView.SupplierId, objView.Alphabet).ToList();
            }

            StringBuilder str = new StringBuilder("");
            str.Append("<table width='100%' class='table table-striped table-condensed'>");
            string suppliername = "";
            if (isGridData == false)
            {
                //suppliername = CommonFunction.UserSupplierList("").Where(o => o.Value == objView.SupplierId.ToString()).Select(o => o.Text).SingleOrDefault();

                //str.Append("<tr>");
                //str.Append("<th style='text-align:left;' colspan='9'><b>Supplier Report</b></th>");
                //str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<th style='text-align:left;'  colspan='9'><b>Branch: </b>" + SessionManagement.SelectedBranchName + "</th>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<td style='text-align:left;' colspan='9'><b>Year:</b> " + objView.ReportYear + "</td>");
                str.Append("</tr>");

            }

            Int32 rowSpan = 1;
            Int32 flag = 1;
            decimal amtPaid = 0;
            decimal rebtAmt = 0;
            string projectName = "";
            string salesmenName = "";
            decimal totalInvAmt = 0;
            decimal totalGstAmt = 0;
            decimal totalInvAfterGst = 0;
            decimal totalRebate = 0;
            decimal totalPaid = 0;
            decimal paidamount = 0;
            decimal totalAmtPaid = 0;
            foreach (var supp in report_items.GroupBy(o => o.supplier_id))
            {
                suppliername = report_items.Where(o => o.supplier_id == supp.Key).Select(o => o.supplier_name).FirstOrDefault();

                str.Append("<tr>");
                str.Append("<td style='text-align:left; font-size:18px;' colspan='11'><b>Supplier: " + suppliername + "</b></td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th>Invoices No</th><th style='text-align:right;'>Invoice Amt</th><th style='text-align:right;'>GST Amt</th><th style='text-align:right;'>Invoice Amt After GST</th>");
                str.Append("<th style='text-align:right; color:red;'>Rebate Amt</th><th style='text-align:right; color:red;'>Amount Paid</th><th style='text-align:right; color:red;'>Actual Amount Paid</th>");
                str.Append("<th>Project</th><th>Salesperson</th>");
                str.Append("<th>Mode Of Payment</th><th style='text-align:left;'>Paid on</th>");
                str.Append("<th>Branch</th>");
                str.Append("</tr>");

                foreach (var repMonth in report_items.Where(o => o.supplier_id == supp.Key).GroupBy(o => o.payment_month))
                {
                    rowSpan = 1;
                    flag = 1;
                    str.Append("<tr>");
                    str.Append("<td colspan='11' style='text-align:center; color:red;'><b>" + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToInt32(repMonth.Key)) + " " + objView.ReportYear + "</b></td>");
                    str.Append("</tr>");

                    foreach (var items in report_items.Where(o => o.payment_month == repMonth.Key && o.supplier_id == supp.Key))
                    {
                        projectName = items.project_name;
                        salesmenName = items.salesmen_name;
                        if (rowSpan == 1)
                        {
                            var _items = report_items.Where(o => o.payment_month == repMonth.Key && o.payment_id == items.payment_id && o.payment_date == items.payment_date && o.supplier_id == supp.Key).ToList();
                            projectName = string.Join(",", _items.Select(o => o.project_name).Distinct().ToArray());
                            salesmenName = string.Join(",", _items.Select(o => o.salesmen_name).Distinct().ToArray());
                            amtPaid = Convert.ToDecimal(_items.Sum(o => o.payment_amount));
                            rebtAmt = Convert.ToDecimal(items.rebate_amount);
                            amtPaid = amtPaid - rebtAmt;
                            paidamount = Convert.ToDecimal(items.paid_amount);
                            
                            rowSpan = Convert.ToInt32(_items.Count());
                            flag = rowSpan;

                            totalRebate += Convert.ToDecimal(rebtAmt);
                            totalPaid += Convert.ToDecimal(amtPaid);

                            totalAmtPaid += Convert.ToDecimal(paidamount);
                        }

                        totalInvAmt += Convert.ToDecimal(items.invoice_amount);
                        totalGstAmt += Convert.ToDecimal(items.gst_amount);
                        totalInvAfterGst += Convert.ToDecimal(items.payment_amount);


                        str.Append("<tr>");
                        str.Append("<td>" + items.supplier_inv_number + "</td><td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(items.invoice_amount) + "</td>");
                        str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(items.gst_amount) + "</td>");
                        str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(items.payment_amount) + "</td>");
                        if (rowSpan > 1 && flag == rowSpan)
                        {
                            str.Append("<td style='text-align:right; vertical-align:middle; color:red;' rowspan=" + rowSpan + ">$" + CommonFunction.ConvertAmountoDecimal(rebtAmt) + "</td><td style='text-align:right; vertical-align:middle; color:red;' rowspan=" + rowSpan + ">$" + CommonFunction.ConvertAmountoDecimal(amtPaid) + "</td><td style='text-align:right; vertical-align:middle; color:red;' rowspan=" + rowSpan + ">$" + CommonFunction.ConvertAmountoDecimal(paidamount) + "</td>");
                        }
                        else if (flag == rowSpan)
                        {
                            str.Append("<td style='text-align:right; color:red;'>$" + CommonFunction.ConvertAmountoDecimal(rebtAmt) + "</td><td style='text-align:right; color:red;'>$" + CommonFunction.ConvertAmountoDecimal(amtPaid) + "</td><td style='text-align:right; color:red;'>$" + CommonFunction.ConvertAmountoDecimal(paidamount) + "</td>");
                        }

                        // str.Append("<td>" + items.project_name + "</td><td>" + items.supplier_name + "</td>");

                        if (rowSpan > 1 && flag == rowSpan)
                        {
                            str.Append("<td style='vertical-align:middle;' rowspan=" + rowSpan + ">" + projectName + "</td><td style='vertical-align:middle;' rowspan=" + rowSpan + ">" + salesmenName + "</td><td style='vertical-align:middle;' rowspan=" + rowSpan + ">" + items.mode_of_payment + " " + items.bank + " " + items.cheque_number + "</td><td style='vertical-align:middle;' rowspan=" + rowSpan + ">" + items.payment_date + "</td><td style='vertical-align:middle;' rowspan=" + rowSpan + ">" + items.branch_name + "</td>");
                        }
                        else if (flag == rowSpan)
                        {
                            str.Append("<td>" + projectName + "</td><td>" + salesmenName + "</td><td>" + items.mode_of_payment + " " + items.bank + " " + items.cheque_number + "</td><td>" + items.payment_date + "</td><td>" + items.branch_name + "</td>");
                        }

                        str.Append("</tr>");

                        flag = flag - 1;
                        if (flag == 0)
                        {
                            rowSpan = 1;
                            flag = 1;
                        }
                    }
                }

            }

            if (report_items != null && report_items.Count > 0)
            {
                str.Append("<tr><td>Total</td>");
                str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(totalInvAmt) + "</td>");
                str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(totalGstAmt) + "</td>");
                str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(totalInvAfterGst) + "</td>");
                str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(totalRebate) + "</td>");
                str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(totalPaid) + "</td>");
                str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(totalAmtPaid) + "</td>");
                str.Append("<td></td><td></td><td></td><td></td><td></td></tr>");
            }

            str.Append("</table>");

            if (isGridData == true)
            {
                objView.GridData = str.ToString();
            }
            else
            {
                objView.GridData = "";
                GenerateReport("Supplier_Report" + suppliername.Replace(" ", "_") + ".xls", str.ToString());
            }
        }


        public ActionResult LoanReport(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            List<SelectListItem> branchList = new List<SelectListItem>();
            objView.Uid = uid;

            objView.SalesmenAndUserList = CommonFunction.GetSalesmenAndUser();
            objView.SalesmenStatusList = CommonFunction.SalesmenStatusList();
            DateTime now = DateTime.Now;
            //var startDate = new DateTime(now.Year, 1, 1);
            //var endDate = new DateTime(now.Year, now.Month + 1, 1).AddDays(-1);

            //var startDate = new DateTime(now.Year, (now.Month - 5), 1);
            var CurrentstartDate = new DateTime(now.Year, now.Month, 1);
            var endDate = CurrentstartDate.AddMonths(1).AddDays(-1);
            if (objView.from_date == null)
            {
                objView.from_date = CurrentstartDate.AddMonths(-5);
            }
            if (objView.to_date == null)
            {
                objView.to_date = endDate;
            }

            if (objView.SalesmenOrUserId != "" && objView.SalesmenOrUserId != null)
            {
                ExportLoanReport(objView, true);
            }
            return View(objView);
        }

        public void ExportLoanReport(ReportViewModel objView, bool isGridData = false)
        {
            Int32 empID = 0;
            //Int32 reType = 2;
            string[] arr = objView.SalesmenOrUserId.ToString().Split('_');
            //if (arr[0].ToString() == "U")
            //{
            //    reType = 1;
            //}
            empID = Convert.ToInt32(arr[1]);
            List<Database.SSP_LoanReport_Result> reportData = new List<Database.SSP_LoanReport_Result>();

            using (Database.PMSEntities objDB = new Database.PMSEntities())
            {
                reportData = objDB.SSP_LoanReport(empID, CommonFunction.GetUserType(arr[0]), objView.from_date, objView.to_date).ToList();
            }

            string Name = "";

            StringBuilder str = new StringBuilder("");

            str.Append("<table width='100%' border='0' class='table table-striped'>");

            if (isGridData == false)
            {
                if (objView.SalesmenOrUserId != "0")
                {
                    Name = CommonFunction.GetSalesmenAndUser().Where(o => o.Value == objView.SalesmenOrUserId.ToString()).Select(o => o.Text).SingleOrDefault();
                }

                str.Append("<tr>");
                str.Append("<th style='text-align:left;' colspan='6'><b>Employee Loan Report</b></th>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th  style='text-align:left;' colspan='6'><b>Branch: </b> " + SessionManagement.SelectedBranchName + "</th>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<td style='text-align:left;' colspan='2'><b>From:</b> " + Convert.ToDateTime(objView.from_date).ToString("dd/MM/yyyy") + "</td>");
                str.Append("<td style='text-align:left;' colspan='4'><b>To:</b> " + Convert.ToDateTime(objView.to_date).ToString("dd/MM/yyyy") + "</td>");
                str.Append("</tr>");

                if (Name != "")
                {
                    str.Append("<tr>");
                    str.Append("<td colspan='6' style='text-align:left;'><b>Employee: </b> " + Name + "</td>");
                    str.Append("</tr>");
                }
            }

            str.Append("<tr>");
            str.Append("<th>Date</th><th>Purpose</th>");
            str.Append("<th style='text-align:right;'>Loan Return</th>");
            str.Append("<th style='text-align:right;'>Loan/Payouts</th>");
            str.Append("<th>Mode of Payment</th><th style='text-align:right;'>Total Balance</th>");
            str.Append("</tr>");

            //foreach(var items in reportData)
            // bool chk = true;
            for (int i = 0; i < reportData.Count; i++)
            {
                str.Append("<tr>");
                str.Append("<td>" + reportData[i].LoanDate.ToString("dd/MM/yyyy") + "</td><td>");
                ///Project details
                str.Append("<ul style='padding-left: 0px; font-weight: bold;'>");
                str.Append("<li>" + reportData[i].purpose + "</li>");
                if (!string.IsNullOrEmpty(reportData[i].ProjectsDetail))
                {
                    str.Append("<li>" + reportData[i].ProjectsDetail.TrimEnd(';').Replace(";", "</li><li>"));
                    str.Append("</li>");
                }
                str.Append("</ul></td>");

                str.Append("<td style='text-align:right;'>");
                if (reportData[i].rec_type == Convert.ToInt32(CommonHelper.RecordType.LoanReturn))
                {
                    str.Append("$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData[i].amount)));////CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData[i].purpose))
                }
                else
                {
                    str.Append("-");
                }


                str.Append("</td>");
                str.Append("<td style='text-align:right;'>");
                //if (reportData[i].rec_type == 1)
                if (reportData[i].rec_type != Convert.ToInt32(CommonHelper.RecordType.LoanReturn))
                {
                    str.Append("$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData[i].amount)));
                }
                else
                {
                    str.Append("-");
                }
                str.Append("</td>");
                str.Append("<td>" + reportData[i].mode_of_payment + "");

                if (reportData[i].bank_name != "")
                {
                    str.Append(" " + reportData[i].bank_name);
                }
                if (reportData[i].cheque_number != "")
                {
                    str.Append(" " + reportData[i].cheque_number);
                }
                str.Append("</td>");
                str.Append("<td style='text-align:right;'>");
                str.Append("$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData[i].TotalBalance)) + "");
                //if (chk)
                //{
                //    if (reportData[i].TotalBalance == reportData[i].amount && reportData[i].rec_type == 1)
                //    {
                //        str.Append("-");
                //    }
                //    else
                //    {
                //        str.Append("$" + reportData[i].TotalBalance + "");
                //    }
                //}
                //else
                //{
                //    str.Append("$" + reportData[i - 1].TotalBalance + "");
                //}

                str.Append("</td>");

                str.Append("</tr>");
                // chk = false;
            }

            str.Append("</table>");
            if (isGridData == true)
            {
                objView.GridData = str.ToString();
            }
            else
            {
                objView.GridData = "";
                GenerateReport("LoanReport_" + Convert.ToDateTime(objView.from_date).Year.ToString() + ".xls", str.ToString());
            }

        }

        public void GenerateReport(string reportName, string reportData)
        {
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment; filename=" + reportName);
            Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
            Response.Write("<head>");
            Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            Response.Write("<!--[if gte mso 9]><xml>");
            Response.Write("<x:ExcelWorkbook>");
            Response.Write("<x:ExcelWorksheets>");
            Response.Write("<x:ExcelWorksheet>");
            Response.Write("<x:Name>Report Data</x:Name>");
            Response.Write("<x:WorksheetOptions>");
            Response.Write("<x:Print>");
            Response.Write("<x:ValidPrinterInfo/>");
            Response.Write("</x:Print>");
            Response.Write("</x:WorksheetOptions>");
            Response.Write("</x:ExcelWorksheet>");
            Response.Write("</x:ExcelWorksheets>");
            Response.Write("</x:ExcelWorkbook>");
            Response.Write("</xml>");
            Response.Write("<![endif]--> ");
            Response.Write(reportData);
            Response.Write("</head>");
            Response.Flush();
        }


        public ActionResult ExpenseReport(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            List<SelectListItem> branchList = new List<SelectListItem>();
            objView.Uid = uid;
            objView.BranchList = CommonFunction.BranchList();
            objView.YearList = CommonFunction.YearList();

            if (objView.ReportYear == 0)
            {
                objView.ReportYear = DateTime.Now.Year;
            }
            if (objView.BranchId > 0)
            {
                ExportExpenseReport(objView, true);
            }
            return View(objView);
        }

        public void ExportExpenseReport(ReportViewModel objView, bool isGridData = false)
        {
            List<Database.SSP_ExpenseReport_Result> reportData = new List<Database.SSP_ExpenseReport_Result>();

            using (Database.PMSEntities objDB = new Database.PMSEntities())
            {
                reportData = objDB.SSP_ExpenseReport(objView.ReportYear, objView.BranchId).ToList();
            }

            StringBuilder str = new StringBuilder("");

            str.Append("<table width='100%' border='0' class='table table-striped'>");

            if (isGridData == false)
            {
                str.Append("<tr>");
                str.Append("<th style='text-align:left;' colspan='14'><b>Expense Report</b></th>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th  style='text-align:left;' colspan='14'><b>Company: </b> " + SessionManagement.SelectedBranchName + "</th>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<td style='text-align:left;' colspan='14'><b>Year:</b> " + objView.ReportYear.ToString() + "</td>");
                str.Append("</tr>");
            }



            foreach (var _rec in reportData.GroupBy(o => o.categoryId))
            {
                str.Append("<tr>");
                str.Append("<th>" + reportData.Where(o => o.categoryId == _rec.Key).Select(o => o.Category).FirstOrDefault() + "</th><th style='text-align:right;'>Jan</th><th style='text-align:right;'>Feb</th><th style='text-align:right;'>Mar</th><th style='text-align:right;'>April</th><th style='text-align:right;'>May</th><th style='text-align:right;'>June</th><th style='text-align:right;'>July</th><th style='text-align:right;'>Aug</th><th style='text-align:right;'>Sep</th><th style='text-align:right;'>Oct</th><th style='text-align:right;'>Nov</th><th style='text-align:right;'>Dec</th><th style='text-align:right;'>" + objView.ReportYear.ToString() + "</th>");
                str.Append("</tr>");
                double grandTotal = 0;
                double total = 0;

                foreach (var items in reportData.Where(o => o.categoryId == _rec.Key).ToList())
                {
                    total = Convert.ToDouble(items.jan) + Convert.ToDouble(items.feb) + Convert.ToDouble(items.mar) + Convert.ToDouble(items.apr) + Convert.ToDouble(items.may) + Convert.ToDouble(items.jun) + Convert.ToDouble(items.jul) + Convert.ToDouble(items.aug) + Convert.ToDouble(items.sep) + Convert.ToDouble(items.oct) + Convert.ToDouble(items.nov) + Convert.ToDouble(items.dec);
                    grandTotal = grandTotal + total;

                    str.Append("<tr>");
                    str.Append("<td>" + items.CategoryItem + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.jan)) + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.feb)) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.mar)) + "</td>");
                    str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.apr)) + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.may)) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.jun)) + "</td>");
                    str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.jul)) + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.aug)) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.sep)) + "</td>");
                    str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.oct)) + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.nov)) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.dec)) + "</td>");
                    str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(total)) + "</td>");
                    str.Append("</tr>");
                }

                str.Append("<tr>");
                str.Append("<td>Subtotal</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(reportData.Where(o => o.categoryId == _rec.Key).Sum(o => o.jan)) + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(reportData.Where(o => o.categoryId == _rec.Key).Sum(o => o.feb)) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(reportData.Where(o => o.categoryId == _rec.Key).Sum(o => o.mar)) + "</td>");
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(reportData.Where(o => o.categoryId == _rec.Key).Sum(o => o.apr)) + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(reportData.Where(o => o.categoryId == _rec.Key).Sum(o => o.may)) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(reportData.Where(o => o.categoryId == _rec.Key).Sum(o => o.jun)) + "</td>");
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(reportData.Where(o => o.categoryId == _rec.Key).Sum(o => o.jul)) + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(reportData.Where(o => o.categoryId == _rec.Key).Sum(o => o.aug)) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(reportData.Where(o => o.categoryId == _rec.Key).Sum(o => o.sep)) + "</td>");
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(reportData.Where(o => o.categoryId == _rec.Key).Sum(o => o.oct)) + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(reportData.Where(o => o.categoryId == _rec.Key).Sum(o => o.nov)) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(reportData.Where(o => o.categoryId == _rec.Key).Sum(o => o.dec)) + "</td>");
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(grandTotal)) + "</td>");
                str.Append("</tr>");
            }

            //str.Append("<tr>");
            //str.Append("<th>Date</th><th>Purpose</th>");
            //str.Append("<th style='text-align:right;'>Closed File</th>");
            //str.Append("<th style='text-align:right;'>Loan/Payouts</th>");
            //str.Append("<th>Mode of Payment</th><th style='text-align:right;'>Total Balance</th>");
            //str.Append("</tr>");

            str.Append("</table>");
            if (isGridData == true)
            {
                objView.GridData = str.ToString();
            }
            else
            {
                objView.GridData = "";
                GenerateReport("ExpenseReport_" + objView.ReportYear.ToString() + "_" + objView.BranchId.ToString() + ".xls", str.ToString());
            }

        }


        #region Customer Invoice Report
        //public ActionResult CustomerInvoiceReport(ReportViewModel objView)
        //{
        //    string uid = User.Identity.GetUserId();
        //    objView.Uid = uid;
        //    //objView.SalesmenAndUserList = CommonFunction.GetSalesmenAndUser();

        //    DateTime now = DateTime.Now;
        //    //var startDate = new DateTime(now.Year, 1, 1);
        //    //var endDate = new DateTime(now.Year, now.Month + 1, 1).AddDays(-1);

        //    //var startDate = new DateTime(now.Year, (now.Month - 5), 1);
        //    var CurrentstartDate = new DateTime(now.Year, now.Month, 1);
        //    var endDate = CurrentstartDate.AddMonths(1).AddDays(-1);
        //    if (objView.from_date == null)
        //    {
        //        objView.from_date = CurrentstartDate.AddMonths(-5);
        //    }
        //    if (objView.to_date == null)
        //    {
        //        objView.to_date = endDate;
        //    }
        //    ExportCustomerInvoiceReport(objView, true);
        //    return View(objView);
        //}

        //public void ExportCustomerInvoiceReport(ReportViewModel objView, bool isGridData = false)
        //{
        //    //Int32 empID = 0;
        //    //Int32 reType = 2;
        //    //string[] arr = objView.SalesmenOrUserId.ToString().Split('_');
        //    //if (arr[0].ToString() == "U")
        //    //{
        //    //    reType = 1;
        //    //}
        //    //empID = Convert.ToInt32(arr[1]);
        //    List<Database.SSP_CustomerInvoiceReport_Result> reportData = new List<Database.SSP_CustomerInvoiceReport_Result>();

        //    using (Database.PMSEntities objDB = new Database.PMSEntities())
        //    {
        //        //reportData = objDB.SSP_CustomerInvoiceReport(SessionManagement.SelectedBranchID, objView.from_date, objView.to_date).ToList();
        //        reportData = objDB.SSP_CustomerInvoiceReport(objView.from_date, objView.to_date).ToList();
        //    }

        //    string Name = "";

        //    StringBuilder str = new StringBuilder("");

        //    str.Append("<table width='100%' border='0' class='table table-striped'>");

        //    if (isGridData == false)
        //    {
        //        //if (objView.SalesmenOrUserId != "0")
        //        //{
        //        //    Name = CommonFunction.GetSalesmenAndUser().Where(o => o.Value == objView.SalesmenOrUserId.ToString()).Select(o => o.Text).SingleOrDefault();
        //        //}

        //        str.Append("<tr>");
        //        str.Append("<th style='text-align:left;' colspan='6'><b>Customer Invoice Report</b></th>");
        //        str.Append("</tr>");

        //        str.Append("<tr>");
        //        str.Append("<th  style='text-align:left;' colspan='6'><b>Branch: </b> " + SessionManagement.SelectedBranchName + "</th>");
        //        str.Append("</tr>");

        //        str.Append("<tr>");
        //        str.Append("<td style='text-align:left;' colspan='2'><b>From:</b> " + Convert.ToDateTime(objView.from_date).ToString("dd/MM/yyyy") + "</td>");
        //        str.Append("<td style='text-align:left;' colspan='4'><b>To:</b> " + Convert.ToDateTime(objView.to_date).ToString("dd/MM/yyyy") + "</td>");
        //        str.Append("</tr>");

        //        //if (Name != "")
        //        //{
        //        //    str.Append("<tr>");
        //        //    str.Append("<td colspan='6' style='text-align:left;'><b>Employee: </b> " + Name + "</td>");
        //        //    str.Append("</tr>");
        //        //}
        //    }

        //    str.Append("<tr>");
        //    str.Append("<th>Date</th>");
        //    str.Append("<th>Invoice Number</th>");
        //    str.Append("<th>Bill To</th>");
        //    //str.Append("<th>Address</th>");
        //    str.Append("<th>Company</th>");
        //    str.Append("<th>Salesmen</th>");
        //    str.Append("<th>Total</th>");
        //    str.Append("</tr>");

        //    //foreach(var items in reportData)
        //    // bool chk = true;
        //    for (int i = 0; i < reportData.Count; i++)
        //    {
        //        str.Append("<tr>");
        //        str.Append("<td>" + reportData[i].Invoice_date + "</td>"); //.ToString("dd/MM/yyyy")
        //        str.Append("<td>" + reportData[i].Invoice_number + "</td>");

        //        str.Append("<td>" + reportData[i].customer_name + "</td>");
        //        //str.Append("<td>" + reportData[i].address1 + "</td>");
        //        str.Append("<td>" + reportData[i].company_name + "</td>");
        //        str.Append("<td>" + reportData[i].salesmen_name + "</td>");
        //        // Items details
        //        #region
        //        decimal TotalAmount = 0;
        //        var list = reportData[i].Total.Split(',').ToList();
        //        decimal itemTotal = list.Count() > 0 ? list.Sum(o => Convert.ToDecimal(o)) : 0;
        //        decimal _tax = 0;
        //        if (itemTotal > 0)
        //        {
        //            if (Convert.ToBoolean(reportData[i].is_tax))
        //            {
        //                _tax = Math.Round(Convert.ToDecimal(itemTotal * reportData[i].tax) / 100, 2); ;
        //            }
        //        }
        //        TotalAmount = Math.Round((itemTotal + _tax), 2);
        //        str.Append("<td>$" + TotalAmount + "</td>");
        //        //str.Append("<td>");
        //        //str.Append("<ul style='padding-left: 0px;'>");

        //        //if (!string.IsNullOrEmpty(reportData[i].Items))
        //        //{
        //        //    str.Append("<li>" + reportData[i].Items.TrimEnd(',').Replace(",", "</li><li>"));
        //        //    str.Append("</li>");
        //        //}
        //        //str.Append("</ul></td>");
        //        #endregion
        //        str.Append("</tr>");
        //        // chk = false;
        //    }

        //    str.Append("</table>");
        //    if (isGridData == true)
        //    {
        //        objView.GridData = str.ToString();
        //    }
        //    else
        //    {
        //        objView.GridData = "";
        //        GenerateReport("CustomeInvoiceReport_" + Convert.ToDateTime(objView.from_date).Year.ToString() + ".xls", str.ToString());
        //    }
        //}
        #endregion

        #region Designer Invoice Report
        public ActionResult DesignerInvoiceReport(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            List<SelectListItem> branchList = new List<SelectListItem>();
            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
                objView.ProjectList = CommonFunction.UserProjectList("00000000-0000-0000-0000-000000000000");
            }
            else
            {
                objView.ProjectList = CommonFunction.UserProjectList(uid);
            }
            objView.Uid = uid;
            DateTime now = DateTime.Now;
            var CurrentstartDate = new DateTime(now.Year, now.Month, 1);
            var endDate = CurrentstartDate.AddMonths(1).AddDays(-1);

            if (objView.from_date == null)
            {
                objView.from_date = CurrentstartDate.AddMonths(-5);
            }
            else
            {
                objView.from_date = objView.from_date;
            }
            if (objView.to_date == null)
            {
                objView.to_date = endDate;
            }
            else
            {
                objView.to_date = objView.to_date;
            }
            objView.BankList = CommonFunction.BankList();
            objView.DesignerList = CommonFunction.DesignerList();
            ExportDesignerInvoiceReport(objView, true);
            return View(objView);
        }


        public void ExportDesignerInvoiceReport(ReportViewModel objView, bool isGridData = false)
        {
            string uid = User.Identity.GetUserId();
            if (User.IsInRole("SuperAdmin"))
            {
                uid = "";
            }
            if (string.IsNullOrWhiteSpace(objView.DesignerName))
            {
                objView.DesignerName = "";
            }
            List<Database.SSP_DesignerInvoiceReport_Result> reportData = new List<Database.SSP_DesignerInvoiceReport_Result>();
            List<Database.SSP_DesignerInvoiceReport_Result> new_reportData = new List<Database.SSP_DesignerInvoiceReport_Result>();


            List<Database.SSP_DesignerInvoiceWithAndWithoutPayment_Result> allInvoiceList = new List<Database.SSP_DesignerInvoiceWithAndWithoutPayment_Result>();

            using (Database.PMSEntities objDB = new Database.PMSEntities())
            {
                reportData = objDB.SSP_DesignerInvoiceReport(uid, Common.SessionManagement.SelectedBranchID, objView.from_date, objView.to_date, objView.BankId, objView.DesignerName).ToList();
                allInvoiceList = objDB.SSP_DesignerInvoiceWithAndWithoutPayment(uid, Common.SessionManagement.SelectedBranchID, objView.from_date, objView.to_date, objView.BankId, objView.DesignerName).ToList();
            }

            string projectName = "";
            string designername = "";
            string bankname = "";

            reportData.AddRange(allInvoiceList.Where(x => !reportData.Select(z => z.invoiceId).Contains(x.invoiceId)).Select(x => new Database.SSP_DesignerInvoiceReport_Result
            {
                amount = 0,
                cheque_details = "",
                invoice_amount_items = x.invoice_amount_items,
                invoice_numbers = x.invoice_numbers,
                is_tax = x.is_tax,
                mode_of_payment_id = 0,
                mode_of_payment_name = "",
                receipt_date = x.Invoice_date,
                Designer_name = x.Designer_name,
                tax = x.tax,
                tax_amount = x.tax_amount,
                invoiceId = x.invoiceId
            }).ToList());

            reportData.OrderByDescending(x => x.receipt_date);

            new_reportData = reportData;

            reportData = reportData.GroupBy(d => d.invoiceId)
    .Select(
        g => new Database.SSP_DesignerInvoiceReport_Result
        {
            amount = g.Sum(s => s.amount),
            Designer_name = g.FirstOrDefault().Designer_name,
            cheque_details = g.FirstOrDefault().cheque_details,
            invoiceId = g.FirstOrDefault().invoiceId,
            receipt_date = g.FirstOrDefault().receipt_date,
            invoice_amount_items = g.FirstOrDefault().invoice_amount_items,
            invoice_numbers = g.FirstOrDefault().invoice_numbers,
            is_tax = g.FirstOrDefault().is_tax,
            mode_of_payment_id = g.FirstOrDefault().mode_of_payment_id,
            mode_of_payment_name = g.FirstOrDefault().mode_of_payment_name,
            ReceiptDate = g.FirstOrDefault().ReceiptDate,
            tax = g.FirstOrDefault().tax,
            tax_amount = g.FirstOrDefault().tax_amount
        }).ToList();

            StringBuilder str = new StringBuilder("");

            str.Append("<table width='100%' border='0' class='table table-striped dataTable' id='tableid' >");

            if (isGridData == false)
            {
                if (objView.ProjectId > 0)
                {
                    projectName = CommonFunction.UserProjectList(Convert.ToString("00000000-0000-0000-0000-000000000000")).Where(o => o.Value == objView.ProjectId.ToString()).Select(o => o.Text).SingleOrDefault();
                }
                if (objView.BankId > 0)
                {
                    bankname = CommonFunction.BankList().Where(o => o.Value == objView.BankId.ToString()).Select(o => o.Text).SingleOrDefault();
                }
                str.Append("<tr>");
                str.Append("<th style='text-align:left;' colspan='8'><b>Designer Payment Report</b></th>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<th  style='text-align:left;' colspan='8'><b>Branch: </b> " + SessionManagement.SelectedBranchName + "</th>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<td style='text-align:left;' colspan='2'><b>From:</b> " + Convert.ToDateTime(objView.from_date).ToString("dd/MM/yyyy") + "</td>");
                str.Append("<td style='text-align:left;' colspan='6'><b>To:</b> " + Convert.ToDateTime(objView.to_date).ToString("dd/MM/yyyy") + "</td>");
                str.Append("</tr>");

                if (Convert.ToString(designername) != "" || Convert.ToString(bankname) != "")
                {
                    str.Append("<tr>");
                }

                if (Convert.ToString(bankname) != "")
                {
                    str.Append("<td colspan='2' style='text-align:left;'><b>Bank: </b> " + bankname + "</td>");
                }
                if (Convert.ToString(designername) != "")
                {
                    str.Append("<td colspan='2' style='text-align:left;'><b>Designer: </b> " + designername + "</td>");
                }


                if (Convert.ToString(designername) != "" || Convert.ToString(bankname) != "")
                {
                    str.Append("<td></td>");
                    str.Append("</tr>");
                    str.Append("<tr><td colspan='8'></td></tr>");
                }
            }

            str.Append("<thead>");
            str.Append("<tr>");
            str.Append("<th>Date</th><th>Payment Mode</th><th>Invoice Numbers</th><th style='text-align:right;'>Invoice Amount</th><th style='text-align:right;'>Payment Amount</th><th style='text-align:right;'>Balance</th><th>Designer</th>");
            str.Append("</tr>");
            str.Append("</thead>");

            str.Append("<tbody>");
            foreach (var items in reportData)
            {
                decimal invoiceitemsAmountSum = Convert.ToDecimal(CommonHelper.CalcualteInvoiceAmount(items.invoice_amount_items, items.tax, items.is_tax, items.tax_amount, true, false));
                str.Append("<tr>");
                str.Append("<td>" + items.receipt_date + "</td>");
                if (items.mode_of_payment_id == 4) // in case  of cheque
                {
                    str.Append("<td>" + items.mode_of_payment_name + " / " + items.cheque_details + "</td>");
                }
                else
                {
                    str.Append("<td>" + items.mode_of_payment_name + "</td>");
                }

                str.Append("<td>" + items.invoice_numbers + "</td>");
                str.Append("<td align='right'>$" + invoiceitemsAmountSum + "</td>");
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.amount)) + "</td>");
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(invoiceitemsAmountSum - items.amount)) + "</td>");
                str.Append("<td>" + items.Designer_name + "</td>");
                str.Append("</tr>");
            }
            str.Append("</tbody>");

            str.Append("<tr>");
            str.Append("<td colspan='4'><strong>Cash</strong></td><td  align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(new_reportData.Where(o => o.mode_of_payment_id == 2).Select(o => o.amount).Sum())) + "</strong></td><td></td><td></td>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<td colspan='4'><strong>Cheque</strong></td><td  align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(new_reportData.Where(o => o.mode_of_payment_id == 4).Select(o => o.amount).Sum())) + "</strong></td><td></td><td></td>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<td colspan='4'><strong>Bank TFR</strong></td><td  align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(new_reportData.Where(o => o.mode_of_payment_id == 3).Select(o => o.amount).Sum())) + "</strong></td><td></td><td></td>");
            str.Append("</tr>");

            double nets = Convert.ToDouble(new_reportData.Where(o => o.mode_of_payment_id == 1).Select(o => o.amount).Sum());
            double gst = (nets * 0.07);

            str.Append("<tr>");
            str.Append("<td colspan='4'><strong>NETS</strong></td><td align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(nets)) + "</strong></td><td><strong>GST</strong></td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(gst)) + "</td>");
            str.Append("</tr>");

            str.Append("<tr>");
            str.Append("<td colspan='4'><strong>Total</strong></td><td align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.amount).Sum())) + "</strong></td><td></td><td></td>");
            str.Append("</tr>");
            str.Append("</table>");
            if (isGridData == true)
            {
                objView.GridData = str.ToString();
            }
            else
            {
                objView.GridData = "";
                GenerateReport("ReceiptReport_" + objView.ReportMonth.ToString() + "_" + objView.ReportYear.ToString() + ".xls", str.ToString());

            }

        }
        #endregion


        public JsonResult GetSalesmenByStatus(bool status)
        {
            try
            {
                List<SelectListItem> salesmenList = new List<SelectListItem>();
                if (status)
                    salesmenList = Common.CommonFunction.Active_Salesmen("");
                else
                    salesmenList = Common.CommonFunction.InActive_Salesmen("");

                return Json(salesmenList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GetSalesmen(string salesmenStatus)
        {
            //  var ddlModeldatasurce=  CommonFunction.SalesmenListStatusWise(id);
            ReportViewModel objView = new ReportViewModel();
            //cc
            objView.SalesmenAndUserList = CommonFunction.SalesmenListStatusWise(salesmenStatus);

            return Json(objView);
        }

        public ActionResult GetSalesmenforLoanReport(string salesmenStatus)
        {
            //  var ddlModeldatasurce=  CommonFunction.SalesmenListStatusWise(id);
            ReportViewModel objView = new ReportViewModel();
            //cc
            objView.SalesmenAndUserList = CommonFunction.GetSalesmenAndUser(salesmenStatus);

            return Json(objView);
        }
    }
}