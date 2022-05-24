using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PMS.Models;
using PMS.Repository.DataService;
using PMS.Repository.Infrastructure;
using PMS.Database;
using PMS.Common;
using System.Text.RegularExpressions;

namespace PMS.Controllers
{
    [Authorize]
    [BranchFilter]
    public class ReceiptsController : Controller
    {
        private readonly IReceiptsRepositor _ReceiptsRepo;
        private readonly IReceiptDetailRepositor _ReceiptDetailRepo;
        private readonly IProject _ProjectRepo;
        private readonly IDesignerReceiptsRepository _DesReceiptsRepo;
        private readonly IDesignerReceiptDetailRepository _DesReceiptDetailRepo;
        public ReceiptsController(IReceiptsRepositor RecRepo, IProject projRepo, IReceiptDetailRepositor recdetRepo, IDesignerReceiptsRepository DesReceiptsRepo,
            IDesignerReceiptDetailRepository DesReceiptDetailRepo)
        {
            _ProjectRepo = projRepo;
            _ReceiptsRepo = RecRepo;
            _ReceiptDetailRepo = recdetRepo;
            _DesReceiptsRepo = DesReceiptsRepo;
            _DesReceiptDetailRepo = DesReceiptDetailRepo;
        }
        public ActionResult Index(ReceiptsViewModel objView)
        {
            if (User.IsInRole("SuperAdmin"))
            {
                objView.UID = "00000000-0000-0000-0000-000000000000";
            }
            else
            {
                objView.UID = User.Identity.GetUserId();
            }
            ViewBag.IsAdminLogin = 1;
            if (User.IsInRole("Salesman"))
            {
                objView.ProjectSalesmenId = Common.CommonFunction.GetSalesmanIdByUser(objView.UID);
                ViewBag.IsAdminLogin = 0;
            }

            DateTime now = DateTime.Now;
            //var startDate = new DateTime(now.Year, (now.Month - 5), 1);
            var CurrentstartDate = new DateTime(now.Year, now.Month, 1);
            var endDate = CurrentstartDate.AddMonths(1).AddDays(-1);
            if (objView.SearchFrom == null)
            {
                objView.SearchFrom = CurrentstartDate.AddMonths(-5);
            }
            if (objView.SearchTo == null)
            {
                objView.SearchTo = endDate;
            }
            if ((string.IsNullOrWhiteSpace(objView.SearchText)))
            {
                objView.SearchText = "";
            }
            objView.projectList = CommonFunction.UserProjectList(objView.UID);
            objView.salesmanList = CommonFunction.Active_Salesmen("");
            objView.isProjectClosed = false;
            return View(objView);
        }

        public ActionResult DeleteById(Int32 Id)
        {
            if (Id > 0)
            {
                receipt objRec = _ReceiptsRepo.FindBy(o => o.id == Id).SingleOrDefault();
                _ReceiptsRepo.Delete(objRec);
                using (PMSEntities obj = new PMSEntities())
                {
                    obj.SSP_Remove_Receipt_Detail(Convert.ToInt32(Id));
                }
                _ReceiptsRepo.Save();
            }
            return RedirectToAction("Index");
        }

        public ActionResult LoadAddEdit(Int32 Id)
        {
            string uid = User.Identity.GetUserId();
            Int32 branchid = SessionManagement.SelectedBranchID;
            ReceiptsViewModel objView = new ReceiptsViewModel();
            Int32 projectId = 0;
            objView.customerList = new List<SelectListItem>();
            objView.salesmanList = CommonFunction.Active_Salesmen("");

            if (Id > 0)
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    receipt objRec = _ReceiptsRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    if (objRec != null)
                    {
                        // projectId = Convert.ToInt32(objRec.project_id);
                        CommonFunction.MergeObjects(objView, objRec, true);
                    }
                }
                else
                {
                    //receipt objRec = _ReceiptsRepo.FindBy(o => o.id == Id && o.created_by == new Guid(uid)).SingleOrDefault();
                    receipt objRec = _ReceiptsRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    if (objRec != null)
                    {
                        //projectId = Convert.ToInt32(objRec.project_id);
                        CommonFunction.MergeObjects(objView, objRec, true);
                    }
                }

