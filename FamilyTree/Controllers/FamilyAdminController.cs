using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyTree.Data;
using FamilyTree.Services;
using Microsoft.AspNet.Identity;

namespace FamilyTree.Controllers
{
    public class FamilyAdminController : Controller
    {
        private Services.DAO.TreeService _treeService;
        public FamilyAdminController()
        {
            _treeService = new Services.DAO.TreeService();
        }
        // GET: FamilyAdmin
        
        public ActionResult Index()
        {

            return View();
        }

        // GET: FamilyAdmin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FamilyAdmin/Create
        [HttpGet]
        public ActionResult AddFamilyName()
        {
            ViewBag.ownerUserName = User.Identity.Name;
            return View();
        }

        // POST: FamilyAdmin/Create
        [HttpPost]
        public ActionResult AddFamilyName(Family familyObject)
        {
            try
            {
                _treeService.AddFamilyName(familyObject);
                return RedirectToAction("Families", "Family");
            }
            catch
            {
                return View();
            }
        }

        
        //Needs Finishing, doesn't work properly, possibly the get action as doesn't retrieve the information
        // GET: FamilyAdmin/Edit/5
        [HttpGet]
        public ActionResult EditFamilyName(int fid)
        {
            return View(_treeService.GetFamily(fid));
        }

        [HttpPost]
        public ActionResult EditFamilyName(int fid, Family famObject)
        {
            try
            {
                _treeService.EditFamilyName(famObject);
                return RedirectToAction("GetFamily", "Family", fid);
            }
            catch
            {
                return RedirectToAction("Families", "Family");
            }
        }

        // GET: FamilyAdmin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FamilyAdmin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
