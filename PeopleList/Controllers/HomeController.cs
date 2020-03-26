using PeopleList.Helpers;
using PeopleList.Models;
using System;
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
                    if ((Roles)Session["role"] >= Roles.Admin)
                    {
                        if ((Roles)Session["role"] == Roles.Admin)
                            ViewData["hidden"] = "true";
                        return View("~/Views/Home/Index.cshtml", HelperConnect.GetPeoples());
                    }
                    else
                    {
                        ViewData["hidden"] = "true";
                        return View("~/Views/Home/List.cshtml", HelperConnect.GetPeoples());
                    }
                }
                else
                {
                    ViewData["error"] = "Неверный логин или пароль";
                    return View("~/Views/Home/AuthError.cshtml");
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
                if ((Roles)Session["role"] >= Roles.Admin)
                {
                    if ((Roles)Session["role"] == Roles.Admin)
                        ViewData["hidden"] = "true";
                    return View("~/Views/Home/Index.cshtml", HelperConnect.GetPeoples());
                }
                else
                {
                    ViewData["hidden"] = "true";
                    return View("~/Views/Home/List.cshtml", HelperConnect.GetPeoples());
                }
            }
        }
        [HttpPost]
        public ActionResult Add(string email, string password, string name, string surname, string birthday)
        {
            if (Session["userId"] == null)
                return View("~/Views/Home/Auth.cshtml");
            else
            {
                if (name != "" && surname != "" && password != "" && email != "" && birthday != "")
                    HelperConnect.AddPeople(name, surname, HelperWorkWithData.GetHash(password), email, birthday);
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
                return View("~/Views/Home/Auth.cshtml");
            else
            {
                Session["PeopleId"] = id;
                People people = HelperConnect.GetPeople(id);
                if ((Roles)Session["role"] >= Roles.SuperAdmin || (int)Session["userId"] == id)
                {
                    ViewData["canEdit"] = "true";
                    return View("~/Views/Home/People.cshtml", people);
                }
                else
                {
                    ViewData["canEdit"] = "false";
                    return View("~/Views/Home/People.cshtml", people);
                }
            }
        }
        public ActionResult Edit(string email, string name, string surname, string birthday)
        {
            if (Session["userId"] == null)
                return View("~/Views/Home/Auth.cshtml");
            else
            {
                int id = (int)Session["PeopleId"];
                if (name != "" && surname != "" && email != "" && birthday != "")
                {
                    HelperConnect.EditPeople(id, name, surname, email, birthday);
                    ViewData["Message"] = "Изменения успешно сохранены";
                    return View("~/Views/Home/EditMessage.cshtml", HelperConnect.GetPeople(id));
                }
                ViewData["Message"] = "Заполнены не все данные";
                return View("~/Views/Home/EditMessage.cshtml", HelperConnect.GetPeople(id));
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
    }
}