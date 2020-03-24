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

            People people = HelperConnect.GetPeople(id);
            if (people.Img != null)
                return View("~/Views/Home/PeopleWithImg.cshtml", people);
            else
                return View("~/Views/Home/PeopleWithoutImg.cshtml", people);
        }
        public ActionResult Edit(int id, string email, string name, string surname, string birthday)
        {
            if (name != "" && surname != "" && email != "" && birthday != "")
            {
                String[] array = birthday.Split('-');
                
                HelperConnect.EditPeople(id, name, surname, email, birthday);
                return View("~/Views/Home/EditGood.cshtml", HelperConnect.GetPeople(id));
            }
            return View("~/Views/Home/EditFalse.cshtml", HelperConnect.GetPeople(id));
        }
        [HttpPost]
        public ActionResult LoadImg(HttpPostedFileBase img, int id)
        {
            if (img != null)
            {
                // получаем имя файла
                string file = id + "." + System.IO.Path.GetFileName(img.FileName).Split('.')[1];
                // сохраняем файл в папку Files в проекте
                img.SaveAs(Server.MapPath("~/files/imgs/" + file));
                HelperConnect.AddImg(id, file);
            }
            People people = HelperConnect.GetPeople(id);
            if (people.Img != null)
                return View("~/Views/Home/PeopleWithImg.cshtml", people);
            else
                return View("~/Views/Home/PeopleWithoutImg.cshtml", people);
        }
    }
}