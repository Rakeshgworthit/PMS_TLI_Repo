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

namespace PMS.Controllers
{
    [Authorize]
    [BranchFilter]
    public class PaymentsController : Controller
    {
        private readonly ILoanRepositor _LoanRepo;
        private readonly ILoanDetailRepository _LoanDetailRepo;
        private readonly IProject _ProjectRepo;
        private readonly IPaymentsRepositor _PaymentRepo;
        private readonly IPaymentDetailRepository _PaymentDetailRepo;
        public PaymentsController(IPaymentsRepositor PayRepo, IPaymentDetailRepository PayDetRepo, IProject projRepo, ILoanRepositor LoanRepo, ILoanDetailRepository LoanDetailRepo)
        {
            _ProjectRepo = projRepo;
            _PaymentRepo = PayRepo;
            _PaymentDetailRepo = PayDetRepo;
            _LoanRepo = LoanRepo;
            _LoanDetailRepo = LoanDetailRepo;
        }

        public ActionResult Index(PaymentsViewModel objView)
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

            objView.projectList = CommonFunction.UserProjectList(objView.UID);
            objView.SalesmenList = CommonFunction.Active_Salesmen("");
            return View(objView);
        }
        public ActionResult DeleteById(Int32 Id)
        {
            if (Id > 0)
            {
                payment objRec = _PaymentRepo.FindBy(o => o.id == Id).SingleOrDefault();
                List<payment_detail> objDetRec = _PaymentDetailRepo.FindBy(o => o.payment_id == Id).ToList();
                if (objDetRec != null && objDetRec.Count > 0)
                {
                    foreach (var item in objDetRec)
                    {
                        _PaymentDetailRepo.Delete(item);
                    }
                    _PaymentDetailRepo.Save();
                }
                _PaymentRepo.Delete(objRec);
                _PaymentRepo.Save();
                loan objRecLoan = _LoanRepo.GetAll().Where(c => c.supplier_payment_Id == Id).FirstOrDefault();
                if (objRecLoan != null)
                {
                    _LoanDetailRepo.RemoveLoanDetailById(objRecLoan.Id);
                }
            }
            return RedirectToAction("Index");

        }

        public class InvoiceCls
        {
            public Int32 Id { get; set; }
            public String Name { get; set; }
            public decimal AgreedAmt { get; set; }
            public decimal InvoiceAmount { get; set; }
            public decimal payment_amt { get; set; }
            public decimal payment_Count { get; set; }

        }

