using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace PeopleList.Helpers
{
    public static class HelperWorkWithData
    {
        private static readonly byte[] salt;
        private static readonly string basePath;

        static HelperWorkWithData()
        {
            salt = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["salt"] ?? "");
            basePath = "files";
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

        public static string Save(this HttpPostedFileBase img, int id, HttpServerUtilityBase Server)
        {
            var file = id + Path.GetExtension(img.FileName);
            img.SaveAs(Server.MapPath("~/files/imgs/" + file));
            return file;
        }

        public static string Save(this HttpPostedFileBase file, string name, HttpServerUtilityBase Server)
        {
            var nameFile = name + Path.GetExtension(file.FileName);
            var savePath = Server.MapPath("~/" + Path.Combine(basePath, "download", nameFile));
            file.SaveAs(savePath);
            return savePath;
        }

        public static string TransformDate(string date)
        {
            date = date.Split(' ')[0];
            var tmp = date.Split('.');
            if (tmp.Length >= 3)
            {
                return tmp[2] + "-" + tmp[1] + "-" + tmp[0];
            }
            else return null;
        }


    }
}