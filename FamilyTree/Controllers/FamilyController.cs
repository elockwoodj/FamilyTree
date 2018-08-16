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

        
        // Loads all Families up the user has created
        public ActionResult Families()
        {
            var uid = User.Identity.Name;
            ViewBag.ownerUserName = User.Identity.Name;
            return View(_treeService.GetFamilies(uid));
        }

        // Loads all families the user has been linked to and returns a partial view
        public ActionResult _Families()
        {
            var uid = User.Identity.Name;
            return PartialView(_treeService.GetLinkFamilies(uid));
        }

        // Selects the specified family from the database
        public ActionResult GetFamily(int fid)
        {
            return View(_treeService.GetFamily(fid));
        }

        // Selects the specified individual from the database
        public ActionResult GetIndividual(int pid)
        {
            return View(_treeService.GetIndividual(pid));
        }

        // Selects all individuals from the specified family id
        public ActionResult GetIndividuals(int fid)
        {
            // place that family id in the viewbag for use within the view
            ViewBag.familyID = fid;
            return View(_treeService.GetIndividuals(fid));
        }

        // Gathers all information on an individual within the database
        public ActionResult ReportIndividual (int pid)
        {
            Individual person = _treeService.GetIndividual(pid);

            // Places information into viewbags for use within the view
            ViewBag.fullName = person.fullName;
            ViewBag.familyID = person.familyID;

            return View(_treeService.GetIndividual(pid));
        }



        // view for Linked users
        public ActionResult GetLinkIndividuals(int fid)
        {
            ViewBag.familyID = fid;
            return View(_treeService.GetIndividuals(fid));
        }

        // Select all usernames that are linked to a familyID
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

        // Get all relatives related to a specified individual
        public ActionResult GetRelatives(int pid)
        {
            // Grab information on the individual for usage inside the view 
            var familyIDSelect = _treeService.GetIndividual(pid);
            ViewBag.personID = pid;
            ViewBag.familyID = familyIDSelect.familyID;
            return View(_treeService.GetRelatives(pid));
        }

        
        [HttpGet]
        public ActionResult AddIndividual(int fid)
        {
                // create a list for genders for usage inside the view
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

        // Add the individual to the individuals table
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

        // Delete a specified family name 
        [HttpGet]
        public ActionResult DeleteFamilyName(int fid)
        {
            return View(_treeService.GetFamily(fid));
        }
        [HttpPost]
        public ActionResult DeleteFamilyName(int fid, Family famObject)
        {
            //Try catch loop to ensure correct information is deleted and doesn't crash application
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

        // Delete Specified linked user
        [HttpGet]
        public ActionResult DeleteLinkedUser(int lid)
        {
            return View(_treeService.GetUserLink(lid));
        }
        public ActionResult DeleteLinkedUser(int lid, UserLink linkObject)
        {
            //Try catch loop to ensure correct information is deleted and doesn't crash application
            try
            {
                UserLink _linkObject = _treeService.GetUserLink(lid);
                _treeService.DeleteLinkedUser(_linkObject);
                return RedirectToAction("GetLinkedUsers", new { fid = linkObject.familyID });
            }
            catch
            {
                return View();
            }
        }

        // Delete specified individual
        [HttpGet]
        public ActionResult DeleteIndividual(int pid, int fid)
        {
            //Store family id for usage in the view
            ViewBag.familyID = fid;
            return View(_treeService.GetIndividual(pid));
        }
        [HttpPost]
        public ActionResult DeleteIndividual(int pid, Individual indObject)
        {
            // Try catch loop to ensure correct information is deletd and doesn't crash application
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

        // Delete specified relative 
        [HttpGet]
        public ActionResult DeleteRelative(int rid)
        {
            //Stores information about relative for usage within the view
            ViewBag.personID = _treeService.GetRelationship(rid).personID;
            ViewBag.relationshipID = rid;
            return View(_treeService.GetRelationship(rid));
        }
        [HttpPost]
        public ActionResult DeleteRelative(int rid, Relationship relObject)
        {
            //try catch loop implemented to ensure correct information is deleted 
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
    }
}
