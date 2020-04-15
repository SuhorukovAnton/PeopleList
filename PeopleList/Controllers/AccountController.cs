using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using PeopleList.Helpers;
using PeopleList.Models;

namespace PeopleList.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Auth(FormAuth form)
        {

            if (ModelState.IsValid)
            {
                var task = HelperConnect.FindUser(form.Email, form.Password);
                var people = task.Result;
                if (people != null)
                {
                    FormsAuthentication.SetAuthCookie(people.id.ToString(), true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", Resources.Resource.UserIsNotFound);
                }
            }
            ViewBag.langs = new List<string>(ConfigurationManager.AppSettings["langs"].Split(','));
            ViewBag.langsFullName = new List<string>(ConfigurationManager.AppSettings["langsFullName"].Split(','));
            ViewData["Auth"] = true;
            ViewData["Layout"] = "~/Views/Shared/_Layout.cshtml";
            return View(form);
        }

        public ActionResult Auth()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");     
            }
            ViewData["Auth"] = true;
            ViewData["Layout"] = "";
            return View();
        }
        public ActionResult AuthWithLayout()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.langs = new List<string>(ConfigurationManager.AppSettings["langs"].Split(','));
            ViewBag.langsFullName = new List<string>(ConfigurationManager.AppSettings["langsFullName"].Split(','));
            ViewData["Layout"] = "~/Views/Shared/_Layout.cshtml";
            ViewData["Auth"] = true;
            return View("Auth");
        }
    }
}