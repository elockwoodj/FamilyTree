using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyTree.Data;
using FamilyTree.Services;

namespace FamilyTree.Controllers
{
    public abstract class ApplicationController : Controller
    {
        // GET: Application
        public ActionResult Index()
        {
            return View();
        }

        public Services.DAO.TreeService _treeService;
        public ApplicationController()
        {
            _treeService = new Services.DAO.TreeService();

        }
    }
}
