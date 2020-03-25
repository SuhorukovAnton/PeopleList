﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using PeopleList.Models;

namespace PeopleList.Helpers
{
    public static class HelperWorkWithData
    {
        private static byte[] salt = new byte[] { 3, 2, 1, 9, 17 };
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
            string file = id + "." + System.IO.Path.GetFileName(img.FileName).Split('.')[1];
            img.SaveAs(Server.MapPath("~/files/imgs/" + file));
            return file;
        }
    }
}