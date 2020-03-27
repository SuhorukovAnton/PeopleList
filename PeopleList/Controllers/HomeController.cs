using PeopleList.Helpers;
using PeopleList.Models;
using System.Web;
using System.Web.Mvc;

namespace PeopleList.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            if (Session["userId"] == null)
                return View("~/Views/Home/Auth.cshtml");
            else
            {
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
        }
        public ActionResult Auth(string email, string password)
        {
            if (password != null)
            {
                People people = HelperConnect.FindUser(email, HelperWorkWithData.GetHash(password));
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
                return View("~/Views/Home/Auth.cshtml");
            else
            {
                Session["PeopleId"] = null;
                ViewData["Layout"] = "";
                return GetView();
            }
        }
        [HttpPost]
        public ActionResult Add(People people)
        {
            if (Session["userId"] == null)
                return View("~/Views/Home/Auth.cshtml");
            else
            {
                if (people.CheckAdd())
                {
                    people.Password = HelperWorkWithData.GetHash(people.Password);
                    HelperConnect.AddPeople(people);
                }
                ViewData["Layout"] = "";
                return View("~/Views/Home/List.cshtml", HelperConnect.GetPeoples());
            }
        }
   
        public ActionResult Remove(int id)
        {
            if (Session["userId"] == null)
                return View("~/Views/Home/Auth.cshtml");
            else
            {
                HelperConnect.RemovePeople(id);
                ViewData["Layout"] = "";
                return View("~/Views/Home/List.cshtml", HelperConnect.GetPeoples());
            }
        }
        public ActionResult Read(int id)
        {
            if (Session["userId"] == null)
            {
                return View("~/Views/Home/Auth.cshtml");
            }
                Session["PeopleId"] = id;
                People people = HelperConnect.GetPeople(id);
            ViewData["canEdit"] = ((Roles)Session["role"] >= Roles.SuperAdmin || (int)Session["userId"] == id).ToString();
            return View("~/Views/Home/People.cshtml", people);
        }
        public ActionResult Edit(People people)
        {
            if (Session["userId"] == null)
                return View("~/Views/Home/Auth.cshtml");
            else
            {
                if (people.CheckEdit())
                {
                    int tmp;
                    if (Session["PeopeId"] != null && int.TryParse(Session["PeopleId"].ToString(), out tmp))
                    {
                        people.id = tmp;
                        HelperConnect.EditPeople(people);
                        ViewData["Message"] = "Изменения успешно сохранены";
                        return View("~/Views/Home/EditMessage.cshtml", HelperConnect.GetPeople(people.id));
                    }
                    else
                    {
                        ViewData["Message"] = "Неверный id";
                        return View("~/Views/Home/EditMessage.cshtml", HelperConnect.GetPeople(people.id));
                    }
                }
                ViewData["Message"] = "Заполнены не все данные";
                return View("~/Views/Home/EditMessage.cshtml", HelperConnect.GetPeople(people.id));
            }
        }
        public ActionResult LoadImg(HttpPostedFileBase img)
        {
            if (Session["userId"] == null)
                return View("~/Views/Home/Auth.cshtml");
            else
            {
                int id = (int)Session["PeopleId"];
                if (img != null)
                {
                    HelperConnect.AddImg(id, HelperWorkWithData.SaveFile(img, id, Server));
                }
                People people = HelperConnect.GetPeople(id);
                return View("~/Views/Home/People.cshtml", people);
            }
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