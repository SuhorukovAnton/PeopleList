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
            ViewData["canEdit"] = (id == int.Parse(User.Identity.Name) || User.IsInRole("SuperAdmin"));
            return View("~/Views/Home/People.cshtml", GetFormEdit(id));
        }

        public ActionResult Edit(int id, FormEdit formEdit)
        {
            ViewData["canEdit"] = id == int.Parse(User.Identity.Name) || User.IsInRole("SuperAdmin");
            ViewData["Img"] = HelperConnect.GetPeople(id).Img;

            var isFind = HelperConnect.FindEmail(formEdit.Email) && HelperConnect.GetPeople(id).Email != formEdit.Email;

            if (isFind)
            {
                ModelState.AddModelError("Email", "Пользователь с таким логином уже в системе");
            }
            else if (ModelState.IsValid)
            {
                HelperConnect.EditPeople(formEdit);
                ViewData["Message"] = "Изменения успешно сохранены";
            }

            return View("~/Views/Home/People.cshtml", formEdit);
        }

        public ActionResult LoadImg(int id, HttpPostedFileBase img)
        {
            if (img != null)
            {
                HelperConnect.AddImg(id, HelperWorkWithData.SaveFile(img, id, Server));

                ViewData["canEdit"] = true;
                return View("~/Views/Home/People.cshtml", GetFormEdit(id));
            }

            return RedirectToAction("MainForm", "Home");
        }

        private ActionResult GetView(FormAdd formAdd)
        {
            ViewData["hiddenAdd"] = !User.IsInRole("Admin");
            return View("~/Views/Home/Index.cshtml", formAdd);
        }

        public ActionResult List()
        {
            if (!User.IsInRole("SuperAdmin"))
            {
                ViewData["hidden"] = true;
            }
            return View(HelperConnect.GetPeoples());
        }

        private FormEdit GetFormEdit(int id)
        {
            var people = HelperConnect.GetPeople(id);
            ViewData["Img"] = people.Img;

            return new FormEdit
            {
                Id = id,
                Name = people.Name,
                Surname = people.Surname,
                Birthday = people.Birthday,
                Email = people.Email
            };
        }
    }
}