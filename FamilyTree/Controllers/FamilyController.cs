using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyTree.Data;
using FamilyTree.Data.BEANS;
using FamilyTree.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Drawing;


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

        public ActionResult _Families()
        {
            var uid = User.Identity.Name;
            return PartialView(_treeService.GetLinkFamilies(uid));
        }

        public ActionResult GetFamily(int fid)
        {
            return View(_treeService.GetFamily(fid));
        }
        public ActionResult GetIndividual(int pid)
        {
            return View(_treeService.GetIndividual(pid));
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

            //VVV
        public ActionResult GetIndividuals(int fid)
        {
            ViewBag.familyID = fid;
            return View(_treeService.GetIndividuals(fid));
        }
        //Partial view for Linked users
        public ActionResult GetLinkIndividuals(int fid)
        {
            ViewBag.familyID = fid;
            return View(_treeService.GetIndividuals(fid));
        }

        public ActionResult GetLinkedUsers(int fid)
        {
            ViewBag.familyID = fid;
            var uid = User.Identity.Name;
            return View(_treeService.GetLinkList(uid, fid));
        }
        //VVV
        public ActionResult _GetRelationships(int fid)
        {
            return PartialView(_treeService.GetRelatives(fid));
        }

        public ActionResult GetRelatives(int pid)
        {
            var familyIDSelect = _treeService.GetIndividual(pid);
            ViewBag.personID = pid;
            ViewBag.familyID = familyIDSelect.familyID;
            return View(_treeService.GetRelatives(pid));
        }

        //public ActionResult GetFamily(int fid)
        //{
        //    return View(_treeService.GetFamilyMembers(fid));
        //}

        [HttpGet]
        public ActionResult AddIndividual(int fid)
        {
                            
                List<SelectListItem> genderList = new List<SelectListItem>();
                genderList.Add(new SelectListItem
                {
                    Text = "Male",
                    Value = "Male"
                });
                genderList.Add(new SelectListItem
                {
                    Text = "Female",
                    Value = "Female"
                });
            ViewBag.genderList = genderList;
            ViewBag.familyID = fid;
            return View();
        }

        // POST: Family/Create
        [HttpPost]
        public ActionResult AddIndividual(int fid, Individual individual)
        {
            try
            {
                // TODO: Add insert logic here
                _treeService.AddIndividual(individual);
                return RedirectToAction("GetIndividuals", "Family", new { fid = fid });
            }
            catch
            {
                return View();
            }
        }

        // POST: Family/Delete/
        [HttpGet]
        public ActionResult DeleteFamilyName(int fid)
        {
            return View(_treeService.GetFamily(fid));
        }
        // POST: Family/Delete/
        [HttpPost]
        public ActionResult DeleteFamilyName(int fid, Family famObject)
        {
            try
            {
                Family _famObject = _treeService.GetFamily(fid);
                _treeService.DeleteFamilyName(_famObject);
                return RedirectToAction("Families", new { uid = User.Identity.Name, Controller = "Family" });
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult DeleteLinkedUser(int lid)
        {
            return View(_treeService.GetUserLink(lid));
        }
        public ActionResult DeleteLinkedUser(int lid, UserLink linkObject)
        {
            try
            {
                UserLink _linkObeject = _treeService.GetUserLink(lid);
                _treeService.DeleteLinkedUser(_linkObeject);
                return RedirectToAction("GetLinkedUsers", new { fid = linkObject.familyID });
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult DeleteIndividual(int pid)
        {
            return View(_treeService.GetIndividual(pid));
        }

        [HttpPost]
        public ActionResult DeleteIndividual(int pid, Individual indObject)
        {
            try
            {
                Individual _indObject = _treeService.GetIndividual(pid);
                _treeService.DeleteIndividual(_indObject);
                return RedirectToAction("GetIndividuals", new { fid = _indObject.familyID, Controller = "Family" });
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult DeleteRelative(int rid)
        {
            ViewBag.personID = _treeService.GetRelationship(rid).personID;
            ViewBag.relationshipID = rid;
            return View(_treeService.GetRelationship(rid));
        }
        [HttpPost]
        public ActionResult DeleteRelative(int rid, Relationship relObject)
        {
            try
            {
                Relationship _relObject = _treeService.GetRelDelete(rid);
                _treeService.DeleteRelative(_relObject);
                return RedirectToAction("GetRelatives", new { pid = _relObject.personID, controller = "Family" });
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
    }
}
