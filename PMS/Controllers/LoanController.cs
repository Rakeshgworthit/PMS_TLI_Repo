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
    public class LoanController : Controller
    {
        private readonly ILoanRepositor _LoanRepo;
        private readonly ILoanDetailRepository _LoanDetailRepo;
        public LoanController(ILoanRepositor LoanRepo, ILoanDetailRepository LoanDetailRepo)
        {
            _LoanRepo = LoanRepo;
            _LoanDetailRepo = LoanDetailRepo;
        }
        public ActionResult Index(LoanViewModel objView)
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
            var startDate = new DateTime(now.Year, now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            if (objView.SearchFrom == null)
            {
                objView.SearchFrom = startDate;
            }
            if (objView.SearchTo == null)
            {
                objView.SearchTo = endDate;
            }
            //objView.RecordTypeList = CommonFunction.GetRecordTypeList();


            return View(objView);
        }

        public ActionResult LoadAddEdit(Int32 Id)
        {
            string uid = User.Identity.GetUserId();
            Int32 branchid = Common.SessionManagement.SelectedBranchID;
            LoanViewModel objView = new LoanViewModel();
            objView.advanceLoanDetails = new List<CustomLoanDetailsViewModel>();
            if (Id > 0)
            {

                if (User.IsInRole("SuperAdmin"))
                {
                    loan objRec = _LoanRepo.FindBy(o => o.Id == Id).SingleOrDefault();
                    if (objRec != null)
                    {

                        objView.LoanDate = objRec.LoanDate;
                        objView.rec_type = objRec.rec_type;
                        objView.person_id = Convert.ToString(objRec.person_type + "_" + objRec.person_id);
                        objView.person_type = objRec.person_type;
                        objView.purpose = objRec.purpose;
                        objView.payment_mode = objRec.payment_mode;
                        objView.bank_id = objRec.bank_id;
                        objView.cheque_number = objRec.cheque_number;
                        objView.created_by = objRec.created_by;
                        objView.created_on = objRec.created_on;
                        objView.updated_by = objRec.updated_by;
                        objView.updated_on = objRec.updated_on;
                        objView.isactive = objRec.isactive;
                        if (objRec.loansDetails.Count() > 0)
                        {
                            objView.advanceLoanDetails.AddRange(objRec.loansDetails.Select(p => new CustomLoanDetailsViewModel()
                            {
                                Amount = p.amount,
                                Id = p.project_id,
                            }).ToList());
                        }
                        objView.amount = objRec.amount;//(objRec.loansDetails.Count() > 0) ? objRec.loansDetails.Sum(p => p.amount) : objRec.amount;
                    }
                }
                else
                {
                    //loan objRec = _LoanRepo.FindBy(o => o.Id == Id && o.created_by == new Guid(uid)).SingleOrDefault();
                    loan objRec = _LoanRepo.FindBy(o => o.Id == Id).SingleOrDefault();
                    if (objRec != null)
                    {

                        objView.LoanDate = objRec.LoanDate;
                        objView.rec_type = objRec.rec_type;
                        objView.person_id = Convert.ToString(objRec.person_type + "_" + objRec.person_id);
                        objView.person_type = objRec.person_type;
                        objView.purpose = objRec.purpose;
                        objView.payment_mode = objRec.payment_mode;
                        objView.bank_id = objRec.bank_id;
                        objView.cheque_number = objRec.cheque_number;
                        //objView.amount = objRec.amount;
                        objView.created_by = objRec.created_by;
                        objView.created_on = objRec.created_on;
                        objView.updated_by = objRec.updated_by;
                        objView.updated_on = objRec.updated_on;
                        objView.isactive = objRec.isactive;
                        if (objRec.loansDetails.Count() > 0)
                        {

                            objView.advanceLoanDetails.AddRange(objRec.loansDetails.Select(p => new CustomLoanDetailsViewModel()
                            {
                                Amount = p.amount,
                                Id = p.project_id,
                            }));
                        }
                        objView.amount = objRec.amount;//(objRec.loansDetails.Count() > 0) ? objRec.loansDetails.Sum(p => p.amount) : objRec.amount;

                    }
                }


            }
            else
            {
                objView.LoanDate = Convert.ToDateTime(DateTime.Now);
                objView.amount = Convert.ToDecimal(0.00);
                objView.isactive = true;
                objView.bank_id = SessionManagement.SelectedBranchBankId;
            }
            objView.branch_Id = SessionManagement.SelectedBranchID;
            objView.RecordTypeList = Common.CommonFunction.GetRecordTypeList();
            objView.SalesmenAndUserList = Common.CommonFunction.GetSalesmenAndUser();
            objView.bankList = Common.CommonFunction.BankList();
            objView.mode_of_paymentList = Common.CommonFunction.ModeofPaymentList();
            objView.IsActiveList = Common.CommonFunction.StatusList();

            /// LoanUserProjectList
            objView.projectList = CommonFunction.LoanUserProjectList(objView.UID);
            return View(objView);
        }


        public JsonResult SaveUpdate(LoanViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            loan objRec = new loan();
            bool isAdvanceLoanDetails = ValidateaAdvanceLoanDetails(objView);
            if (objView.Id > 0)
            {
                objRec = _LoanRepo.FindBy(o => o.Id == objView.Id).SingleOrDefault();
                if (objRec != null)
                {

                    objRec.branch_Id = SessionManagement.SelectedBranchID;
                    objRec.LoanDate = objView.LoanDate;
                    objRec.rec_type = objView.rec_type;

                    string PersonType = Convert.ToString(objView.person_id).Split('_')[0];
                    string PersonID = Convert.ToString(objView.person_id).Split('_')[1];
                    objRec.person_id = Convert.ToInt32(PersonID);
                    objRec.person_type = Convert.ToString(PersonType);


                    objRec.purpose = objView.purpose;
                    objRec.payment_mode = objView.payment_mode;

                    objRec.bank_id = objView.bank_id;
                    objRec.cheque_number = objView.cheque_number;
                    //objView.amount = objRec.amount;
                    objRec.amount = objView.amount;//isAdvanceLoanDetails ? objView.advanceLoanDetails.Sum(p => p.Amount) : objView.amount;

                    objRec.created_by = objView.created_by;
                    objRec.created_on = objView.created_on;
                    objRec.updated_on = DateTime.Now;
                    objRec.updated_by = new Guid(uid);
                    objRec.isactive = objView.isactive;
                    _LoanDetailRepo.RemoveLoanDetailById(objRec.Id);
                    if (isAdvanceLoanDetails)
                    {
                        List<loansDetail> objLoanDetail = new List<loansDetail>();
                        objLoanDetail.AddRange(objView.advanceLoanDetails.Select(
                            d => new loansDetail()
                            {
                                amount = d.Amount,
                                loan_Id = objRec.Id,
                                project_id = d.Id,
                                created_by = objRec.created_by,
                                created_on = objRec.created_on,
                                updated_on = objRec.updated_on,
                                updated_by = objRec.updated_by,
                                isactive = true,
                            }
                            ));
                        objRec.loansDetails = objLoanDetail;
                    }
                    _LoanRepo.Add(objRec);
                    _LoanRepo.Save();
                }
                return Json(new { msg = "Loan successfully updated.", cls = "success", id = Convert.ToString(objRec.Id) });

            }
            else
            {

                objRec.branch_Id = SessionManagement.SelectedBranchID;
                objRec.LoanDate = objView.LoanDate;
                objRec.rec_type = objView.rec_type;

                string PersonType = Convert.ToString(objView.person_id).Split('_')[0];
                string PersonID = Convert.ToString(objView.person_id).Split('_')[1];
                objRec.person_id = Convert.ToInt32(PersonID);
                objRec.person_type = Convert.ToString(PersonType);


                objRec.purpose = objView.purpose;
                objRec.payment_mode = objView.payment_mode;

                objRec.bank_id = objView.bank_id;
                objRec.cheque_number = objView.cheque_number;
                //objView.amount = objRec.amount;
                objRec.amount = objView.amount;//isAdvanceLoanDetails ? objView.advanceLoanDetails.Sum(p => p.Amount) : objView.amount;

                //   Common.CommonFunction.MergeObjects(objRec, objView, true);
                objRec.isactive = objView.isactive;


                objRec.created_on = DateTime.Now;
                objRec.created_by = new Guid(uid);
                objRec.updated_on = DateTime.Now;
                objRec.updated_by = new Guid(uid);
                if (isAdvanceLoanDetails)
                {
                    List<loansDetail> onjLoanDetail = new List<loansDetail>();
                    onjLoanDetail.AddRange(objView.advanceLoanDetails.Select(
                        d => new loansDetail()
                        {
                            amount = d.Amount,
                            loan_Id = objRec.Id,
                            project_id = d.Id,
                            created_by = objRec.created_by,
                            created_on = objRec.created_on,
                            updated_on = objRec.updated_on,
                            updated_by = objRec.updated_by,
                            isactive = true,
                        }
                        ));
                    objRec.loansDetails = onjLoanDetail;
                }
                _LoanRepo.Add(objRec);
                _LoanRepo.Save();
                return Json(new { msg = "Loan successfully saved.", cls = "success", id = Convert.ToString(objRec.Id) });
            }
        }


        public ActionResult DeleteById(Int32 Id)
        {
            if (Id > 0)
            {
                loan objRec = _LoanRepo.FindBy(o => o.Id == Id).SingleOrDefault();
                if (objRec != null)
                {
                    _LoanDetailRepo.RemoveLoanDetailById(objRec.Id);
                }
                //_LoanRepo.Delete(objRec);
                //_LoanRepo.Save();
            }
            return RedirectToAction("Index");
        }

        public ActionResult PrintPreview(Int32 Id)
        {
            PrintPaymentViewModel objViewprint = new PrintPaymentViewModel();
            SSP_PrintLoan_Result objLoanResult = new SSP_PrintLoan_Result();


            using (PMSEntities obj = new PMSEntities())
            {
                objLoanResult = obj.SSP_PrintLoan(Id).SingleOrDefault();
            }

            objViewprint.LoanPrint = objLoanResult;
            objViewprint.LoanDetails = _LoanDetailRepo.GetLoanProjectsDetailsByLoanId(Id);
            return View(objViewprint);
        }

        #region Page Level Functions
        /// <summary>
        /// Validate is need to insert the record /
        /// Flag Record Type is Advance only
        /// </summary>
        /// <param name="objView"></param>
        /// <returns></returns>
        private static bool ValidateaAdvanceLoanDetails(LoanViewModel objView)
        {
            bool isRecordExistWithData = false;
            //Code Will uncomment after Client Confirmation
            //isRecordExistWithData = string.Equals(CommonFunction.GetRecordTypeById(objView.rec_type), CommonHelper.GetEnumDescription(CommonHelper.RecordType.Advanced)) && (objView.advanceLoanDetails.Count() > 0);
            isRecordExistWithData = (objView.advanceLoanDetails.Count() > 0);
            if (isRecordExistWithData & objView.advanceLoanDetails.Count() == 1)
            {
                isRecordExistWithData = (objView.advanceLoanDetails.FirstOrDefault().Id > 0 || objView.advanceLoanDetails.FirstOrDefault().Amount > 0);
            }
            return isRecordExistWithData;
        }
        #endregion

    }
}