                objView.customerList = CommonFunction.SalesmenCustomerList(Convert.ToInt32(objView.sales_man_id));
                objView.projectList = CustomerProjects(Convert.ToInt32(objView.customer_id));
                objView.invoiceList = ProjectInvoices(Convert.ToInt32(objView.customer_id));

                objView.invoiceListWithProjects = ProjectInvoicesWithProject(Convert.ToInt32(objView.customer_id));

                List<receipt_details> objRDList = new List<receipt_details>();
                List<receipt_detail> objRDRec = _ReceiptDetailRepo.FindBy(o => o.receipt_id == Id).ToList();

                foreach (var items in objRDRec)
                {
                    objRDList.Add(new receipt_details
                    {
                        project_id = Convert.ToInt64(items.project_id),
                        gst_amount = Convert.ToDecimal(items.gst_amount),
                        gst_percentage = Convert.ToDecimal(items.gst_percentage),
                        amount = Convert.ToDecimal(items.amount),
                        total_amount = Convert.ToDecimal(items.total_amount),
                        invoice_id = Convert.ToInt32(items.invoice_id)
                    });
                }
                objView.receipt_details = objRDList;
            }
            else
            {
                //objView.amount = Convert.ToDecimal(0.00);
                // objView.gst_percentage = Convert.ToDecimal(Common.SessionManagement.BranchGST);
                // objView.gst_amount = Convert.ToDecimal(0.00);               
                // objView.total_amount = Convert.ToDecimal(0.00);
                objView.receipt_date = DateTime.Now;
                objView.isactive = true;
                objView.isProjectClosed = false;
                objView.receipt_details = new List<receipt_details>();
                objView.projectList = new List<SelectListItem>();
                objView.invoiceList = new List<SelectListItem>();
                objView.bank_id = SessionManagement.SelectedBranchBankId;
            }
            if (User.IsInRole("SuperAdmin"))
            {
                objView.UID = "00000000-0000-0000-0000-000000000000";
            }
            else
            {
                objView.UID = User.Identity.GetUserId();
            }
            ViewBag.IsAdminLogin = 1;
            if (User.IsInRole("Salesman"))
            {
                objView.sales_man_id = Common.CommonFunction.GetSalesmanIdByUser(objView.UID);
                ViewBag.IsAdminLogin = 0;
            }

            objView.bankList = CommonFunction.BankList();
            objView.mode_of_paymentList = CommonFunction.ModeofPaymentList();

            if (User.IsInRole("SuperAdmin"))
            {
                objView.branchList = CommonFunction.BranchList();
                //objView.projectList = Common.CommonFunction.UserProjectListWithID("00000000-0000-0000-0000-000000000000", projectId);
            }
            else
            {
                objView.branchList = CommonFunction.UserBranchList(uid);
                //objView.projectList = Common.CommonFunction.UserProjectListWithID(uid, projectId);
            }

            objView.IsActiveList = CommonFunction.StatusList();

