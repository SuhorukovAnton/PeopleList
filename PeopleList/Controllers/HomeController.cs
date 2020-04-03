﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PeopleList.Filters;
using PeopleList.Helpers;
using PeopleList.Models;

namespace PeopleList.Controllers
{
    [Authorize]
    [Culture]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("AuthWithLayout", "Account");
            }
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
                ModelState.AddModelError("Email", Resources.Resource.EmailIsBusy);
            }
            else if (ModelState.IsValid)
            {
                HelperConnect.EditPeople(formEdit);
                ViewData["Message"] = Resources.Resource.SaveIsSuccessfully;
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
        [AllowAnonymous]
        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            // Список культур
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
    }
}