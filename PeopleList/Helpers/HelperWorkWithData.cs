using System;
using System.Configuration;
using System.Text;
using System.Web;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;

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

        public static string SaveFile(HttpPostedFileBase img, int id, HttpServerUtilityBase Server)
        {
            var file = id + "." + System.IO.Path.GetFileName(img.FileName).Split('.')[1];
            img.SaveAs(Server.MapPath("~/files/imgs/" + file));
            return file;
        }
        public static void StartSession(HttpSessionStateBase Session, int id, Roles role)
        {
            Session["userId"] = id;
            Session["role"] = role;
        }
    }
}