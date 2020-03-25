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
                    Session["userId"] = people.id;
                    Session["role"] = people.Role;
                    ViewData["Layout"] = "";
                    
                    if ((Roles)Session["role"] >= Roles.Admin)
                    {
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
            Session["PeopleId"] = null;
            ViewData["Layout"] = "";
            if ((Roles)Session["role"] >= Roles.Admin)
            {
                return View("~/Views/Home/Index.cshtml", HelperConnect.GetPeoples());
            }
            else
            {
                ViewData["hidden"] = "true";
                return View("~/Views/Home/List.cshtml", HelperConnect.GetPeoples());
            }
        }
        [HttpPost]
        public ActionResult Add(string email, string password, string name, string surname, string birthday)
        {
            if (name != "" && surname != "" && password != "" && email != "" && birthday != "")
                    HelperConnect.AddPeople(name, surname, HelperWorkWithData.GetHash(password), email, birthday);
            ViewData["Layout"] = "";
            return View("~/Views/Home/List.cshtml", HelperConnect.GetPeoples());
        }
   
        public ActionResult Remove(int id)
        {
            HelperConnect.RemovePeople(id);
            ViewData["Layout"] = "";
            return View("~/Views/Home/List.cshtml", HelperConnect.GetPeoples());
        }
        public ActionResult Read(int id)
        {
            Session["PeopleId"] = id;
            People people = HelperConnect.GetPeople(id);
            return View("~/Views/Home/People.cshtml", people);
        }
        public ActionResult Edit(string email, string name, string surname, string birthday)
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
        public ActionResult LoadImg(HttpPostedFileBase img)
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