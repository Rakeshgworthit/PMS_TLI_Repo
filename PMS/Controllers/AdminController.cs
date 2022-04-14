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
    public class AdminController : Controller
    {
        // GET: Admin
        private readonly ICompanyRepositor _CompanyRepo;
        private readonly IBanksRepositor _BankRepo;
        private readonly IBranchesRepositor _BranchRepo;
        private readonly ISalesmenRepositor _SalesmenRepo;
        private readonly IExpenseCategoryRepository _ExpenseCatRepo;

        public AdminController(ICompanyRepositor CompanyRepo, IBranchesRepositor BranchRepo, IBanksRepositor bankRepo, ISalesmenRepositor SalesmenRepo, IExpenseCategoryRepository expensecatRepo)
        {
            _CompanyRepo = CompanyRepo;
            _BankRepo = bankRepo;
            _BranchRepo = BranchRepo;
            _SalesmenRepo = SalesmenRepo;
            _ExpenseCatRepo = expensecatRepo;
        }
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult ExpenseCategory(ExpenseCategotyViewModel objView)
        {
            List<SelectListItem> excatlist = Common.CommonFunction.ExpenseCategoryList();
            excatlist.RemoveAt(0);
            excatlist.Insert(0, new SelectListItem { Text = "----Parent Category----", Value = "0" });

            objView.ExpenseCategoryList = excatlist.ToList();


            return View(objView);
        }
        public ActionResult CategorySaveUpdate(ExpenseCategotyViewModel objView)
        {
            string uid = User.Identity.GetUserId();

            if (objView.id > 0)
            {
                ExpenseCategory objRec = _ExpenseCatRepo.FindBy(i => i.id == objView.id).SingleOrDefault();

                //Common.CommonFunction.MergeObjects(objRec, objView, true);
                objRec.Parent = objView.Parent;
                objRec.ExpenseCategory1 = objView.ExpenseCategory1;
                objRec.IsActive = objView.IsActive;
                _ExpenseCatRepo.Save();
                return Json(new { msg = "Expense Category record updated successfully.", cls = "success", id = objView.id });

            }
            else
            {
                ExpenseCategory objRec = new ExpenseCategory();
                //Common.CommonFunction.MergeObjects(objRec, objView, true);
                objRec.Parent = objView.Parent;
                objRec.ExpenseCategory1 = objView.ExpenseCategory1;
                objRec.IsActive = objView.IsActive;
                _ExpenseCatRepo.Add(objRec);
                _ExpenseCatRepo.Save();
                return Json(new { msg = "Expense Category created successfully.", cls = "success", id = objRec.id });
            }
        }
        public ActionResult CategoryLoadAddEdit(Int32 Id)
        {
            string uid = User.Identity.GetUserId();
            Int32 branchid = SessionManagement.SelectedBranchID;
            ExpenseCategotyViewModel objView = new ExpenseCategotyViewModel();
            List<SelectListItem> excatlist = Common.CommonFunction.ExpenseCategoryList();
            excatlist.RemoveAt(0);
            excatlist.Insert(0, new SelectListItem { Text = "----Parent Category----", Value = "0" });

            objView.ExpenseCategoryList = excatlist.ToList();

            if (Id > 0)
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    ExpenseCategory objRec = _ExpenseCatRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    if (objRec != null)
                    {
                        CommonFunction.MergeObjects(objView, objRec, true);
                    }
                }
                else
                {
                    branch objRec = _BranchRepo.FindBy(o => o.id == Id && o.created_by == new Guid(uid)).SingleOrDefault();
                    if (objRec != null)
                    {
                        CommonFunction.MergeObjects(objView, objRec, true);
                    }
                }
            }
            else
            {
                objView.IsActive = true;
            }
            return View(objView);
        }
        public ActionResult CategoryDeleteById(Int32 Id)
        {
            if (Id > 0)
            {
                ExpenseCategory objRec = _ExpenseCatRepo.FindBy(o => o.id == Id).SingleOrDefault();
                _ExpenseCatRepo.Delete(objRec);
                _ExpenseCatRepo.Save();
            }
            return RedirectToAction("ExpenseCategory");
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Company(CompanyViewModel objView)
        {           
            return View(objView);
        }

        public ActionResult CompanySaveUpdate(CompanyViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            if (objView.id > 0)
            {
                company objRec = _CompanyRepo.FindBy(i => i.id == objView.id).SingleOrDefault();
                objView.created_date = objRec.created_date;
                objView.created_by = objRec.created_by;
                Common.CommonFunction.MergeObjects(objRec, objView, true);
                objRec.modified_date = DateTime.Now;
                objRec.modified_by = new Guid(uid);
                _CompanyRepo.Save();
                return Json(new { msg = "Company record updated successfully.", cls = "success", id = objView.id });

            }
            else
            {
                company objRec = new company();
                Common.CommonFunction.MergeObjects(objRec, objView, true);
                objRec.created_date = DateTime.Now;
                objRec.created_by = new Guid(uid);
                objRec.modified_date = DateTime.Now;
                objRec.modified_by = new Guid(uid);
                _CompanyRepo.Add(objRec);
                _CompanyRepo.Save();
                return Json(new { msg = "Company record created successfully.", cls = "success", id = objRec.id });
            }
        }
        public ActionResult CompanyLoadAddEdit(Int32 Id)
        {
            string uid = User.Identity.GetUserId();
            Int32 branchid = SessionManagement.SelectedBranchID;
            CompanyViewModel objView = new CompanyViewModel();
            if (Id > 0)
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    company objRec = _CompanyRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    if (objRec != null)
                    {
                        CommonFunction.MergeObjects(objView, objRec, true);
                    }
                }
                else
                {
                    company objRec = _CompanyRepo.FindBy(o => o.id == Id && o.created_by == new Guid(uid)).SingleOrDefault();
                    if (objRec != null)
                    {
                        CommonFunction.MergeObjects(objView, objRec, true);
                    }
                }

            }
            else {
                objView.isactive = true;
            }

            return View(objView);
        }
        public ActionResult CompanyDeleteById(Int32 Id)
        {
            if (Id > 0)
            {
                company objRec = _CompanyRepo.FindBy(o => o.id == Id).SingleOrDefault();
                _CompanyRepo.Delete(objRec);
                _CompanyRepo.Save();
            }
            return RedirectToAction("Company");
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Branches(BranchViewModel objView)
        {

            objView.CompanyList = Common.CommonFunction.CompanyList();
            objView.CountryList = Common.CommonFunction.CountryList();

            return View(objView);
        }
        public ActionResult BranchSaveUpdate(BranchViewModel objView)
        {
            string uid = User.Identity.GetUserId();

            if (objView.id > 0)
            {
                branch objRec = _BranchRepo.FindBy(i => i.id == objView.id).SingleOrDefault();
                objView.created_date = objRec.created_date;
                objView.created_by = objRec.created_by;
                CommonFunction.MergeObjects(objRec, objView, true);
                objRec.modified_date = DateTime.Now;
                objRec.company_id = objView.company_id;
                objRec.modified_by = new Guid(uid);
                _BranchRepo.Save();
                return Json(new { msg = "Company record updated successfully.", cls = "success", id = objView.id });

            }
            else
            {
                //Harshad Added Company Id Increment 
                branch objRec = new branch();
                //using (PMSEntities P = new PMSEntities())
                //{
                //    var companyId = P.companies.FirstOrDefault().id;
                //    objRec.company_id = companyId;
                //}
                Common.CommonFunction.MergeObjects(objRec, objView, true);
                objRec.created_date = DateTime.Now;
                objRec.company_id = objView.company_id;
                objRec.created_by = new Guid(uid);
                objRec.modified_date = DateTime.Now;
                objRec.modified_by = new Guid(uid);
                _BranchRepo.Add(objRec);
                _BranchRepo.Save();
                return Json(new { msg = "Company record created successfully.", cls = "success", id = objRec.id });
            }
        }
        public ActionResult BranchLoadAddEdit(Int32 Id)
        {
            string uid = User.Identity.GetUserId();
            Int32 branchid = SessionManagement.SelectedBranchID;
            BranchViewModel objView = new BranchViewModel();
            objView.CompanyList = Common.CommonFunction.CompanyList();
            objView.CountryList = Common.CommonFunction.CountryList();
            using (PMSEntities P = new PMSEntities())
            {
                var companyId = P.companies.FirstOrDefault().id;
                objView.company_id = companyId;
            }
            if (Id > 0)
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    branch objRec = _BranchRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    if (objRec != null)
                    {
                        objView.company_id = objRec.company_id;
                        CommonFunction.MergeObjects(objView, objRec, true);
                    }
                }
                else
                {
                    branch objRec = _BranchRepo.FindBy(o => o.id == Id && o.created_by == new Guid(uid)).SingleOrDefault();
                    if (objRec != null)
                    {
                        CommonFunction.MergeObjects(objView, objRec, true);
                    }
                }
            }
            else {
                objView.isactive = true;
            }

            return View(objView);
        }
        public ActionResult BranchDeleteById(Int32 Id)
        {
            if (Id > 0)
            {
                branch objRec = _BranchRepo.FindBy(o => o.id == Id).SingleOrDefault();
                _BranchRepo.Delete(objRec);
                _BranchRepo.Save();
            }
            return RedirectToAction("Branches");
        }

        public ActionResult Salesmen(SalesmenViewModel objView)
        {           
            //objView.BranchList = Common.CommonFunction.BranchList();
            objView.branch_Id = SessionManagement.SelectedBranchID;
            if (objView.Search_Text == null)
            {
                objView.Search_Text = "";
            }
            else
            {
                objView.Search_Text = objView.Search_Text;
            }
            return View(objView);
        }
        public ActionResult SalesmenSaveUpdate(SalesmenViewModel objView)
        {
            string uid = User.Identity.GetUserId();

            if (objView.id > 0)
            {
                salesman objRec = _SalesmenRepo.FindBy(i => i.id == objView.id).SingleOrDefault();
                objView.created_date = objRec.created_date;
                objView.created_by = objRec.created_by;
                Common.CommonFunction.MergeObjects(objRec, objView, true);
                objRec.modified_date = DateTime.Now;
                objRec.modified_by = new Guid(uid);
                _SalesmenRepo.Save();
                return Json(new { msg = "Saleman record updated successfully.", cls = "success", id = objView.id });

            }
            else
            {
                salesman objRec = new salesman();
                Common.CommonFunction.MergeObjects(objRec, objView, true);
                objRec.created_date = DateTime.Now;
                objRec.created_by = new Guid(uid);
                objRec.modified_date = DateTime.Now;
                objRec.modified_by = new Guid(uid);
                _SalesmenRepo.Add(objRec);
                _SalesmenRepo.Save();
                return Json(new { msg = "Saleman record created successfully.", cls = "success", id = objRec.id });
            }
        }
        public ActionResult SalesmenLoadAddEdit(Int32 Id)
        {
            string uid = User.Identity.GetUserId();
            Int32 branchid = SessionManagement.SelectedBranchID;
            SalesmenViewModel objView = new SalesmenViewModel();
            objView.BranchList = Common.CommonFunction.BranchList();
            if (Id > 0)
            {
                salesman objRec = _SalesmenRepo.FindBy(o => o.id == Id).SingleOrDefault();
                if (objRec != null)
                {
                    CommonFunction.MergeObjects(objView, objRec, true);
                }

                //if (User.IsInRole("SuperAdmin"))
                //{
                //    salesman objRec = _SalesmenRepo.FindBy(o => o.id == Id).SingleOrDefault();
                //    if (objRec != null)
                //    {
                //        CommonFunction.MergeObjects(objView, objRec, true);
                //    }
                //}
                //else
                //{
                //    salesman objRec = _SalesmenRepo.FindBy(o => o.id == Id && o.created_by == new Guid(uid)).SingleOrDefault();
                //    if (objRec != null)
                //    {
                //        CommonFunction.MergeObjects(objView, objRec, true);
                //    }
                //}               

            }
            else {
                objView.saleman_commission = 50;
                objView.isactive = true;
                objView.branch_Id = SessionManagement.SelectedBranchID;
            }
            
            return View(objView);
        }
        public ActionResult SalesmenDeleteById(Int32 Id)
        {
            if (Id > 0)
            {
                salesman objRec = _SalesmenRepo.FindBy(o => o.id == Id).SingleOrDefault();
                objRec.isactive = false;
                _SalesmenRepo.Save();
            }
            return RedirectToAction("Salesmen");
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Banks()
        {
            return View();
        }
        public ActionResult LoadAddEditBank(Int32? Id = 0)
        {
            BankViewModel objView = new BankViewModel();
            if (Id > 0)
            {
                bank objRec = _BankRepo.FindBy(o => o.id == Id).SingleOrDefault();
                if (objRec != null)
                {
                    CommonFunction.MergeObjects(objView, objRec, true);
                }
            }
            else
            {

                objView.isactive = true;
            }
            List<SelectListItem> branchList = CommonFunction.BranchList();
            branchList.Insert(0, new SelectListItem { Value = "0", Text = "Please Select" });
            objView.BranchList = branchList;
            return View(objView);
        }

        public JsonResult SaveBank(BankViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            bank objRec = new bank();

            if (objView.id > 0)
            {
                objRec = _BankRepo.FindBy(o => o.id == objView.id).SingleOrDefault();
                if (objRec != null)
                {
                    objView.created_by = objRec.created_by;
                    objView.created_date = objRec.created_date;
                    CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.modified_date = DateTime.Now;
                    objRec.modified_by = new Guid(uid);
                    _BankRepo.Save();
                }
                return Json(new { msg = "Bank updated successfully.", cls = "success" });

            }
            else
            {
                CommonFunction.MergeObjects(objRec, objView, true);
                objRec.created_date = DateTime.Now;
                objRec.created_by = new Guid(uid);
                objRec.modified_date = DateTime.Now;
                objRec.modified_by = new Guid(uid);
                _BankRepo.Add(objRec);
                _BankRepo.Save();
                return Json(new { msg = "Bank created successfully.", cls = "success" });
            }
        }

        public ActionResult DeleteBankById(Int32 Id)
        {
            if (Id > 0)
            {
                bank objRec = _BankRepo.FindBy(o => o.id == Id).SingleOrDefault();
                _BankRepo.Delete(objRec);
                _BankRepo.Save();
            }
            return RedirectToAction("Banks");
        }

    }
}