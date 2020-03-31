using System.Web;
using System.Web.Mvc;
using PeopleList.Helpers;
using PeopleList.Models;

namespace PeopleList.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

       
        public ActionResult Index()
        {
            Session["PeopleId"] = null;
            ViewData["Layout"] = "~/Views/Shared/_Layout.cshtml";
            return GetView(null);
        }
        public ActionResult MainForm()
        {
            Session["PeopleId"] = null;
            ViewData["Layout"] = "";
            return GetView(null);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Add(FormAdd formAdd)
        {
            if (ModelState.IsValid)
            {
                formAdd.Password = HelperWorkWithData.GetHash(formAdd.Password);
                HelperConnect.AddPeople(formAdd);
            }
            ViewData["Layout"] = "";
            return GetView(formAdd);
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Remove(int id)
        {
            HelperConnect.RemovePeople(id);
            ViewData["Layout"] = "";
            return View("~/Views/Home/List.cshtml", HelperConnect.GetPeoples());
        }

        public ActionResult Read(int id)
        {
            Session["PeopleId"] = id;
            ViewData["canEdit"] = (id == int.Parse(User.Identity.Name) || User.IsInRole("SuperAdmin")).ToString();
            return View("~/Views/Home/People.cshtml", GetFormEdit(id));
        }

        public ActionResult Edit(FormEdit formEdit)
        {
            
            if (Session["PeopleId"] != null && int.TryParse(Session["PeopleId"].ToString(), out var tmp))
            {
                ViewData["canEdit"] = (tmp == int.Parse(User.Identity.Name) || User.IsInRole("SuperAdmin")).ToString();
                ViewData["Img"] =HelperConnect.GetPeople(tmp).Img;
                bool isFind = HelperConnect.FindEmail(formEdit.Email) && HelperConnect.GetPeople(tmp).Email != formEdit.Email;
                if (ModelState.IsValid && !isFind)
                {
                    HelperConnect.EditPeople(tmp, formEdit);
                    ViewData["Message"] = "Изменения успешно сохранены";
                }
                else
                {
                    if (isFind)
                    {
                        ModelState.AddModelError("Email", "Пользователь с таким логином уже в системе");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Неверный id");
            }
        return View("~/Views/Home/People.cshtml", formEdit);
        }

        public ActionResult LoadImg(HttpPostedFileBase img)
        {
            if (img != null && int.TryParse(Session["PeopleId"].ToString(), out var tmp))
            {
                HelperConnect.AddImg(tmp, HelperWorkWithData.SaveFile(img, tmp, Server));
               
                ViewData["canEdit"] = "True";
                return View("~/Views/Home/People.cshtml", GetFormEdit(tmp));
            }
            return RedirectToAction("MainForm", "Home");
        }

        private ActionResult GetView(FormAdd formAdd)
        {
            if (User.IsInRole("Admin"))
            {
                ViewData["hiddenAdd"] = "false";
                return View("~/Views/Home/Index.cshtml", formAdd);
            }
            else
            {
                ViewData["hiddenAdd"] = "true";
                return View("~/Views/Home/Index.cshtml", formAdd);
            }
        }
        public ActionResult List()
        {
            if (!User.IsInRole("SuperAdmin"))
            {
                ViewData["hidden"] = "true";
            }
            return View(HelperConnect.GetPeoples());
        }
        private FormEdit GetFormEdit(int id)
        {
            People people = HelperConnect.GetPeople(id);
            ViewData["Img"] = people.Img;
            return new FormEdit() { Name = people.Name, Surname = people.Surname, Birthday = people.Birthday, Email = people.Email };
        }
    }
}