using PeopleList.Helpers;
using PeopleList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PeopleList.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            return View(HelperConnect.GetPeoples());
        }
        [HttpPost]
        public ActionResult Add(string name, string surname)
        {
            HelperConnect.AddPeoples(name, surname);
            return View(HelperConnect.GetPeoples());
        }

    }
}