using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyTree.Data;
using FamilyTree.Services;

namespace FamilyTree.Controllers
{
    public class FamilyAdminController : ApplicationController
    {
        public FamilyAdminController() { }
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
            return View();
        }

        // POST: FamilyAdmin/Create
        [HttpPost]
        public ActionResult AddFamilyName(Family familyName)
        {
            try
            {
                _treeService.AddFamilyName(familyName);
                return RedirectToAction("Families", "Family");
            }
            catch
            {
                return View();
            }
        }

        // GET: FamilyAdmin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FamilyAdmin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
