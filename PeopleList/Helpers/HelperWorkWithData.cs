using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
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
        public static void CreateXML(HttpServerUtilityBase Server)
        {
            List<People> peoples = HelperConnect.GetPeoples();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(Server.MapPath("~/files/peoples.xml"), settings))
            {
                writer.WriteStartElement("Peoples");
                peoples.ForEach((people) =>
                {
                    writer.WriteStartElement("People");
                    people.WriteXml(writer);
                    writer.WriteEndElement();
                });
                writer.WriteEndElement();
            }
        }
        public static void UnloadXML(HttpResponseBase Response, HttpServerUtilityBase Server)
        {
            Response.ContentType = "application/xml";
            Response.AppendHeader("Content-Disposition", "attachment; filename=peoples.xml");
            Response.TransmitFile(Server.MapPath("~/files/peoples.xml"));
            Response.End();
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

        public static List<People> AddPeopleXml(string path)
        {
            List<People> peoples = new List<People>();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            try
            {
                using (XmlReader reader = XmlReader.Create(path, settings))
                {
                    reader.ReadToFollowing("People");
                    while (reader.Name == "People")
                    {
                        People people = People.ReadXml(reader);
                        people.Birthday = TransformDate(people.Birthday);
                        HelperConnect.AddPeople(people);
                    }
                }
            }
            catch
            {

            }
            return peoples;
        }

        public static void AddPeopleJSON(string path)
        {
            string text = "";
            using (StreamReader sr = new StreamReader(path, Encoding.Default))
            {             
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    text += line;
                }
            }
            List<PeopleJson> peoplesJson = JsonConvert.DeserializeObject<List<PeopleJson>>(text);
            List<People> peoples = HelperConnect.GetPeoples();
            peoplesJson.ForEach(people =>
            {
                HelperConnect.AddPeople(new People() { Name = people.Name, Birthday = TransformDate(people.Birthday), Email = people.Email, Password = people.Password, Surname = people.Surname, Role = (Roles)people.Role });
            });

        }

        public static void CreateJSON(HttpServerUtilityBase Server)
        {
            List<People> peoples = HelperConnect.GetPeoples();
            List<PeopleJson> peoplesJson = new List<PeopleJson>();
            peoples.ForEach(people =>
            {
                peoplesJson.Add(new PeopleJson() { Name = people.Name, Birthday = people.Birthday, Email = people.Email, Password = people.Password, Surname = people.Surname, Role = (int)people.Role });
            });
            string json = JsonConvert.SerializeObject(peoplesJson, new JsonSerializerSettings());
            using (StreamWriter sw = new StreamWriter(Server.MapPath("~/files/peoples.json"), false, Encoding.Default))
            {
                sw.WriteLine(json);
            }
        }

        public static void UnloadJSON(HttpResponseBase Response, HttpServerUtilityBase Server)
        {
            Response.ContentType = "application/json";
            Response.AppendHeader("Content-Disposition", "attachment; filename=peoples.json");
            Response.TransmitFile(Server.MapPath("~/files/peoples.json"));
            Response.End();
        }
    }
}