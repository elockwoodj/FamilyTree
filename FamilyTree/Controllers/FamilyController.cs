﻿using System;
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
            //ViewBag.ownerUserName = User.Identity.Name;
            var uid = User.Identity.Name;
            return View(_treeService.GetFamilies(uid));
        }

        public ActionResult GetFamily(int fid)
        {
            return View(_treeService.GetFamily(fid));
        }
        //[HttpGet]
        //public ActionResult AddFamilyName()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult AddFamilyName(Family familyName)
        //{
        //    try
        //    {
        //        _treeService.AddFamilyName(familyName);
        //        return RedirectToAction("Families");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        public ActionResult GetIndividuals(int fid)
        {
            return View(_treeService.GetIndividuals(fid));
        }
        
        public ActionResult GetRelationships(int fid)
        {
            return View(_treeService.GetRelationships(fid));
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
                return RedirectToAction("Families", "Family", new { User.Identity.Name });
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
