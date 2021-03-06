﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyTree.Data;
using FamilyTree.Data.BEANS;
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

        //Adds the current users username to the viewbag to be used in view
        [HttpGet]
        public ActionResult AddFamilyName()
        {
            ViewBag.ownerUserName = User.Identity.Name;
            return View();
        }
        // Adds a Family Name to the Family Table
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

        // Adds familyID and the owners user name to ViewBag to be accessed in view
        [HttpGet]
        public ActionResult AddLink(int fid)
        {
            ViewBag.familyID = fid;
            ViewBag.ownerUserName = User.Identity.Name;
            return View();
        }
        // Adds a linked user to the database
        [HttpPost]
        public ActionResult AddLink(UserLink otherUser)
        {
            try
            {
                _treeService.AddLink(otherUser);
                return RedirectToAction("GetIndividuals", "Family", new { fid = otherUser.familyID });
            }
            catch
            {
                return View();
            }
        }
        
        
        // Retrieves Family Object from database and edits the information before redirecting to the Families action
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

        // Retrieves Individual Object from database and edits the information before redirecting to the GetIndividuals action
        [HttpGet]
        public ActionResult EditIndividual(int pid)
        {
            ViewBag.familyID = _treeService.GetIndividual(pid).familyID;
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

            foreach(var item in _treeService.GetListForRelatives(fid , pid))
            {
                relativeList.Add(
                    new SelectListItem()
                    {
                        Text = item.fullName,
                        Value = item.relativeID.ToString()
                    });                    
            };
            ViewBag.relativeList = relativeList;
            //List for Relationship Type, used in view
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
            //List for relative Role, used in the view
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
        public ActionResult AddRelative(Relationship relaObject) //This will add a relationship from a person to a relative and a from that relative to the person
        {
            try
            {
                _treeService.AddRelative(relaObject);  //Add the intended relationship
                
                var rela = _treeService.GetRelative(relaObject.relationshipID);
                var gender = _treeService.GetRelativeGender(relaObject.personID);
                var pid = relaObject.personID; //Stores personID to rewrite for inverse
                var person = _treeService.GetIndividual(pid);
                var rid = relaObject.relativeID; //Stores relativeID to rewrite for inverse

                
                switch (rela.relationshipTypeID)
                {
                    case 1: //Inserting a Sibling, therefore you are also a sibling 
                        break;
                    case 2: //Inserting a Parent, therefore you are a child
                        rela.relationshipTypeID = 3;
                        break;
                    case 3: //Inserting a Child, therefore you are a parent
                        rela.relationshipTypeID = 2;
                        //Inserting a child means the number of children column needs to increase in couple
                        break;
                    case 4: //Inserting a Marriage, therefore you are married
                        break;
                    default:
                        break;
                } //Changes the relationship type for inverse, ie adding a Parent will cause the Parent to have a Child relationship added

                if (gender == "Male")
                {
                    switch (rela.relativeRole)
                    {
                        case 1: //Inserting a Brother and you are Male
                            break;
                        case 2: //Inserting a Sister and you are Male
                            rela.relativeRole = 1;
                            break;
                        case 3: //Inserting a Father and you are Male
                            rela.relativeRole = 5;
                            break;
                        case 4: //Inserting a Mother and you are Male
                            rela.relativeRole = 5;
                            break;
                        case 5: //Inserting a Child and you are Male
                            rela.relativeRole = 3;
                            break;
                        case 6: //Inserting a Husband and you are Male
                            break;
                        case 7: //Inserting a Wife and you are Male
                            rela.relativeRole = 6;
                            break;
                        default:
                            break;
                    }
                }

                else
                {
                    switch (rela.relativeRole)
                    {
                        case 1: //Inserting a Brother are you are Female
                            rela.relativeRole = 2;
                            break;
                        case 2: //Inserting a Sister and you are Female
                            break;
                        case 3: //Inserting a Father and you are Female
                            rela.relativeRole = 5;
                            break;
                        case 4: //Inserting a Mother and you are Female
                            rela.relativeRole = 5;
                            break;
                        case 5: //Inserting a Child and you are Female
                            rela.relativeRole = 4;
                            break;
                        case 6: //Ineserting a Husband and you are Female
                            rela.relativeRole = 7;
                            break;
                        case 7: //Inserting a Wife and you are Female
                            break;
                        default:
                            break;
                    }
                }

                //if (rela.relativeRole == 5 && person.isParent == 0) //If you're adding a child and the record isn't a parent, change them to a parent
                //{
                //    person.isParent = 1;
                //    _treeService.EditIndividual(person);
                //}
                //else { }
                rela.personID = rid;
                rela.relativeID = pid;
                
                _treeService.AddRelative(rela);        //Add the inverse of the relationship 
                return RedirectToAction("GetRelatives", new { pid = pid, controller = "Family" });
            }
            catch
            {
                return View();
            }
        } 

        // Allows edit of a linked username
        [HttpGet]
        public ActionResult EditLink(int lid)
        {
            return View(_treeService.GetUserLink(lid));
        }
        [HttpPost]
        public ActionResult EditLink(int lid, UserLink obj)
        {
            try
            {
                _treeService.EditLink(obj);
                return RedirectToAction("GetLinkedUsers", new { fid = obj.familyID , controller = "Family"});
            }
            catch
            {
                return RedirectToAction("GetLinkedUsers", new { fid = obj.familyID, controller = "Family" });
            }
        }

        // Takes the relationship object from the database, storing some information for the view and edits the information before saving
        [HttpGet]
        public ActionResult EditRelative(int rid)
        {
            int pid = _treeService.GetRelationship(rid).personID;
            ViewBag.personID = pid;
            ViewBag.relationshipID = rid;
            return View(_treeService.GetRelationship(rid));
        }
        [HttpPost]
        public ActionResult EditRelative(relaBEAN relObject)
        {
            // try/catch to ensure the programme doesn't crash
            try
            {
                Relationship editRela = new Relationship
                {
                    relationshipID = relObject.relationshipID,
                    notableInformation = relObject.notableInformation,
                    relationshipStartDate = relObject.relationshipStartDate,
                    relationshipEndDate = relObject.relationshipEndDate
                };

                _treeService.EditRelative(editRela);
                return RedirectToAction("GetRelatives", "Family", new { pid = relObject.personID });
            }
            catch
            {
                return RedirectToAction("GetRelatives", "Family", new { pid = relObject.personID });
            }
        }

        

    }
}
