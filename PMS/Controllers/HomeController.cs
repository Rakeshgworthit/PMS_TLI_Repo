using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PMS.Models;
using PMS.Database;

namespace PMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index(string ReturnUrl)
        {
            string uid = User.Identity.GetUserId();
            List<SelectListItem> branchList = new List<SelectListItem>();
            //if (User.IsInRole("SuperAdmin"))
            //{
            //    uid = "";
            //    branchList = Common.CommonFunction.BranchList();
            //}
            //else
            //{
            //    branchList = Common.CommonFunction.UserBranchListNew(uid);
            //}
            branchList = Common.CommonFunction.BranchList();
            branchList = branchList.Where(c => c.Value != "0").ToList();


            HomeViewModel objView = new HomeViewModel()
            {
                BranchList = branchList,
                ReturnUrl = ReturnUrl
            };

            return View(objView);
        }

        public ActionResult SetBranch(HomeViewModel objView)
        {
            Common.SessionManagement.SelectedBranchID = objView.BranchID;

            //using (Database.PMSEntities objDB = new Database.PMSEntities())
            //{
            //    Common.SessionManagement.SelectedBranchName = (from b in objDB.branches
            //                                                   join c in objDB.companies on b.company_id equals c.id
            //                                                   where b.id == objView.BranchID
            //                                                   select b.branch_name + ", " + c.company_name 
            //                                                  ).SingleOrDefault();
            //}

            ////Repository.Infrastructure.IBranchesRepositor _objRepo = new Repository.DataService.BranchesService();
            ////Common.SessionManagement.SelectedBranchName = _objRepo.FindBy(o => o.id == objView.BranchID).Select(o => o.branch_name).SingleOrDefault();
            Repository.Infrastructure.IBranchesRepositor _objRepo = new Repository.DataService.BranchesService();
            var objBranch = _objRepo.FindBy(o => o.id == objView.BranchID).SingleOrDefault();
            Common.SessionManagement.SelectedBranchName = Convert.ToString(objBranch.branch_name);

            if (String.IsNullOrEmpty(objBranch.gst_reg_no))
            {
                Common.SessionManagement.BranchGST = 0;
            }
            else
            {
                Common.SessionManagement.BranchGST = 7;
            }

            using (PMSEntities obj = new PMSEntities())
            {
                Int32 bankId = obj.banks.Where(o => o.branch_id == Common.SessionManagement.SelectedBranchID).Select(o => o.id).FirstOrDefault();
                Common.SessionManagement.SelectedBranchBankId = bankId;                
            }

                //using (Database.PMSEntities objDB = new Database.PMSEntities())
                //{
                //    Database.company  company = objDB.companies.Where(o => o.id == objView.BranchID).SingleOrDefault();
                //    Common.SessionManagement.SelectedBranchName = Convert.ToString(company.company_name);

                //    if (String.IsNullOrEmpty(company.gst_reg_no))
                //    {
                //        Common.SessionManagement.BranchGST = 0;
                //    }
                //    else
                //    {
                //        Common.SessionManagement.BranchGST = 7;
                //    }
                //}

                if (!string.IsNullOrEmpty(objView.ReturnUrl))
            {
               // return RedirectToAction("Index", "Projects");
               return Redirect(objView.ReturnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Projects");
            }
        }


        public void test2()
        {
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment; filename=Print.xls");
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
            Response.Write("<html><body><table><tr><td><table><tr><td style='background-color:#d2d2d2;'>Date</td><td>Date 2</td></tr><tr><td>Row 1</td><td>Row 2</td></tr><tr><td>Row 3</td><td>Row 4</td></tr></table></td><td><table><tr><td>Date 4</td><td>Date 5</td></tr><tr><td>Row 1</td><td>Row 2</td></tr><tr><td>Row 3</td><td>Row 4</td></tr></table></td></tr></table></body></html>"); // give ur html string here
            Response.Write("</head>");
            Response.Flush();
        }

        public JsonResult GetBranchList(string code)
        {
            try
            {
                List<SelectListItem> searchList = Common.CommonFunction.BranchListByCode(code == null ? "" : code);
                List<test> branchList = new List<test>();
                foreach (var items in searchList)
                {
                    branchList.Add(new test { id = items.Value, label = items.Text, value = items.Text });
                }
                return Json(branchList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                return null;
            }
        }

        public class test
        {
            public string id { get; set; }
            public string label { get; set; }
            public string value { get; set; }
        }


        public JsonResult GetCustomerList()
        {
            try
            {
                List<SelectListItem> customerList = Common.CommonFunction.CustomerList();                
                return Json(customerList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                return null;
            }
        }

        public JsonResult GetSupplierList()
        {
            try
            {
                List<SelectListItem> customerList = Common.CommonFunction.UserSupplierList("");
                return Json(customerList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                return null;
            }
        }

        public JsonResult GetProjectListByYear(Int32 year, Int32 salesmenId)
        {
            try
            {
                string uid = User.Identity.GetUserId();
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "00000000-0000-0000-0000-000000000000";
                }
                List<SelectListItem> customerList = Common.CommonFunction.UserProjectListByYear(uid.ToString(),year,salesmenId);
                return Json(customerList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                return null;
            }
        }

        public JsonResult GetSupplierListByCode(string str)
        {
            try
            {
                List<SelectListItem> supplierList = Common.CommonFunction.UserSupplierList("").ToList();
                if(str != "0")
                {
                    if (str == "1")
                    {
                        supplierList = supplierList.Where(o => (o.Text.StartsWith("0") || o.Text.StartsWith("1") || o.Text.StartsWith("2")
                        || o.Text.StartsWith("3") || o.Text.StartsWith("4") || o.Text.StartsWith("5") || o.Text.StartsWith("6") || o.Text.StartsWith("7")
                        || o.Text.StartsWith("8") || o.Text.StartsWith("9"))).ToList();


                    }
                    else
                    {
                        supplierList = supplierList.Where(o => o.Text.StartsWith(str)).ToList();
                    }

                    
                    supplierList.Insert(0, new SelectListItem { Text = "Select Supplier", Value = "0" });
                }
                return Json(supplierList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                return null;
            }
        }
    }
}