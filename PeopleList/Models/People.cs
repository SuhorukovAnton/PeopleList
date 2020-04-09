using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace PeopleList.Models
{
    public class People
    {
        public int id { set; get; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "NameRequired")]
        public string Name { set; get; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "SurnameRequired")]
        public string Surname { set; get; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "EmailRequired")]
        [DataType(DataType.EmailAddress)]
        [ValidEmail(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "EmailValid")]
        [NoFindEmail(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "EmailIsBusy")]
        public string Email { set; get; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "BirthdayRequired")]
        [DataType(DataType.Date)]
        [ValidBirthday(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "BirthdayValid")]
        public string Birthday { set; get; }
        public string Password { set; get; }
        public string Img { set; get; }
        public Roles Role { set; get; }

        public static People ReadXml(XmlReader r)
        {
            People people = new People();
            r.ReadStartElement();
            people.Name = r.ReadElementContentAsString("Name", "");
            people.Surname = r.ReadElementContentAsString("Surname", "");
            people.Email = r.ReadElementContentAsString("Email", "");
            people.Birthday = r.ReadElementContentAsString("Birthday", "");
            people.Password = r.ReadElementContentAsString("Password", "");
            people.Role = (Roles)int.Parse(r.ReadElementContentAsString("Role", ""));
            r.ReadEndElement();
            return people;
        }

        public void WriteXml(XmlWriter w)
        {
            w.WriteElementString("Name", Name);
            w.WriteElementString("Surname", Surname);
            w.WriteElementString("Email", Email);
            w.WriteElementString("Birthday", Birthday);
            w.WriteElementString("Password", Password);
            w.WriteElementString("Role", ((int)Role).ToString());
        }
    }

    public class PeopleJson
    {
        public string Name { set; get; }
        public string Surname { set; get; }
        public string Email { set; get; }
        public string Birthday { set; get; }
        public string Password { set; get; }
        public int Role { set; get; }
    }
}