            return View(objView);
        }

        public List<SelectListItem> CustomerProjects(Int32 customer_id)
        {
            using (PMSEntities objDB = new PMSEntities())
            {
                var _list = (from p in objDB.projects
                             where p.customer_id == customer_id
                             select new SelectListItem
                             {
                                 Text = p.project_name + " - " + p.project_number,
                                 Value = p.id.ToString()
                             }).OrderBy(o => o.Text).ToList();
                return _list;
            }
        }

        public List<SelectListItem> ProjectInvoices(Int32 customer_id)
        {
            using (PMSEntities objDB = new PMSEntities())
            {
                var _list = (from p in objDB.tbl_invoice
                             where p.Bill_to == customer_id
                             select new SelectListItem
                             {
                                 Text = p.Invoice_number,
                                 Value = p.Id.ToString()
                             }).OrderBy(o => o.Text).ToList();
                return _list;
            }
        }

        public JsonResult GetProjectInvoices(Int32 customer_id)
        {
            try
            {
                List<SelectListItem> searchList = new List<SelectListItem>();
                searchList = ProjectInvoices(customer_id);
                return Json(searchList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                return null;
            }
        }

        public JsonResult SaveUpdate(ReceiptsViewModel objView)
        {
            if (!string.IsNullOrEmpty(Request.Params["receiptobj"]))
            {
                objView = ReceiptsViewModel.FromJson(Request.Params["receiptobj"]);
            }
            string uid = User.Identity.GetUserId();
            receipt objRec = new receipt();

            if (objView.id > 0)
            {
                //objRec = _ReceiptsRepo.FindBy(o => o.id == objView.id).SingleOrDefault();
                //if (objRec != null)
                //{
                //    objView.created_by = objRec.created_by;
                //    objView.created_date = objRec.created_date;
                //    Common.CommonFunction.MergeObjects(objRec, objView, true);
                //    objRec.modified_date = DateTime.Now;
                //    objRec.modified_by = new Guid(uid);
                //    _ReceiptsRepo.Save();
                //}

                objRec = _ReceiptsRepo.FindBy(o => o.id == objView.id).SingleOrDefault();
                if (objRec != null)
                {
                    objView.created_by = objRec.created_by;
                    objView.created_date = objRec.created_date;
                    CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.modified_date = DateTime.Now;
                    objRec.modified_by = new Guid(uid);
                    _ReceiptsRepo.Save();

                    using (PMSEntities obj = new PMSEntities())
                    {
                        obj.SSP_Remove_Receipt_Detail(Convert.ToInt32(objRec.id));

                        foreach (var item in objView.receipt_details)
                        {
                            obj.SSP_Add_Receipt_Detail(Convert.ToInt32(objRec.id), Convert.ToDecimal(item.amount), Convert.ToDecimal(item.gst_percentage), Convert.ToDecimal(item.gst_amount), Convert.ToDecimal(item.total_amount), Convert.ToInt32(item.project_id), Convert.ToInt32(item.invoice_id));
                        }
                    }
                }

                return Json(new { id = objView.id, msg = "Receipt updated successfully.", cls = "success" });
            }
            else
            {
                CommonFunction.MergeObjects(objRec, objView, true);
                objRec.created_date = DateTime.Now;
                objRec.created_by = new Guid(uid);
                objRec.modified_date = DateTime.Now;
                objRec.modified_by = new Guid(uid);
                _ReceiptsRepo.Add(objRec);
                _ReceiptsRepo.Save();

                using (PMSEntities objdb = new PMSEntities())
                {
                    foreach (var item in objView.receipt_details)
                    {
                        objdb.SSP_Add_Receipt_Detail(Convert.ToInt32(objRec.id), Convert.ToDecimal(item.amount), Convert.ToDecimal(item.gst_percentage), Convert.ToDecimal(item.gst_amount), Convert.ToDecimal(item.total_amount), Convert.ToInt32(item.project_id), Convert.ToInt32(item.invoice_id));
                    }
                }

                return Json(new { id = objRec.id, msg = "Receipt created successfully.", cls = "success" });
            }
        }

        public Int32 CheckReceipt(Int32 projectId)
        {
            Int32 count = 0;
            count = _ReceiptDetailRepo.FindBy(o => o.project_id == projectId).Count();
            //  count = _ReceiptsRepo.FindBy(o => o.project_id == projectId).Count();
            count = count + 1;
            return count;
        }

        public ActionResult PrintPreview(Int32 Id)
        {

            PrintReceiptViewModel objViewprint = new PrintReceiptViewModel();
            #region Old Backup
            //objViewprint.ssp_receiptsById_result = _ReceiptsRepo.ReceiptById(Convert.ToInt32(Id));
            //using (PMSEntities objDB = new PMSEntities())
            //{

            //    objViewprint.ssp_receiptDetailByIReceiptid_result = objDB.SSP_ReceiptDetailByReceiptId(Convert.ToInt64(Id)).ToList();
            //}
            #endregion

            using (PMSEntities objDB = new PMSEntities())
            {
                objViewprint.ssp_receiptsById_result = _ReceiptsRepo.ReceiptById(Convert.ToInt32(Id));
                objViewprint.ReceiptsDetail = objDB.SSP_ReceiptsDetailById(Convert.ToInt64(Id)).ToList();
            }
            return View(objViewprint);
        }



        /// <summary>
        /// Get Invoice By Project
        /// </summary>
        /// <param name="project_id">selected projected id</param>
        /// <param name="bill_to">customer</param>
        /// <returns></returns>
        public JsonResult GetInvoiceByProject(Int32 project_id, Int32 bill_to)
        {
            try
            {
                List<SelectListItem> searchList = new List<SelectListItem>();
                using (PMSEntities objDB = new PMSEntities())
                {
                    var _list = objDB.SSP_GetCustomerInvoicesByProject(project_id, bill_to).ToList();
                    if (_list.Count() > 0)
                    {
                        foreach (var item in _list)
                        {
                            if (string.IsNullOrEmpty(item.PaymentAmount) || item.item_amount > Convert.ToDecimal(CommonHelper.CalcualteInvoiceAmount(item.PaymentAmount, item.tax.HasValue ? item.tax.Value : 0, item.is_tax, item.tax_amount, false, false)))
                            {
                                searchList.Add(new SelectListItem
                                {
                                    Text = item.Invoice_number,
                                    Value = item.Id.ToString()
                                });
                            }
                        }


                        //searchList.AddRange(_list.Where(C => C.item_amount > Convert.ToDecimal(CommonHelper.CalcualteInvoiceAmount(C.PaymentAmount, C.tax.HasValue ? C.tax.Value : 0, C.is_tax, false, false))).Select(d =>
                        //       new SelectListItem
                        //       {
                        //           Text = d.Invoice_number,
                        //           Value = d.Id.ToString()
                        //       }
                        //    ));
                    }
                }
                return Json(searchList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                return null;
            }
        }
        public JsonResult GetCustomerProjects(Int32 customer_id)
        {
            try
            {
                List<SelectListItem> searchList = new List<SelectListItem>();
                searchList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
                searchList.AddRange(CustomerProjects(customer_id));
                return Json(searchList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                return null;
            }
        }

        public List<CustomSelectListItem> ProjectInvoicesWithProject(Int32 customer_id)
        {
            using (PMSEntities objDB = new PMSEntities())
            {
                var _list = (from p in objDB.tbl_invoice
                             join i in objDB.tbl_invoice_items on p.Id equals i.invoice_id
                             where p.Bill_to == customer_id
                             select new CustomSelectListItem
                             {
                                 Text = p.Invoice_number,
                                 Value = p.Id.ToString(),
                                 FlagId = i.project_id.Value.ToString()
                             }).OrderBy(o => o.Text).ToList();
                return _list;
            }
        }

        #region Designer Receipts

        public ActionResult DesignerReceipts(DesignerReceiptsViewModel objView)
        {
            if (User.IsInRole("SuperAdmin"))
            {
                objView.UID = "00000000-0000-0000-0000-000000000000";
            }
            else
            {
                objView.UID = User.Identity.GetUserId();
            }

            DateTime now = DateTime.Now;
            //var startDate = new DateTime(now.Year, (now.Month - 5), 1);
            var CurrentstartDate = new DateTime(now.Year, now.Month, 1);
            var endDate = CurrentstartDate.AddMonths(1).AddDays(-1);
            if (objView.SearchFrom == null)
            {
                objView.SearchFrom = CurrentstartDate.AddMonths(-5);
            }
            if (objView.SearchTo == null)
            {
                objView.SearchTo = endDate;
            }
            if ((string.IsNullOrWhiteSpace(objView.SearchText)))
            {
                objView.SearchText = "";
            }
            return View(objView);
        }

        public ActionResult LoadAddEditDesignerReceipt(Int32 Id)
        {
            string uid = User.Identity.GetUserId();
            Int32 branchid = SessionManagement.SelectedBranchID;
            DesignerReceiptsViewModel objView = new DesignerReceiptsViewModel();

            if (Id > 0)
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    designer_receipts objRec = _DesReceiptsRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    if (objRec != null)
                    {
                        CommonFunction.MergeObjects(objView, objRec, true);
                        objView.designer_name = objRec.designer_name;
                    }
                }
                else
                {
                    designer_receipts objRec = _DesReceiptsRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    if (objRec != null)
                    {
                        CommonFunction.MergeObjects(objView, objRec, true);
                        objView.designer_name = objRec.designer_name;
                    }
                }

                List<designer_receipt_details> objRDList = new List<designer_receipt_details>();
                List<designer_receipt_detail> objRDRec = _DesReceiptDetailRepo.FindBy(o => o.receipt_id == Id).ToList();
                objView.invoiceList = DesignerInvoiceByReceiptId(Id);

                foreach (var items in objRDRec)
                {
                    objRDList.Add(new designer_receipt_details
                    {
                        gst_amount = Convert.ToDecimal(items.gst_amount),
                        gst_percentage = Convert.ToDecimal(items.gst_percentage),
                        amount = Convert.ToDecimal(items.amount),
                        total_amount = Convert.ToDecimal(items.total_amount),
                        invoice_id = Convert.ToInt32(items.invoice_id)
                    });
                }
                objView.receipt_details = objRDList;
            }
            else
            {
                objView.receipt_date = DateTime.Now;
                objView.isactive = true;
                objView.receipt_details = new List<designer_receipt_details>();
                objView.invoiceList = new List<SelectListItem>();
                objView.bank_id = SessionManagement.SelectedBranchBankId;
            }
            objView.bankList = CommonFunction.BankList();
            objView.mode_of_paymentList = CommonFunction.ModeofPaymentList();
            objView.designer_list = CommonFunction.DesignerList(objView.designer_name);
            if (User.IsInRole("SuperAdmin"))
            {
                objView.branchList = CommonFunction.BranchList();
            }
            else
            {
                objView.branchList = CommonFunction.UserBranchList(uid);
            }

            objView.IsActiveList = CommonFunction.StatusList();

            return View(objView);
        }

        public ActionResult Delete_DesignerReceptById(Int32 Id)
        {
            if (Id > 0)
            {
                designer_receipts objRec = _DesReceiptsRepo.FindBy(o => o.id == Id).SingleOrDefault();
                _DesReceiptsRepo.Delete(objRec);
                _DesReceiptsRepo.Save();
                using (PMSEntities obj = new PMSEntities())
                {
                    obj.SSP_Remove_DesignerReceipt_Detail(Convert.ToInt32(Id));
                }
            }
            return RedirectToAction("DesignerReceipts");
        }

        public JsonResult GetDesignerInvoice()
        {
            try
            {
                List<SelectListItem> searchList = new List<SelectListItem>();
                searchList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
                searchList.AddRange(DesignerInvoice());
                return Json(searchList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<SelectListItem> DesignerInvoice()
        {
            using (PMSEntities objDB = new PMSEntities())
            {
                List<SelectListItem> lst = new List<SelectListItem>();
                var TotalInvoice = objDB.tbl_designer_invoice.ToList();
                var InvoiceDetail = objDB.tbl_designerinvoice_items.ToList();
                var ReceiptList = objDB.designer_receipt_detail.ToList();


                foreach (var item in TotalInvoice)
                {
                    if (ReceiptList.Select(x => x.invoice_id).Contains(item.Id))
                    {
                        decimal? invoiceTotalAmt = InvoiceDetail.Where(x => x.invoice_id == item.Id)?.Select(x => x.item_amount).Sum();
                        decimal? PayedAmount = ReceiptList.Where(x => x.invoice_id == item.Id)?.Select(x => x.total_amount).Sum();
                        if (PayedAmount < invoiceTotalAmt)
                        {
                            SelectListItem obj = new SelectListItem();
                            obj.Text = item.Invoice_number;
                            obj.Value = item.Id.ToString();
                            lst.Add(obj);
                        }
                    }
                    else
                    {
                        SelectListItem obj = new SelectListItem();
                        obj.Text = item.Invoice_number;
                        obj.Value = item.Id.ToString();
                        lst.Add(obj);
                    }
                }
                return lst;
            }
        }


        public List<SelectListItem> DesignerInvoiceByReceiptId(int receiptId)
        {
            using (PMSEntities objDB = new PMSEntities())
            {
                List<SelectListItem> lst = new List<SelectListItem>();

                List<int?> ReceiptList = objDB.designer_receipt_detail.Where(x => x.receipt_id == receiptId)?.Select(x => x.invoice_id).ToList();
                if (ReceiptList != null && ReceiptList.Count > 0)
                {
                    lst = objDB.tbl_designer_invoice.Where(x => ReceiptList.Contains(x.Id)).ToList().Select(x => new SelectListItem
                    {
                        Text = x.Invoice_number,
                        Value = x.Id.ToString()
                    }).OrderBy(o => o.Text).ToList();
                }
                return lst;
            }
        }

        public JsonResult DesignerInvoiceByDesignerName(string designer_name)
        {
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    List<SelectListItem> lst = new List<SelectListItem>();
                    var invoicelist = objDB.tbl_designer_invoice.Where(x => x.Designer_name == designer_name).ToList();
                    if (invoicelist != null && invoicelist.Count > 0)
                    {
                        lst = invoicelist.Select(x => new SelectListItem
                        {
                            Text = x.Invoice_number,
                            Value = x.Id.ToString()
                        }).OrderBy(o => o.Text).ToList();
                    }
                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
            }
            catch { return new JsonResult(); }
        }

        public JsonResult SaveUpdateDesignerReceipt(DesignerReceiptsViewModel objView)
        {
            if (!string.IsNullOrEmpty(Request.Params["receiptobj"]))
            {
                objView = DesignerReceiptsViewModel.FromJson(Request.Params["receiptobj"]);
            }
            string uid = User.Identity.GetUserId();
            designer_receipts objRec = new designer_receipts();

            if (objView.id > 0)
            {
                objRec = _DesReceiptsRepo.FindBy(o => o.id == objView.id).SingleOrDefault();
                if (objRec != null)
                {
                    objView.created_by = objRec.created_by;
                    objView.created_date = objRec.created_date;
                    CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.modified_date = DateTime.Now;
                    objRec.modified_by = new Guid(uid);
                    objRec.branch_id = SessionManagement.SelectedBranchID;
                    _DesReceiptsRepo.Save();

                    using (PMSEntities obj = new PMSEntities())
                    {
                        obj.SSP_Remove_DesignerReceipt_Detail(Convert.ToInt32(objRec.id));

                        foreach (var item in objView.receipt_details)
                        {
                            obj.SSP_Add_DesignerReceipt_Detail(Convert.ToInt32(objRec.id), Convert.ToDecimal(item.amount), Convert.ToDecimal(item.gst_percentage), Convert.ToDecimal(item.gst_amount), Convert.ToDecimal(item.total_amount), Convert.ToInt32(item.invoice_id));
                        }
                    }
                }

                return Json(new { id = objView.id, msg = "Designer Receipt updated successfully.", cls = "success" });
            }
            else
            {
                CommonFunction.MergeObjects(objRec, objView, true);
                objRec.created_date = DateTime.Now;
                objRec.created_by = new Guid(uid);
                objRec.modified_date = DateTime.Now;
                objRec.modified_by = new Guid(uid);
                objRec.branch_id = SessionManagement.SelectedBranchID;
                _DesReceiptsRepo.Add(objRec);
                _DesReceiptsRepo.Save();

                using (PMSEntities objdb = new PMSEntities())
                {
                    foreach (var item in objView.receipt_details)
                    {
                        objdb.SSP_Add_DesignerReceipt_Detail(Convert.ToInt32(objRec.id), Convert.ToDecimal(item.amount), Convert.ToDecimal(item.gst_percentage), Convert.ToDecimal(item.gst_amount), Convert.ToDecimal(item.total_amount), Convert.ToInt32(item.invoice_id));
                    }
                }

                return Json(new { id = objRec.id, msg = "Designer Receipt created successfully.", cls = "success" });
            }
        }


        public ActionResult DesignerReceiptPrintPreview(Int32 Id)
        {

            PrintDesignerReceiptViewModel objViewprint = new PrintDesignerReceiptViewModel();
            using (PMSEntities objDB = new PMSEntities())
            {
                objViewprint.ssp_receiptsById_result = _DesReceiptsRepo.ReceiptById(Convert.ToInt32(Id));
                objViewprint.ReceiptsDetail = objDB.SSP_DesignerReceiptsDetailById(Convert.ToInt64(Id)).ToList();
            }
            return View(objViewprint);
        }
        #endregion
        public ActionResult LoadSMS(Int32 projectId)
        {
            PaymentsReceived objView = new PaymentsReceived();
            objView.PaymentReceived = PaymentList(projectId);
            Session["payments_received"] = objView.PaymentReceived;
            return View(objView);
        }
        public List<PaymentsReceived> PaymentList(int id)
        {
            List<PaymentsReceived> objList = new List<PaymentsReceived>();
            List<ReceiptsViewModel> ReceiptsViewModelobjList = new List<ReceiptsViewModel>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = objDB.SSP_PaymentDetailsForSMS(id).ToList();
                if (sList.Count() > 0)
                {
                    foreach (var items in sList)
                    {
                        objList.Add(new PaymentsReceived
                        {
                            date_amount = items.date_amount
                            ,
                            cheque_details = items.cheque_details,
                            mobileno = items.mobile,
                            totalPayment = items.totalPayment,
                            branchAddress = items.branchAddress,
                            accountNo = items.accountNo
                        });
                    }
                }

            }
            return objList;
        }
        [HttpPost]
        public JsonResult SendSMS()
        {
            //PaymentsReceived objView = Session["payments_received"] as PaymentsReceived;
            var pay = Session["payments_received"] as IEnumerable<PaymentsReceived>;
           
            string returnMsg, clsStatus;
            string total = "0.0", mob = "", branch = "", account = "";
            try
            {
                string sMsg = "Thank you for the payment made to TLI Pte Ltd \n PAYMENTS RECEIVED :\n";
                if (pay.Count() > 0)
                {
                    foreach (var x in pay)
                    {
                        sMsg += x.date_amount + "\n" + x.cheque_details + "\n";
                        total = x.totalPayment;
                        mob = x.mobileno;
                        branch = x.branchAddress;
                        account = x.accountNo;
                    }
                }
                sMsg += "\n Total Payment Received as of : " + total;
                sMsg += " \n (*This include payment mode via cheque/transfer, amount which is still pending by bank)";
                sMsg += "\n Kindly contact us at 87499919/63859919 if there is any discrepancy.\n Cash payment MUST be acknowledged with our company official receipt. Bank transfer payment should only be made to our corporate bank account: TLI Pte Ltd - "+account;
                sMsg += "\n Accounts \n " + branch + " \n TLI \n (THIS IS AN AUTO - GENERATED PAYMENT UPDATE) ";
                string MTURL = "http://gateway80.onewaysms.sg/api2.aspx";
                string apiusername = System.Configuration.ConfigurationManager.AppSettings["apiusername"];
                string apipassword = System.Configuration.ConfigurationManager.AppSettings["apipassword"];
                if (String.IsNullOrEmpty(mob))
                {
                    returnMsg = "Mobile number is not provided!";
                    clsStatus = "error";
                    return Json(new { msg = returnMsg, cls = clsStatus });
                }
                else
                {
                    bool res = mob.All(c => "0123456789".Contains(c));
                    if (res==false)
                    {
                        returnMsg = "Format of mobile no. is not valid !";
                        clsStatus = "error";
                        return Json(new { msg = returnMsg, cls = clsStatus });
                    }
                  
                }
                mob = "65" + mob.Replace(" ", string.Empty);
                string sURL = MTURL + "?apiusername=" + apiusername + "&apipassword=" + apipassword + "&mobileno=" + mob + "&senderid=onewaysms&languagetype=1&message=" + sMsg;
                using (var web = new System.Net.WebClient())
                {
                    string result = web.DownloadString(sURL);
                    returnMsg = "SMS sent successfully !";
                    clsStatus = "success";
                }
            }
            catch
            {
                returnMsg = "Some error occurred !";
                clsStatus = "error";
            }
            return Json(new { msg = returnMsg, cls = clsStatus });
        }

    }
}