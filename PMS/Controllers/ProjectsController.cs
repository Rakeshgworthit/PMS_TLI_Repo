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
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;

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
            if (User.IsInRole("Salesman"))
            {
                objView.ProjectSalesmenId = Common.CommonFunction.GetSalesmanIdByUser(objView.UID);
                //branchList = Common.CommonFunction.BranchList();
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
            if (User.IsInRole("Salesman"))
            {
                objView.ProjectSalesmenId = Common.CommonFunction.GetSalesmanIdByUser(objView.UID);
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

        public ActionResult ProjectDocument(Int32 id, bool viewdoc = false)
        {
            ProjectDocumentViewModel objView = new ProjectDocumentViewModel();
            List<project_document_list> objPDList = new List<project_document_list>();
            SqlCommand Cmd;
            try
            {
                using (var Conn = Common.CommonFunction.GetConnection())
                {
                    Cmd = new SqlCommand("Get_Documents_For_Project", Conn);
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("@projectid", id);                    
                    IDataReader Ireader = Cmd.ExecuteReader();
                    while (Ireader.Read())
                    {
                        project_document_list _GetQuotationDeatils = new project_document_list();
                        _GetQuotationDeatils.document_id = Ireader.GetInt64(0);
                        _GetQuotationDeatils.document_path = Ireader.GetString(1);
                        _GetQuotationDeatils.file_name = Ireader.GetString(2);
                        // _GetQuotationDeatils.document_name = Ireader.GetString(CommonColumns.document_name);
                        _GetQuotationDeatils.project_number = Ireader.GetString(3);
                        _GetQuotationDeatils.project_id = Ireader.GetInt64(4);
                        _GetQuotationDeatils.uploaded_on = Ireader.GetDateTime(5);
                        _GetQuotationDeatils.Uploaded_By_Name = Ireader.GetInt32(6);
                        _GetQuotationDeatils.Id = Ireader.GetInt16(7);
                        _GetQuotationDeatils.file_desc = Ireader.GetString(8);

                        objPDList.Add(_GetQuotationDeatils);
                    }
                    Common.CommonFunction.CloseConnection(Conn);
                }
                if (objView == null)
                {
                    objView.project_id = id;
                }
                else
                {
                    objView.project_document_list = objPDList;
                    objView.project_id = id;
                }
                if (viewdoc == true)
                {
                    return View("LoadDocuments", objView);
                }
                else
                {
                    return View(objView);
                }
            }
            catch (Exception ex)
            {
                throw ex;
                return null;
            }
            finally
            {
                objView = null;
                objPDList = null;
            }
        }

        public ActionResult SaveDocument(Int32 project_id, string file_desc)
        {
            string uid = User.Identity.GetUserId();
            string fname = "";
            string filename = "";
            string extension = string.Empty;
            string fileNameWithExt = string.Empty;
            string filepathRoot = string.Empty;
            string FilePath = ConfigurationManager.AppSettings["PhysicalPath"].ToString();
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
                    string fileName = file.FileName;
                    fileNameWithExt = fileName;
                    string doc_Path = "/UploadDocuments/" + "/Projects/" + project_id;
                    string dirPath = FilePath + doc_Path;

                    string UploadPath = "~/UploadDocuments/Projects/" + project_id ;
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    string addTimeStampToFileName = string.Empty;
                    //if (file.ContentLength == 0)
                    //   continue;
                    if (file.ContentLength > 0)
                    {
                        //addTimeStampToFileName = string.Concat(Path.GetFileNameWithoutExtension(fileName), DateTime.Now.ToString("yyyyMMddHHmmssfff"), Path.GetExtension(file.FileName));
                        filepathRoot = Path.Combine(HttpContext.Request.MapPath(UploadPath) + "/" + fileNameWithExt);
                        extension = Path.GetExtension(file.FileName);
                        file.SaveAs(filepathRoot);
                    }                    
                    using (var Conn = Common.CommonFunction.GetConnection())
                    {
                        SqlCommand cmd = new SqlCommand("Upsert_project_Document", Conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DOCUMENT_NAME", fileName);
                        cmd.Parameters.AddWithValue("@ACTIVITY_TYPE", 0);
                        cmd.Parameters.AddWithValue("@SUB_ACTIVITY_TYPE", 0);
                        cmd.Parameters.AddWithValue("@REMARKS", file_desc);
                        cmd.Parameters.AddWithValue("@ACTIVE_FLAG", 1);
                        cmd.Parameters.AddWithValue("@CREATED_BY",1);
                        cmd.Parameters.AddWithValue("@SUPER_ID", project_id);
                        cmd.Parameters.AddWithValue("@ID", 11);
                        cmd.Parameters.AddWithValue("@ID_TYPE", 6);
                        cmd.Parameters.AddWithValue("@SUBDETAILS_ID", 0);
                        cmd.Parameters.AddWithValue("@SUBSUBDETAILS_ID", 0);
                        cmd.Parameters.AddWithValue("@FILE_TYPE", extension);
                        cmd.Parameters.AddWithValue("@DOCUMENT_CONTENT_TYPE_ID", 0);
                        cmd.Parameters.AddWithValue("@DOCUMENT_PATH ", doc_Path);
                        cmd.Parameters.AddWithValue("@DOC_CONFIG_ID ", 1);
                        cmd.Parameters.AddWithValue("@COMPANY_ID ", 1);
                        cmd.Parameters.AddWithValue("@PROJECT_BUDGET_DETAILS_ID ", 0);
                        cmd.ExecuteNonQuery();
                        Common.CommonFunction.CloseConnection(Conn);
                    }

                }
                return RedirectToAction("ProjectDocument", new { Id = project_id });
            }
            catch (Exception ex)
            {
                throw ex;
                return null;
            }
            finally
            {
            }
        }


        public void DeleteDocument(Int32 Id, string FilePath, string FileName,long project_id)
        {
            string EnroutePath = "~" + FilePath + '/' + FileName;
            using (var Conn  = Common.CommonFunction.GetConnection())
            {
                try
                {
                    System.IO.File.Delete(Server.MapPath(EnroutePath));
                    SqlCommand cmd = new SqlCommand("SSP_DeleteDocument", Conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DocumentId", Id);
                    cmd.Parameters.AddWithValue("@ProjectId", project_id);
                    cmd.ExecuteNonQuery();
                    Common.CommonFunction.CloseConnection(Conn);
                }
                catch (Exception ex)
                {
                    
                }

            }
            // return RedirectToAction("ProjectDocument", new { Id = project_id });
        }


    }
}