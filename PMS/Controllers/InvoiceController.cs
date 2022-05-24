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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using static PMS.Models.InvoiceViewModel;

namespace PMS.Controllers
{
    [Authorize]
    [BranchFilter]
    public class InvoiceController : Controller
    {
        private readonly IInvoice _InvoiceRepo;
        private readonly ISupplierInvoiceRepository _InvoiceRepoSupplier;
        private readonly IDesignerInvoice _InvoiceRepoDesigner;

        public InvoiceController(IInvoice InvRepo, ISupplierInvoiceRepository InvRepoSupp, IDesignerInvoice InvRepoDes)
        {
            _InvoiceRepo = InvRepo;
            _InvoiceRepoSupplier = InvRepoSupp;
            _InvoiceRepoDesigner = InvRepoDes;
        }

        // GET: Invoice

        #region "Invoice"

        public ActionResult Index(InvoiceViewModel objView)
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
                objView.SearchSales_person = Common.CommonFunction.GetSalesmanIdByUser(objView.UID);
                ViewBag.IsAdminLogin = 0;
            }
            DateTime now = DateTime.Now;
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
            objView.BillToList = CommonFunction.CustomerList();
            objView.SalesPersonList = CommonFunction.Active_Salesmen("");
            return View(objView);
        }

        public ActionResult Taxinvoice(InvoiceViewModel objView)
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

            objView.BillToList = CommonFunction.CustomerList();
            objView.SalesPersonList = CommonFunction.AllSalesmen();
            return View(objView);
        }

        public ActionResult LoadAddEdit(Int32 id)
        {
            InvoiceViewModel objView = new InvoiceViewModel();
            objView.ContractTypeList = CommonFunction.ContractTypeList();
            objView.SalesPersonList = CommonFunction.Active_Salesmen("");
            objView.PaymentTermList = CommonFunction.PaymentTermsList();
            objView.BillToList = new List<SelectListItem>();
            objView.CompanyList = CommonFunction.BranchList();
            objView.IsTaxList = CommonFunction.StatusList();

            tbl_invoice objRec = new tbl_invoice();

            List<tbl_invoice_items> InvoiceItemList = new List<tbl_invoice_items>();

            if (id > 0)
            {
                objRec = _InvoiceRepo.FindBy(o => o.Id == id).SingleOrDefault();
                CommonFunction.MergeObjects(objView, objRec);

                objView.BillToList = CommonFunction.SalesmenCustomerList(objRec.Sales_person);
                objView.hdnProjectList = CustomerProjects(objRec.Bill_to);

                using (PMSEntities objDB = new PMSEntities())
                {
                    InvoiceItemList = objDB.tbl_invoice_items.Where(o => o.invoice_id == id).ToList();
                }
            }
            else
            {
                objView.Invoice_date = DateTime.Now;
                objView.hdnProjectList = new List<SelectListItem>();
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
                objView.Sales_person = Common.CommonFunction.GetSalesmanIdByUser(objView.UID);
                ViewBag.IsAdminLogin = 0;
            }
            objView.InvoiceItemList = InvoiceItemList;

            return View(objView);
        }

        public string GetInvoiceNumber(Int32 company_id)
        {
            Int32 totalRec = 0;
            string comp_ini = "";
            using (PMSEntities objDB = new PMSEntities())
            {
                //totalRec = objDB.tbl_invoice.Where(o => o.Company_id == company_id).Count();
                //totalRec = totalRec + 1;
                string getLastInvoiceVal = objDB.tbl_invoice.OrderByDescending(x=>x.Id).FirstOrDefault(o => o.Company_id == company_id)?.Invoice_number;

                if (!string.IsNullOrEmpty(getLastInvoiceVal))
                {
                    getLastInvoiceVal =  getLastInvoiceVal.Substring(getLastInvoiceVal.Length - 4);
                    totalRec = Convert.ToInt32(getLastInvoiceVal) + 1;
                }
                else
                {
                    totalRec = 1;
                }

                comp_ini = objDB.branches.Where(o => o.id == company_id).Select(o => o.company_initial).SingleOrDefault();
            }

            return comp_ini + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year + "-" + totalRec.ToString().PadLeft(4, '0');
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

        public List<SelectListItem> CustomerProjectsWitoutprojectName(Int32 customer_id)
        {
            List<SelectListItem> _list = new List<SelectListItem>();
            _list.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
            using (PMSEntities objDB = new PMSEntities())
            {
                var _listProject = (from p in objDB.projects
                                    where p.customer_id == customer_id
                                    select new SelectListItem
                                    {
                                        Text = p.project_name,
                                        Value = p.id.ToString()
                                    }).OrderBy(o => o.Text).ToList();
                if (_list.Count() > 0)
                { _list.AddRange(_listProject); }
            }
            return _list;
        }


        public JsonResult GetCustomerProjects(Int32 customer_id)
        {
            try
            {
                List<SelectListItem> searchList = new List<SelectListItem>();
                searchList = CustomerProjects(customer_id);
                return Json(searchList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                return null;
            }
        }


        public JsonResult GetCustomerProjectsByName(Int32 customer_id)
        {
            try
            {
                List<SelectListItem> searchList = new List<SelectListItem>();
                searchList = CustomerProjectsWitoutprojectName(customer_id);
                return Json(searchList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                return null;
            }
        }

        public JsonResult SaveUpdate(InvoiceViewModel objView)
        {
            if (!string.IsNullOrEmpty(Request.Params["invoiceobj"]))
            {
                objView = InvoiceViewModel.FromJson(Request.Params["invoiceobj"]);
            }

            string uid = User.Identity.GetUserId();
            tbl_invoice objRec = new tbl_invoice();

            if (objView.Id > 0)
            {
                objRec = _InvoiceRepo.FindBy(o => o.Id == objView.Id).SingleOrDefault();
                if (objRec != null)
                {
                    objView.created_by = objRec.created_by;
                    objView.created_date = objRec.created_date;
                    CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.modification_date = DateTime.Now;
                    objRec.modification_by = new Guid(uid);
                    _InvoiceRepo.Save();

                    using (PMSEntities obj = new PMSEntities())
                    {
                        obj.SSP_Remove_Invoice_Items(Convert.ToInt32(objRec.Id));

                        foreach (var item in objView.invoice_details)
                        {
                            obj.SSP_Add_Invoice_Items(Convert.ToInt32(objRec.Id), Convert.ToString(item.item_description), Convert.ToDecimal(item.item_amount), Convert.ToString(item.Contract_number), Convert.ToInt32(item.project_id));
                        }
                    }
                }

                return Json(new { msg = "Invoice updated successfully.", cls = "success", id = Convert.ToString(objRec.Id) });
            }
            else
            {
                CommonFunction.MergeObjects(objRec, objView, true);
                objRec.created_date = DateTime.Now;
                objRec.created_by = new Guid(uid);
                objRec.modification_date = DateTime.Now;
                objRec.modification_by = new Guid(uid);

                _InvoiceRepo.Add(objRec);
                _InvoiceRepo.Save();

                try
                {
                    using (PMSEntities obj = new PMSEntities())
                    {
                        foreach (var item in objView.invoice_details)
                        {
                            obj.SSP_Add_Invoice_Items(Convert.ToInt32(objRec.Id), Convert.ToString(item.item_description), Convert.ToDecimal(item.item_amount), Convert.ToString(item.Contract_number), Convert.ToInt32(item.project_id));
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                return Json(new { msg = "Invoice created successfully.", cls = "success", id = Convert.ToString(objRec.Id) });
            }
        }

        public ActionResult PrintPreview(Int32 Id)
        {
            PrintPaymentViewModel objViewprint = new PrintPaymentViewModel();
            List<tbl_invoice_items> invItems = new List<tbl_invoice_items>();

            using (PMSEntities obj = new PMSEntities())
            {
                invItems = obj.tbl_invoice_items.Where(o => o.invoice_id == Id).ToList();
                objViewprint.Invoice = obj.SSP_PrintInvoices(Id).SingleOrDefault();
            }

            objViewprint.Invoice_Items = invItems;

            return View(objViewprint);
        }

        public ActionResult DeleteById(Int32 Id)
        {
            decimal tax = 0;
            if (Id > 0)
            {
                tbl_invoice objRec = _InvoiceRepo.FindBy(o => o.Id == Id).SingleOrDefault();
                if (objRec != null)
                {
                    tax = Convert.ToDecimal(objRec.tax);
                    _InvoiceRepo.Delete(objRec);
                    _InvoiceRepo.Save();
                    using (Database.PMSEntities obj = new PMSEntities())
                    {
                        obj.SSP_Remove_Invoice_Items(Id);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public JsonResult GetCustomerBySalesmen(Int32 id)
        {
            try
            {
                List<SelectListItem> searchList = new List<SelectListItem>();
                searchList = CommonFunction.SalesmenCustomerList(id);
                return Json(searchList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                return null;
            }
        }


        public JsonResult GetProjectDescriptionByProject(Int32 project_id)
        {
            try
            {
                string projectDescription = string.Empty;
                using (PMSEntities objDB = new PMSEntities())
                {
                    var project = objDB.projects.Where(i => i.id == project_id).FirstOrDefault();
                    projectDescription = project != null ? project.project_number : string.Empty;

                }
                return Json(projectDescription, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                return null;
            }
        }

        #endregion

        #region "SupplierInvoice"

        public ActionResult SupplierInvoice(InvoiceViewModel objView)
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

            //objView.BillToList = CommonFunction.CustomerList();
            objView.SuppliersList = CommonFunction.UserSupplierList("");
            return View(objView);
        }

        public ActionResult LoadAddEditSupplierInvoice(Int32 id)
        {
            Int32 branchid = SessionManagement.SelectedBranchID;
            SupplierInvoiceViewModel objView = new SupplierInvoiceViewModel();
            //objView.ContractTypeList = CommonFunction.ContractTypeList();
            objView.SalesPersonList = CommonFunction.Active_Salesmen("");
            //objView.PaymentTermList = CommonFunction.PaymentTermsList();
            //objView.BillToList = new List<SelectListItem>();
            //objView.CompanyList = CommonFunction.BranchList();
            //objView.IsTaxList = CommonFunction.StatusList();

            objView.SuppliersList = CommonFunction.UserSupplierList("");

            tbl_supplier_invoice objRec = new tbl_supplier_invoice();
            List<SupplierInvoiceEditList> InvoiceItemEditList = new List<SupplierInvoiceEditList>();
            List<tbl_supplierinvoice_items> InvoiceItemList = new List<tbl_supplierinvoice_items>();

            if (id > 0)
            {
                objRec = _InvoiceRepoSupplier.FindBy(o => o.Id == id).SingleOrDefault();
                CommonFunction.MergeObjects(objView, objRec);
                objView.Invoice_date = objRec.invoice_date;
                //objView.BillToList = CommonFunction.SalesmenCustomerList(objRec.Sales_person);
                //objView.hdnProjectList = CustomerProjects(objRec.Bill_to);

                using (PMSEntities objDB = new PMSEntities())
                {
                    InvoiceItemList = objDB.tbl_supplierinvoice_items.Where(o => o.invoice_id == id).ToList();

                    foreach (var rec in InvoiceItemList)
                    {
                        SupplierInvoiceEditList _item = new SupplierInvoiceEditList();
                        _item.SupplierInvoice = rec;
                        _item.ProjectList = SalespersonProjects(rec.salesperson_id, branchid);
                        InvoiceItemEditList.Add(_item);
                    }
                }
            }
            else
            {
                objView.supplier_id = 0;
                objView.Invoice_date = DateTime.Now;
                //objView.hdnProjectList = new List<SelectListItem>();
            }
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
                ViewBag.SalesmanId = Common.CommonFunction.GetSalesmanIdByUser(objView.UID);
            }
            else
            {
                ViewBag.SalesmanId = 0;
            }
            objView.SupplierInvoiceItemList = InvoiceItemEditList;        

            return View(objView);
        }

        public JsonResult GetSalespersonProjects(Int32 customer_id, Int32 branch_id)
        {
            try
            {
                List<SelectListItem> searchList = new List<SelectListItem>();
                searchList = SalespersonProjects(customer_id, branch_id);
                return Json(searchList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                return null;
            }
        }

        public List<SelectListItem> SalespersonProjects(Int32 customer_id, Int32 branch_id)
        {
            using (PMSEntities objDB = new PMSEntities())
            {
                var _list = (from p in objDB.projects
                             where p.salesmen_id == customer_id && p.branch_id==branch_id && p.status_id != 3 //don't load closed projects
                             select new SelectListItem
                             {
                                 Text = p.project_name + " - " + p.project_number,
                                 Value = p.id.ToString()
                             }).OrderBy(o => o.Text).ToList();
                return _list;
            }
        }

        public JsonResult SaveUpdateSupplierInvoice(string JsonBudget)
        {

            SupplierInvoiceViewModel objView = SupplierInvoiceViewModel.FromJsonNew(JsonBudget);
            //JsonConvert.DeserializeObject<SupplierInvoiceViewModel>(JsonBudget);
            string FilePath = ConfigurationManager.AppSettings["PhysicalPath"].ToString();
            string fname = "";
            string filename = "";
            string extension = string.Empty;
            string fileNameWithExt = string.Empty;
            string filepathRoot = string.Empty;
            int ItemId = 0;
           

            string uid = User.Identity.GetUserId();
            tbl_supplier_invoice objRec = new tbl_supplier_invoice();

            if (objView.Id > 0)
            {
                objRec = _InvoiceRepoSupplier.FindBy(o => o.Id == objView.Id).SingleOrDefault();
                if (objRec != null)
                {
                    objView.created_by = objRec.created_by;
                    objView.creation_date = objRec.creation_date;
                    CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.invoice_date = objView.Invoice_date;
                    objRec.modification_date = DateTime.Now;
                    objRec.modified_by = new Guid(uid);
                    _InvoiceRepoSupplier.Save();

                    using (PMSEntities obj = new PMSEntities())
                    {
                        List<int> list1 = new List<int>();
                        foreach (var update_item in objView.invoice_detailsNew.Where(o => o.Id > 0).ToList())
                        {
                            list1.Add(update_item.Id);
                        }
                        List<int> list2 = new List<int>();
                        foreach (var update_item in obj.tbl_supplierinvoice_items.Where(o => o.invoice_id == objView.Id).ToList())
                        {
                            list2.Add(update_item.Id);
                        }

                        IEnumerable<int> ID = list2.Except(list1);

                        if (ID.Count() > 0)
                        {
                            foreach (var _ID in ID)
                            {
                                int a = obj.SSP_Remove_SupplierInvoice_Items(_ID);

                                if (a > 0)
                                {
                                    foreach (var update_item in objView.invoice_detailsNew.Where(o => o.Id > 0).ToList())
                                    {
                                        tbl_supplierinvoice_items _obj = obj.tbl_supplierinvoice_items.Where(o => o.Id == update_item.Id).SingleOrDefault();

                                        if (_obj != null)
                                        {
                                            _obj.salesperson_id = update_item.salesperson_id;
                                            _obj.project_id = update_item.project_id;
                                            _obj.invoice_number = update_item.invoice_number;
                                            _obj.gst = update_item.gst;
                                            _obj.invoice_amt_without_gst = update_item.invoice_amt_without_gst;
                                            _obj.invoice_amt_with_gst = update_item.invoice_amt_with_gst;
                                            _obj.agreed_amt_without_gst = update_item.agreed_amt_without_gst;
                                            _obj.agreed_amt = update_item.agreed_amt;

                                            obj.SaveChanges();                                           
                                        }
                                        if (update_item.FileName != "")
                                        {
                                            fname = update_item.FileName.Substring(12, (update_item.FileName.Length - 12));
                                            ItemId = update_item.Id;
                                            filename = string.Concat(Path.GetFileNameWithoutExtension(fname), update_item.invoice_number, Path.GetExtension(fname));
                                            string doc_Path = "/UploadDocuments/" + "/ProjectBudget/" + update_item.project_id + "/";
                                            Common.CommonFunction.SaveDocument(filename, update_item.project_id, extension, doc_Path + filename, ItemId);
                                        }
                                    }
                                    foreach (var item in objView.invoice_detailsNew.Where(o => o.Id == 0).ToList())
                                    {
                                        obj.SSP_Add_SupplierInvoice_Items(Convert.ToInt32(objRec.Id), Convert.ToInt32(item.salesperson_id),
                                            Convert.ToInt32(item.project_id), Convert.ToString(item.invoice_number),
                                            Convert.ToDecimal(item.gst), Convert.ToDecimal(item.invoice_amt_without_gst), Convert.ToDecimal(item.invoice_amt_with_gst), Convert.ToDecimal(item.agreed_amt_without_gst),
                                            Convert.ToDecimal(item.agreed_amt));
                                        if (item.FileName != "")
                                        {
                                            fname = item.FileName.Substring(12, (item.FileName.Length - 12));
                                            ItemId = Common.CommonFunction.GetSupplierInvoiceItemId(item.Id, item.project_id, item.salesperson_id, item.invoice_number);
                                            filename = string.Concat(Path.GetFileNameWithoutExtension(fname), item.invoice_number, Path.GetExtension(fname));
                                            string doc_Path = "/UploadDocuments/" + "/ProjectBudget/" + item.project_id + "/";
                                            Common.CommonFunction.SaveDocument(filename, item.project_id, extension, doc_Path + filename, ItemId);
                                        }
                                    }
                                }
                                else
                                {
                                    return Json(new { msg = "Supplier Invoice Alreday have in Supplier Payment.", cls = "Error", id = Convert.ToString(objRec.Id) });
                                }

                            }
                        }
                        else
                        {
                            foreach (var update_item in objView.invoice_detailsNew.Where(o => o.Id > 0).ToList())
                            {
                                tbl_supplierinvoice_items _obj = obj.tbl_supplierinvoice_items.Where(o => o.Id == update_item.Id).SingleOrDefault();

                                if (_obj != null)
                                {
                                    _obj.salesperson_id = update_item.salesperson_id;
                                    _obj.project_id = update_item.project_id;
                                    _obj.invoice_number = update_item.invoice_number;
                                    _obj.gst = update_item.gst;
                                    _obj.invoice_amt_without_gst = update_item.invoice_amt_without_gst;
                                    _obj.invoice_amt_with_gst = update_item.invoice_amt_with_gst;
                                    _obj.agreed_amt_without_gst = update_item.agreed_amt_without_gst;
                                    _obj.agreed_amt = update_item.agreed_amt;

                                    obj.SaveChanges();
                                }
                                if (update_item.FileName != "" )
                                {
                                    fname = update_item.FileName.Substring(12, (update_item.FileName.Length - 12));
                                    ItemId = update_item.Id;
                                    filename = string.Concat(Path.GetFileNameWithoutExtension(fname), update_item.invoice_number, Path.GetExtension(fname));
                                    string doc_Path = "/UploadDocuments/" + "/ProjectBudget/" + update_item.project_id + "/";
                                    Common.CommonFunction.SaveDocument(filename, update_item.project_id, extension, doc_Path + filename, ItemId);
                                }
                            }
                            foreach (var item in objView.invoice_detailsNew.Where(o => o.Id == 0).ToList())
                            {
                                obj.SSP_Add_SupplierInvoice_Items(Convert.ToInt32(objRec.Id), Convert.ToInt32(item.salesperson_id),
                                    Convert.ToInt32(item.project_id), Convert.ToString(item.invoice_number),
                                    Convert.ToDecimal(item.gst), Convert.ToDecimal(item.invoice_amt_without_gst), Convert.ToDecimal(item.invoice_amt_with_gst), Convert.ToDecimal(item.agreed_amt_without_gst),
                                    Convert.ToDecimal(item.agreed_amt));

                                if (item.FileName != "")
                                {
                                    fname = item.FileName.Substring(12, (item.FileName.Length - 12));
                                    ItemId = Common.CommonFunction.GetSupplierInvoiceItemId(Convert.ToInt32(objRec.Id), item.project_id, item.salesperson_id, item.invoice_number);
                                    filename = string.Concat(Path.GetFileNameWithoutExtension(fname), item.invoice_number, Path.GetExtension(fname));
                                    string doc_Path = "/UploadDocuments/" + "/ProjectBudget/" + item.project_id + "/";
                                    Common.CommonFunction.SaveDocument(filename, item.project_id, extension, doc_Path + filename, ItemId);
                                }
                            }
                        }
                    }
                }

                return Json(new { msg = "Supplier Invoice updated successfully.", cls = "success", id = Convert.ToString(objRec.Id) });
            }
            else
            {
                CommonFunction.MergeObjects(objRec, objView, true);
                objRec.invoice_date = objView.Invoice_date;
                objRec.creation_date = DateTime.Now;
                objRec.created_by = new Guid(uid);
                objRec.modification_date = DateTime.Now;
                objRec.modified_by = new Guid(uid);

                _InvoiceRepoSupplier.Add(objRec);
                _InvoiceRepoSupplier.Save();

                using (PMSEntities obj = new PMSEntities())
                {
                    foreach (var item in objView.invoice_detailsNew)
                    {
                        obj.SSP_Add_SupplierInvoice_Items(Convert.ToInt32(objRec.Id), Convert.ToInt32(item.salesperson_id),
                               Convert.ToInt32(item.project_id), Convert.ToString(item.invoice_number),
                               Convert.ToDecimal(item.gst), Convert.ToDecimal(item.invoice_amt_without_gst), Convert.ToDecimal(item.invoice_amt_with_gst), Convert.ToDecimal(item.agreed_amt_without_gst), Convert.ToDecimal(item.agreed_amt));

                       
                        fname = item.FileName.Substring(12, (item.FileName.Length-12));                        
                        ItemId = Common.CommonFunction.GetSupplierInvoiceItemId(Convert.ToInt32(objRec.Id), item.project_id, item.salesperson_id, item.invoice_number);
                        filename = string.Concat(Path.GetFileNameWithoutExtension(fname), item.invoice_number, Path.GetExtension(fname));
                        string doc_Path = "/UploadDocuments/" + "/ProjectBudget/" + item.project_id + "/";
                        Common.CommonFunction.SaveDocument(filename, item.project_id, extension, doc_Path + filename, ItemId);

                    }
                }

                return Json(new { msg = "Supplier Invoice created successfully.", cls = "success", id = Convert.ToString(objRec.Id) });
            }
        }
        public JsonResult UploadDocument(string ProjectId,string InvoiceNo)
        {
            string FilePath = ConfigurationManager.AppSettings["PhysicalPath"].ToString();
            string fname = "";
            string filename = "";
            string extension = string.Empty;
            string fileNameWithExt = string.Empty;
            string filepathRoot = string.Empty;
            try
            {
                //SupplierInvoiceViewModel objView = SupplierInvoiceViewModel.FromJsonNew(JsonBudget);
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
                    string fileName = file.FileName;
                    fileNameWithExt = fileName;
                    string doc_Path = "/UploadDocuments/" + "/ProjectBudget/" + ProjectId + "/";
                    string dirPath = FilePath + doc_Path;

                    string UploadPath = "~/UploadDocuments/ProjectBudget/" + ProjectId;
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    string addTimeStampToFileName = string.Empty;
                    //if (file.ContentLength == 0)
                    //   continue;
                    if (file.ContentLength > 0)
                    {
                        addTimeStampToFileName = string.Concat(Path.GetFileNameWithoutExtension(fileName), InvoiceNo, Path.GetExtension(file.FileName));
                        filepathRoot = Path.Combine(HttpContext.Request.MapPath(UploadPath), addTimeStampToFileName);
                        extension = Path.GetExtension(file.FileName);
                        file.SaveAs(filepathRoot);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return Json(new { msg = "Document Uploaded Successfully", cls = "success" });
        }
        public ActionResult Delete_SupplierInvoiceById(Int32 Id)
        {
            if (Id > 0)
            {
                tbl_supplier_invoice objRec = _InvoiceRepoSupplier.FindBy(o => o.Id == Id).SingleOrDefault();
                if (objRec != null)
                {
                   //_InvoiceRepoSupplier.Delete(objRec);
                   //_InvoiceRepoSupplier.Save();
                    using (Database.PMSEntities obj = new PMSEntities())
                    {
                        int a = obj.SSP_CHECK_SUPPLIER_PAYMENT(Id);
                    }
                }
            }

            return RedirectToAction("SupplierInvoice");
        }

        #endregion

        #region Designer Invoice

        public ActionResult DesignerInvoice(InvoiceViewModel objView)
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

        public JsonResult SaveUpdateDesignerInvoice(InvoiceViewModel objView)
        {
            if (!string.IsNullOrEmpty(Request.Params["invoiceobj"]))
            {
                objView = InvoiceViewModel.DesignerInvoiceFromJson(Request.Params["invoiceobj"]);
            }

            string uid = User.Identity.GetUserId();
            tbl_designer_invoice objRec = new tbl_designer_invoice();

            if (objView.Id > 0)
            {
                objRec = _InvoiceRepoDesigner.FindBy(o => o.Id == objView.Id).SingleOrDefault();
                if (objRec != null)
                {
                    objView.created_by = objRec.created_by;
                    objView.created_date = objRec.created_date;
                    CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.modification_date = DateTime.Now;
                    objRec.modification_by = new Guid(uid);
                    _InvoiceRepoDesigner.Save();

                    using (PMSEntities obj = new PMSEntities())
                    {
                        obj.SSP_Remove_DesignerInvoice_Items(Convert.ToInt32(objRec.Id));

                        foreach (var item in objView.invoice_details)
                        {
                            obj.SSP_Add_DesignerInvoice_Items(Convert.ToInt32(objRec.Id), Convert.ToString(item.item_description), Convert.ToDecimal(item.item_amount), Convert.ToString(item.Contract_number));
                        }
                    }
                }

                return Json(new { msg = "Designer Invoice updated successfully.", cls = "success", id = Convert.ToString(objRec.Id) });
            }
            else
            {
                CommonFunction.MergeObjects(objRec, objView, true);
                objRec.created_date = DateTime.Now;
                objRec.created_by = new Guid(uid);
                objRec.modification_date = DateTime.Now;
                objRec.modification_by = new Guid(uid);

                _InvoiceRepoDesigner.Add(objRec);
                _InvoiceRepoDesigner.Save();

                try
                {
                    using (PMSEntities obj = new PMSEntities())
                    {
                        foreach (var item in objView.invoice_details)
                        {
                            obj.SSP_Add_DesignerInvoice_Items(Convert.ToInt32(objRec.Id), Convert.ToString(item.item_description), Convert.ToDecimal(item.item_amount), Convert.ToString(item.Contract_number));
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                return Json(new { msg = "Designer Invoice created successfully.", cls = "success", id = Convert.ToString(objRec.Id) });
            }
        }

        public ActionResult LoadAddEditDesignerInvoice(Int32 id)
        {
            InvoiceViewModel objView = new InvoiceViewModel();
            objView.ContractTypeList = CommonFunction.ContractTypeList();
            objView.PaymentTermList = CommonFunction.PaymentTermsList();
            objView.CompanyList = CommonFunction.BranchList();
            objView.IsTaxList = CommonFunction.StatusList();

            tbl_designer_invoice objRec = new tbl_designer_invoice();

            List<tbl_designerinvoice_items> DesignerInvoiceItemList = new List<tbl_designerinvoice_items>();

            if (id > 0)
            {
                objRec = _InvoiceRepoDesigner.FindBy(o => o.Id == id).SingleOrDefault();
                CommonFunction.MergeObjects(objView, objRec);

                //objView.BillToList = CommonFunction.GetDesignerList();

                using (PMSEntities objDB = new PMSEntities())
                {
                    DesignerInvoiceItemList = objDB.tbl_designerinvoice_items.Where(o => o.invoice_id == id).ToList();
                }
            }
            else
            {
                //objView.BillToList = CommonFunction.GetDesignerList();
                objView.Invoice_date = DateTime.Now;
            }

            objView.DesignerInvoiceItemList = DesignerInvoiceItemList;

            return View(objView);
        }

        public ActionResult Delete_DesignerInvoiceById(Int32 Id)
        {
            if (Id > 0)
            {
                tbl_designer_invoice objRec = _InvoiceRepoDesigner.FindBy(o => o.Id == Id).SingleOrDefault();
                if (objRec != null)
                {
                    _InvoiceRepoDesigner.Delete(objRec);
                    _InvoiceRepoDesigner.Save();
                    using (Database.PMSEntities obj = new PMSEntities())
                    {
                        obj.SSP_Remove_DesignerInvoice_Items(Id);
                    }
                }
            }

            return RedirectToAction("DesignerInvoice");
        }

        public string GetDesignerInvoiceNumber(Int32 company_id)
        {
            Int32 totalRec = 0;
            string comp_ini = "";
            using (PMSEntities objDB = new PMSEntities())
            {
                //totalRec = objDB.tbl_designer_invoice.Where(o => o.Company_id == company_id).Count();
                //totalRec = totalRec + 1;
                string getLastInvoiceVal = objDB.tbl_designer_invoice.OrderByDescending(x => x.Id).FirstOrDefault(o => o.Company_id == company_id)?.Invoice_number;

                if (!string.IsNullOrEmpty(getLastInvoiceVal))
                {
                    getLastInvoiceVal = getLastInvoiceVal.Substring(getLastInvoiceVal.Length - 4);
                    totalRec = Convert.ToInt32(getLastInvoiceVal) + 1;
                }
                else
                {
                    totalRec = 1;
                }
                comp_ini = objDB.branches.Where(o => o.id == company_id).Select(o => o.company_initial).SingleOrDefault();
            }

            return comp_ini + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year + "-" + totalRec.ToString().PadLeft(4, '0');
        }


        public ActionResult DesignerInvoicePrintPreview(Int32 Id)
        {
            PrintPaymentViewModel objViewprint = new PrintPaymentViewModel();
            List<tbl_designerinvoice_items> invItems = new List<tbl_designerinvoice_items>();

            using (PMSEntities obj = new PMSEntities())
            {
                invItems = obj.tbl_designerinvoice_items.Where(o => o.invoice_id == Id).ToList();
                objViewprint.Designer_Invoice = obj.SSP_Print_DesignerInvoices(Id).SingleOrDefault();
            }

            objViewprint.Designer_Invoice_Items = invItems;

            return View(objViewprint);
        }
        #endregion

        #region Verification

        public ActionResult LoadApproval(Int32 Id)
        {
            InvoiceApprovalInfoViewModel objView = new InvoiceApprovalInfoViewModel();
            List<ApprovalInfoList> objPDList = new List<ApprovalInfoList>();
            decimal AppAmount = 0;
            string Remarks = "";
            decimal totalAmount = 0;

            SqlCommand Cmd;
            try
            {
                using (var Conn = Common.CommonFunction.GetConnection())
                {
                    Cmd = new SqlCommand("SSP_GETApprovalInfo", Conn);
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("@InvoiceId", Id);
                    IDataReader Ireader = Cmd.ExecuteReader();
                    while (Ireader.Read())
                    {
                        ApprovalInfoList _GetApprovalItem = new ApprovalInfoList();
                        _GetApprovalItem.InvoiceId = Ireader.GetInt32(0);
                        _GetApprovalItem.Supplier_Id = Ireader.GetInt32(1);
                        _GetApprovalItem.InvoiceDetailId = Ireader.GetInt32(2);
                        _GetApprovalItem.invoice_amt_without_gst = Ireader.GetDecimal(3);
                        _GetApprovalItem.agreed_amt_without_gst = Ireader.GetDecimal(4);
                        _GetApprovalItem.invoice_amt_with_gst = Ireader.GetDecimal(5);
                        _GetApprovalItem.Agreed_Amt = Ireader.GetDecimal(6);
                        _GetApprovalItem.InvoiceAmount = Ireader.GetDecimal(7);
                        _GetApprovalItem.ApprovedAmount = Ireader.GetDecimal(8);
                        _GetApprovalItem.InvoiceAmountAftGst = Ireader.GetDecimal(9);
                        _GetApprovalItem.ApprovedAmtAftGst = Ireader.GetDecimal(10);
                        _GetApprovalItem.ProjectNumber = Ireader.GetString(11);
                        _GetApprovalItem.Salesman = Ireader.GetString(12);
                        _GetApprovalItem.ApprovedRemarks = Ireader.GetString(13);
                        _GetApprovalItem.ProjectId = Convert.ToInt64(Ireader.GetInt32(14));
                        _GetApprovalItem.documentPath = Ireader.GetString(15);
                        AppAmount = _GetApprovalItem.ApprovedAmtAftGst;
                        Remarks = _GetApprovalItem.ApprovedRemarks;
                        totalAmount = _GetApprovalItem.InvoiceAmountAftGst;
                        objPDList.Add(_GetApprovalItem);
                    }
                    Common.CommonFunction.CloseConnection(Conn);
                }
                if (objView == null)
                {
                    objView.Id = Id;
                }
                else
                {
                    objView.ApprovalInfoList = objPDList;
                    objView.TotalAmount = totalAmount;
                    objView.ApprovedAmount = AppAmount;
                    objView.ApprovedRemarks = Remarks;
                    objView.Id = Id;

                }

                return View(objView);
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

        public JsonResult UpdateApprovedStatus(string InvoiceId, decimal Amount, string Remarks)
        {
            SqlCommand Cmd;
            try
            {
                using (var Conn = Common.CommonFunction.GetConnection())
                {
                    Cmd = new SqlCommand("SSP_UpdateApprovalAmount", Conn);
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("@Id", InvoiceId);
                    Cmd.Parameters.AddWithValue("@Amount", Amount);
                    Cmd.Parameters.AddWithValue("@Remarks", Remarks);
                    Cmd.ExecuteNonQuery();
                        
                }
                return Json(new { msg = "Receipt Approved successfully.", cls = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { msg = "Approved Failed.", cls = "error" });
            }

        }

        public JsonResult GetFilePathByProjectBudgetDetailsId(Int64 ProjectId, int ProjectBudgetDetailsId)
        {
            string path = string.Empty;
            try
            {
                long projectId = ProjectId;
                List<GetDocumentdetails> getDocs = Get_DocumentsForProjectBudget(ProjectBudgetDetailsId);
                if (getDocs.Count > 0)
                {
                    foreach (var item in getDocs)
                    {
                        path = item.document_path;
                    }
                }
                else
                {
                    path = string.Empty;
                }

                //path = _IProjectsBudget.GetFilePathById(ProjectId);
            }
            catch (Exception ex)
            {
                //ExceptionLog.WriteLog(ex, "Method Name: GetFilePathById");
                throw ex; 
            }
            finally
            {

            }
            return Json(new { data = path }, JsonRequestBehavior.AllowGet);
        }

        public List<GetDocumentdetails> Get_DocumentsForProjectBudget(int ProjectBudgetDetailsId)
        {
            List<InvoiceViewModel> _GetDocumentList;
            List<GetDocumentdetails> _GetDocumentdetails = new List<GetDocumentdetails>();

            try
            {

                _GetDocumentList = new List<InvoiceViewModel>();
                _GetDocumentList = Get_DocumentsForProjectBudgetDetails(ProjectBudgetDetailsId);
                if (_GetDocumentList != null && _GetDocumentList.Count > 0)
                {
                    for (int i = 0; _GetDocumentList.Count > i; i++)
                    {
                        if (_GetDocumentList[i].doc_id == 11)
                        {
                            GetDocumentdetails _GetDocumentpath = new GetDocumentdetails();
                            _GetDocumentpath.document_path = _GetDocumentList[i].document_path;
                            _GetDocumentdetails.Add(_GetDocumentpath);
                        }

                    }

                }
                else
                {
                }

            }
            catch (Exception ex)
            {
                //ExceptionLog.WriteLog(ex, "Method Name: Get_Documents");
                throw ex;
            }
            finally
            {
                _GetDocumentList = null;
            }
            return _GetDocumentdetails;
        }

        public List<InvoiceViewModel> Get_DocumentsForProjectBudgetDetails(int SupplierInvoiceItemId)
        {
            List<InvoiceViewModel> obj = new List<InvoiceViewModel>();
            try
            {
                using (var Conn = Common.CommonFunction.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("Get_Documents_For_Project_Budget", Conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PROJECT_BUDGET_DETAILS_ID", SupplierInvoiceItemId);
                    IDataReader Ireader = cmd.ExecuteReader();
                    while (Ireader.Read())
                    {
                        InvoiceViewModel _GetQuotationDeatils = new InvoiceViewModel();
                        _GetQuotationDeatils.doc_id = Ireader.GetInt16(0);
                        _GetQuotationDeatils.document_path = Ireader.GetString(1);
                        _GetQuotationDeatils.id_type = Ireader.GetInt16(2);
                        _GetQuotationDeatils.project_number = Ireader.GetString(3);
                        obj.Add(_GetQuotationDeatils);
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                //ExceptionLog.WriteLog(ex, "Method Name: Get_DocumentsForProjectBudget Parameters= " + ProjectBudgetDetailsId);
                throw ex;
            }
            finally
            {
                obj = null;
            }
        }


        #endregion
    }
}