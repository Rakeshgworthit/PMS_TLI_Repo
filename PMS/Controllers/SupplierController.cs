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
    public class SupplierController : Controller
    {
        private readonly ISupplierRepositor  _SuppRepo;

        public SupplierController(ISupplierRepositor suppRepo)
        {
            _SuppRepo = suppRepo;
        }

        // Supplier Listing
        public ActionResult Index()
        {
            return View();
        }

        // Open Supplier Add/Edit
        public ActionResult LoadAddEdit(Int32? Id=0)
        {
           
            SupplierViewModel objView = new SupplierViewModel();
            if (Id > 0)
            {
                supplier objRec = _SuppRepo.FindBy(o => o.id == Id).SingleOrDefault();
                if (objRec != null)
                {
                    Common.CommonFunction.MergeObjects(objView, objRec, true);
                }
            } else
            {
                objView.isactive = true;
            }
            objView.StatusList = Common.CommonFunction.StatusList();
            
            objView.GSTRegisteredList = Common.CommonFunction.GSTRegistered();
            return View(objView);
        }

        public ActionResult SaveUpdate(SupplierViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            if (objView.id > 0)
            {
                supplier objRec = _SuppRepo.FindBy(i => i.id == objView.id).SingleOrDefault();
                objView.created_date = objRec.created_date;
                objView.created_by = objRec.created_by;
                Common.CommonFunction.MergeObjects(objRec, objView, true);
                objRec.modified_date = DateTime.Now;
                objRec.modified_by = new Guid(uid);
                _SuppRepo.Save();
                return Json(new { msg = "Supplier record updated successfully.", cls = "success", id=objView.id });

            } else {
                supplier objRec = new supplier();
                Common.CommonFunction.MergeObjects(objRec, objView, true);
                 objRec.created_date = DateTime.Now;
                objRec.created_by = new Guid(uid);
                objRec.modified_date = DateTime.Now;
                objRec.modified_by = new Guid(uid);
                _SuppRepo.Add(objRec);
                _SuppRepo.Save();
                return Json(new { msg = "Supplier record created successfully.", cls = "success", id = objRec.id });
            }          
        }

        public ActionResult DeleteById(Int32 Id)
        {
            if (Id > 0)
            {
                supplier objRec = _SuppRepo.FindBy(o => o.id == Id).SingleOrDefault();
                _SuppRepo.Delete(objRec);
                _SuppRepo.Save();

            }
            return RedirectToAction("Index");

        }
    }
}