using Newtonsoft.Json;
using NLog;
using PeopleList.Helpers;
using PeopleList.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PeopleList.Core
{
    public class PeopleJSONReader : IReader
    {
        public Logger logger = LogManager.GetCurrentClassLogger();
        public async Task AddPeople(string path)
        {
            try
            {
                var text = "";
                using (var sr = new StreamReader(path, Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        text += line;
                    }
                }
                var peoples = JsonConvert.DeserializeObject<List<People>>(text);
                foreach (var people in peoples)
                {
                    people.Birthday = HelperWorkWithData.TransformDate(people.Birthday);
                    await HelperConnect.AddPeople(people);
                }
            }catch(Exception e)
            {
                logger.Error("Wrong JSON format: " + e.Message);
            }
        }

        public void Create(HttpServerUtilityBase Server)
        {
            var peoples = HelperConnect.GetPeoples();
            var json = JsonConvert.SerializeObject(peoples, new JsonSerializerSettings());
            using (var sw = new StreamWriter(Server.MapPath("~/files/peoples.json"), false, Encoding.Default))
            {
                sw.WriteLine(json);
            }
        }

        public void Unload(HttpResponseBase Response, HttpServerUtilityBase Server)
        {
            Response.ContentType = "application/json";
            Response.AppendHeader("Content-Disposition", "attachment; filename=peoples.json");
            Response.TransmitFile(Server.MapPath("~/files/peoples.json"));
            Response.End();
        }
    }
}