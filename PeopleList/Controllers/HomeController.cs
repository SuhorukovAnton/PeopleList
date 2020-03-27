using System.Web;
using System.Web.Mvc;

using PeopleList.Helpers;
using PeopleList.Models;

namespace PeopleList.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            if (Session["userId"] == null)
            {
                return View("~/Views/Home/Auth.cshtml");
            }

            ViewData["Layout"] = "~/Views/Shared/_Layout.cshtml";
            if ((Roles)Session["role"] >= Roles.Admin)
            {
                return View(HelperConnect.GetPeoples());
            }
            else
            {
                ViewData["hidden"] = "true";
                return View("~/Views/Home/List.cshtml", HelperConnect.GetPeoples());
            }
        }

        public ActionResult Auth(string email, string password)
        {
            if (password != null)
            {
                var people = HelperConnect.FindUser(email, password);
                if (people != null)
                {
                    HelperWorkWithData.StartSession(Session, people.id, people.Role);
                    ViewData["Layout"] = "";
                    return GetView();
                }
            }

            ViewData["error"] = "Неверный логин или пароль";
            return View("~/Views/Home/AuthError.cshtml");
        }

        public ActionResult MainForm()
        {
            if (Session["userId"] == null)
            {
                return View("~/Views/Home/Auth.cshtml");
            }

            Session["PeopleId"] = null;
            ViewData["Layout"] = "";
            return GetView();
        }

        [HttpPost]
        public ActionResult Add(People people)
        {
            if (Session["userId"] == null)
            {
                return View("~/Views/Home/Auth.cshtml");
            }

            if (people.CheckAdd())
            {
                people.Password = HelperWorkWithData.GetHash(people.Password);
                HelperConnect.AddPeople(people);
            }

            ViewData["Layout"] = "";
            return View("~/Views/Home/List.cshtml", HelperConnect.GetPeoples());
        }

        public ActionResult Remove(int id)
        {
            if (Session["userId"] == null)
            {
                return View("~/Views/Home/Auth.cshtml");
            }

            HelperConnect.RemovePeople(id);
            ViewData["Layout"] = "";
            return View("~/Views/Home/List.cshtml", HelperConnect.GetPeoples());
        }

        public ActionResult Read(int id)
        {
            if (Session["userId"] == null)
            {
                return View("~/Views/Home/Auth.cshtml");
            }

            Session["PeopleId"] = id;
            ViewData["canEdit"] = ((Roles)Session["role"] >= Roles.SuperAdmin || (int)Session["userId"] == id).ToString();
            return View("~/Views/Home/People.cshtml", HelperConnect.GetPeople(id));
        }

        public ActionResult Edit(People people)
        {
            if (Session["userId"] == null)
            {
                return View("~/Views/Home/Auth.cshtml");
            }

            if (people.CheckEdit())
            {
                if (Session["PeopleId"] != null && int.TryParse(Session["PeopleId"].ToString(), out var tmp))
                {
                    people.id = tmp;
                    HelperConnect.EditPeople(people);
                    ViewData["Message"] = "Изменения успешно сохранены";
                }
                else
                {
                    ViewData["Message"] = "Неверный id";
                }
            }

            ViewData["Message"] = "Заполнены не все данные";
            return View("~/Views/Home/EditMessage.cshtml", HelperConnect.GetPeople(people.id));
        }

        public ActionResult LoadImg(HttpPostedFileBase img)
        {
            if (Session["userId"] == null)
            {
                return View("~/Views/Home/Auth.cshtml");
            }

            var id = (int)Session["PeopleId"];
            if (img != null)
            {
                HelperConnect.AddImg(id, HelperWorkWithData.SaveFile(img, id, Server));
            }

            var people = HelperConnect.GetPeople(id);
            return View("~/Views/Home/People.cshtml", people);
        }

        private ActionResult GetView()
        {
            if ((Roles)Session["role"] >= Roles.Admin)
            {
                if ((Roles)Session["role"] == Roles.Admin)
                {
                    ViewData["hidden"] = "true";
                }
                return View("~/Views/Home/Index.cshtml", HelperConnect.GetPeoples());
            }
            else
            {
                ViewData["hidden"] = "true";
                return View("~/Views/Home/List.cshtml", HelperConnect.GetPeoples());
            }
        }
    }
}