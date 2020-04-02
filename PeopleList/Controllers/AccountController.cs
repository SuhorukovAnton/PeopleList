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
                var people = HelperConnect.FindUser(form.Email, form.Password);
                if (people != null)
                {
                    FormsAuthentication.SetAuthCookie(people.id.ToString(), true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }
            return View(form);
        }

        public ActionResult Auth()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}