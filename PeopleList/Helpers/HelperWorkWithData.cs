using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Newtonsoft.Json;
using PeopleList.Models;

namespace PeopleList.Helpers
{
    public static class HelperWorkWithData
    {
        private static readonly byte[] salt;

        static HelperWorkWithData()
        {
            salt = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["salt"] ?? "");
        }

        public static string GetHash(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));
        }

        public static string SaveImg(HttpPostedFileBase img, int id, HttpServerUtilityBase Server)
        {
            var file = id + "." + Path.GetFileName(img.FileName).Split('.')[1];
            img.SaveAs(Server.MapPath("~/files/imgs/" + file));
            return file;
        }
        public static string SaveFile(HttpPostedFileBase file, string name, HttpServerUtilityBase Server)
        {
            var nameFile = name + "." + Path.GetFileName(file.FileName).Split('.')[1];
            file.SaveAs(Server.MapPath("~/files/download/" + nameFile));
            return Server.MapPath("~/files/download/" + nameFile);
        }
   
        public static string TransformDate(string date)
        {
            date= date.Split(' ')[0];
            var tmp = date.Split('.');
            if (tmp.Length >= 3)
            {
                return tmp[2] + "-" + tmp[1] + "-" + tmp[0];
            }
            else return null;
        }

       
    }
}