using PeopleList.Helpers;
using PeopleList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace PeopleList.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            return View(HelperConnect.GetPeoples());
        }
        public ActionResult MainForm()
        {
            return View(HelperConnect.GetPeoples());
        }
        [HttpPost]
        public ActionResult Add(string email, string password, string name, string surname, string birthday)
        {
            if (name != "" && surname != "" && password != "" && email != "" && birthday != "")
            {
                byte[] salt = new byte[] { 3, 2, 1, 9, 17 };

                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));
                    HelperConnect.AddPeople(name, surname, hashed, email, birthday);
            }
            return View("~/Views/Home/List.cshtml", HelperConnect.GetPeoples());
        }
   
        public ActionResult Remove(int id)
        {
            HelperConnect.RemovePeople(id);
            return View("~/Views/Home/List.cshtml", HelperConnect.GetPeoples());
        }
        public ActionResult Read(int id)
        {

            return View(HelperConnect.GetPeople(id));
        }
        public ActionResult Edit(int id, string email, string name, string surname, string birthday)
        {
            if (name != "" && surname != "" && email != "" && birthday != "")
            {
                HelperConnect.EditPeople(id, name, surname, email, birthday.Replace('-', '.'));
                return View("~/Views/Home/EditGood.cshtml", HelperConnect.GetPeople(id));
            }
            return View("~/Views/Home/EditFalse.cshtml", HelperConnect.GetPeople(id));
        }
    }
}