        public JsonResult GetInvoiceList(Int32 Id)
        {
            try
            {
                List<InvoiceCls> searchList = new List<InvoiceCls>();
                List<SSP_GetInvoiceList_Result> _list = new List<SSP_GetInvoiceList_Result>();
                using (PMSEntities obj = new PMSEntities())
                {
                    _list = obj.SSP_GetInvoiceList(Id, SessionManagement.SelectedBranchID).ToList();
                    //searchList =
                }

                foreach (var _rec in _list)
                {
                    InvoiceCls clsObj = new InvoiceCls
                    {
                        Id = _rec.Id,
                        Name = _rec.invoice_number,
                        AgreedAmt = Convert.ToDecimal(_rec.agreed_amt), //- Convert.ToDecimal(_rec.paid_amt)
                        InvoiceAmount = Convert.ToDecimal(_rec.InvoiceAmount),
                        payment_amt = Convert.ToDecimal(_rec.payment_amt),
                        payment_Count = Convert.ToInt32(_rec.payment_Count)
                    };
                    searchList.Add(clsObj);
                }
                searchList.Insert(0, new InvoiceCls { Name = "Please Select", Id = 0, AgreedAmt = 0 });
                return Json(searchList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                return null;
            }
        }

        public ActionResult LoadAddEdit(Int32 Id)
        {
            string uid = User.Identity.GetUserId();
            Int32 branchid = SessionManagement.SelectedBranchID;
            PaymentsViewModel objView = new PaymentsViewModel();
            objView.mode_of_paymentList = CommonFunction.ModeofPaymentList();
            Int32 projectId = 0;
            List<SSP_GetInvoiceList_Result> objInvList = new List<SSP_GetInvoiceList_Result>();

            if (Id > 0)
            {
                #region Loan detail

                List<loansDetail> loansDetails = new List<loansDetail>();
                using (PMSEntities obj = new PMSEntities())
                {
                    var loans = obj.SSP_SupplierLoanDetailbyPaymentId(Id).ToList();
                    if (loans.Count() > 0)
                    {
                        loansDetails.AddRange(loans.Select(l => new loansDetail()
                        {
                            amount = Convert.ToDecimal(l.amount),
                            supplierinvoice_item_id = l.supplierinvoice_item_id,
                            project_id = Convert.ToInt32(l.project_id),

                        }));

                        objView.loan_return = loansDetails.Select(x => x.amount).Sum();
                    }

                }

                #endregion

                if (User.IsInRole("SuperAdmin"))
                {
                    payment objRec = _PaymentRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    if (objRec != null)
                    {
                        // projectId = Convert.ToInt32(objRec.project_id);
                        CommonFunction.MergeObjects(objView, objRec, true);
                    }
                }
                else
                {
                    //payment objRec = _PaymentRepo.FindBy(o => o.id == Id && o.created_by == new Guid(uid)).SingleOrDefault();
                    payment objRec = _PaymentRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    if (objRec != null)
                    {
                        //  projectId = Convert.ToInt32(objRec.project_id);
                        CommonFunction.MergeObjects(objView, objRec, true);
                    }
                }

                List<payment_details> objPDList = new List<payment_details>();
                List<payment_detail> objPDRec = _PaymentDetailRepo.FindBy(o => o.payment_id == Id).ToList();

                foreach (var items in objPDRec)
                {
                    decimal amount = 0;
                    objPDList.Add(new payment_details
                    {
                        // project_id = Convert.ToInt64(items.project_id),
                        // supplier_inv_number = items.supplier_inv_number,
                        // gst_amount = Convert.ToDecimal(items.gst_amount),
                        //  gst_percentage = Convert.ToDecimal(items.gst_percentage),
                        //  invoice_amount = Convert.ToDecimal(items.invoice_amount),
                        invoice_id = items.supplierinvoice_item_id,
                        agreed_amount = Convert.ToDecimal(items.agreed_amount),
                        payment_amount = Convert.ToDecimal(items.payment_amount),
                        loan_amount = amount,
                    });
                }

                objView.payment_details = objPDList;
                if (projectId > 0)
                {
                    objView.isProjectClosed = false;
                    project objProRec = _ProjectRepo.FindBy(o => o.id == projectId).SingleOrDefault();
                    if (objProRec.status_id == 3)
                    {
                        objView.isProjectClosed = true;
                    }
                }

                objInvList = CommonFunction.SupplierInvoiceList(Convert.ToInt32(objView.supplier_id));

            }
            else
            {

                objView.gst_amount = Convert.ToDecimal(0.00);
                objView.rebate_amount = Convert.ToDecimal(0.00);
                objView.paid_amount = Convert.ToDecimal(0.00);
                objView.invoice_amount = Convert.ToDecimal(0.00);
                objView.payment_amount = Convert.ToDecimal(0.00);
                objView.loan_return = Convert.ToDecimal(0.00);
                objView.payment_date = DateTime.Now;
                objView.isactive = true;
                objView.payment_mode = 4;
                objView.isProjectClosed = false;
                objView.payment_details = new List<payment_details>();
                objView.bank_id = SessionManagement.SelectedBranchBankId;
            }

            //objView.InvoiceList = objInvList.Where(c => c.InvoiceAmount >= 0).ToList();
            objView.InvoiceList = objInvList.ToList();

            if (User.IsInRole("SuperAdmin"))
            {
                objView.supplierList = CommonFunction.UserSupplierList("00000000-0000-0000-0000-000000000000");
                // objView.projectList = CommonFunction.UserProjectListWithID("00000000-0000-0000-0000-000000000000", projectId);
            }
            else
            {
                objView.supplierList = CommonFunction.UserSupplierList(uid);
                // objView.projectList = CommonFunction.UserProjectListWithID(uid, projectId);
            }
            objView.bankList = CommonFunction.BankList();

            // objView.IsActiveList = Common.CommonFunction.StatusList();

            return View(objView);
        }

        public Int32 CheckExistingPayment(PaymentChkExistingViewModel objView)
        {
            using (PMSEntities pmsobj = new PMSEntities())
            {
                if (!string.IsNullOrEmpty(Request.Params["payobjchkExist"]))
                {
                    objView = PaymentChkExistingViewModel.FromJson(Request.Params["payobjchkExist"]);
                }
                var arry = objView.payment_details_chk_Existing.Select(o => new { o.supplier_inv_number, o.project_id }).ToList();


                List<string> supinvlst = arry.Select(o => o.supplier_inv_number).ToList();
                List<long?> projectlst = arry.Select(o => (long?)o.project_id).ToList();

                var PaymentObj = (from pay in pmsobj.payments
                                  join paydet in pmsobj.payment_detail on pay.id equals paydet.payment_id
                                  where pay.id != objView.id && pay.supplier_id == objView.supplier_id
                                  // && supinvlst.Contains(paydet.supplier_inv_number)
                                  // && projectlst.Contains(paydet.project_id)
                                  select new { pay.id }).Count();

                return PaymentObj;

            }

        }

        public JsonResult SaveUpdate(PaymentsViewModel objView)
        {
            if (!string.IsNullOrEmpty(Request.Params["paymentobj"]))
            {
                objView = PaymentsViewModel.FromJson(Request.Params["paymentobj"]);
            }

            string uid = User.Identity.GetUserId();
            payment objRec = new payment();

            if (objView.id > 0)
            {
                objRec = _PaymentRepo.FindBy(o => o.id == objView.id).SingleOrDefault();

                loan objRecLoan = _LoanRepo.GetAll().Where(c => c.supplier_payment_Id == objView.id).FirstOrDefault();
                if (objRec != null)
                {
                    //objRec.actual_amount_received = objView.actual_amount_received;
                    objView.created_by = objRec.created_by;
                    objView.created_date = objRec.created_date;
                    CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.modified_date = DateTime.Now;
                    objRec.modified_by = new Guid(uid);
                    _PaymentRepo.Save();

                    using (PMSEntities obj = new PMSEntities())
                    {
                        obj.SSP_Remove_Payment_Detail(Convert.ToInt32(objRec.id));

                        foreach (var item in objView.payment_details)
                        {
                            obj.SSP_Add_Payment_Detail(Convert.ToInt32(objRec.id), Convert.ToInt32(item.invoice_id), Convert.ToDecimal(item.payment_amount), Convert.ToDecimal(item.agreed_amount));
                        }
                    }
                }
                #region Load add edit
                //Remove Loan & Loan Details
                if (objRecLoan != null)
                {
                    using (PMSEntities obj = new PMSEntities())
                    {
                        obj.SSP_Remove_LoanLoanDetailsFromSupplierPayment(objRecLoan.Id);
                        //_LoanDetailRepo.RemoveLoanDetailById(objRecLoan.Id);
                    }
                }
               
                //Add New Loan and Loan Details
                if (objView.loan_return > 0)
                {
                    objRecLoan = new loan();
                    objRecLoan.supplier_payment_Id = Convert.ToInt32(objRec.id);
                    objRecLoan.branch_Id = SessionManagement.SelectedBranchID;
                    objRecLoan.LoanDate = DateTime.Now;
                    objRecLoan.rec_type = Convert.ToInt32(CommonHelper.RecordType.LoanReturn);
                    objRecLoan.person_id = Convert.ToInt32(objRec.supplier_id);
                    objRecLoan.person_type = Convert.ToString(CommonHelper.UserType.S);
                    objRecLoan.purpose = objRec.remarks;
                    objRecLoan.payment_mode = objView.payment_mode;
                    objRecLoan.bank_id = objView.bank_id;
                    //int cheqNumber = 0;
                    //int.TryParse(objRec.cheque_number, out cheqNumber);
                    objRecLoan.cheque_number = objRec.cheque_number;
                    //objView.amount = objRec.amount;
                    objRecLoan.amount = objView.loan_return;
                    //   Common.CommonFunction.MergeObjects(objRec, objView, true);
                    objRecLoan.isactive = objView.isactive;
                    objRecLoan.created_on = DateTime.Now;
                    objRecLoan.created_by = new Guid(uid);
                    objRecLoan.updated_on = DateTime.Now;
                    objRecLoan.updated_by = new Guid(uid);

                    List<loansDetail> onjLoanDetail = new List<loansDetail>();
                    var loanDetail = new loansDetail()
                    {
                        amount = Convert.ToDecimal(objView.loan_return),
                        loan_Id = objRecLoan.Id,
                        created_by = objRecLoan.created_by,
                        created_on = objRecLoan.created_on,
                        updated_on = objRecLoan.updated_on,
                        updated_by = objRecLoan.updated_by,
                        supplierinvoice_item_id = null,
                        isactive = true,
                    };
                    onjLoanDetail.Add(loanDetail);
                    if (onjLoanDetail.Count() > 0)
                    {
                        objRecLoan.loansDetails = onjLoanDetail;
                    }
                    _LoanRepo.Add(objRecLoan);
                    _LoanRepo.Save();
                }

                #endregion
                Session["supplierId"] = objRec.supplier_id;
                return Json(new { msg = "Payment updated successfully.", cls = "success", id = Convert.ToString(objRec.id) });

            }
            else
            {

                CommonFunction.MergeObjects(objRec, objView, true);
                objRec.created_date = DateTime.Now;
                objRec.created_by = new Guid(uid);
                objRec.modified_date = DateTime.Now;
                objRec.modified_by = new Guid(uid);
                _PaymentRepo.Add(objRec);
                _PaymentRepo.Save();
                using (PMSEntities obj = new PMSEntities())
                {
                    foreach (var item in objView.payment_details)
                    {
                        obj.SSP_Add_Payment_Detail(Convert.ToInt32(objRec.id), Convert.ToInt32(item.invoice_id), Convert.ToDecimal(item.payment_amount), Convert.ToDecimal(item.agreed_amount));
                    }
                }


                #region Add Loan
                if (objView.loan_return > 0)
                {
                    loan objRecLoan = new loan();
                    objRecLoan.supplier_payment_Id = Convert.ToInt32(objRec.id);
                    objRecLoan.branch_Id = SessionManagement.SelectedBranchID;
                    objRecLoan.LoanDate = DateTime.Now;
                    objRecLoan.rec_type = Convert.ToInt32(CommonHelper.RecordType.LoanReturn);
                    objRecLoan.person_id = Convert.ToInt32(objRec.supplier_id);
                    objRecLoan.person_type = Convert.ToString(CommonHelper.UserType.S);
                    objRecLoan.purpose = objRec.remarks;
                    objRecLoan.payment_mode = objView.payment_mode;
                    objRecLoan.bank_id = objView.bank_id;
                    //int cheqNumber = 0;
                    //int.TryParse(objRec.cheque_number, out cheqNumber);
                    objRecLoan.cheque_number = objRec.cheque_number;
                    //objView.amount = objRec.amount;
                    objRecLoan.amount = objView.loan_return;
                    //   Common.CommonFunction.MergeObjects(objRec, objView, true);
                    objRecLoan.isactive = objView.isactive;
                    objRecLoan.created_on = DateTime.Now;
                    objRecLoan.created_by = new Guid(uid);
                    objRecLoan.updated_on = DateTime.Now;
                    objRecLoan.updated_by = new Guid(uid);

                    List<loansDetail> onjLoanDetail = new List<loansDetail>();

                    var loanDetail = new loansDetail()
                    {
                        amount = Convert.ToDecimal(objView.loan_return),
                        loan_Id = objRecLoan.Id,
                        created_by = objRecLoan.created_by,
                        created_on = objRecLoan.created_on,
                        updated_on = objRecLoan.updated_on,
                        updated_by = objRecLoan.updated_by,
                        supplierinvoice_item_id = null,
                        isactive = true,
                    };
                    onjLoanDetail.Add(loanDetail);
                    if (onjLoanDetail.Count() > 0)
                    {
                        objRecLoan.loansDetails = onjLoanDetail;
                    }
                    _LoanRepo.Add(objRecLoan);
                    _LoanRepo.Save();
                }
                #endregion
                Session["supplierId"] = objRec.supplier_id;
                return Json(new { msg = "Payment created successfully.", cls = "success", id = Convert.ToString(objRec.id) });

            }
        }

        public ActionResult PrintPreview(Int32 Id)
        {
            PrintPaymentViewModel objViewprint = new PrintPaymentViewModel();
            objViewprint.ssp_paymentById_result = _PaymentRepo.PaymentById(Convert.ToInt32(Id));
            using (PMSEntities obj = new PMSEntities())
            {
                objViewprint.SSP_PaymentDetails = obj.SSP_GetPaymentDetails(Convert.ToInt32(Id)).ToList();
            }
            return View(objViewprint);
        }

        public decimal GetSupplierRebateAmt(Int32 Id)
        {
            decimal _ret = 0;
            using (PMSEntities obj = new PMSEntities())
            {
                _ret = Convert.ToDecimal(obj.suppliers.Where(o => o.id == Id).Select(o => o.rebate_per).SingleOrDefault());
            }
            return _ret;
        }

        public ActionResult LoadSMS(string collectionDate)
        {
            ViewBag.collectionDate = collectionDate;
            return View();
        }
        [HttpPost]
        public JsonResult SendSMS(string collectionDate)
        {
            long supplierId = (long)Session["supplierId"];

            var mobile = CommonFunction.SupplierMobile(supplierId);
            string returnMsg, clsStatus, supMob;
            supMob = mobile;
            //supMob = "918400966002";
            try
            {
                string sMsg = "Dear Valued Supplier";
                sMsg += "\n This is  from TLI.";
                sMsg += " \n Kindly be informed that your cheque payment is ready for collection on (Collection Date)" + collectionDate;
                sMsg += "\n @ 140 Paya Lebar Rd #01-19 AZ Building,  Singapore 409015.";
                string MTURL = "http://gateway80.onewaysms.sg/api2.aspx";
                string apiusername = System.Configuration.ConfigurationManager.AppSettings["apiusername"];
                string apipassword = System.Configuration.ConfigurationManager.AppSettings["apipassword"];
                if (String.IsNullOrEmpty(supMob))
                {
                    returnMsg = "Mobile number is not provided!";
                    clsStatus = "error";
                    return Json(new { msg = returnMsg, cls = clsStatus });
                }
                supMob = "65" + supMob.Replace(" ", string.Empty);

                sMsg = HttpUtility.UrlEncode(sMsg, System.Text.Encoding.GetEncoding("ISO-8859-1"));
                string sURL = MTURL + "?apiusername=" + apiusername + "&apipassword=" + apipassword + "&mobileno=" + supMob + "&senderid=onewaysms&languagetype=1&message=" + sMsg;
                using (var web = new System.Net.WebClient())
                {
                    string result = web.DownloadString(sURL);
                    returnMsg = "SMS sent successfully !";
                    clsStatus = "success";
                }
            }
            catch (Exception e)
            {
                returnMsg = "Some error occurred !";
                clsStatus = "error";
            }
            return Json(new { msg = returnMsg, cls = clsStatus });
        }
    }
}