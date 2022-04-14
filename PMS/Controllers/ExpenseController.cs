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
    public class ExpenseController : Controller
    {
        // GET: Expense

        //  private readonly IProject _ProjectRepo;
        //private readonly IProjectAdditionRepository _ProjectAdditionRepo;
        //  private readonly ICustomerRepositor _CustomerRepo;

        private readonly IExpense _ExpenseRepo;
        // private readonly IProjectAdditionRepository _ProjectAdditionRepo;
        // private readonly ICustomerRepositor _CustomerRepo;

        public ExpenseController(IExpense expRepo)
        {
            _ExpenseRepo = expRepo;

        }
        public ActionResult Index(ProjectViewModel objView)
        {
            List<SelectListItem> branchList = new List<SelectListItem>();

            //if (User.IsInRole("SuperAdmin"))
            //{
            //    objView.UID = "00000000-0000-0000-0000-000000000000";
            //    //branchList = Common.CommonFunction.BranchList();
            //}
            //else
            //{
            objView.UID = User.Identity.GetUserId();
            // branchList = Common.CommonFunction.UserBranchList(objView.UID);
            //  }
            //branchList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
            // objView.BranchList = branchList;

            objView.StatusList = CommonFunction.ProjectStatusList();
            objView.SalesmenList = CommonFunction.AllSalesmen();
            if (objView.ProjectStatusId == null)
            {
                objView.ProjectStatusId = 2;
            }

            DateTime now = DateTime.Now;
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
            if ((string.IsNullOrWhiteSpace(objView.SearchText)))
            {
                objView.SearchText = "";
            }
            return View(objView);
        }
        public ActionResult _LoadExpense(Int32 Id)
        {
            string uid = User.Identity.GetUserId();
            ExpenseViewModel objView = new ExpenseViewModel();
            Int32 branchid = SessionManagement.SelectedBranchID;

            int categoryid = 0;
            if (Id > 0)
            {
                //  if (User.IsInRole("SuperAdmin"))
                // {

                // var objRec1 = _ExpenseRepo.FindBy(o => o.id == Id);
                //  expense objRec = _ExpenseRepo.FindBy(o => o.id == Id).SingleOrDefault();
                //    if (objRec != null)
                //    {
                //        Common.CommonFunction.MergeObjects(objView, objRec, true);
                //    }
                //}
                //else
                //{
                //project objRec = _ProjectRepo.FindBy(o => o.id == Id && o.created_by == new Guid(uid)).SingleOrDefault();
                //   project objRec1 = _ProjectRepo.FindBy(o => o.id == Id).SingleOrDefault();

                expense objRec = _ExpenseRepo.FindBy(o => o.id == Id).SingleOrDefault();
                if (objRec != null)
                {
                    Common.CommonFunction.MergeObjects(objView, objRec, true);
                }
                // }

                categoryid = Convert.ToInt32(objRec.Expense_Category_Id);
            }
            else
            {
                //   objView.gst_percentage = Convert.ToDecimal(Common.SessionManagement.BranchGST);
                objView.Amount = Convert.ToDecimal(0.00);
                //  objView.gst_amount = Convert.ToDecimal(0.00);
                //  objView.total_amount = Convert.ToDecimal(0.00);
                objView.Expense_Date = DateTime.Now;
                //  objView.isactive = true;
                // objView.status_id = 2;
            }

            // objView.SalesmenList = Common.CommonFunction.UserSalesmenList(uid);
            //  objView.BankList = Common.CommonFunction.BankList();
            // objView.CustomerList = Common.CommonFunction.CustomerList();
            objView.CompanyList = CommonFunction.BranchList();
            objView.ExpenseCategoryList = CommonFunction.ExpenseCategoryList();
            objView.ExpenseItemList = CommonFunction.ExpenseItemist(categoryid);

            // objView.StatusList = Common.CommonFunction.ProjectStatusList();
            // objView.ActiveInactiveList = Common.CommonFunction.StatusList();
            objView.bankList = CommonFunction.BankList();

            return View(objView);
        }
        public JsonResult GetExpenseItembyCategory(Int32 categoryid)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            using (PMSEntities objDB = new PMSEntities())
            {
                var sList = (from s in objDB.ExpenseCategories
                             where s.Parent == categoryid
                             && s.IsActive == true
                             orderby s.ExpenseCategory1
                             select new SelectListItem
                             {
                                 Text = s.ExpenseCategory1,
                                 Value = s.id.ToString()
                             }
                             ).ToList();

                foreach (var items in sList)
                {
                    objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                }
            }
            objList.Insert(0, new SelectListItem { Text = "Select Company", Value = "0" });
            return Json(objList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SaveExpense(ExpenseViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            expense objRec = new expense();

            if (objView.id > 0)
            {
                objRec = _ExpenseRepo.FindBy(o => o.id == objView.id).SingleOrDefault();
                if (objRec != null)
                {
                    objView.created_by = objRec.Created_By;
                    objView.created_date = Convert.ToDateTime(objRec.Created_Date);
                    //objView.branch_id = objRec.branch_id;
                    objView.branch_id = SessionManagement.SelectedBranchID;
                    CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.Modified_Date = DateTime.Now;
                    objRec.Modified_By = new Guid(uid);
                    _ExpenseRepo.Save();
                }
                return Json(new { msg = "Project updated successfully.", cls = "success" });

            }
            else
            {
                CommonFunction.MergeObjects(objRec, objView, true);
                objRec.Branch_Id = SessionManagement.SelectedBranchID;
                objRec.Created_Date = DateTime.Now;
                objRec.Created_By = new Guid(uid);
                objRec.Modified_Date = DateTime.Now;
                objRec.Modified_By = new Guid(uid);
                _ExpenseRepo.Add(objRec);
                _ExpenseRepo.Save();
                return Json(new { msg = "Project created successfully.", cls = "success" });

            }
        }
        public ActionResult DeleteExpenseById(Int32 Id)
        {
            if (Id > 0)
            {
                expense objRec = _ExpenseRepo.FindBy(o => o.id == Id).SingleOrDefault();
                _ExpenseRepo.Delete(objRec);
                _ExpenseRepo.Save();
            }
            return RedirectToAction("Index");
        }
        public ActionResult PrintPreview(Int32 Id)
        {
            PrintExpenseViewModel objViewprint = new PrintExpenseViewModel();
            PMSEntities objDB = new PMSEntities();

            //var getdata = from exp in objDB.expenses
            //              join br in objDB.branches on exp.Branch_Id.Value equals br.id
            //              join ctg in objDB.ExpenseCategories on exp.Expense_Category_Id equals ctg.id 
            //              join subCat in objDB.ExpenseCategories on exp.Expense_Item_Id equals subCat.id
            //              join bnk in objDB.banks on exp.bank_id equals bnk.id
            //              join brnch in objDB.branches on  bnk.branch_id.Value equals brnch.id
            //              where exp.id == Id
            //              select new PrintExpenseViewModel
            //              {

            //              };



            var Get_expense_data = objDB.expenses.FirstOrDefault(x => x.id == Id);
            if (Get_expense_data != null)
            {
                var Get_ExpenseCategoryList = objDB.ExpenseCategories.Where(x => x.id == Get_expense_data.Expense_Category_Id || x.id == Get_expense_data.Expense_Item_Id).ToList();
                var BranchList = objDB.branches.ToList();
                var BankDetail = objDB.banks.FirstOrDefault(x => x.id == Get_expense_data.bank_id);
                objViewprint.BranchName = BranchList.FirstOrDefault(x => x.id == Get_expense_data.Branch_Id).branch_name;
                objViewprint.Branch_Address = BranchList.FirstOrDefault(x => x.id == Get_expense_data.Branch_Id).address1;//Added by Harshad
                objViewprint.Branch_Phone = BranchList.FirstOrDefault(x => x.id == Get_expense_data.Branch_Id).phone;//Added by Harshad
                objViewprint.Branch_ZipCode = BranchList.FirstOrDefault(x => x.id == Get_expense_data.Branch_Id).zip_code;//Added by Harshad
                objViewprint.CategoryName = Get_ExpenseCategoryList.FirstOrDefault(x => x.id == Get_expense_data.Expense_Category_Id).ExpenseCategory1;
                objViewprint.SubCategoryName = Get_ExpenseCategoryList.FirstOrDefault(x => x.id == Get_expense_data.Expense_Item_Id).ExpenseCategory1;
                if (Get_expense_data.bank_id > 0)
                {
                    objViewprint.BankName = BranchList.FirstOrDefault(x => x.id == BankDetail.branch_id).company_initial + " - " + BankDetail.bank_name;
                }
                objViewprint.ChequeNumber = Get_expense_data.cheque_number;
                objViewprint.ExpenseDate = Get_expense_data.Expense_Date.Value.ToString("dd/MM/yyyy");
                objViewprint.Remark = Get_expense_data.remarks;
                objViewprint.Amount = Get_expense_data.Amount; ;
                objViewprint.Pay_To = Get_expense_data.Pay_To; ;
            }
            return View(objViewprint);
        }
    }
}