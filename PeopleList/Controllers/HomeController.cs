using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;
using NLog;
using PeopleList.Core;
using PeopleList.Filters;
using PeopleList.Helpers;
using PeopleList.Models;

namespace PeopleList.Controllers
{
    [Authorize]
    [Culture]
    public class HomeController : Controller
    {
        PeopleJSONReader JSONReader = new PeopleJSONReader();
        PeopleXmlReader XmlReader = new PeopleXmlReader();
       

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {         
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("AuthWithLayout", "Account");
            }
            var people = await HelperConnect.GetPeople(int.Parse(User.Identity.Name));
            ViewData["Email"] = people.Email;
            Session["PeopleId"] = null;
            ViewData["Layout"] = "~/Views/Shared/_Layout.cshtml";
            return GetView();
        }
        public ActionResult MainForm()
        {
            Session["PeopleId"] = null;
            ViewData["Layout"] = "";
            return GetView();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult OpenAdd()
        {
            ViewData["Layout"] = "";
            return View("~/Views/Home/Add.cshtml");
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Add(FormAdd formAdd)
        {
            if (ModelState.IsValid)
            {
                formAdd.Password = HelperWorkWithData.GetHash(formAdd.Password);
                await HelperConnect.AddPeople(formAdd);
                return RedirectToAction("MainForm", "Home");
            }
            ViewData["Layout"] = "";
            return View(formAdd);
        }
        
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Remove(int id)
        {
            await HelperConnect.RemovePeople(id);
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Read(int id)
        {
            ViewData["canEdit"] = (id == int.Parse(User.Identity.Name) || User.IsInRole("SuperAdmin"));
            return View("~/Views/Home/People.cshtml", await GetFormEdit(id));
        }

        public async Task<ActionResult> Edit(int id, FormEdit formEdit)
        {
            ViewData["canEdit"] = id == int.Parse(User.Identity.Name) || User.IsInRole("SuperAdmin");
            ViewData["Img"] = HelperConnect.GetPeople(id).Result.Img;

            var isFind = HelperConnect.FindEmail(formEdit.Email).Result && HelperConnect.GetPeople(id).Result.Email != formEdit.Email;

            if (isFind)
            {
                ModelState.AddModelError("Email", Resources.Resource.EmailIsBusy);
            }
            else if (ModelState.IsValid)
            {
               await HelperConnect.EditPeople(formEdit);
                ViewData["Message"] = Resources.Resource.SaveIsSuccessfully;
            }

            return View("~/Views/Home/People.cshtml", formEdit);
        }

        public async Task<ActionResult> LoadImg(int id, HttpPostedFileBase img)
        {
            if (img != null)
            {
                await HelperConnect.AddImg(id, HelperWorkWithData.SaveImg(img, id, Server));

                ViewData["canEdit"] = true;
                return View("~/Views/Home/People.cshtml", GetFormEdit(id));
            }

            return RedirectToAction("MainForm", "Home");
        }
        public async Task<ActionResult> LoadXmlOrJSON(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string path = HelperWorkWithData.SaveFile(file, "peoples", Server);
                if (path.EndsWith(".xml"))
                {
                   await XmlReader.AddPeople(path);
                }
                else
                {
                    await JSONReader.AddPeople(path);
                }
            }

            return RedirectToAction("MainForm", "Home");
        }

        private ActionResult GetView()
        {
            ViewData["hiddenAdd"] = !User.IsInRole("Admin");
            return View("~/Views/Home/Index.cshtml");
        }

        public ActionResult List()
        {
            if (!User.IsInRole("SuperAdmin"))
            {
                ViewData["hidden"] = true;
            }
            return View(HelperConnect.GetPeoples());
        }

        private async Task<FormEdit> GetFormEdit(int id)
        {
            var people = await HelperConnect.GetPeople(id);
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
        [AllowAnonymous]
        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            List<string> cultures = new List<string>() { "ru", "en" };
            if (!cultures.Contains(lang))
            {
                lang = "ru";
            }
            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang;  
            else
            {
                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public void UnloadPeoplesXML()
        {
            XmlReader.Create(Server);
            XmlReader.Unload(Response, Server);
        }
        public void UnloadPeoplesJSON()
        {
            JSONReader.Create(Server);
            JSONReader.Unload(Response, Server);
        }
    }
}