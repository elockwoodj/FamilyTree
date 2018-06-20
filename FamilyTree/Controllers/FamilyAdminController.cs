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
        public ActionResult EditFamilyName(Family famObject)
        {
            try
            {
                _treeService.EditFamilyName(famObject);
                return RedirectToAction("Families", "Family");
            }
            catch
            {
                return RedirectToAction("Families", "Family");
            }
        }
        [HttpGet]
        public ActionResult EditIndividual(int pid)
        {
            return View(_treeService.GetIndividual(pid));
        }
        [HttpPost]
        public ActionResult EditIndividual(Individual individual)
        {
            try
            {
                _treeService.EditIndividual(individual);
                return RedirectToAction("GetIndividuals", "Family", new { fid = individual.familyID });
            }
            catch
            {
                return RedirectToAction("GetIndividuals", "Family", new { fid = individual.familyID });
            }
        }

        [HttpGet]
        public ActionResult AddRelative(int fid, int pid)
        {
            //Assign personID and familyID so they are autopopulated in the view
            ViewBag.personID = pid;
            ViewBag.familyID = fid;
            //List for relativeID
            List<SelectListItem> relativeList = new List<SelectListItem>();

            foreach(var item in _treeService.GetRelatives(pid))
            {
                relativeList.Add(
                    new SelectListItem()
                    {
                        Text = item.fullName,
                        Value = item.relativeID.ToString()
                    });
                    
            };
            ViewBag.relativeList = relativeList;
            //List for Relationship Type
            List<SelectListItem> typeList = new List<SelectListItem>();

            foreach( var item in _treeService.GetTypes())
            {
                typeList.Add(
                    new SelectListItem()
                    {
                        Text = item.typeDescription,
                        Value = item.relationshipTypeID.ToString()
                    }
                );
            }
            ViewBag.typeList = typeList;

            //List for relative Role

            List<SelectListItem> roleList = new List<SelectListItem>();
            
            foreach(var item in _treeService.GetRoles())
            {
                roleList.Add(
                    new SelectListItem()
                    {
                        Text = item.roleDescription,
                        Value = item.relativeRole.ToString()
                    }); 
            }
            ViewBag.roleList = roleList;

            return View();
        }
        [HttpPost]
        public ActionResult AddRelative(Relationship relaObject)
        {
            try
            {
                _treeService.AddRelative(relaObject);
                return RedirectToAction("GetIndividuals", new { fid = relaObject.familyID, controller = "Family" });
            }
            catch
            {
                return View();
            }
        }
    }
}
