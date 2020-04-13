using NLog;
using PeopleList.Helpers;
using PeopleList.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace PeopleList.Core
{
    public class PeopleXmlReader : IReader
    {
        public Logger logger = LogManager.GetCurrentClassLogger();
        public async Task AddPeople(string path)
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
                        People people = new People();
                        people.ReadXml(reader);
                        people.Birthday = HelperWorkWithData.TransformDate(people.Birthday);
                        await HelperConnect.AddPeople(people);
                    }
                }
            }
            catch(Exception e)
            {
                logger.Error("Wrong format XML:" + e.Message);
            }
        }

        public void Create(HttpServerUtilityBase Server)
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

        public void Unload(HttpResponseBase Response, HttpServerUtilityBase Server)
        {
            Response.ContentType = "application/xml";
            Response.AppendHeader("Content-Disposition", "attachment; filename=peoples.xml");
            Response.TransmitFile(Server.MapPath("~/files/peoples.xml"));
            Response.End();
        }
    }
}