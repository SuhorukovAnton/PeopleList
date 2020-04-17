using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
            WriteLangsToViewBag();
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
            WriteLangsToViewBag();
            ViewData["Layout"] = "~/Views/Shared/_Layout.cshtml";
            ViewData["Auth"] = true;
            return View("Auth");
        }

        public void WriteLangsToViewBag()
        {
            List<string> langs = new List<string>(ConfigurationManager.AppSettings["langs"].Split(','));
            List<string> nameLangs = new List<string>();
            langs.ForEach(elem =>
            {
                nameLangs.Add(HelperWorkWithData.FirstUpper(new CultureInfo(elem).NativeName));
            });
            ViewBag.langs = langs;
            ViewBag.langsFullName = nameLangs;
        }
    }
}