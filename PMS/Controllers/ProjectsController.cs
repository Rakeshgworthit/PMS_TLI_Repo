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
    public class ProjectsController : Controller
    {
        // GET: Projects

        private readonly IProject _ProjectRepo;
        private readonly IProjectAdditionRepository _ProjectAdditionRepo;
        private readonly ICustomerRepositor _CustomerRepo;

        public ProjectsController(IProject projRepo, IProjectAdditionRepository additionRepo, ICustomerRepositor custRepo)
        {
            _ProjectRepo = projRepo;
            _ProjectAdditionRepo = additionRepo;
            _CustomerRepo = custRepo;
        }

        public ActionResult Index(ProjectViewModel objView)
        {
            List<SelectListItem> branchList = new List<SelectListItem>();

            if (User.IsInRole("SuperAdmin"))
            {
                objView.UID = "00000000-0000-0000-0000-000000000000";
                //branchList = Common.CommonFunction.BranchList();
            }
            else
            {
                objView.UID = User.Identity.GetUserId();
               // branchList = Common.CommonFunction.UserBranchList(objView.UID);
            }
            //branchList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
            // objView.BranchList = branchList;

            objView.StatusList = CommonFunction.ProjectStatusList();
            objView.SalesmenList = CommonFunction.AllSalesmen();
            if(objView.ProjectStatusId == null)
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
            else {
                objView.from_date = objView.from_date;
            }
            if (objView.to_date == null)
            {
                objView.to_date = endDate;
            }
            else {
                objView.to_date = objView.to_date;
            }
            if((string.IsNullOrWhiteSpace(objView.SearchText)))
            {
                objView.SearchText = "";
            }
            return View(objView);
        }

        public ActionResult _LoadProject(Int32 Id)
        {
            string uid = User.Identity.GetUserId();
            ProjectViewModel objView = new ProjectViewModel();

            if (Id > 0)
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    project objRec = _ProjectRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    if (objRec != null)
                    {
                        Common.CommonFunction.MergeObjects(objView, objRec, true);
                    }
                }
                else
                {
                    //project objRec = _ProjectRepo.FindBy(o => o.id == Id && o.created_by == new Guid(uid)).SingleOrDefault();
                    project objRec = _ProjectRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    if (objRec != null)
                    {
                        Common.CommonFunction.MergeObjects(objView, objRec, true);
                    }
                }
            }
            else {
                objView.gst_percentage = Convert.ToDecimal(Common.SessionManagement.BranchGST);
                objView.contract_amount = Convert.ToDecimal(0.00);
                objView.gst_amount = Convert.ToDecimal(0.00);
                objView.total_amount = Convert.ToDecimal(0.00);
                objView.contract_date = DateTime.Now;
                objView.isactive = true;
                objView.status_id = 2;
                objView.bank_id = SessionManagement.SelectedBranchBankId;
            }

            objView.SalesmenList = Common.CommonFunction.Active_Salesmen(uid);
            objView.BankList = Common.CommonFunction.BankList();
            objView.CustomerList = Common.CommonFunction.CustomerList();

            //if (User.IsInRole("SuperAdmin"))
            //{
            //    objView.BranchList = Common.CommonFunction.BranchList();
            //}
            //else
            //{                
            //    objView.BranchList = Common.CommonFunction.UserBranchList(uid);
            //}

            objView.StatusList = Common.CommonFunction.ProjectStatusList();
            objView.ActiveInactiveList = Common.CommonFunction.StatusList();

            return View(objView);
        }

        public JsonResult SaveProject(ProjectViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            project objRec = new project();

            if (objView.id > 0)
            {                
                objRec = _ProjectRepo.FindBy(o => o.id == objView.id).SingleOrDefault();
                if (objRec != null)
                {
                    objView.created_by = objRec.created_by;
                    objView.created_date = objRec.created_date;
                    //objView.branch_id = objRec.branch_id;
                    objView.branch_id = SessionManagement.SelectedBranchID;
                    CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.modified_date = DateTime.Now;
                    objRec.modified_by = new Guid(uid);
                    _ProjectRepo.Save();
                }
                return Json(new { msg = "Project updated successfully.", cls = "success" });
              
            }
            else
            {
                CommonFunction.MergeObjects(objRec, objView, true);
                objRec.branch_id = SessionManagement.SelectedBranchID;
                objRec.created_date = DateTime.Now;
                objRec.created_by = new Guid(uid);
                objRec.modified_date = DateTime.Now;
                objRec.modified_by = new Guid(uid);
                _ProjectRepo.Add(objRec);
                _ProjectRepo.Save();
                return Json(new { msg = "Project created successfully.", cls = "success" });
               
            }
        }

        public ActionResult Additions(ProjectAdditionsViewModel objView)
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
           // var startDate = new DateTime(now.Year, (now.Month - 5), 1);
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
            objView.isProjectClosed  = false;

            objView.ProjectList = CommonFunction.UserProjectList(objView.UID);
            objView.SalesmenList = CommonFunction.Active_Salesmen("");
            return View(objView);           
        }

        public ActionResult _LoadAdditions(Int32 Id)
        {
            string uid = User.Identity.GetUserId();
            Int32 branchid =SessionManagement.SelectedBranchID;
            ProjectAdditionsViewModel objView = new ProjectAdditionsViewModel();
            objView.AdditionTypeList = CommonFunction.AdditionOmissionTypeList();
            Int32 projectId = 0;
            if (Id > 0)
            {
             
                if (User.IsInRole("SuperAdmin"))
                {
                    additions_omissions objRec = _ProjectAdditionRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    if (objRec != null)
                    {
                        projectId = Convert.ToInt32(objRec.project_id);
                        CommonFunction.MergeObjects(objView, objRec, true);
                    }
                }
                else
                {
                    //additions_omissions objRec = _ProjectAdditionRepo.FindBy(o => o.id == Id && o.created_by == new Guid(uid)).SingleOrDefault();
                    additions_omissions objRec = _ProjectAdditionRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    if (objRec != null)
                    {
                        projectId = Convert.ToInt32(objRec.project_id);
                        CommonFunction.MergeObjects(objView, objRec, true);
                    }


                }
                if (projectId > 0)
                {
                    objView.isProjectClosed = false;
                    project objProRec = _ProjectRepo.FindBy(o => o.id == projectId).SingleOrDefault();
                    if (objProRec.status_id == 3)
                    {
                        objView.isProjectClosed = true;
                    }

                }


            }
            else
            {
                objView.amount = Convert.ToDecimal(0.00);
                objView.gst_percentage = Convert.ToDecimal(Common.SessionManagement.BranchGST);
                objView.gst_amount = Convert.ToDecimal(0.00);
                objView.total_amount = Convert.ToDecimal(0.00);
                objView.date = DateTime.Now;
                objView.isactive = true;
                objView.isProjectClosed = false;
            }
            if (User.IsInRole("SuperAdmin"))
            {
                objView.ProjectList = CommonFunction.UserProjectListWithID("00000000-0000-0000-0000-000000000000", projectId);
            }
            else
            {
                objView.ProjectList = CommonFunction.UserProjectListWithID(uid, projectId);
            }
            objView.ActiveInactiveList = Common.CommonFunction.StatusList();
           

            return View(objView);
        }

        public JsonResult SaveProjectAdditions(ProjectAdditionsViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            additions_omissions objRec = new additions_omissions();

            if (objView.id > 0)
            {
                objRec = _ProjectAdditionRepo.FindBy(o => o.id == objView.id).SingleOrDefault();
                if (objRec != null)
                {
                    objView.created_by = objRec.created_by;
                    objView.created_date = objRec.created_date;
                    CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.modified_date = DateTime.Now;
                    objRec.modified_by = new Guid(uid);
                    _ProjectAdditionRepo.Save();
                }
                return Json(new { msg = "Project addition updated successfully.", cls = "success" });

            }
            else
            {
                CommonFunction.MergeObjects(objRec, objView, true);
                objRec.created_date = DateTime.Now;
                objRec.created_by = new Guid(uid);
                objRec.modified_date = DateTime.Now;
                objRec.modified_by = new Guid(uid);
                _ProjectAdditionRepo.Add(objRec);
                _ProjectAdditionRepo.Save();
                return Json(new { msg = "Project addition created successfully.", cls = "success" });

            }
        }

        
        public string GetSalemanCommision(Int32 salemanid)
        {
            try
            {
                return CommonFunction.GetSalemanCommision(salemanid).ToString();
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public string GetCustomerData(Int32 customerid)
        {
            try
            {
                return CommonFunction.GetCustomerData(customerid).ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public ActionResult DeleteAdditionById(Int32 Id)
        {
            if (Id > 0)
            {
                additions_omissions objRec = _ProjectAdditionRepo.FindBy(o => o.id == Id).SingleOrDefault();
                _ProjectAdditionRepo.Delete(objRec);
                _ProjectAdditionRepo.Save();
            }
            return RedirectToAction("Additions");
        }

        public ActionResult DeleteProjectById(Int32 Id)
        {
            if (Id > 0)
            {
                project objRec = _ProjectRepo.FindBy(o => o.id == Id).SingleOrDefault();
                _ProjectRepo.Delete(objRec);
                _ProjectRepo.Save();
            }
            return RedirectToAction("Index");
        }

    }
}