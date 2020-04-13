using System;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

using NLog;

using PeopleList.Helpers;
using PeopleList.Models;

namespace PeopleList.Core
{
    public class PeopleXmlReader : IReader
    {
        public Logger Logger { get; set; }
        public PeopleXmlReader()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }

        public async Task AddPeople(string path)
        {
            var settings = new XmlReaderSettings
            {
                IgnoreWhitespace = true
            };

            try
            {
                using (var reader = XmlReader.Create(path, settings))
                {
                    reader.ReadToFollowing("People");
                    while (reader.Name == "People")
                    {
                        var people = new People();
                        people.ReadXml(reader);
                        people.Birthday = HelperWorkWithData.TransformDate(people.Birthday);
                        await HelperConnect.AddPeople(people);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("Wrong format XML:" + e.Message);
            }
        }

        public void Create(HttpServerUtilityBase Server)
        {
            var peoples = HelperConnect.GetPeoples();
            var settings = new XmlWriterSettings
            {
                Indent = true
            };
            using (var writer = XmlWriter.Create(Server.MapPath("~/files/peoples.xml"), settings))
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