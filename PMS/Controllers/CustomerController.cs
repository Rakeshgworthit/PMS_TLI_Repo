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
    public class CustomerController : Controller
    {
        // GET: Customer
        private readonly ICustomerRepositor _CustRepo;

        public CustomerController(ICustomerRepositor custRepo)
        {
            _CustRepo = custRepo;
        }

        public ActionResult Index(CustomerViewModel objView)
        {
            if (objView.customersearch == null)
            {
                objView.customersearch = "";
            }
            else {
                objView.customersearch = objView.customersearch;
            }
                
            return View(objView);
        }

        public ActionResult _LoadCustomer(Int32 Id)
        {
            CustomerViewModel objView = new CustomerViewModel();
            if (Id > 0)
            {
                customer objRec = _CustRepo.FindBy(o => o.id == Id).SingleOrDefault();
                if (objRec != null)
                {
                    Common.CommonFunction.MergeObjects(objView, objRec, true);
                }
            }
            else
            {
                objView.isactive = true;
            }
            objView.StatusList = Common.CommonFunction.StatusList();
            objView.GSTRegisterList = Common.CommonFunction.StatusList();
            return View(objView);
        }

        public JsonResult SaveCustomer(CustomerViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            if (objView.id > 0)
            {
                customer objRec = new customer();
               // Int32 alreadyExist = _CustRepo.FindBy(o => o.email == objView.email && o.id != objView.id).Count();
                //if (alreadyExist == 0)
                //{
                    objRec = _CustRepo.FindBy(o => o.id == objView.id).SingleOrDefault();
                    if (objRec != null)
                    {
                        objView.created_by = objRec.created_by;
                        objView.created_date = objRec.created_date;
                    if (objView.name1 != null)
                    {
                        objView.name1 = Convert.ToString(objView.name1).Trim();
                    }
                    if (objView.name2 != null)
                    {
                        objView.name2 = Convert.ToString(objView.name2).Trim();
                    }
                    if (objView.block_no != null)
                    {
                        objView.block_no = Convert.ToString(objView.block_no).Trim();
                    }
                    if (objView.job_site != null)
                    {
                        objView.job_site = Convert.ToString(objView.job_site).Trim();
                    }
                    if (objView.address != null)
                    {
                        objView.address = Convert.ToString(objView.address).Trim();
                    }
                    if (objView.contact_person != null)
                    {
                        objView.contact_person = Convert.ToString(objView.contact_person).Trim();
                    }
                    CommonFunction.MergeObjects(objRec, objView, true);
                        objRec.modified_date = DateTime.Now;
                        objRec.modified_by = new Guid(uid);
                        _CustRepo.Save();
                    }
                    return Json(new { msg = "Customer record updated successfully.", cls = "success", id= objView.id });
                //}else
                //{
                //    return Json(new { msg = "Customer with e-mail " + objView.email + " already exist.", cls = "error", id = objView.id });
                //}
            }
            else
            {
                customer objRec = new customer();
                //Int32 alreadyExist = _CustRepo.FindBy(o => o.email == objView.email).Count();

                //if (alreadyExist == 0)
                //{     
                if (objView.name1 != null)
                {
                    objView.name1 = Convert.ToString(objView.name1).Trim();
                }
                if (objView.name2 != null)
                {
                    objView.name2 = Convert.ToString(objView.name2).Trim();
                }
                if (objView.block_no != null)
                {
                    objView.block_no = Convert.ToString(objView.block_no).Trim();
                }
                if (objView.job_site != null) { 
                    objView.job_site = Convert.ToString(objView.job_site).Trim();
                }
                if (objView.address != null)
                {
                    objView.address = Convert.ToString(objView.address).Trim();
                }
                if(objView.contact_person!=null)
                { 
                    objView.contact_person = Convert.ToString(objView.contact_person).Trim();
                }
                Common.CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.created_date = DateTime.Now;
                    objRec.created_by = new Guid(uid);
                    objRec.modified_date = DateTime.Now;
                    objRec.modified_by = new Guid(uid);
                    _CustRepo.Add(objRec);
                    _CustRepo.Save();
                    return Json(new { msg = "Customer record created successfully.", cls = "success", id=objRec.id });
                //}
                //else
                //{
                //    return Json(new { msg = "Customer with e-mail " + objView.email + " already exist.", cls = "error", id =0 });
                //}
            }

        }

        public ActionResult DeleteById(Int32 Id)
        {
            if (Id > 0)
            {
                customer objRec = _CustRepo.FindBy(o => o.id == Id).SingleOrDefault();
                _CustRepo.Delete(objRec);
                _CustRepo.Save();

            }
            return RedirectToAction("Index");

        }
    }
}