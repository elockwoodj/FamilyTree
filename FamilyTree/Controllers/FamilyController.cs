using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyTree.Data;
using FamilyTree.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace FamilyTree.Controllers
{
    public class FamilyController : Controller
    {
        private Services.DAO.TreeService _treeService;
        public FamilyController()
        {
            _treeService = new FamilyTree.Services.DAO.TreeService();
        }

        

        public ActionResult Families()
        {
            string userEmail = User.Identity.GetUserId();
            return View(_treeService.GetFamilies(userEmail));
        }
            
        
        
        public ActionResult AddIndividual()
        {
            return View();
        }

        // POST: Family/Create
        [HttpPost]
        public ActionResult AddIndividual(Individual individual)
        {
            try
            {
                // TODO: Add insert logic here
                _treeService.AddIndividual(individual);
                return RedirectToAction("Family",Families,new { });
            }
            catch
            {
                return View();
            }
        }

        // GET: Family/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Family/Edit/5
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

        // GET: Family/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Family/Delete/5
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
